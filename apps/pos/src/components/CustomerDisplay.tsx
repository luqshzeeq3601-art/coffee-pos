import React from 'react';
import type { CartItem } from './PosShell';
import { BotanicalSprig, PosIcon, RoastMark } from './PosIcon';
import './CustomerDisplay.css';

export interface CustomerDisplayProps {
  isOpen: boolean;
  onClose: () => void;
  cart: CartItem[];
  subtotal: number;
  sstTax: number;
  grandTotal: number;
  outletName: string;
  orderNumber: string;
}

const money = (value: number) => 'RM ' + value.toLocaleString('en-MY', { minimumFractionDigits: 2, maximumFractionDigits: 2 });

const QrPattern: React.FC<{ size?: number }> = ({ size = 96 }) => (
  <svg className="rl-cds-qr-pattern" width={size} height={size} viewBox="0 0 100 100" aria-hidden="true" focusable="false">
    <g fill="none" stroke="currentColor" strokeWidth="5" strokeLinecap="round" strokeLinejoin="round">
      <path d="M10 36V12h24M64 12h24v24M88 64v24H64M36 88H12V64" />
    </g>
    <g fill="currentColor">
      <rect x="20" y="20" width="11" height="11" rx="1" />
      <rect x="69" y="20" width="11" height="11" rx="1" />
      <rect x="20" y="69" width="11" height="11" rx="1" />
      <rect x="46" y="20" width="6" height="6" rx="1" />
      <rect x="42" y="34" width="8" height="8" rx="1" />
      <rect x="55" y="38" width="6" height="6" rx="1" />
      <rect x="42" y="53" width="6" height="6" rx="1" />
      <rect x="53" y="50" width="11" height="11" rx="1" />
      <rect x="69" y="50" width="7" height="7" rx="1" />
      <rect x="35" y="68" width="7" height="7" rx="1" />
      <rect x="48" y="70" width="8" height="8" rx="1" />
      <rect x="62" y="67" width="6" height="6" rx="1" />
      <rect x="76" y="76" width="5" height="5" rx="1" />
    </g>
  </svg>
);

const CoffeeCupArtwork: React.FC = () => (
  <div className="rl-cds-coffee-art" aria-hidden="true">
    <BotanicalSprig className="rl-cds-coffee-art__sprig" />
    <span className="rl-cds-coffee-art__cup" />
  </div>
);

export const CustomerDisplay: React.FC<CustomerDisplayProps> = ({
  isOpen,
  onClose,
  cart,
  subtotal,
  sstTax,
  grandTotal,
  outletName,
  orderNumber
}) => {
  if (!isOpen) return null;

  return (
    <div className="rl-cds-overlay" role="dialog" aria-modal="true" aria-label="Customer Facing Display">
      <div className="rl-cds-container">
        <section className="rl-cds-items-pane" aria-label="Current order">
          <header className="rl-cds-header">
            <div className="rl-cds-brand">
              <RoastMark size={67} className="rl-cds-brand__mark" />
              <div>
                <h1>Artisan Roast Co.</h1>
                <p><span>{outletName}</span><b>•</b><span>Order {orderNumber}</span></p>
              </div>
            </div>
            <button type="button" className="rl-cds-close-btn" onClick={onClose} aria-label="Close Customer Display" title="Close Customer Display">
              <PosIcon name="close" size={27} strokeWidth={1.55} />
            </button>
            <BotanicalSprig className="rl-cds-header__sprig" />
          </header>

          <div className={'rl-cds-items-list ' + (cart.length === 0 ? 'rl-cds-items-list--empty' : '')}>
            {cart.length === 0 ? (
              <div className="rl-cds-empty" role="status">
                <CoffeeCupArtwork />
                <div className="rl-cds-welcome-heading"><span /> <h2>Welcome!</h2> <span /></div>
                <p>Your order will appear here as the barista rings it up.</p>
              </div>
            ) : (
              <div className="rl-cds-order-list">
                <div className="rl-cds-order-list__heading">Your order</div>
                {cart.map(item => (
                  <div key={item.id} className="rl-cds-item-row">
                    <div className="rl-cds-item-quantity">{item.quantity}×</div>
                    <div className="rl-cds-item-info">
                      <strong>{item.name}</strong>
                      {item.selectedModifiers.length > 0 && <span>{item.selectedModifiers.join(' · ')}</span>}
                    </div>
                    <strong className="rl-cds-item-price">{money(item.unitPrice * item.quantity)}</strong>
                  </div>
                ))}
              </div>
            )}
          </div>
          <BotanicalSprig className="rl-cds-items-pane__sprig" />
        </section>

        <aside className="rl-cds-payment-pane" aria-label="Payment summary">
          <div className="rl-cds-summary-box">
            <div className="rl-cds-summary-row"><span>Subtotal</span><strong>{money(subtotal)}</strong></div>
            <div className="rl-cds-summary-row"><span>SST (6%)</span><strong>{money(sstTax)}</strong></div>
            <div className="rl-cds-summary-divider" />
            <div className="rl-cds-total-row"><span>Total Payable</span><strong>{money(grandTotal)}</strong></div>
          </div>

          <section className="rl-cds-qr-box" aria-label="DuitNow QR instant scan payment">
            <div className="rl-cds-qr-heading"><span /> <div><QrPattern size={18} /> <strong>DUITNOW QR INSTANT SCAN</strong></div> <span /></div>
            <div className="rl-cds-qr-card">
              <QrPattern size={91} />
              <h2>SCAN TO PAY WITH<br />DUITNOW / MAYBANK / TNG</h2>
              <div className="rl-cds-qr-rule"><i /><RoastMark size={25} /><i /></div>
              <p>Scan with any Malaysian banking<br />or e-Wallet app to pay <strong>{money(grandTotal)}</strong></p>
            </div>
          </section>
        </aside>
      </div>
    </div>
  );
};
