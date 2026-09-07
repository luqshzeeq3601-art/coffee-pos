import type { WasteReason } from '@coffee-pos/contracts';

export type StaffRole = 'Owner' | 'Manager' | 'Cashier' | 'Barista';

export interface OutletItem {
  id: string;
  name: string;
  address: string;
  isMainOutlet: boolean;
  isActive: boolean;
}

export interface StaffItem {
  id: string;
  name: string;
  email: string;
  role: StaffRole;
  assignedOutletIds: string[];
  hasPinSet: boolean;
  isActive: boolean;
}

export interface DeviceItem {
  id: string;
  name: string;
  outletId: string;
  deviceType: 'POS' | 'KDS';
  status: 'Enrolled' | 'PendingEnrollment';
  enrollmentCode?: string;
  lastSeen: string;
  isActive: boolean;
}

export interface AdminStockItem {
  id: string;
  sku: string;
  name: string;
  category: string;
  unit: string;
  currentStock: number;
  reorderThreshold: number;
  costPerUnit: number;
  isActive: boolean;
}

export interface AdminWastageItem {
  id: string;
  stockItemId: string;
  stockItemName: string;
  quantityWasted: number;
  unit: string;
  reason: WasteReason;
  costImpact: number;
  notes?: string;
  loggedByName: string;
  time: string;
}

export interface AdminMyInvoisItem {
  id: string;
  invoiceNumber: string;
  uuid: string;
  buyerName: string;
  buyerTin: string;
  totalExcludingTax: number;
  totalTaxAmount: number;
  totalPayable: number;
  status: 'Valid' | 'Submitted' | 'Invalid' | 'Cancelled';
  validatedAt: string;
}

export interface PermissionRow {
  permission: string;
  owner: boolean;
  manager: boolean;
  cashier: boolean;
  barista: boolean;
}

export const INITIAL_OUTLETS: OutletItem[] = [
  {
    id: 'out-bangsar',
    name: 'Bangsar Flagship',
    address: 'No. 12, Jalan Telawi 3, Bangsar, 59100 KL',
    isMainOutlet: true,
    isActive: true
  },
  {
    id: 'out-damansara',
    name: 'Damansara Heights',
    address: 'Lot 4G, Plaza Damansara, Bukit Damansara, 50490 KL',
    isMainOutlet: false,
    isActive: true
  }
];

export const INITIAL_STAFF: StaffItem[] = [
  {
    id: 'usr-1',
    name: 'Azman Owner',
    email: 'azman@artisanroast.my',
    role: 'Owner',
    assignedOutletIds: ['all'],
    hasPinSet: true,
    isActive: true
  },
  {
    id: 'usr-2',
    name: 'Nurul Manager',
    email: 'nurul@artisanroast.my',
    role: 'Manager',
    assignedOutletIds: ['out-bangsar'],
    hasPinSet: true,
    isActive: true
  },
  {
    id: 'usr-3',
    name: 'Ahmad Cashier',
    email: 'ahmad@artisanroast.my',
    role: 'Cashier',
    assignedOutletIds: ['out-bangsar'],
    hasPinSet: true,
    isActive: true
  },
  {
    id: 'usr-4',
    name: 'Siti Barista',
    email: 'siti@artisanroast.my',
    role: 'Barista',
    assignedOutletIds: ['out-bangsar'],
    hasPinSet: true,
    isActive: true
  }
];

export const INITIAL_DEVICES: DeviceItem[] = [
  {
    id: 'dev-1',
    name: 'Counter 1 POS (Main Register)',
    outletId: 'out-bangsar',
    deviceType: 'POS',
    status: 'Enrolled',
    lastSeen: 'Active now',
    isActive: true
  },
  {
    id: 'dev-2',
    name: 'Espresso Bar KDS',
    outletId: 'out-bangsar',
    deviceType: 'KDS',
    status: 'Enrolled',
    lastSeen: 'Active now',
    isActive: true
  },
  {
    id: 'dev-3',
    name: 'Filter & Pastry KDS',
    outletId: 'out-bangsar',
    deviceType: 'KDS',
    status: 'Enrolled',
    lastSeen: 'Active now',
    isActive: true
  }
];

