export interface HourlySalesPointDto {
  hour: number;
  label: string;
  orderCount: number;
  grossSales: number;
  netSales: number;
}

export interface ProductMixItemDto {
  productId: string;
  productName: string;
  category: string;
  unitsSold: number;
  totalRevenue: number;
  percentOfSales: number;
}

export interface PaymentBreakdownDto {
  method: string;
  transactionCount: number;
  totalAmount: number;
  percentage: number;
}

export interface SalesSummaryReportDto {
  tenantId: string;
  outletId: string;
  date: string;
  totalOrders: number;
  grossSales: number;
  discountsTotal: number;
  netSales: number;
  taxTotal: number;
  cashSales: number;
  cardSales: number;
  qrSales: number;
  averageOrderValue: number;
  hourlyVelocity: HourlySalesPointDto[];
  productMix: ProductMixItemDto[];
  paymentBreakdown: PaymentBreakdownDto[];
}
