import { ChitItem, ChitStatus, DiningOption } from '@coffee-pos/ui';

export interface KdsOrder {
  id: string;
  orderNumber: string;
  station: 'Espresso' | 'Filter' | 'Food' | 'All';
  status: ChitStatus;
  diningOption: DiningOption;
  elapsedSeconds: number;
  tableOrCustomer: string;
  items: ChitItem[];
  notes?: string;
  createdAt: string;
}

export const INITIAL_KDS_ORDERS: KdsOrder[] = [
  {
    id: 'kds-ord-104',
    orderNumber: '#104',
    station: 'Espresso',
    status: 'New',
    diningOption: 'DineIn',
    elapsedSeconds: 145,
    tableOrCustomer: 'Table 4 • Sarah',
    items: [
      {
        id: 'kds-item-1',
        name: 'Oat Flat White',
        quantity: 2,
        modifiers: ['Double Shot', 'Less Sweet (50%)']
      },
      {
        id: 'kds-item-2',
        name: 'Iced Spanish Latte',
        quantity: 1,
        modifiers: ['Soy Milk', 'Extra Ice']
      }
    ],
    notes: 'Serve oat flat white extra hot',
    createdAt: '15:40:12'
  },
  {
    id: 'kds-ord-105',
    orderNumber: '#105',
    station: 'Filter',
    status: 'Preparing',
    diningOption: 'Takeaway',
    elapsedSeconds: 380,
    tableOrCustomer: 'Takeaway • John',
    items: [
      {
        id: 'kds-item-3',
        name: 'Pour Over (Ethiopia Guji)',
        quantity: 1,
        modifiers: ['Hot', 'V60 Dripper']
      },
      {
        id: 'kds-item-4',
        name: 'Batch Brew (Daily Roast)',
        quantity: 1,
        modifiers: ['Large Cup']
      }
    ],
    createdAt: '15:36:20'
  },
  {
    id: 'kds-ord-106',
    orderNumber: '#106',
    station: 'Food',
    status: 'Preparing',
    diningOption: 'DineIn',
    elapsedSeconds: 420,
    tableOrCustomer: 'Table 2 • Alex',
    items: [
      {
        id: 'kds-item-5',
        name: 'Sourdough Grilled Cheese',
        quantity: 1,
        modifiers: ['Add Truffle Oil', 'Crispy']
      },
      {
        id: 'kds-item-6',
        name: 'Almond Croissant',
        quantity: 2,
        modifiers: ['Warm Up']
      }
    ],
    notes: 'Serve food together with coffee',
    createdAt: '15:35:40'
  },
  {
    id: 'kds-ord-102',
    orderNumber: '#102',
    station: 'Espresso',
    status: 'Overdue',
    diningOption: 'DineIn',
    elapsedSeconds: 615, // > 8 mins (480s) -> Overdue
    tableOrCustomer: 'Table 8 • VIP',
    items: [
      {
        id: 'kds-item-7',
        name: 'Matcha Espresso Fusion',
        quantity: 3,
        modifiers: ['Oat Milk', 'No Sugar']
      }
    ],
    notes: 'Customer inquiring at counter',
    createdAt: '15:32:00'
  },
  {
    id: 'kds-ord-101',
    orderNumber: '#101',
    station: 'Espresso',
    status: 'Ready',
    diningOption: 'Takeaway',
    elapsedSeconds: 520,
    tableOrCustomer: 'Takeaway • David',
    items: [
      {
        id: 'kds-item-8',
        name: 'Espresso (Double)',
        quantity: 1,
        modifiers: ['Single Origin Colombia']
      }
    ],
    createdAt: '15:34:00'
  }
];
