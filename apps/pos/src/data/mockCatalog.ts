export interface CatalogCategory {
  id: string;
  name: string;
  code: string;
}

export interface CatalogItem {
  id: string;
  categoryId: string;
  name: string;
  price: number;
  description: string;
  availableModifiers: string[];
}

export const CATEGORIES: CatalogCategory[] = [
  { id: 'cat-all', name: 'All Items', code: 'ALL' },
  { id: 'cat-espresso', name: 'Espresso Bar', code: 'ESP' },
  { id: 'cat-filter', name: 'Filter & Pour Over', code: 'FLT' },
  { id: 'cat-tea', name: 'Tea & Non-Coffee', code: 'TEA' },
  { id: 'cat-food', name: 'Pastries & Food', code: 'FOD' }
];

export const CATALOG_ITEMS: CatalogItem[] = [
  {
    id: 'item-1',
    categoryId: 'cat-espresso',
    name: 'Espresso (Double)',
    price: 10.0,
    description: 'House blend single origin Colombia Huila',
    availableModifiers: ['Single Origin +RM 2.00', 'Extra Shot +RM 3.00']
  },
  {
    id: 'item-2',
    categoryId: 'cat-espresso',
    name: 'Oat Flat White',
    price: 14.5,
    description: 'Velvety microfoam with Oatly Barista',
    availableModifiers: ['Extra Shot +RM 3.00', 'Less Sweet (50%)', 'Vanilla Syrup +RM 2.00']
  },
  {
    id: 'item-3',
    categoryId: 'cat-espresso',
    name: 'Iced Spanish Latte',
    price: 16.0,
    description: 'Condensed milk espresso with whole milk over ice',
    availableModifiers: ['Soy Milk +RM 2.00', 'Oat Milk +RM 2.50', 'Less Sweet (50%)', 'Extra Ice']
  },
  {
    id: 'item-4',
    categoryId: 'cat-filter',
    name: 'Pour Over (Ethiopia Guji)',
    price: 18.0,
    description: 'Jasmine, peach, bergamot notes. V60 extraction',
    availableModifiers: ['Hot', 'Iced +RM 1.00', 'V60', 'Origami Dripper']
  },
  {
    id: 'item-5',
    categoryId: 'cat-filter',
    name: 'Batch Brew (Daily Roast)',
    price: 11.0,
    description: 'Clean filter brew, rotated daily',
    availableModifiers: ['Large Cup +RM 3.00', 'With Milk +RM 1.50']
  },
  {
    id: 'item-6',
    categoryId: 'cat-tea',
    name: 'Matcha Espresso Fusion',
    price: 17.5,
    description: 'Ceremonial Uji matcha, fresh milk, double ristretto',
    availableModifiers: ['Oat Milk +RM 2.50', 'Less Sweet (50%)', 'No Sugar']
  },
  {
    id: 'item-7',
    categoryId: 'cat-tea',
    name: 'Artisan Hojicha Latte',
    price: 15.0,
    description: 'Roasted green tea with steamed fresh milk',
    availableModifiers: ['Oat Milk +RM 2.50', 'Less Sweet']
  },
  {
    id: 'item-8',
    categoryId: 'cat-food',
    name: 'Almond Croissant',
    price: 12.0,
    description: 'Twice-baked French butter croissant with frangipane',
    availableModifiers: ['Warm Up', 'Extra Butter +RM 1.50']
  },
  {
    id: 'item-9',
    categoryId: 'cat-food',
    name: 'Sourdough Grilled Cheese',
    price: 22.0,
    description: 'Triple cheese blend on artisan sourdough with tomato relish',
    availableModifiers: ['Add Truffle Oil +RM 4.00', 'Add Bacon +RM 5.00']
  }
];