export const INITIAL_STOCK_ITEMS: AdminStockItem[] = [
  { id: 'stk-1', sku: 'BEAN-HOUSE-01', name: 'Artisan Espresso Blend Beans', category: 'Coffee Beans', unit: 'Grams', currentStock: 12500, reorderThreshold: 2000, costPerUnit: 0.085, isActive: true },
  { id: 'stk-2', sku: 'BEAN-ETH-01', name: 'Ethiopia Guji Single Origin', category: 'Coffee Beans', unit: 'Grams', currentStock: 4500, reorderThreshold: 1000, costPerUnit: 0.120, isActive: true },
  { id: 'stk-3', sku: 'MILK-OAT-01', name: 'Oatly Barista Edition (1L)', category: 'Dairy & Alternatives', unit: 'Milliliters', currentStock: 24000, reorderThreshold: 5000, costPerUnit: 0.015, isActive: true },
  { id: 'stk-4', sku: 'MILK-FRESH-01', name: 'Farm Fresh Whole Milk (1L)', category: 'Dairy & Alternatives', unit: 'Milliliters', currentStock: 30000, reorderThreshold: 6000, costPerUnit: 0.008, isActive: true },
  { id: 'stk-5', sku: 'PSTR-CROIS-01', name: 'Almond Croissant', category: 'Bakery', unit: 'Pieces', currentStock: 18, reorderThreshold: 5, costPerUnit: 6.50, isActive: true }
];

export const INITIAL_WASTAGE_ENTRIES: AdminWastageItem[] = [
  {
    id: 'wst-1',
    stockItemId: 'stk-1',
    stockItemName: 'Artisan Espresso Blend Beans',
    quantityWasted: 150,
    unit: 'Grams',
    reason: 'CalibrationDialIn',
    costImpact: 12.75,
    notes: 'Morning grinder dial-in & extraction calibration',
    loggedByName: 'Ahmad Barista',
    time: '08:15 AM'
  }
];

export const INITIAL_MYINVOIS_DOCUMENTS: AdminMyInvoisItem[] = [
  {
    id: 'inv-1',
    invoiceNumber: 'INV-2026-00101',
    uuid: 'LHDN-UUID-20260819-A1B2C3D4',
    buyerName: 'Petronas Digital Sdn Bhd',
    buyerTin: 'C2584563200',
    totalExcludingTax: 240.00,
    totalTaxAmount: 14.40,
    totalPayable: 254.40,
    status: 'Valid',
    validatedAt: '19 Aug 2026, 14:15'
  },
  {
    id: 'inv-2',
    invoiceNumber: 'INV-2026-00102',
    uuid: 'LHDN-UUID-20260819-E5F6G7H8',
    buyerName: 'Muhammad Danial',
    buyerTin: 'IG3456789010',
    totalExcludingTax: 55.00,
    totalTaxAmount: 3.30,
    totalPayable: 58.30,
    status: 'Submitted',
    validatedAt: '19 Aug 2026, 15:42'
  }
];

export const PERMISSION_MATRIX: PermissionRow[] = [
  { permission: 'Process Sales & Tender Cash', owner: true, manager: true, cashier: true, barista: false },
  { permission: 'Hold & Resume Open Tickets', owner: true, manager: true, cashier: true, barista: false },
  { permission: 'View Kitchen Queue (KDS) & Bump', owner: true, manager: true, cashier: true, barista: true },
  { permission: 'Open & Close Shifts (Z-Report)', owner: true, manager: true, cashier: true, barista: false },
  { permission: 'Authorize Line Item & Ticket Voids', owner: true, manager: true, cashier: false, barista: false },
  { permission: 'Manage Catalog, Prices & Modifiers', owner: true, manager: true, cashier: false, barista: false },
  { permission: 'Receive Supplier Stock & Log Wastage', owner: true, manager: true, cashier: false, barista: false },
  { permission: 'Outlet & Terminal Device Management', owner: true, manager: false, cashier: false, barista: false },
  { permission: 'Staff Onboarding & Access Control', owner: true, manager: false, cashier: false, barista: false }
];
