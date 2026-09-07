export interface ReceiptLineItemDto {
  name: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
  modifiers?: string[];
}

export interface ReceiptTaxSummaryDto {
  taxName: string;
  ratePercent: number;
  taxableAmount: number;
  taxAmount: number;
}

export interface ReceiptPaymentDto {
  method: string;
  amount: number;
  referenceCode?: string;
}

export interface ReceiptDto {
  receiptNumber: string;
  orderNumber: string;
  merchantName: string;
  companyRegistrationNumber: string;
  sstRegistrationNumber: string;
  outletName: string;
  outletAddress: string;
  outletPhone: string;
  cashierName: string;
  terminalName: string;
  transactionTime: string;
  diningOption: string;
  tableOrCustomer?: string;
  items: ReceiptLineItemDto[];
  subtotal: number;
  discountsTotal: number;
  taxSummary: ReceiptTaxSummaryDto[];
  grandTotal: number;
  payments: ReceiptPaymentDto[];
  totalPaid: number;
  changeGiven: number;
  qrVerificationUrl?: string;
  footerNotes?: string;
}

export interface ReceiptPrintPayloadDto {
  receiptNumber: string;
  base64EscPosPayload: string;
  plainTextRepresentation: string;
  byteLength: number;
  containsDrawerKick: boolean;
}
