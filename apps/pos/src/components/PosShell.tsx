import React, { useEffect, useMemo, useRef, useState } from 'react';
import { Modal, PinPad, TextInput } from '@coffee-pos/ui';
import { CATEGORIES, CATALOG_ITEMS, CatalogItem } from '../data/mockCatalog';
import { posOutbox } from '../storage/posOutboxDb';
import { posSyncEngine } from '../sync/posSyncEngine';
import { CustomerDisplay } from './CustomerDisplay';
import { MemberSearchModal, MemberRecord } from './MemberSearchModal';
import { BotanicalSprig, PosIcon, PosIconName, RoastMark } from './PosIcon';
import './PosShell.css';

export interface CartItem {
  id: string;
  catalogItemId: string;
  name: string;
  unitPrice: number;
  quantity: number;
  selectedModifiers: string[];
}

export interface OpenTicket {
  id: string;
  orderNumber: string;
  tableOrCustomer: string;
  diningOption: 'DineIn' | 'Takeaway';
  items: CartItem[];
  subtotal: number;
  total: number;
  timeAgo: string;
  needsReview?: boolean;
}

export interface CashMovement {
  id: string;
  type: 'CashIn' | 'CashOut';
  amount: number;
  reason: string;
  time: string;
}

export interface PosShellProps {
  currentCashierName?: string;
  currentCashierRole?: string;
  outletName?: string;
  onLockSession?: () => void;
}

type PosTab = 'register' | 'tickets' | 'shifts';
type DiningOption = 'DineIn' | 'Takeaway';
type PaymentMethod = 'Cash' | 'DuitNowQR' | 'Card';

const ART_POSITIONS: Record<string, string> = {
  'item-1': '0% 0%',
  'item-2': '50% 0%',
  'item-3': '100% 0%',
  'item-4': '0% 50%',
  'item-5': '50% 50%',
  'item-6': '100% 50%',
  'item-7': '0% 100%',
  'item-8': '50% 100%',
  'item-9': '100% 100%'
};

const MOCK_CUSTOMERS: MemberRecord[] = [
  { id: 'c-1', name: 'Sarah Lee', phone: '+60123456789', tier: 'Silver', points: 245 },
  { id: 'c-2', name: 'Farid Kamil', phone: '+60198765432', tier: 'Gold', points: 520 },
  { id: 'c-3', name: 'Alex Tan', phone: '+60163334444', tier: 'Bronze', points: 85 }
];

