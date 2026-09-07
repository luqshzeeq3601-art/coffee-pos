export type MyInvoisStatus = 'Draft' | 'Submitted' | 'Valid' | 'Invalid' | 'Cancelled';

export interface BuyerDetailsDto {
  tin: string; // Tax Identification Number e.g. C2584563200
  idType: 'NRIC' | 'BRN' | 'PASSPORT' | 'ARMY';
  idValue: string;
  name: string;
  phoneNumber?: string;
  email?: string;
  address?: string;
}

export interface MyInvoisDocumentDto {
  id: string;
  tenantId: string;
  salesTransactionId: string;
  invoiceNumber: string;
  uuid?: string; // LHDN unique identifier
  longId?: string;
  status: MyInvoisStatus;
  buyer: BuyerDetailsDto;
  totalExcludingTax: number;
  totalTaxAmount: number;
  totalPayable: number;
  qrCodeUrl?: string;
  validationErrors?: string[];
  submittedAtUtc?: string;
  validatedAtUtc?: string;
  createdAtUtc: string;
}

export interface SubmitInvoiceRequest {
  salesTransactionId: string;
  invoiceNumber: string;
  buyer: BuyerDetailsDto;
  totalExcludingTax: number;
  totalTaxAmount: number;
  totalPayable: number;
}

export interface CancelInvoiceRequest {
  reason: string;
}
