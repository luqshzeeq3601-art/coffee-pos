import React from 'react';
import { MoneyDisplay } from './MoneyDisplay';
import { Badge } from './Badge';
import './ReceiptView.css';

export interface ReceiptItem {
  name: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
  modifiers?: string[];
}

export interface ReceiptTaxLine {
  taxName: string;
  taxAmount: number;
}

export interface ReceiptViewProps {
  merchantName?: string;
  ssmNumber?: string;
  sstNumber?: string;
  outletName?: string;
  outletAddress?: string;
  outletPhone?: string;
  receiptNumber: string;
  orderNumber: string;
  cashierName: string;
  transactionTime?: string;
  diningOption: string;
  tableOrCustomer?: string;
  items: ReceiptItem[];
  subtotal: number;
  discountsTotal?: number;
  taxes: ReceiptTaxLine[];
  grandTotal: number;
  paymentMethod: string;
  totalPaid: number;
  changeGiven?: number;
  qrCodeUrl?: string;
  footerNotes?: string;
  className?: string;
}

export const ReceiptView: React.FC<ReceiptViewProps> = ({
  merchantName = 'ARTISAN ROAST CO.',
  ssmNumber = 'SSM: 202401012345 (1234567-A)',
  sstNumber = 'SST: W10-2024-001234',
  outletName = 'Bangsar Flagship',
  outletAddress = 'No. 12, Jalan Telawi 3, Bangsar, 59100 KL',
  outletPhone = '+60 3-2287 1234',
  receiptNumber,
  orderNumber,
  cashierName,
  transactionTime = '2026-08-19 15:45',
  diningOption,
  tableOrCustomer,
  items,
  subtotal,
  discountsTotal = 0,
  taxes,
  grandTotal,
  paymentMethod,
  totalPaid,
  changeGiven = 0,
  qrCodeUrl,
  footerNotes = 'Thank you for brewing with us!',
  className = ''
}) => {
  return (
    <div className={`rl-receipt-view ${className}`}>
      {/* Receipt Top Perforation */}
      <div className="rl-receipt-view__perforation" />

      <div className="rl-receipt-view__content">
        {/* Header */}
        <header className="rl-receipt-view__header">
          <span className="rl-receipt-view__logo">☕</span>
          <h2 className="rl-receipt-view__merchant">{merchantName}</h2>
          <span className="rl-receipt-view__sub">{ssmNumber}</span>
          <span className="rl-receipt-view__sub">{sstNumber}</span>
          <span className="rl-receipt-view__outlet">{outletName}</span>
          <span className="rl-receipt-view__address">{outletAddress}</span>
          <span className="rl-receipt-view__phone">Tel: {outletPhone}</span>
        </header>

        <div className="rl-receipt-view__divider" />

        {/* Metadata */}
        <div className="rl-receipt-view__meta">
          <div className="rl-receipt-view__meta-row">
            <span>Receipt: {receiptNumber}</span>
            <span>Order: {orderNumber}</span>
          </div>
          <div className="rl-receipt-view__meta-row">
            <span>Date: {transactionTime}</span>
            <Badge tone="neutral">{diningOption}</Badge>
          </div>
          <div className="rl-receipt-view__meta-row">
            <span>Cashier: {cashierName}</span>
            {tableOrCustomer && <strong>Ref: {tableOrCustomer}</strong>}
          </div>
        </div>

        <div className="rl-receipt-view__divider" />

        {/* Items Table */}
        <div className="rl-receipt-view__items">
          <div className="rl-receipt-view__items-header">
            <span>Item</span>
            <span style={{ textAlign: 'right' }}>Total (RM)</span>
          </div>
          {items.map((item, idx) => (
            <div key={idx} className="rl-receipt-view__item">
              <div className="rl-receipt-view__item-main">
                <span>{item.quantity}x {item.name}</span>
                <MoneyDisplay amount={item.lineTotal} size="sm" />
              </div>
              {item.modifiers && item.modifiers.length > 0 && (
                <ul className="rl-receipt-view__item-modifiers">
                  {item.modifiers.map((m, mIdx) => (
                    <li key={mIdx}>↳ {m}</li>
                  ))}
                </ul>
              )}
            </div>
          ))}
        </div>

        <div className="rl-receipt-view__divider" />

        {/* Financial Breakdown */}
        <div className="rl-receipt-view__totals">
          <div className="rl-receipt-view__row">
            <span>Subtotal</span>
            <MoneyDisplay amount={subtotal} size="sm" />
          </div>
          {discountsTotal > 0 && (
            <div className="rl-receipt-view__row">
              <span>Discount</span>
              <MoneyDisplay amount={-discountsTotal} size="sm" />
            </div>
          )}
          {taxes.map((t, tIdx) => (
            <div key={tIdx} className="rl-receipt-view__row">
              <span>{t.taxName}</span>
              <MoneyDisplay amount={t.taxAmount} size="sm" />
            </div>
          ))}
          <div className="rl-receipt-view__row rl-receipt-view__row--grand">
            <span>TOTAL</span>
            <MoneyDisplay amount={grandTotal} size="md" />
          </div>
          <div className="rl-receipt-view__row">
            <span>Paid ({paymentMethod})</span>
            <MoneyDisplay amount={totalPaid} size="sm" />
          </div>
          {changeGiven > 0 && (
            <div className="rl-receipt-view__row rl-receipt-view__row--change">
              <span>Change Given</span>
              <MoneyDisplay amount={changeGiven} size="sm" />
            </div>
          )}
        </div>

        <div className="rl-receipt-view__divider" />

        {/* Footer & QR Verification */}
        <footer className="rl-receipt-view__footer">
          {qrCodeUrl && (
            <div className="rl-receipt-view__qr-box">
              <span className="rl-receipt-view__qr-mock">📱 [ DIGITAL VERIFICATION QR ]</span>
            </div>
          )}
          <p className="rl-receipt-view__notes">{footerNotes}</p>
        </footer>
      </div>

      {/* Receipt Bottom Perforation */}
      <div className="rl-receipt-view__perforation rl-receipt-view__perforation--bottom" />
    </div>
  );
};
