export type PaymentMethod = 'Cash' | 'DuitNowQR' | 'CreditCard' | 'DebitCard' | 'Custom';
export type PaymentStatus = 'Pending' | 'Completed' | 'Failed' | 'Refunded';

export interface PaymentDto {
  id: string;
  transactionId: string;
  paymentMethod: PaymentMethod;
  amount: number;
  tenderedAmount?: number;
  changeAmount?: number;
  referenceCode?: string;
  status: PaymentStatus;
  createdAtUtc: string;
}

export interface RefundDto {
  id: string;
  transactionId: string;
  amount: number;
  reason: string;
  approvedByUserId: string;
  approvedByUserName: string;
  createdAtUtc: string;
}

export interface SalesTransactionDto {
  id: string;
  tenantId: string;
  outletId: string;
  orderId: string;
  orderNumber: string;
  receiptNumber: string;
  cashierId: string;
  cashierName: string;
  subtotal: number;
  discountTotal: number;
  taxTotal: number; // 6% SST
  grandTotal: number;
  paidAmount: number;
  changeAmount: number;
  payments: PaymentDto[];
  refunds: RefundDto[];
  createdAtUtc: string;
}

export interface ProcessPaymentRequest {
  orderId: string;
  idempotencyKey: string;
  paymentMethod: PaymentMethod;
  amount: number;
  tenderedAmount?: number;
  referenceCode?: string;
}

export interface SplitTenderItem {
  paymentMethod: PaymentMethod;
  amount: number;
  tenderedAmount?: number;
  referenceCode?: string;
}

export interface ProcessSplitPaymentRequest {
  orderId: string;
  idempotencyKey: string;
  tenders: SplitTenderItem[];
}

export interface ProcessRefundRequest {
  transactionId: string;
  amount: number;
  reason: string;
  managerPin: string;
}
