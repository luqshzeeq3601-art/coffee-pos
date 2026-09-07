export type ShiftStatus = 'Open' | 'Closed';
export type CashMovementType = 'CashIn' | 'CashOut';

export interface CashMovementDto {
  id: string;
  shiftId: string;
  type: CashMovementType;
  amount: number;
  reason: string;
  cashierId: string;
  cashierName: string;
  createdAtUtc: string;
}

export interface ShiftDto {
  id: string;
  tenantId: string;
  outletId: string;
  cashierId: string;
  cashierName: string;
  status: ShiftStatus;
  openedAtUtc: string;
  closedAtUtc?: string;
  openingFloat: number;
  cashSales: number;
  cashRefunds: number;
  cashInTotal: number;
  cashOutTotal: number;
  expectedCash: number;
  actualCountedCash?: number;
  variance?: number; // actual - expected
  movements: CashMovementDto[];
}

export interface OpenShiftRequest {
  openingFloat: number;
}

export interface CashMovementRequest {
  type: CashMovementType;
  amount: number;
  reason: string;
}

export interface CloseShiftRequest {
  actualCountedCash: number;
  closingNotes?: string;
}

export interface ShiftSummaryReportDto {
  shiftId: string;
  outletName: string;
  cashierName: string;
  openedAtUtc: string;
  closedAtUtc?: string;
  isClosed: boolean;
  openingFloat: number;
  cashSales: number;
  cardSales: number;
  qrSales: number;
  grossSales: number;
  discountsTotal: number;
  taxTotal: number; // 6% SST
  netSales: number;
  cashIn: number;
  cashOut: number;
  expectedCashInDrawer: number;
  actualCountedCash?: number;
  variance?: number;
}
