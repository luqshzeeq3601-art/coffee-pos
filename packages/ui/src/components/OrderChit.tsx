import React from 'react';
import { Badge } from './Badge';
import { Button } from './Button';
import './OrderChit.css';

export type ChitStatus = 'New' | 'Preparing' | 'Ready' | 'HandedOff' | 'Overdue';
export type DiningOption = 'DineIn' | 'Takeaway' | 'Delivery';

export interface ChitItem {
  id: string;
  name: string;
  quantity: number;
  modifiers?: string[];
  notes?: string;
}

export interface OrderChitProps {
  orderNumber: string;
  status: ChitStatus;
  diningOption: DiningOption;
  elapsedSeconds: number;
  items: ChitItem[];
  tableOrCustomer?: string;
  notes?: string;
  onAdvanceStatus?: (nextStatus: ChitStatus) => void;
  className?: string;
}

export const OrderChit: React.FC<OrderChitProps> = ({
  orderNumber,
  status,
  diningOption,
  elapsedSeconds,
  items,
  tableOrCustomer,
  notes,
  onAdvanceStatus,
  className = ''
}) => {
  const formatTimer = (totalSeconds: number) => {
    const mins = Math.floor(totalSeconds / 60);
    const secs = totalSeconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  };

  const getStatusTone = (s: ChitStatus) => {
    switch (s) {
      case 'New': return 'info';
      case 'Preparing': return 'warning';
      case 'Ready': return 'success';
      case 'Overdue': return 'danger';
      default: return 'neutral';
    }
  };

  const getNextAction = (s: ChitStatus): { label: string; next: ChitStatus } | null => {
    switch (s) {
      case 'New': return { label: 'Start Prep', next: 'Preparing' };
      case 'Preparing': return { label: 'Mark Ready', next: 'Ready' };
      case 'Ready': return { label: 'Hand Off', next: 'HandedOff' };
      case 'Overdue': return { label: 'Expedite & Ready', next: 'Ready' };
      default: return null;
    }
  };

  const nextAction = getNextAction(status);

  return (
    <article className={`rl-chit rl-chit--${status.toLowerCase()} ${className}`}>
      {/* Visual Status Notch on Top */}
      <div className={`rl-chit__notch rl-chit__notch--${status.toLowerCase()}`} aria-hidden="true" />

      <header className="rl-chit__header">
        <div className="rl-chit__meta">
          <span className="rl-chit__number">{orderNumber}</span>
          <span className="rl-chit__dining">{diningOption === 'DineIn' ? 'DINE-IN' : 'TAKEAWAY'}</span>
        </div>
        <div className="rl-chit__status-group">
          <span className="rl-chit__timer" title="Elapsed Time">
            ⏱ {formatTimer(elapsedSeconds)}
          </span>
          <Badge tone={getStatusTone(status)}>{status}</Badge>
        </div>
      </header>

      {tableOrCustomer && (
        <div className="rl-chit__customer">
          <strong>{tableOrCustomer}</strong>
        </div>
      )}

      {/* Item List */}
      <div className="rl-chit__items">
        {items.map(item => (
          <div key={item.id} className="rl-chit__item">
            <div className="rl-chit__item-main">
              <span className="rl-chit__item-qty">{item.quantity}×</span>
              <span className="rl-chit__item-name">{item.name}</span>
            </div>
            {item.modifiers && item.modifiers.length > 0 && (
              <ul className="rl-chit__modifiers">
                {item.modifiers.map((mod, idx) => (
                  <li key={idx} className="rl-chit__modifier">↳ {mod}</li>
                ))}
              </ul>
            )}
            {item.notes && (
              <div className="rl-chit__item-notes">Note: {item.notes}</div>
            )}
          </div>
        ))}
      </div>

      {notes && (
        <div className="rl-chit__order-notes">
          <em>Order Note: {notes}</em>
        </div>
      )}

      {/* Action Footer */}
      {nextAction && onAdvanceStatus && (
        <footer className="rl-chit__footer">
          <Button
            variant={status === 'Overdue' ? 'destructive' : status === 'Ready' ? 'secondary' : 'primary'}
            size="pos-touch"
            onClick={() => onAdvanceStatus(nextAction.next)}
            style={{ width: '100%' }}
          >
            {nextAction.label}
          </Button>
        </footer>
      )}
    </article>
  );
};
