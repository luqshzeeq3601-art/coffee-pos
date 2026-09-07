export interface CategoryDto {
  id: string;
  name: string;
  code: string;
  sortOrder: number;
  isActive: boolean;
}

export interface ModifierDto {
  id: string;
  name: string;
  priceDelta: number;
  isDefault: boolean;
  sortOrder: number;
}

export interface ModifierGroupDto {
  id: string;
  name: string;
  minSelections: number;
  maxSelections: number;
  isRequired: boolean;
  modifiers: ModifierDto[];
}

export interface VariantDto {
  id: string;
  name: string;
  sku?: string;
  barcode?: string;
  price: number;
  costPrice?: number;
  sortOrder: number;
}

export interface ProductDto {
  id: string;
  categoryId: string;
  categoryName?: string;
  name: string;
  description: string;
  basePrice: number;
  isTaxInclusive: boolean;
  taxRateId?: string;
  taxRatePercent?: number;
  variants: VariantDto[];
  modifierGroups: ModifierGroupDto[];
  isActive: boolean;
}

export interface TaxRateDto {
  id: string;
  name: string;
  ratePercent: number; // e.g. 6.00 for 6% SST
  code: string;
  isDefault: boolean;
  isActive: boolean;
}

export interface DiscountDto {
  id: string;
  name: string;
  discountType: 'Percentage' | 'FixedAmount';
  value: number; // e.g. 10.0 for 10% or 5.0 for RM 5.00
  requiresManagerApproval: boolean;
  isActive: boolean;
}

export interface CatalogResponseDto {
  categories: CategoryDto[];
  products: ProductDto[];
  taxRates: TaxRateDto[];
  discounts: DiscountDto[];
}

export type CatalogDto = CatalogResponseDto;

