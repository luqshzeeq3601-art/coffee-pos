export type PrepStation = 'EspressoBar' | 'FilterBar' | 'PastryKitchen' | 'All';
export type ChitStatus = 'Queued' | 'Preparing' | 'Ready' | 'Completed' | 'Recalled';

export interface KitchenChitItemDto {
  id: string;
  productId: string;
  productName: string;
  variantName?: string;
  modifiersSummary?: string;
  notes?: string;
  quantity: number;
  isPrepared: boolean;
}

export interface KitchenChitDto {
  id: string;
  tenantId: string;
  outletId: string;
  orderId: string;
  orderNumber: string;
  diningOption: 'DineIn' | 'Takeaway' | 'Delivery';
  customerName?: string;
  tableNumber?: string;
  station: PrepStation;
  status: ChitStatus;
  elapsedSeconds: number;
  createdAtUtc: string;
  startedAtUtc?: string;
  completedAtUtc?: string;
  items: KitchenChitItemDto[];
}

export interface BumpChitRequest {
  nextStatus?: ChitStatus;
}

export interface RecallChitRequest {
  targetStatus?: ChitStatus;
}