const money = (value: number) => `RM ${value.toLocaleString('en-MY', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;

const getTicketPerson = (ticket: OpenTicket) => {
  const [first, ...rest] = ticket.tableOrCustomer.split(' • ');
  if (ticket.diningOption === 'Takeaway' && rest.length > 0) {
    return { primary: rest.join(' • '), secondary: '', isTable: false };
  }
  return { primary: first, secondary: rest.join(' • '), isTable: ticket.diningOption === 'DineIn' };
};

const ticketItemNote = (name: string) => {
  if (name.includes('Pour Over')) return 'Brewed to order';
  if (name.includes('Croissant')) return 'Freshly baked';
  return 'Queued for preparation';
};

const StatusPill: React.FC<{ tone?: 'online' | 'active' | 'takeaway' | 'warning' | 'danger'; children: React.ReactNode; dot?: boolean; title?: string }> = ({ tone = 'active', children, dot = false, title }) => (
  <span className={`rl-status-pill rl-status-pill--${tone}`} title={title}>
    {dot && <span className="rl-status-pill__dot" aria-hidden="true" />}
    {children}
  </span>
);

interface ActionButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'solid' | 'outline' | 'soft' | 'danger' | 'ghost';
  icon?: PosIconName;
  full?: boolean;
}

const ActionButton: React.FC<ActionButtonProps> = ({ variant = 'outline', icon, full = false, className = '', children, ...props }) => (
  <button className={`rl-action rl-action--${variant} ${full ? 'rl-action--full' : ''} ${className}`} {...props}>
    {icon && <PosIcon name={icon} size={18} strokeWidth={1.7} />}
    <span>{children}</span>
  </button>
);

const Amount: React.FC<{ value: number; size?: 'sm' | 'md' | 'lg' | 'hero'; sign?: boolean; negative?: boolean }> = ({ value, size = 'md', sign = false, negative = false }) => (
  <span className={`rl-amount rl-amount--${size} ${negative ? 'rl-amount--negative' : ''}`}>
    {negative ? '-' : sign && value > 0 ? '+' : ''}{money(Math.abs(value))}
  </span>
);

const ProductArt: React.FC<{ itemId: string; size?: 'card' | 'ticket' }> = ({ itemId, size = 'card' }) => (
  <span className={`rl-product-art rl-product-art--${size}`} style={{ '--art-position': ART_POSITIONS[itemId] ?? '0% 0%' } as React.CSSProperties} aria-hidden="true" />
);

const BrandLockup: React.FC = () => (
  <div className="rl-pos-brand-lockup">
    <RoastMark size={56} />
    <div className="rl-pos-brand-lockup__name">ROAST LEDGER</div>
    <div className="rl-pos-brand-lockup__sub">COFFEE</div>
  </div>
);

const NavItem: React.FC<{ active: boolean; label: string; icon: PosIconName; onClick: () => void }> = ({ active, label, icon, onClick }) => (
  <button type="button" className={`rl-pos-nav-item ${active ? 'rl-pos-nav-item--active' : ''}`} onClick={onClick} aria-current={active ? 'page' : undefined}>
    <PosIcon name={icon} size={28} strokeWidth={1.45} />
    <span>{label}</span>
  </button>
);

interface TicketCardProps {
  ticket: OpenTicket;
  onResume: (ticket: OpenTicket) => void;
}

const TicketCard: React.FC<TicketCardProps> = ({ ticket, onResume }) => {
  const person = getTicketPerson(ticket);
  const itemLabel = ticket.items.length === 1 ? 'item' : 'items';

  return (
    <article className={'rl-ticket-card ' + (ticket.needsReview ? 'rl-ticket-card--review' : '')}>
      <header className="rl-ticket-card__header">
        <span className="rl-ticket-card__number">{ticket.orderNumber}</span>
        <StatusPill
          tone={ticket.needsReview ? 'warning' : ticket.diningOption === 'DineIn' ? 'active' : 'takeaway'}
          title={ticket.needsReview ? 'This ticket needs review before payment' : undefined}
        >
          <PosIcon name={ticket.needsReview ? 'warning' : ticket.diningOption === 'DineIn' ? 'cup' : 'bag'} size={17} />
          {ticket.needsReview ? 'REVIEW REQUIRED' : ticket.diningOption === 'DineIn' ? 'DINE-IN' : 'TAKEAWAY'}
        </StatusPill>
      </header>

      <div className="rl-ticket-card__customer">
        <span className="rl-ticket-card__avatar">
          <PosIcon name={person.isTable ? 'table' : 'user'} size={22} />
        </span>
        <h2>{person.primary}{person.secondary && <><span>•</span>{person.secondary}</>}</h2>
      </div>

      <div className="rl-ticket-card__items" aria-label={ticket.items.length + ' order ' + itemLabel}>
        {ticket.items.map(item => (
          <div key={item.id} className="rl-ticket-card__item">
            <ProductArt itemId={item.catalogItemId} size="ticket" />
            <div>
              <strong>{item.quantity}x {item.name}</strong>
              <span>{ticketItemNote(item.name)}</span>
              {item.selectedModifiers.length > 0 && <small className="rl-ticket-card__modifier">{item.selectedModifiers.join(' · ')}</small>}
            </div>
          </div>
        ))}
      </div>

      <div className="rl-ticket-card__meta">
        <span><PosIcon name="clock" size={22} />{ticket.timeAgo}</span>
        <span><PosIcon name="receipt" size={22} /><small>Total</small><strong>{money(ticket.total)}</strong></span>
      </div>

      <ActionButton
        icon={ticket.needsReview ? 'warning' : 'ticket'}
        variant={ticket.needsReview ? 'soft' : 'solid'}
        full
        onClick={() => onResume(ticket)}
      >
        {ticket.needsReview ? 'Review ticket' : 'Resume ticket'}
      </ActionButton>
    </article>
  );
};

export const PosShell: React.FC<PosShellProps> = ({ currentCashierName = 'Ahmad Cashier', currentCashierRole = 'Cashier', outletName = 'Bangsar Flagship', onLockSession }) => {
  const [selectedCategory, setSelectedCategory] = useState<string>('cat-all');
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [favoriteProducts, setFavoriteProducts] = useState<string[]>(['item-2']);
  const [favoritesOnly, setFavoritesOnly] = useState<boolean>(false);
  const [isFilterOpen, setIsFilterOpen] = useState<boolean>(false);
  const filterButtonRef = useRef<HTMLButtonElement | null>(null);
  const filterPopoverRef = useRef<HTMLDivElement | null>(null);
  const filterCloseButtonRef = useRef<HTMLButtonElement | null>(null);
  const [cart, setCart] = useState<CartItem[]>([{ id: 'cart-1', catalogItemId: 'item-2', name: 'Oat Flat White', unitPrice: 14.5, quantity: 1, selectedModifiers: ['Double Shot', 'Less Sweet (50%)'] }]);
  const [diningOption, setDiningOption] = useState<DiningOption>('DineIn');
  const [activeOrderNumber, setActiveOrderNumber] = useState<string>('#104');
  const [activeTableOrCustomer, setActiveTableOrCustomer] = useState<string>('Table 4');
  const [activeTab, setActiveTab] = useState<PosTab>('register');
  const [ticketSearchQuery, setTicketSearchQuery] = useState<string>('');
  const [ticketFilter, setTicketFilter] = useState<'all' | 'dineIn' | 'takeaway' | 'review'>('all');

  const [isOverrideModalOpen, setIsOverrideModalOpen] = useState<boolean>(false);
  const [overrideReason, setOverrideReason] = useState<string>('');
  const [isHoldModalOpen, setIsHoldModalOpen] = useState<boolean>(false);
  const [holdTableName, setHoldTableName] = useState<string>('');
  const [isPaymentModalOpen, setIsPaymentModalOpen] = useState<boolean>(false);
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>('Cash');
  const [cashTendered, setCashTendered] = useState<number>(0);
  const [lastPaymentResult, setLastPaymentResult] = useState<{ receiptNumber: string; totalPaid: number; change: number; method: string } | null>(null);

  const [openingFloat] = useState<number>(300.0);
  const [shiftCashSales, setShiftCashSales] = useState<number>(1420.5);
  const [cashMovements, setCashMovements] = useState<CashMovement[]>([{ id: 'mov-1', type: 'CashOut', amount: 25.0, reason: 'Emergency ice bag purchase', time: '14:20' }]);
  const [isCashMovementModalOpen, setIsCashMovementModalOpen] = useState<boolean>(false);
  const [movementType, setMovementType] = useState<'CashIn' | 'CashOut'>('CashOut');
  const [movementAmount, setMovementAmount] = useState<string>('');
  const [movementReason, setMovementReason] = useState<string>('');
  const [isCloseShiftModalOpen, setIsCloseShiftModalOpen] = useState<boolean>(false);
  const [countedCashInput, setCountedCashInput] = useState<string>('');
  const [closedZReport, setClosedZReport] = useState<{ openedAt: string; closedAt: string; openingFloat: number; cashSales: number; cashIn: number; cashOut: number; expectedCash: number; actualCounted: number; variance: number } | null>(null);

  const [syncState, setSyncState] = useState({ isOnline: true, isSyncing: false, pendingCount: 0, lastError: undefined as string | undefined });
  const [customizingProduct, setCustomizingProduct] = useState<CatalogItem | null>(null);
  const [customizerMilk, setCustomizerMilk] = useState<string>('Oat Milk');
  const [customizerExtras, setCustomizerExtras] = useState<string[]>([]);
  const [openTickets, setOpenTickets] = useState<OpenTicket[]>([
    { id: 'ticket-105', orderNumber: '#105', tableOrCustomer: 'Takeaway • John', diningOption: 'Takeaway', items: [{ id: 'c-105-1', catalogItemId: 'item-4', name: 'Pour Over (Ethiopia Guji)', unitPrice: 18.0, quantity: 1, selectedModifiers: ['V60 Dripper'] }], subtotal: 18.0, total: 19.08, timeAgo: '5m ago' },
    { id: 'ticket-106', orderNumber: '#106', tableOrCustomer: 'Table 2 • Alex', diningOption: 'DineIn', items: [{ id: 'c-106-1', catalogItemId: 'item-8', name: 'Almond Croissant', unitPrice: 12.0, quantity: 2, selectedModifiers: ['Warm Up'] }], subtotal: 24.0, total: 25.44, timeAgo: '12m ago', needsReview: true }
  ]);

  const [attachedCustomer, setAttachedCustomer] = useState<MemberRecord | null>(null);
  const [isCustomerModalOpen, setIsCustomerModalOpen] = useState<boolean>(false);
  const [customerSearchQuery, setCustomerSearchQuery] = useState<string>('');
  const [appliedPointsDiscount, setAppliedPointsDiscount] = useState<number>(0);
  const [isSyncQueueModalOpen, setIsSyncQueueModalOpen] = useState<boolean>(false);
  const [isCustomerDisplayOpen, setIsCustomerDisplayOpen] = useState<boolean>(false);

  useEffect(() => {
    const unsubscribe = posSyncEngine.subscribe(state => setSyncState({ isOnline: state.isOnline, isSyncing: state.isSyncing, pendingCount: state.pendingCount, lastError: state.lastError }));
    return () => unsubscribe();
  }, []);

  useEffect(() => {
    if (!isFilterOpen) return;

    const focusTimer = window.setTimeout(() => filterCloseButtonRef.current?.focus(), 0);
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key !== 'Escape') return;
      event.preventDefault();
      setIsFilterOpen(false);
      window.setTimeout(() => filterButtonRef.current?.focus(), 0);
    };
    const handlePointerDown = (event: PointerEvent) => {
      const target = event.target;
      if (!(target instanceof Node)) return;
      const clickedInside = filterPopoverRef.current?.contains(target) || filterButtonRef.current?.contains(target);
      if (clickedInside) return;
      event.preventDefault();
      event.stopPropagation();
      setIsFilterOpen(false);
      window.setTimeout(() => filterButtonRef.current?.focus(), 0);
    };

    document.addEventListener('keydown', handleKeyDown);
    document.addEventListener('pointerdown', handlePointerDown, true);
    return () => {
      window.clearTimeout(focusTimer);
      document.removeEventListener('keydown', handleKeyDown);
      document.removeEventListener('pointerdown', handlePointerDown, true);
    };
  }, [isFilterOpen]);

  useEffect(() => {
    if (activeTab !== 'register' && isFilterOpen) setIsFilterOpen(false);
  }, [activeTab, isFilterOpen]);

  const rawSubtotal = cart.reduce((sum, item) => sum + item.unitPrice * item.quantity, 0);
  const subtotal = Math.max(0, rawSubtotal - appliedPointsDiscount);
  const sstTax = subtotal * 0.06;
  const grandTotal = subtotal + sstTax;
  const cashChange = Math.max(0, cashTendered - grandTotal);
  const totalCashIn = cashMovements.filter(m => m.type === 'CashIn').reduce((sum, m) => sum + m.amount, 0);
  const totalCashOut = cashMovements.filter(m => m.type === 'CashOut').reduce((sum, m) => sum + m.amount, 0);
  const expectedCashInDrawer = openingFloat + shiftCashSales + totalCashIn - totalCashOut;

  const filteredProducts = useMemo(() => CATALOG_ITEMS.filter(item => {
    const matchesCat = selectedCategory === 'cat-all' || item.categoryId === selectedCategory;
    const matchesFavorite = !favoritesOnly || favoriteProducts.includes(item.id);
    const query = searchQuery.trim().toLowerCase();
    return matchesCat && matchesFavorite && (!query || item.name.toLowerCase().includes(query) || item.description.toLowerCase().includes(query));
  }), [favoriteProducts, favoritesOnly, searchQuery, selectedCategory]);

  const filteredTickets = useMemo(() => openTickets.filter(ticket => {
    const query = ticketSearchQuery.trim().toLowerCase();
    const matchesQuery = !query || [ticket.orderNumber, ticket.tableOrCustomer, ...ticket.items.map(item => item.name)].join(' ').toLowerCase().includes(query);
    const matchesFilter = ticketFilter === 'all'
      || (ticketFilter === 'dineIn' && ticket.diningOption === 'DineIn')
      || (ticketFilter === 'takeaway' && ticket.diningOption === 'Takeaway')
      || (ticketFilter === 'review' && ticket.needsReview);
    return matchesQuery && matchesFilter;
  }), [openTickets, ticketFilter, ticketSearchQuery]);

  const addToCartDirect = (product: CatalogItem, modifiers: string[], priceDelta = 0) => setCart(prev => [...prev, { id: `cart-${Date.now()}-${Math.random()}`, catalogItemId: product.id, name: product.name, unitPrice: product.price + priceDelta, quantity: 1, selectedModifiers: modifiers }]);

  const handleProductClick = (product: CatalogItem) => {
    if (product.categoryId === 'cat-espresso') {
      setCustomizingProduct(product);
      setCustomizerMilk('Oat Milk');
      setCustomizerExtras([]);
    } else addToCartDirect(product, []);
  };

  const confirmCustomizer = () => {
    if (!customizingProduct) return;
    const modifiers = [customizerMilk, ...customizerExtras];
    const extraPrice = (customizerMilk === 'Almond Milk' ? 1.5 : 0) + customizerExtras.length * 3;
    addToCartDirect(customizingProduct, modifiers, extraPrice);
    setCustomizingProduct(null);
  };

  const updateQuantity = (cartItemId: string, delta: number) => setCart(prev => prev.map(item => {
    if (item.id !== cartItemId) return item;
    const nextQuantity = item.quantity + delta;
    return nextQuantity > 0 ? { ...item, quantity: nextQuantity } : null;
  }).filter((item): item is CartItem => item !== null));

  const clearCart = () => { if (cart.length > 0) setIsOverrideModalOpen(true); };
  const confirmVoid = () => { setCart([]); setIsOverrideModalOpen(false); setOverrideReason(''); };

  const handleHoldTicket = () => { if (cart.length > 0) { setHoldTableName(activeTableOrCustomer); setIsHoldModalOpen(true); } };

  const confirmHoldTicket = () => {
    const newTicket: OpenTicket = { id: `ticket-${Date.now()}`, orderNumber: activeOrderNumber, tableOrCustomer: holdTableName.trim() || 'Quick Tab', diningOption, items: cart, subtotal, total: grandTotal, timeAgo: 'Just now', needsReview: false };
    setOpenTickets(prev => [newTicket, ...prev]);
    setCart([]);
    setIsHoldModalOpen(false);
    setActiveOrderNumber(`#${Math.floor(100 + Math.random() * 900)}`);
    setActiveTableOrCustomer('New Customer');
  };

  const resumeTicket = (ticket: OpenTicket) => {
    setCart(ticket.items);
    setActiveOrderNumber(ticket.orderNumber);
    setActiveTableOrCustomer(ticket.tableOrCustomer);
    setDiningOption(ticket.diningOption);
    setOpenTickets(prev => prev.filter(t => t.id !== ticket.id));
    setActiveTab('register');
    window.setTimeout(() => document.getElementById('pos-catalog-search')?.focus(), 0);
  };

  const openPaymentModal = () => { setPaymentMethod('Cash'); setCashTendered(grandTotal); setIsPaymentModalOpen(true); };

  const completePayment = () => {
    const receiptNum = `RCP-${new Date().toISOString().slice(0, 10).replace(/-/g, '')}-${Math.floor(1000 + Math.random() * 9000)}`;
    if (paymentMethod === 'Cash') setShiftCashSales(prev => prev + grandTotal);
    posOutbox.enqueue('Payment', 'Create', { receiptNumber: receiptNum, orderNumber: activeOrderNumber, amount: grandTotal, method: paymentMethod, items: cart }, `idemp-${receiptNum}`).then(() => posSyncEngine.triggerSync());
    setLastPaymentResult({ receiptNumber: receiptNum, totalPaid: paymentMethod === 'Cash' ? cashTendered : grandTotal, change: paymentMethod === 'Cash' ? cashChange : 0, method: paymentMethod });
    setIsPaymentModalOpen(false);
  };

  const startNewOrder = () => { setLastPaymentResult(null); setCart([]); setActiveOrderNumber(`#${Math.floor(100 + Math.random() * 900)}`); setActiveTableOrCustomer('New Customer'); };

  const confirmCashMovement = () => {
    const amount = parseFloat(movementAmount);
    if (Number.isNaN(amount) || amount <= 0 || !movementReason.trim()) return;
    setCashMovements(prev => [{ id: `mov-${Date.now()}`, type: movementType, amount, reason: movementReason.trim(), time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) }, ...prev]);
    setIsCashMovementModalOpen(false);
    setMovementAmount('');
    setMovementReason('');
  };

  const confirmCloseShift = () => {
    const counted = parseFloat(countedCashInput) || 0;
    setClosedZReport({ openedAt: '08:00 AM', closedAt: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }), openingFloat, cashSales: shiftCashSales, cashIn: totalCashIn, cashOut: totalCashOut, expectedCash: expectedCashInDrawer, actualCounted: counted, variance: counted - expectedCashInDrawer });
    setIsCloseShiftModalOpen(false);
  };

  const toggleFavorite = (productId: string) => setFavoriteProducts(prev => prev.includes(productId) ? prev.filter(id => id !== productId) : [...prev, productId]);

  const closeCatalogFilters = () => {
    setIsFilterOpen(false);
    window.setTimeout(() => filterButtonRef.current?.focus(), 0);
  };

  const clearCatalogSearch = () => {
    setSearchQuery('');
    window.setTimeout(() => document.getElementById('pos-catalog-search')?.focus(), 0);
  };

  const showAllCatalogItems = () => {
    setSelectedCategory('cat-all');
    setSearchQuery('');
    setFavoritesOnly(false);
    setIsFilterOpen(false);
    window.setTimeout(() => document.getElementById('pos-catalog-search')?.focus(), 0);
  };

  const clearCatalogFilters = () => {
    setSelectedCategory('cat-all');
    setSearchQuery('');
    setFavoritesOnly(false);
    closeCatalogFilters();
  };

  const handleCategoryKeyDown = (event: React.KeyboardEvent<HTMLButtonElement>, index: number) => {
    if (!['ArrowRight', 'ArrowLeft', 'Home', 'End'].includes(event.key)) return;
    event.preventDefault();
    const nextIndex = event.key === 'Home'
      ? 0
      : event.key === 'End'
        ? CATEGORIES.length - 1
        : (index + (event.key === 'ArrowRight' ? 1 : -1) + CATEGORIES.length) % CATEGORIES.length;
    const nextCategory = CATEGORIES[nextIndex];
    setSelectedCategory(nextCategory.id);
    document.getElementById(`rl-category-${nextCategory.id}`)?.focus();
  };

  return (
    <div className="rl-pos-shell">
      <aside className="rl-pos-rail">
        <BrandLockup />
        <nav className="rl-pos-nav" aria-label="Cashier navigation">
          <NavItem active={activeTab === 'register'} label="Register" icon="cart" onClick={() => setActiveTab('register')} />
          <NavItem active={activeTab === 'tickets'} label={`Tickets (${openTickets.length})`} icon="ticket" onClick={() => setActiveTab('tickets')} />
          <NavItem active={activeTab === 'shifts'} label="Shifts" icon="clock" onClick={() => setActiveTab('shifts')} />
        </nav>
        <button type="button" className="rl-pos-profile" onClick={onLockSession} title="Lock Register / Switch Staff">
          <span className="rl-pos-profile__avatar">AC</span><span className="rl-pos-profile__info"><strong>{currentCashierName}</strong><small>{currentCashierRole}</small></span><span className="rl-pos-profile__online" aria-label="Cashier online" /><PosIcon name="lock" size={15} strokeWidth={1.8} />
        </button>
      </aside>

      <div className="rl-pos-workspace">
        <header className="rl-pos-topbar">
          <div className="rl-pos-context"><PosIcon name="map-pin" size={28} strokeWidth={1.55} /><strong>{outletName}</strong><span className="rl-pos-context__separator">•</span><span>Active Shift: {currentCashierName}</span></div>
          <div className="rl-pos-topbar__actions">
            <button type="button" className="rl-sync-status" onClick={() => setIsSyncQueueModalOpen(true)} title="Open sync queue"><span className={`rl-sync-status__dot ${syncState.isOnline ? '' : 'rl-sync-status__dot--offline'}`} />{syncState.isOnline ? (syncState.isSyncing ? 'SYNCING' : 'ONLINE') : 'OFFLINE'}<span>•</span>{syncState.isOnline ? 'SYNC ACTIVE' : `${syncState.pendingCount} QUEUED`}</button>
            <ActionButton icon="cloud" onClick={() => posSyncEngine.setSimulatedOnline(!syncState.isOnline)}>{syncState.isOnline ? 'Go Offline' : 'Go Online'}</ActionButton>
            <ActionButton icon="monitor" onClick={() => setIsCustomerDisplayOpen(true)}>Customer Display</ActionButton>
          </div>
        </header>

        {activeTab === 'register' && (
          <main className="rl-pos-page rl-pos-register-page">
            <section className="rl-register-catalog">
              <div className="rl-register-search-row">
                <div className="rl-search-box">
                  <PosIcon name="search" size={28} strokeWidth={1.45} />
                  <label className="sr-only" htmlFor="pos-catalog-search">Search catalog</label>
                  <input id="pos-catalog-search" value={searchQuery} onChange={event => setSearchQuery(event.target.value)} placeholder="Search coffee, tea, pastries..." />
                  {searchQuery && <button type="button" className="rl-search-box__clear" aria-label="Clear catalog search" onClick={clearCatalogSearch}><PosIcon name="close" size={19} /></button>}
                  <button ref={filterButtonRef} type="button" className="rl-search-box__filter" aria-label="Open catalog filters" aria-haspopup="dialog" aria-expanded={isFilterOpen} aria-controls="rl-catalog-filter-popover" onClick={() => setIsFilterOpen(open => !open)}><PosIcon name="sliders" size={24} /><span className="rl-search-box__filter-label">Filters</span></button>
                </div>
                {isFilterOpen && <div ref={filterPopoverRef} id="rl-catalog-filter-popover" className="rl-catalog-filter-popover" role="dialog" aria-label="Catalog filters">
                  <div className="rl-catalog-filter-popover__header"><div><span className="rl-catalog-filter-popover__kicker">CATALOG FILTERS</span><strong>Quick filters</strong></div><div className="rl-catalog-filter-popover__actions"><button type="button" onClick={clearCatalogFilters}>Clear filters</button><button ref={filterCloseButtonRef} type="button" className="rl-catalog-filter-popover__close" aria-label="Close catalog filters" onClick={closeCatalogFilters}><PosIcon name="close" size={18} /></button></div></div>
                  <label className="rl-catalog-filter-option"><input type="checkbox" checked={favoritesOnly} onChange={event => setFavoritesOnly(event.target.checked)} /><span>Favorites only</span><small>{favoriteProducts.length} saved</small></label>
                </div>}
              </div>
              <div className="rl-category-tabs-wrap"><div className="rl-category-tabs" role="tablist" aria-label="Catalog categories. Scroll horizontally for more categories.">{CATEGORIES.map((category, index) => <button key={category.id} id={`rl-category-${category.id}`} type="button" role="tab" aria-selected={selectedCategory === category.id} aria-controls="rl-product-grid" tabIndex={selectedCategory === category.id ? 0 : -1} className={selectedCategory === category.id ? 'rl-category-tab rl-category-tab--active' : 'rl-category-tab'} onClick={() => setSelectedCategory(category.id)} onKeyDown={event => handleCategoryKeyDown(event, index)}>{category.name}</button>)}</div><span className="rl-category-tabs__cue" aria-hidden="true"><span>More</span><PosIcon name="chevron-down" size={15} /></span></div>
              <div id="rl-product-grid" className="rl-product-grid" role="tabpanel" aria-labelledby={`rl-category-${selectedCategory}`}>
                {filteredProducts.map(product => {
                const isFavorite = favoriteProducts.includes(product.id);
                return <article key={product.id} className={`rl-product-card ${product.id === 'item-2' ? 'rl-product-card--featured' : ''}`}>
                  <button type="button" className="rl-product-card__main" onClick={() => handleProductClick(product)}><ProductArt itemId={product.id} /><span className="rl-product-card__copy"><strong>{product.name}</strong><span>{product.description}</span></span><span className="rl-product-card__price">{money(product.price)}</span></button>
                  <button type="button" className={`rl-product-card__favorite ${isFavorite ? 'rl-product-card__favorite--selected' : ''}`} onClick={() => toggleFavorite(product.id)} aria-label={`${isFavorite ? 'Remove' : 'Add'} ${product.name} favorite`} aria-pressed={isFavorite}><PosIcon name="heart" size={19} strokeWidth={1.45} /></button>
                </article>;
                })}
              </div>
              {filteredProducts.length === 0 && <div className="rl-empty-state"><PosIcon name="search" size={28} /><strong>{searchQuery.trim() ? 'No coffee found' : favoritesOnly ? 'No saved coffees here' : 'No coffee in this category'}</strong><span>{searchQuery.trim() ? 'Try another search or clear it.' : favoritesOnly ? 'Save a coffee or show all items to continue.' : 'Try another category or show all items.'}</span>{searchQuery.trim() && <ActionButton icon="close" onClick={clearCatalogSearch}>Clear search</ActionButton>}{(favoritesOnly || selectedCategory !== 'cat-all') && <ActionButton icon="search" variant="ghost" onClick={showAllCatalogItems}>Show all items</ActionButton>}</div>}
            </section>

             <aside className="rl-order-panel" aria-label="Current order">
               <div className="rl-order-panel__header"><div><h2>{activeOrderNumber}</h2><p>{activeTableOrCustomer}</p></div><ActionButton icon="user" onClick={() => setIsCustomerModalOpen(true)}>{attachedCustomer ? attachedCustomer.name : 'Add Customer'}</ActionButton></div>
               <div className="rl-dining-toggle" role="group" aria-label="Dining option"><button type="button" aria-pressed={diningOption === 'DineIn'} className={diningOption === 'DineIn' ? 'rl-dining-toggle__button rl-dining-toggle__button--active' : 'rl-dining-toggle__button'} onClick={() => setDiningOption('DineIn')}><PosIcon name="cup" size={20} /> Dine-In</button><button type="button" aria-pressed={diningOption === 'Takeaway'} className={diningOption === 'Takeaway' ? 'rl-dining-toggle__button rl-dining-toggle__button--active' : 'rl-dining-toggle__button'} onClick={() => setDiningOption('Takeaway')}><PosIcon name="bag" size={20} /> Takeaway</button></div>
               <div className="rl-order-panel__items">{cart.length === 0 ? <div className="rl-order-empty"><PosIcon name="cup" size={30} /><span>Start an order by selecting a coffee.</span></div> : cart.map(item => <div key={item.id} className="rl-order-item"><ProductArt itemId={item.catalogItemId} size="ticket" /><div className="rl-order-item__body"><div className="rl-order-item__heading"><strong>{item.name}</strong><span>{money(item.unitPrice * item.quantity)}</span></div><ul>{item.selectedModifiers.map(modifier => <li key={modifier}>{modifier}</li>)}</ul><div className="rl-quantity-stepper" aria-label={`Quantity for ${item.name}`}><button type="button" onClick={() => updateQuantity(item.id, -1)} aria-label={`Decrease ${item.name} quantity`}><PosIcon name="minus" size={16} /></button><span>{item.quantity}</span><button type="button" onClick={() => updateQuantity(item.id, 1)} aria-label={`Increase ${item.name} quantity`}><PosIcon name="plus" size={16} /></button></div></div></div>)}</div>
               <div className="rl-order-panel__footer"><div className="rl-summary-row"><span>Subtotal</span><span>{money(subtotal)}</span></div><div className="rl-summary-row"><span>SST (6%)</span><span>{money(sstTax)}</span></div><div className="rl-summary-row rl-summary-row--total"><strong>Total</strong><strong>{money(grandTotal)}</strong></div><div className="rl-order-actions"><ActionButton icon="trash" disabled={cart.length === 0} onClick={clearCart}>Void</ActionButton><ActionButton icon="pause" variant="soft" disabled={cart.length === 0} onClick={handleHoldTicket}>Hold</ActionButton><ActionButton icon="card" variant="solid" full disabled={cart.length === 0} onClick={openPaymentModal}>Charge</ActionButton></div></div>
             </aside>
             <div className="rl-mobile-payment-bar" aria-label="Mobile checkout summary"><div><span>Total</span><strong aria-live="polite">{money(grandTotal)}</strong></div><ActionButton icon="card" variant="solid" disabled={cart.length === 0} onClick={openPaymentModal}>Charge</ActionButton></div>
           </main>
        )}

        {activeTab === 'tickets' && (
          <main className="rl-pos-page rl-tickets-page">
            <div className="rl-page-heading"><h1>Open Tickets <span>({openTickets.length})</span></h1><p>Select a ticket to resume editing or proceed with payment.</p></div>
            <div className="rl-ticket-toolbar" role="toolbar" aria-label="Ticket filters">
              <label className="rl-ticket-search"><PosIcon name="search" size={21} /><span className="sr-only">Search tickets</span><input value={ticketSearchQuery} onChange={event => setTicketSearchQuery(event.target.value)} placeholder="Search tickets" /></label>
              {([['all', 'All'], ['dineIn', 'Dine in'], ['takeaway', 'Takeaway'], ['review', 'Needs review']] as const).map(([value, label]) => <button key={value} type="button" className={`rl-ticket-filter ${ticketFilter === value ? 'rl-ticket-filter--active' : ''}`} aria-pressed={ticketFilter === value} onClick={() => setTicketFilter(value)}>{label}</button>)}
            </div>
            {filteredTickets.length === 0 ? <div className="rl-empty-state rl-empty-state--page"><PosIcon name="ticket" size={34} /><strong>{openTickets.length === 0 ? 'No Open Tickets' : 'No Matching Tickets'}</strong><span>{openTickets.length === 0 ? 'All orders have been charged or voided.' : 'Try another search or filter.'}</span></div> : <div className="rl-ticket-grid">{filteredTickets.map(ticket => <TicketCard key={ticket.id} ticket={ticket} onResume={resumeTicket} />)}</div>}
          </main>
        )}

        {activeTab === 'shifts' && (
          <main className="rl-pos-page rl-shifts-page">{closedZReport ? <section className="rl-z-report"><div className="rl-z-report__header"><StatusPill tone="active" dot>SHIFT CLOSED</StatusPill><h1>Shift Z-Report</h1><p>{outletName} • {currentCashierName}</p><span>Opened {closedZReport.openedAt} · Closed {closedZReport.closedAt}</span></div><div className="rl-z-report__rows"><div><span>Opening Cash Float</span><strong>{money(closedZReport.openingFloat)}</strong></div><div><span>+ Cash Sales</span><strong>{money(closedZReport.cashSales)}</strong></div><div><span>+ Cash In</span><strong>{money(closedZReport.cashIn)}</strong></div><div><span>- Cash Out</span><strong>{money(closedZReport.cashOut)}</strong></div><div className="rl-z-report__highlight"><span>Expected Drawer Balance</span><strong>{money(closedZReport.expectedCash)}</strong></div><div className="rl-z-report__highlight"><span>Actual Counted Cash</span><strong>{money(closedZReport.actualCounted)}</strong></div><div className={closedZReport.variance === 0 ? 'rl-z-report__variance rl-z-report__variance--balanced' : 'rl-z-report__variance'}><span>Drawer Variance</span><strong>{closedZReport.variance === 0 ? 'RM 0.00 · Balanced' : money(closedZReport.variance)}</strong></div></div><div className="rl-z-report__actions"><ActionButton icon="printer" onClick={() => window.alert('Printing 80mm Z-Report...')}>Print Z-Report</ActionButton><ActionButton icon="clock" variant="solid" onClick={() => setClosedZReport(null)}>Start New Shift</ActionButton></div></section> : <div className="rl-shifts-grid"><section className="rl-shift-summary-panel"><div className="rl-shift-summary-panel__top"><div><span className="rl-section-kicker">CURRENT REGISTER SHIFT</span><h1>{currentCashierName}</h1><p>Opened at 08:00 AM <span>•</span> Active</p></div><StatusPill tone="active" dot>ACTIVE SHIFT</StatusPill></div><div className="rl-drawer-balance" aria-live="polite"><span>EXPECTED CASH IN DRAWER</span><div className="rl-drawer-balance__rule"><i /><RoastMark size={25} /><i /></div><strong>{money(expectedCashInDrawer)}</strong><BotanicalSprig /></div><div className="rl-shift-stats"><div><span className="rl-shift-stat__icon"><PosIcon name="cash" size={23} /></span><span>Opening Float</span><strong>{money(openingFloat)}</strong></div><div><span className="rl-shift-stat__icon"><PosIcon name="receipt" size={23} /></span><span>Cash Sales</span><strong>{money(shiftCashSales)}</strong></div><div><span className="rl-shift-stat__icon"><PosIcon name="arrows" size={23} /></span><span>Cash In / Out</span><strong><em>+{money(totalCashIn)}</em><b> / {money(totalCashOut)}</b></strong></div></div><div className="rl-shift-actions"><ActionButton icon="cash" full onClick={() => setIsCashMovementModalOpen(true)}>Cash In / Out</ActionButton><ActionButton icon="lock" variant="danger" full onClick={() => { setCountedCashInput(''); setIsCloseShiftModalOpen(true); }}>Close Shift (Z-Report)</ActionButton></div></section><section className="rl-movements-panel"><h2>Cash Drawer Movements <span className="rl-panel-count">({cashMovements.length})</span></h2><div className="rl-movement-summary" aria-label="Shift cash movement totals"><div><span>Cash in</span><strong>{money(totalCashIn)}</strong></div><div><span>Cash out</span><strong>{money(totalCashOut)}</strong></div><div><span>Movement count</span><strong>{cashMovements.length}</strong></div></div><div className="rl-movements-list">{cashMovements.length === 0 ? <div className="rl-empty-state"><PosIcon name="cash" size={28} /><strong>No cash movements yet</strong><span>Record a cash in or cash out to keep the drawer balanced.</span></div> : cashMovements.map(movement => <div key={movement.id} className="rl-movement-row"><StatusPill tone={movement.type === 'CashIn' ? 'active' : 'takeaway'}>{movement.type === 'CashIn' ? 'Cash in' : 'Cash out'}</StatusPill><strong>{movement.reason}</strong><span>{movement.time}</span><Amount value={movement.amount} sign negative={movement.type === 'CashOut'} /></div>)}</div><BotanicalSprig className="rl-movements-panel__sprig" /></section></div>}</main>
        )}
      </div>

      <Modal isOpen={isCashMovementModalOpen} title="Record Cash Drawer Movement" onClose={() => setIsCashMovementModalOpen(false)} footer={<><ActionButton onClick={() => setIsCashMovementModalOpen(false)}>Cancel</ActionButton><ActionButton variant="solid" disabled={!movementAmount || !movementReason} onClick={confirmCashMovement}>Record Movement</ActionButton></>}><div className="rl-modal-form"><div className="rl-modal-choice-row"><button type="button" aria-pressed={movementType === 'CashIn'} className={movementType === 'CashIn' ? 'is-selected' : ''} onClick={() => setMovementType('CashIn')}>+ Cash In</button><button type="button" aria-pressed={movementType === 'CashOut'} className={movementType === 'CashOut' ? 'is-selected' : ''} onClick={() => setMovementType('CashOut')}>− Cash Out</button></div><TextInput label="Amount (RM)" placeholder="0.00" type="number" value={movementAmount} onChange={event => setMovementAmount(event.target.value)} /><TextInput label="Reason / Note" placeholder="e.g. Ice bag delivery, oat milk run" value={movementReason} onChange={event => setMovementReason(event.target.value)} /></div></Modal>

      <Modal isOpen={isCloseShiftModalOpen} title="Close Register Shift" onClose={() => setIsCloseShiftModalOpen(false)} footer={<><ActionButton onClick={() => setIsCloseShiftModalOpen(false)}>Cancel</ActionButton><ActionButton variant="danger" icon="lock" disabled={!countedCashInput} onClick={confirmCloseShift}>Verify &amp; Close Shift</ActionButton></>}><div className="rl-modal-form"><p>Count the physical cash drawer and enter the total. The system will compare it with registered transactions.</p><TextInput label="Total Counted Cash (RM)" placeholder="Enter physical cash total" type="number" value={countedCashInput} onChange={event => setCountedCashInput(event.target.value)} /><div className="rl-modal-warning">Cash count remains blind until this shift is closed.</div></div></Modal>

      <Modal isOpen={customizingProduct !== null} title={`Customize: ${customizingProduct?.name ?? ''}`} onClose={() => setCustomizingProduct(null)} footer={<><ActionButton onClick={() => setCustomizingProduct(null)}>Cancel</ActionButton><ActionButton variant="solid" onClick={confirmCustomizer}>Add to Cart</ActionButton></>}><div className="rl-modal-form"><h3>Milk Options</h3><div className="rl-modal-choice-grid">{['Oat Milk', 'Soy Milk', 'Almond Milk', 'Whole Milk'].map(milk => <button key={milk} type="button" aria-pressed={customizerMilk === milk} className={customizerMilk === milk ? 'is-selected' : ''} onClick={() => setCustomizerMilk(milk)}>{milk}{milk === 'Almond Milk' && <small>+RM 1.50</small>}</button>)}</div><h3>Extras</h3><div className="rl-modal-choice-grid">{['Double Shot', 'Vanilla Syrup', 'Caramel Drizzle'].map(extra => { const selected = customizerExtras.includes(extra); return <button key={extra} type="button" aria-pressed={selected} className={selected ? 'is-selected' : ''} onClick={() => setCustomizerExtras(prev => selected ? prev.filter(item => item !== extra) : [...prev, extra])}>{extra}<small>+RM 3.00</small></button>; })}</div></div></Modal>

      <Modal isOpen={isHoldModalOpen} title="Hold Open Ticket" onClose={() => setIsHoldModalOpen(false)} footer={<><ActionButton onClick={() => setIsHoldModalOpen(false)}>Cancel</ActionButton><ActionButton variant="solid" onClick={confirmHoldTicket}>Hold Ticket</ActionButton></>}><div className="rl-modal-form"><p>Assign a table number or customer name to place this order on hold.</p><TextInput label="Table / Customer Reference" placeholder="e.g. Table 5 or VIP Sarah" value={holdTableName} onChange={event => setHoldTableName(event.target.value)} /></div></Modal>

      <Modal isOpen={isOverrideModalOpen} title="Manager Void Authorization" onClose={() => setIsOverrideModalOpen(false)} footer={<><ActionButton onClick={() => setIsOverrideModalOpen(false)}>Cancel</ActionButton><ActionButton variant="danger" icon="trash" onClick={confirmVoid}>Authorize &amp; Void Cart</ActionButton></>}><div className="rl-modal-form"><p>Voiding an active order ticket requires manager approval.</p><TextInput label="Void Reason" placeholder="e.g. Customer change order" value={overrideReason} onChange={event => setOverrideReason(event.target.value)} /></div></Modal>

      <Modal isOpen={isPaymentModalOpen} title={`Payment · ${activeOrderNumber}`} onClose={() => setIsPaymentModalOpen(false)} footer={<><ActionButton onClick={() => setIsPaymentModalOpen(false)}>Back</ActionButton><ActionButton variant="solid" icon="card" disabled={paymentMethod === 'Cash' && cashTendered < grandTotal} onClick={completePayment}>Confirm Payment · {money(grandTotal)}</ActionButton></>}><div className="rl-payment-modal"><div className="rl-payment-modal__total"><span>Total Payable</span><strong>{money(grandTotal)}</strong></div><div className="rl-payment-methods">{(['Cash', 'DuitNowQR', 'Card'] as PaymentMethod[]).map(method => <button key={method} type="button" aria-pressed={paymentMethod === method} className={paymentMethod === method ? 'is-selected' : ''} onClick={() => setPaymentMethod(method)}>{method === 'Cash' ? 'Cash' : method === 'DuitNowQR' ? 'DuitNow QR' : 'Card'}</button>)}</div>{paymentMethod === 'Cash' && <div className="rl-payment-cash"><div className="rl-payment-denoms"><button type="button" onClick={() => setCashTendered(grandTotal)}>Exact · {money(grandTotal)}</button>{[20, 50, 100].map(value => <button key={value} type="button" onClick={() => setCashTendered(value)}>{money(value)}</button>)}</div><div className="rl-payment-cash__summary"><span>Tendered <strong>{money(cashTendered)}</strong></span><span>Change <strong>{money(cashChange)}</strong></span></div></div>}{paymentMethod === 'DuitNowQR' && <div className="rl-payment-placeholder"><PosIcon name="card" size={32} /><strong>DuitNow QR ready</strong><span>Present the QR to the customer to scan.</span></div>}{paymentMethod === 'Card' && <div className="rl-payment-placeholder"><PosIcon name="card" size={32} /><strong>PAX A920 terminal ready</strong><span>Insert, swipe, or tap the customer card.</span></div>}</div></Modal>

      <Modal isOpen={lastPaymentResult !== null} title="Payment Successful" onClose={startNewOrder} footer={<><ActionButton icon="printer" onClick={() => window.alert('Printing 80mm ESC/POS thermal receipt...')}>Print Receipt</ActionButton><ActionButton variant="solid" onClick={startNewOrder}>New Order</ActionButton></>}>
        {lastPaymentResult && <div className="rl-receipt-modal"><div className="rl-receipt-modal__brand"><RoastMark size={34} /><strong>ROAST LEDGER COFFEE</strong><span>{outletName}</span></div><div className="rl-receipt-modal__rows"><span>Receipt <strong>{lastPaymentResult.receiptNumber}</strong></span><span>Payment Method <strong>{lastPaymentResult.method}</strong></span><span>Amount Paid <strong>{money(lastPaymentResult.totalPaid)}</strong></span>{lastPaymentResult.change > 0 && <span>Change Given <strong>{money(lastPaymentResult.change)}</strong></span>}</div><div className="rl-receipt-modal__success"><PosIcon name="check" size={20} /> Order sent to kitchen rail</div></div>}
      </Modal>

      <MemberSearchModal
        isOpen={isCustomerModalOpen}
        query={customerSearchQuery}
        members={MOCK_CUSTOMERS}
        attachedMemberId={attachedCustomer?.id}
        onQueryChange={setCustomerSearchQuery}
        onClose={() => setIsCustomerModalOpen(false)}
        onAttach={customer => { setAttachedCustomer(customer); setAppliedPointsDiscount(0); setIsCustomerModalOpen(false); }}
        onRedeem={customer => { setAttachedCustomer(customer); setAppliedPointsDiscount(10); setIsCustomerModalOpen(false); }}
        onDetach={() => { setAttachedCustomer(null); setAppliedPointsDiscount(0); setIsCustomerModalOpen(false); }}
      />

      <Modal isOpen={isSyncQueueModalOpen} title="Offline Outbox & Sync Health" onClose={() => setIsSyncQueueModalOpen(false)} footer={<><ActionButton variant="solid" disabled={!syncState.isOnline || syncState.isSyncing} onClick={() => posSyncEngine.triggerSync()}>{syncState.isSyncing ? 'Syncing…' : 'Force Sync Now'}</ActionButton><ActionButton onClick={() => setIsSyncQueueModalOpen(false)}>Close</ActionButton></>}><div className="rl-sync-modal"><div className="rl-sync-modal__metrics"><div><span>Network Status</span><StatusPill tone={syncState.isOnline ? 'online' : 'warning'} dot>{syncState.isOnline ? 'Online' : 'Offline'}</StatusPill></div><div><span>Pending Outbox</span><strong>{syncState.pendingCount} transactions</strong></div></div><p><strong>Offline safety guardrails active.</strong> Transactions remain durable in the local outbox and use unique idempotency keys before syncing.</p></div></Modal>

      <CustomerDisplay isOpen={isCustomerDisplayOpen} onClose={() => setIsCustomerDisplayOpen(false)} cart={cart} subtotal={subtotal} sstTax={sstTax} grandTotal={grandTotal} outletName={outletName} orderNumber={activeOrderNumber} />
    </div>
  );
};
