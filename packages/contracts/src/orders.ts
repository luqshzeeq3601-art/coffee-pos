export type OrderStatus = 'Draft' | 'Open' | 'Paid' | 'Cancelled';
export type DiningOption = 'DineIn' | 'Takeaway' | 'Delivery';

export interface OrderModifierDto {
  id: string;
  modifierId: string;
  name: string;
  priceDelta: number;
}

export interface OrderLineItemDto {
  id: string;
  productId: string;
  variantId?: string;
  name: string;
  unitPrice: number;
  quantity: number;
  modifiers: OrderModifierDto[];
  notes?: string;
  lineTotal: number;
}

export interface OrderDto {
  id: string;
  tenantId: string;
  outletId: string;
  orderNumber: string;
  status: OrderStatus;
  diningOption: DiningOption;
  tableOrCustomer?: string;
  cashierId?: string;
  cashierName?: string;
  items: OrderLineItemDto[];
  subtotal: number;
  discountTotal: number;
  taxTotal: number; // 6% SST
  grandTotal: number;
  notes?: string;
  createdAtUtc: string;
  updatedAtUtc: string;
}

export interface CreateOrderLineItemRequest {
  productId: string;
  variantId?: string;
  name: string;
  unitPrice: number;
  quantity: number;
  modifiers?: {
    modifierId: string;
    name: string;
    priceDelta: number;
  }[];
  notes?: string;
}

export interface CreateOrderRequest {
  diningOption: DiningOption;
  tableOrCustomer?: string;
  notes?: string;
  items: CreateOrderLineItemRequest[];
}

export interface HoldTicketRequest {
  tableOrCustomer: string;
  notes?: string;
}

export interface VoidOrderRequest {
  reason: string;
  managerPin?: string;
}
