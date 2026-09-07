export type TransferStatus = 'Draft' | 'Dispatched' | 'Received' | 'Cancelled';

export interface StockTransferItemDto {
  id: string;
  stockItemId: string;
  stockItemName: string;
  sku: string;
  quantity: number;
  unit: string;
}

export interface StockTransferDto {
  id: string;
  tenantId: string;
  transferNumber: string;
  sourceOutletId: string;
  sourceOutletName: string;
  destinationOutletId: string;
  destinationOutletName: string;
  status: TransferStatus;
  items: StockTransferItemDto[];
  dispatchedByName?: string;
  dispatchedAtUtc?: string;
  receivedByName?: string;
  receivedAtUtc?: string;
  notes?: string;
  createdAtUtc: string;
}

export interface CreateStockTransferRequest {
  sourceOutletId: string;
  destinationOutletId: string;
  items: {
    stockItemId: string;
    quantity: number;
  }[];
  notes?: string;
}

export interface DispatchStockTransferRequest {
  notes?: string;
}

export interface ReceiveStockTransferRequest {
  notes?: string;
}
