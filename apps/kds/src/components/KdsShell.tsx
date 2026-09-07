import React, { useState, useEffect } from 'react';
import {
  Button,
  Badge,
  OrderChit,
  StateView,
  ChitStatus
} from '@coffee-pos/ui';
import { INITIAL_KDS_ORDERS, KdsOrder } from '../data/mockKdsOrders';
import './KdsShell.css';

export const KdsShell: React.FC = () => {
  const [orders, setOrders] = useState<KdsOrder[]>(INITIAL_KDS_ORDERS);
  const [bumpHistory, setBumpHistory] = useState<KdsOrder[]>([]);
  const [selectedStation, setSelectedStation] = useState<'All' | 'Espresso' | 'Filter' | 'Food'>('All');
  const [queueFilter, setQueueFilter] = useState<'active' | 'ready' | 'completed'>('active');

  // Live timer tick every 1 second
  useEffect(() => {
    const timer = setInterval(() => {
      setOrders(prevOrders =>
        prevOrders.map(order => {
          if (order.status === 'HandedOff') return order;
          const nextSeconds = order.elapsedSeconds + 1;
          const isOverdue = nextSeconds > 480 && (order.status === 'New' || order.status === 'Preparing');
          return {
            ...order,
            elapsedSeconds: nextSeconds,
            status: isOverdue ? 'Overdue' : order.status
          };
        })
      );
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  // Advance Order Status
  const handleAdvanceStatus = (orderId: string, nextStatus: ChitStatus) => {
    setOrders(prev =>
      prev.map(order => {
        if (order.id === orderId) {
          const updated = { ...order, status: nextStatus };
          if (nextStatus === 'HandedOff') {
            setBumpHistory(h => [updated, ...h]);
          }
          return updated;
        }
        return order;
      })
    );
  };

  // Recall last bumped order
  const handleRecallLastBump = () => {
    if (bumpHistory.length === 0) return;
    const lastBumped = bumpHistory[0];
    setBumpHistory(h => h.slice(1));
    setOrders(prev =>
      prev.map(order => (order.id === lastBumped.id ? { ...order, status: 'Ready' } : order))
    );
  };

  // Filter orders by station and queue
  const filteredOrders = orders.filter(order => {
    const matchesStation = selectedStation === 'All' || order.station === selectedStation;

    if (!matchesStation) return false;

    if (queueFilter === 'active') {
      return order.status === 'New' || order.status === 'Preparing' || order.status === 'Overdue';
    }
    if (queueFilter === 'ready') {
      return order.status === 'Ready';
    }
    if (queueFilter === 'completed') {
      return order.status === 'HandedOff';
    }
    return true;
  });

  const activeCount = orders.filter(o => o.status === 'New' || o.status === 'Preparing' || o.status === 'Overdue').length;
  const readyCount = orders.filter(o => o.status === 'Ready').length;

  return (
    <div className="rl-kds-shell">
      {/* 1. Top Control Bar */}
      <header className="rl-kds-topbar">
        <div className="rl-kds-topbar__brand-group">
          <span className="rl-kds-topbar__logo">☕</span>
          <div>
            <h1 className="rl-kds-topbar__title">ROAST LEDGER KDS</h1>
            <span className="rl-kds-topbar__location">Bangsar Flagship • Kitchen Rail</span>
          </div>
        </div>

        {/* Station Filter Tabs */}
        <div className="rl-kds-stations" role="tablist">
          {(['All', 'Espresso', 'Filter', 'Food'] as const).map(st => (
            <button
              key={st}
              type="button"
              role="tab"
              aria-selected={selectedStation === st}
              className={`rl-kds-station-btn ${selectedStation === st ? 'rl-kds-station-btn--active' : ''}`}
              onClick={() => setSelectedStation(st)}
            >
              {st === 'All' ? 'All Stations' : `${st} Bar`}
            </button>
          ))}
        </div>

        {/* Queue and Recall Actions */}
        <div className="rl-kds-topbar__actions">
          <div className="rl-kds-queues">
            <button
              type="button"
              className={`rl-kds-queue-btn ${queueFilter === 'active' ? 'rl-kds-queue-btn--active' : ''}`}
              onClick={() => setQueueFilter('active')}
            >
              Active ({activeCount})
            </button>
            <button
              type="button"
              className={`rl-kds-queue-btn ${queueFilter === 'ready' ? 'rl-kds-queue-btn--active' : ''}`}
              onClick={() => setQueueFilter('ready')}
            >
              Ready ({readyCount})
            </button>
            <button
              type="button"
              className={`rl-kds-queue-btn ${queueFilter === 'completed' ? 'rl-kds-queue-btn--active' : ''}`}
              onClick={() => setQueueFilter('completed')}
            >
              History ({bumpHistory.length})
            </button>
          </div>

          <Button
            variant="outline"
            size="md"
            disabled={bumpHistory.length === 0}
            onClick={handleRecallLastBump}
            title="Re-open last handed off ticket"
          >
            ↺ Recall Bump
          </Button>
        </div>
      </header>

      {/* 2. Main Order Rail Grid */}
      <main className="rl-kds-main">
        {filteredOrders.length === 0 ? (
          <div className="rl-kds-empty-container">
            <StateView
              kind="empty"
              title="Kitchen Rail Clear"
              description={
                queueFilter === 'active'
                  ? 'All coffee and food orders have been prepared or handed off. Great job!'
                  : 'No tickets in this status category.'
              }
            />
          </div>
        ) : (
          <div className="rl-kds-grid">
            {filteredOrders.map(order => (
              <OrderChit
                key={order.id}
                orderNumber={order.orderNumber}
                status={order.status}
                diningOption={order.diningOption}
                elapsedSeconds={order.elapsedSeconds}
                tableOrCustomer={order.tableOrCustomer}
                items={order.items}
                notes={order.notes}
                onAdvanceStatus={nextStatus => handleAdvanceStatus(order.id, nextStatus)}
              />
            ))}
          </div>
        )}
      </main>
    </div>
  );
};
