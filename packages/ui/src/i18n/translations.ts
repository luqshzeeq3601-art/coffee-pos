export type LanguageCode = 'en' | 'ms';

export interface TranslationDictionary {
  appName: string;
  register: string;
  tickets: string;
  shifts: string;
  inventory: string;
  reports: string;
  myinvois: string;
  dineIn: string;
  takeaway: string;
  subtotal: string;
  total: string;
  charge: string;
  void: string;
  hold: string;
  payWithCash: string;
  payWithQr: string;
  payWithCard: string;
  change: string;
  tendered: string;
  exact: string;
  addCustomer: string;
  memberPoints: string;
  redeemDiscount: string;
  onlineActive: string;
  offlineMode: string;
  syncNow: string;
  kitchenChits: string;
  bumpOrder: string;
  recallTicket: string;
  elapsedTime: string;
}

export const TRANSLATIONS: Record<LanguageCode, TranslationDictionary> = {
  en: {
    appName: 'Roast Ledger POS',
    register: 'Register',
    tickets: 'Open Tickets',
    shifts: 'Shifts',
    inventory: 'Inventory & Stock',
    reports: 'Sales Analytics',
    myinvois: 'MyInvois e-Invoice',
    dineIn: 'Dine-In',
    takeaway: 'Takeaway',
    subtotal: 'Subtotal',
    total: 'Total',
    charge: 'Charge',
    void: 'Void',
    hold: 'Hold',
    payWithCash: '💵 Cash',
    payWithQr: '📱 DuitNow QR',
    payWithCard: '💳 Card',
    change: 'Change to Return',
    tendered: 'Tendered',
    exact: 'Exact',
    addCustomer: '+ Add Customer',
    memberPoints: 'Points',
    redeemDiscount: 'Redeem 100 pts (RM 10 off)',
    onlineActive: 'Online • Sync Active',
    offlineMode: 'Offline Mode',
    syncNow: 'Force Sync Now',
    kitchenChits: 'Kitchen Order Rails',
    bumpOrder: 'Mark as Ready',
    recallTicket: 'Recall Bumped Ticket',
    elapsedTime: 'Elapsed'
  },
  ms: {
    appName: 'Roast Ledger POS',
    register: 'Daftar Tunai',
    tickets: 'Tiket Terbuka',
    shifts: 'Syif Tunai',
    inventory: 'Inventori & Stok',
    reports: 'Analitik Jualan',
    myinvois: 'e-Invois MyInvois',
    dineIn: 'Makan Sini',
    takeaway: 'Bawa Pulang',
    subtotal: 'Jumlah Kecil',
    total: 'Jumlah Keseluruhan',
    charge: 'Bayar',
    void: 'Batal',
    hold: 'Simpan',
    payWithCash: '💵 Tunai',
    payWithQr: '📱 DuitNow QR',
    payWithCard: '💳 Kad',
    change: 'Baki Dikembalikan',
    tendered: 'Diterima',
    exact: 'Tepat',
    addCustomer: '+ Tambah Pelanggan',
    memberPoints: 'Mata Ganjaran',
    redeemDiscount: 'Tebus 100 mata (Diskaun RM 10)',
    onlineActive: 'Dalam Talian • Segerak Aktif',
    offlineMode: 'Mod Luar Talian',
    syncNow: 'Segerakkan Sekarang',
    kitchenChits: 'Rel Pesanan Dapur',
    bumpOrder: 'Tandakan Siap',
    recallTicket: 'Panggil Semula Tiket',
    elapsedTime: 'Masa Berlalu'
  }
};

export const getTranslation = (lang: LanguageCode = 'en'): TranslationDictionary => {
  return TRANSLATIONS[lang] || TRANSLATIONS.en;
};
