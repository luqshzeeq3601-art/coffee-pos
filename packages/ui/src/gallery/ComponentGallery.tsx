import React, { useState } from 'react';
import { Button } from '../components/Button';
import { Badge } from '../components/Badge';
import { MoneyDisplay } from '../components/MoneyDisplay';
import { PinPad } from '../components/PinPad';
import { TextInput } from '../components/TextInput';
import { OrderChit, ChitStatus } from '../components/OrderChit';
import { StateView } from '../components/StateView';
import { Modal } from '../components/Modal';
import { ReceiptView } from '../components/ReceiptView';
import '../tokens.css';

import './ComponentGallery.css';

export const ComponentGallery: React.FC = () => {
  const [pinStatus, setPinStatus] = useState<string>('Ready for staff PIN');
  const [pinError, setPinError] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [chitStatus, setChitStatus] = useState<ChitStatus>('New');

  const handlePinComplete = (pin: string) => {
    if (pin === '1234') {
      setPinStatus('PIN Authorized: Siti Manager (Manager)');
      setPinError(null);
    } else if (pin === '5678') {
      setPinStatus('PIN Authorized: Ahmad Cashier (Cashier)');
      setPinError(null);
    } else {
      setPinError('Invalid PIN code');
      setPinStatus('Authentication failed');
    }
  };

  return (
    <div className="rl-gallery">
      <header className="rl-gallery__header">
        <div className="rl-gallery__title-group">
          <h1 className="rl-gallery__title">Roast Ledger Design System</h1>
          <p className="rl-gallery__subtitle">Shared UI Primitives & Tactile Coffee Production Workspace</p>
        </div>
        <div className="rl-gallery__header-badge">
          <Badge tone="success" isDot>Roast Ledger Contract Locked</Badge>
        </div>
      </header>

      {/* 1. Tokens Section */}
      <section className="rl-gallery__section">
        <h2 className="rl-gallery__section-title">01. Color Palette Tokens</h2>
        <div className="rl-gallery__palette-grid">
          {[
            { name: 'Char', hex: '#1C2220', role: 'Primary text, dark surface' },
            { name: 'Porcelain', hex: '#F3F6F4', role: 'App canvas, light surface' },
            { name: 'Bottle Green', hex: '#246B58', role: 'Primary action, success' },
            { name: 'Roast Amber', hex: '#D88A2D', role: 'Warning, waiting, offline' },
            { name: 'Coffee Cherry', hex: '#B6404B', role: 'Destructive, overdue chit' },
            { name: 'Steam Blue', hex: '#DDE9E7', role: 'Selection, secondary surface' }
          ].map(c => (
            <div key={c.name} className="rl-gallery__swatch-card">
              <div
                className="rl-gallery__swatch-color"
                style={{ backgroundColor: c.hex, border: c.name === 'Porcelain' ? '1px solid #D9E2DE' : 'none' }}
              />
              <div className="rl-gallery__swatch-info">
                <strong>{c.name}</strong>
                <code>{c.hex}</code>
                <span>{c.role}</span>
              </div>
            </div>
          ))}
        </div>
      </section>

      {/* 2. Buttons Section */}
      <section className="rl-gallery__section">
        <h2 className="rl-gallery__section-title">02. Buttons & Touch Targets</h2>
        <div className="rl-gallery__row">
          <Button variant="primary">Primary (Bottle Green)</Button>
          <Button variant="secondary">Secondary (Steam Blue)</Button>
          <Button variant="destructive">Destructive (Cherry)</Button>
          <Button variant="outline">Outline</Button>
          <Button variant="ghost">Ghost</Button>
          <Button variant="primary" isLoading>Processing</Button>
          <Button variant="primary" disabled>Disabled</Button>
        </div>
        <div className="rl-gallery__row" style={{ marginTop: '16px' }}>
          <Button variant="primary" size="sm">Small (36px)</Button>
          <Button variant="primary" size="md">Medium Floor (44px)</Button>
          <Button variant="primary" size="lg">Large (48px)</Button>
          <Button variant="primary" size="pos-touch">POS Cashier Touch (48px)</Button>
        </div>
      </section>

      {/* 3. Badges & Status Pills */}
      <section className="rl-gallery__section">
        <h2 className="rl-gallery__section-title">03. Badges & Status Indicators</h2>
        <div className="rl-gallery__row">
          <Badge tone="neutral">Neutral</Badge>
          <Badge tone="success" isDot>Ready for Pickup</Badge>
          <Badge tone="warning" isDot>Preparing (Rush)</Badge>
          <Badge tone="danger" isDot>Overdue +12m</Badge>
          <Badge tone="info">Dine-In #12</Badge>
          <Badge tone="syncing" isDot>Syncing Outbox</Badge>
        </div>
      </section>

      {/* 4. MoneyDisplay Section */}
      <section className="rl-gallery__section">
        <h2 className="rl-gallery__section-title">04. Tabular Monospace Money (MYR)</h2>
        <div className="rl-gallery__row" style={{ alignItems: 'baseline' }}>
          <div>
            <span className="rl-gallery__label">Small:</span>
            <MoneyDisplay amount={8.5} size="sm" />
          </div>
          <div>
            <span className="rl-gallery__label">Standard (Line Item):</span>
            <MoneyDisplay amount={24.5} size="md" />
          </div>
          <div>
            <span className="rl-gallery__label">Large (Subtotal):</span>
            <MoneyDisplay amount={148.0} size="lg" />
          </div>
          <div>
            <span className="rl-gallery__label">Hero (Checkout Total):</span>
            <MoneyDisplay amount={1284.9} size="hero" />
          </div>
          <div>
            <span className="rl-gallery__label">Negative (Refund):</span>
            <MoneyDisplay amount={-35.0} size="lg" />
          </div>
        </div>
      </section>

      {/* 5. Signature Component: Order Chit */}
      <section className="rl-gallery__section">
        <h2 className="rl-gallery__section-title">05. Signature Element: Order Rail Chit</h2>
        <p className="rl-gallery__desc">Functional kitchen chits with order sequence, elapsed timer, status notch, and one-touch progression.</p>
        <div className="rl-gallery__chit-grid">
          <OrderChit
            orderNumber="#104"
            status={chitStatus}
            diningOption="DineIn"
            elapsedSeconds={254}
            tableOrCustomer="Table 4 • Sarah"
            items={[
              { id: '1', name: 'Oat Flat White', quantity: 2, modifiers: ['Double Shot', 'Less Sweet (50%)'] },
              { id: '2', name: 'Almond Croissant', quantity: 1, modifiers: ['Warm Up'] }
            ]}
            notes="Serve croissant first"
            onAdvanceStatus={(next: ChitStatus) => setChitStatus(next)}
          />

          <OrderChit
            orderNumber="#105"
            status="Preparing"
            diningOption="Takeaway"
            elapsedSeconds={412}
            tableOrCustomer="Takeaway • John"
            items={[
              { id: '3', name: 'Iced Spanish Latte', quantity: 1, modifiers: ['Soy Milk', 'Extra Ice'] },
              { id: '4', name: 'Pour Over (Ethiopia Guji)', quantity: 1, modifiers: ['Hot', 'V60'] }
            ]}
          />

          <OrderChit
            orderNumber="#102"
            status="Overdue"
            diningOption="DineIn"
            elapsedSeconds={890}
            tableOrCustomer="Table 8 • VIP"
            items={[
              { id: '5', name: 'Matcha Espresso Fusion', quantity: 3, modifiers: ['Oat Milk'] }
            ]}
            notes="Customer waiting at counter"
            onAdvanceStatus={() => {}}
          />
        </div>
      </section>

      {/* 6. Form Inputs & PIN Pad */}
      <section className="rl-gallery__section">
        <h2 className="rl-gallery__section-title">06. Inputs & Touch PIN Keypad</h2>
        <div className="rl-gallery__interactive-split">
          <div className="rl-gallery__form-column">
            <TextInput label="Product Name" placeholder="e.g. Colombian Supremo" />
            <TextInput label="Barcode / SKU" placeholder="Scan or enter SKU" leftAddon="🔍" />
            <TextInput label="Price (RM)" placeholder="18.00" leftAddon="RM" helperText="Inclusive of 6% SST" />
            <TextInput label="Customer Phone" placeholder="012-3456789" error="Invalid Malaysian phone number format" />
            <Button variant="secondary" onClick={() => setIsModalOpen(true)}>Open Manager Override Modal</Button>
          </div>

          <div className="rl-gallery__pinpad-column">
            <PinPad
              title="Staff Quick-Switch"
              subtitle="Punch 4-digit PIN (Try 1234 or 5678)"
              error={pinError}
              onComplete={handlePinComplete}
            />
            <div className="rl-gallery__pin-result" role="status">
              <strong>Status:</strong> {pinStatus}
            </div>
          </div>
        </div>
      </section>

      {/* 7. Operational State Views */}
      <section className="rl-gallery__section">
        <h2 className="rl-gallery__section-title">07. Operational State Views (Zero Fluff)</h2>
        <div className="rl-gallery__state-grid">
          <StateView
            kind="empty"
            title="No Active Orders"
            description="The kitchen order rail is clear. New paid orders will appear here automatically."
            actionLabel="Take New Order"
            onAction={() => {}}
          />
          <StateView
            kind="offline"
            title="Offline Mode Active"
            description="Cash sales and open tickets continue to work locally. 3 sales queued in outbox."
            actionLabel="Check Connection"
            onAction={() => {}}
          />
          <StateView
            kind="review-required"
            title="Manager Review Required"
            description="Void request exceeds cashier limit (RM 50.00). Manager PIN authorization needed."
            actionLabel="Authorize Override"
            onAction={() => setIsModalOpen(true)}
          />
        </div>
      </section>

      {/* 8. 80mm Thermal Receipt View */}
      <section className="rl-gallery__section">
        <h2 className="rl-gallery__section-title">08. 80mm ESC/POS & Digital Receipt View</h2>
        <p className="rl-gallery__desc">Faithful 80mm thermal receipt rendering with Malaysian SST breakdown, itemized modifiers, and digital verification QR.</p>
        <ReceiptView
          receiptNumber="RCP-20260819-1004"
          orderNumber="#104"
          cashierName="Ahmad Cashier"
          diningOption="Dine-In"
          tableOrCustomer="Table 4 • Sarah"
          items={[
            { name: 'Oat Flat White', quantity: 2, unitPrice: 14.5, lineTotal: 29.0, modifiers: ['Double Shot', 'Less Sweet (50%)'] },
            { name: 'Almond Croissant', quantity: 1, unitPrice: 12.0, lineTotal: 12.0, modifiers: ['Warm Up'] }
          ]}
          subtotal={41.0}
          taxes={[{ taxName: 'SST (6%)', taxAmount: 2.46 }]}
          grandTotal={43.46}
          paymentMethod="Cash"
          totalPaid={50.0}
          changeGiven={6.54}
          qrCodeUrl="https://verify.artisanroast.com/rcp/RCP-20260819-1004"
        />
      </section>

      {/* Modal Demonstration */}
      <Modal
        isOpen={isModalOpen}
        title="Manager Approval Required"
        onClose={() => setIsModalOpen(false)}
        footer={
          <>
            <Button variant="outline" onClick={() => setIsModalOpen(false)}>Cancel</Button>
            <Button variant="destructive" onClick={() => setIsModalOpen(false)}>Confirm Void</Button>
          </>
        }
      >
        <p>This action requires manager permission. The item <strong>Pour Over V60 (RM 24.00)</strong> will be voided before sale completion.</p>
        <TextInput label="Reason for Void" placeholder="e.g. Customer changed mind / wrong bean" />
      </Modal>

    </div>
  );
};
