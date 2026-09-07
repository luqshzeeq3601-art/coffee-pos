export type UnitOfMeasure = 'Grams' | 'Milliliters' | 'Pieces' | 'Kilograms' | 'Liters';
export type StockMovementType = 'SaleDepletion' | 'ReceiveStock' | 'ManualAdjustment' | 'WasteWritedown' | 'Transfer';
export type WasteReason = 'Spillage' | 'Expired' | 'CalibrationDialIn' | 'QualityDefect' | 'StaffTraining';
export type PurchaseOrderStatus = 'Draft' | 'Ordered' | 'Received' | 'Cancelled';

export interface StockLedgerEntryDto {
  id: string;
  tenantId: string;
  stockItemId: string;
  type: StockMovementType;
  quantityDelta: number;
  balanceAfter: number;
  reason: string;
  referenceId?: string;
  performedByName: string;
  createdAtUtc: string;
}

export interface StockItemDto {
  id: string;
  tenantId: string;
  outletId: string;
  sku: string;
  name: string;
  category: string;
  unit: UnitOfMeasure;
  currentStock: number;
  reorderThreshold: number;
  costPerUnit: number;
  isLowStock: boolean;
  lastUpdatedUtc: string;
}

export interface RecipeItemDto {
  id: string;
  stockItemId: string;
  stockItemName: string;
  unit: UnitOfMeasure;
  quantityRequired: number;
}

export interface RecipeDto {
  id: string;
  productId: string;
  variantId?: string;
  productName: string;
  variantName?: string;
  items: RecipeItemDto[];
}

export interface AdjustStockRequest {
  newQuantity: number;
  reason: string;
}

export interface ReceiveStockRequest {
  quantityReceived: number;
  costPerUnit?: number;
  supplierInvoiceNumber?: string;
  notes?: string;
}

export interface DepleteRecipeStockRequest {
  productId: string;
  variantId?: string;
  quantitySold: number;
  orderNumber: string;
}

export interface StockWastageDto {
  id: string;
  tenantId: string;
  outletId: string;
  stockItemId: string;
  stockItemName: string;
  quantityWasted: number;
  unit: UnitOfMeasure;
  reason: WasteReason;
  costImpact: number;
  notes?: string;
  loggedByName: string;
  createdAtUtc: string;
}

export interface RecordWastageRequest {
  stockItemId: string;
  quantityWasted: number;
  reason: WasteReason;
  notes?: string;
}

export interface PurchaseOrderItemDto {
  id: string;
  stockItemId: string;
  stockItemName: string;
  unit: UnitOfMeasure;
  quantityOrdered: number;
  quantityReceived: number;
  unitCost: number;
  lineTotal: number;
}

export interface PurchaseOrderDto {
  id: string;
  tenantId: string;
  outletId: string;
  poNumber: string;
  supplierName: string;
  status: PurchaseOrderStatus;
  totalCost: number;
  notes?: string;
  createdAtUtc: string;
  receivedAtUtc?: string;
  items: PurchaseOrderItemDto[];
}

export interface CreatePurchaseOrderItemRequest {
  stockItemId: string;
  quantityOrdered: number;
  unitCost: number;
}

export interface CreatePurchaseOrderRequest {
  supplierName: string;
  notes?: string;
  items: CreatePurchaseOrderItemRequest[];
}
