import React, { useState } from 'react';
import {
  Button,
  Badge,
  TextInput,
  Modal,
  MoneyDisplay
} from '@coffee-pos/ui';
import { WasteReason } from '@coffee-pos/contracts';
import {
  AdminEmptyTableRow,
  AdminField,
  AdminMetric,
  AdminMetricRow,
  AdminPageHeader,
  AdminPermissionMark,
  AdminSectionHeader,
  AdminStatusChip,
  AdminTableFrame,
  AdminToolbar
} from './AdminPrimitives';
import {
  INITIAL_DEVICES,
  INITIAL_MYINVOIS_DOCUMENTS,
  INITIAL_OUTLETS,
  INITIAL_STAFF,
  INITIAL_STOCK_ITEMS,
  INITIAL_WASTAGE_ENTRIES,
  PERMISSION_MATRIX,
  type AdminMyInvoisItem,
  type AdminStockItem,
  type AdminWastageItem,
  type DeviceItem,
  type OutletItem,
  type StaffItem,
  type StaffRole
} from '../data/mockAdminData';
import './AdminShell.css';

type AdminNavKey = 'reports' | 'inventory' | 'myinvois' | 'outlets' | 'staff' | 'devices';

const PAGE_TITLES: Record<AdminNavKey, string> = {
  reports: 'Sales & Analytics',
  inventory: 'Inventory & Stock',
  myinvois: 'MyInvois e-Invoice',
  outlets: 'Outlets & Stores',
  staff: 'Staff & Access',
  devices: 'Devices & Terminals'
};

type AdminIconName =
  | 'leaf'
  | 'analytics'
  | 'inventory'
  | 'invoice'
  | 'outlet'
  | 'staff'
  | 'device'
  | 'bag'
  | 'wallet'
  | 'percent'
  | 'ticket'
  | 'clock'
  | 'card'
  | 'download'
  | 'plus'
  | 'refresh'
  | 'waste'
  | 'check';

const AdminIcon: React.FC<{ name: AdminIconName; size?: number; className?: string }> = ({ name, size = 20, className = '' }) => {
  const common = {
    width: size,
    height: size,
    viewBox: '0 0 24 24',
    fill: 'none',
    stroke: 'currentColor',
    strokeWidth: 1.7,
    strokeLinecap: 'round' as const,
    strokeLinejoin: 'round' as const,
    className,
    'aria-hidden': true
  };

  switch (name) {
    case 'leaf':
      return <svg {...common}><path d="M20.5 3.5C12 3.8 6.2 7 5 13.3c-.8 4.1 2.2 6.7 5.7 5.8C17 17.6 19.5 11.7 20.5 3.5Z" /><path d="M4 21c3.1-4.5 6.6-7.3 11.1-9.5" /><path d="M8.4 8.9c1.2.2 2.2.7 3.1 1.5" /></svg>;
    case 'analytics':
      return <svg {...common}><path d="M4 19.5V5.8" /><path d="M4 19.5h16" /><path d="M7.5 16v-3.7" /><path d="M11.8 16V8.2" /><path d="M16.1 16v-6" /><path d="M7.5 12.3h.1M11.8 8.2h.1M16.1 10h.1" /></svg>;
    case 'inventory':
      return <svg {...common}><path d="m12 3 8 4.3v9.4L12 21l-8-4.3V7.3L12 3Z" /><path d="m4.3 7.5 7.7 4.2 7.7-4.2M12 11.7V21" /></svg>;
    case 'invoice':
      return <svg {...common}><path d="M6 3.5h9l3 3v14H6z" /><path d="M14.5 3.8v3h3M9 11h6M9 14.5h6M9 18h3" /></svg>;
    case 'outlet':
      return <svg {...common}><path d="M4 20.5h16M5 20.5V9h14v11.5M3 9h18l-1.6-5H4.6L3 9ZM8 9v3M12 9v3M16 9v3M9 20.5v-5h6v5" /></svg>;
    case 'staff':
      return <svg {...common}><circle cx="9" cy="8" r="3" /><path d="M3.5 20c.2-3.3 2.2-5 5.5-5s5.3 1.7 5.5 5M16.5 5.5a2.5 2.5 0 0 1 0 5M16.5 14c2.3.2 3.7 2.1 4 4.5" /></svg>;
    case 'device':
      return <svg {...common}><rect x="3.5" y="4" width="17" height="12" rx="1.5" /><path d="M8 20h8M12 16v4M7 8h10M7 11h6" /></svg>;
    case 'bag':
      return <svg {...common}><path d="M5 8.5h14l1 11H4l1-11ZM8 8.5V6a4 4 0 0 1 8 0v2.5" /><path d="M9 12h.1M15 12h.1" /></svg>;
    case 'wallet':
      return <svg {...common}><path d="M4 7.5h14.5A1.5 1.5 0 0 1 20 9v9.5A1.5 1.5 0 0 1 18.5 20h-14A1.5 1.5 0 0 1 3 18.5V6a2 2 0 0 1 2-2h12" /><path d="M16 13h4M16.2 13.1h.1" /></svg>;
    case 'percent':
      return <svg {...common}><path d="m5 19 14-14" /><circle cx="7" cy="7" r="2" /><circle cx="17" cy="17" r="2" /></svg>;
    case 'ticket':
      return <svg {...common}><path d="M4 7.5a2.5 2.5 0 0 0 0 5v4h16v-4a2.5 2.5 0 0 0 0-5v-4H4v4Z" /><path d="M12 6v1M12 10v1M12 14v1" /></svg>;
    case 'clock':
      return <svg {...common}><circle cx="12" cy="12" r="8.5" /><path d="M12 7v5l3.2 2" /></svg>;
    case 'card':
      return <svg {...common}><rect x="3" y="5" width="18" height="14" rx="2" /><path d="M3 9h18M7 14h4" /></svg>;
    case 'download':
      return <svg {...common}><path d="M12 3v11M8 10l4 4 4-4M5 20h14" /></svg>;
    case 'plus':
      return <svg {...common}><path d="M12 5v14M5 12h14" /></svg>;
    case 'refresh':
      return <svg {...common}><path d="M20 11a8 8 0 0 0-14.9-3.8L4 9.5M4 4v5.5h5.5M4 13a8 8 0 0 0 14.9 3.8L20 14.5M20 20v-5.5h-5.5" /></svg>;
    case 'waste':
      return <svg {...common}><path d="M5 7h14M10 4h4l1 3H9l1-3ZM7 7l1 13h8l1-13M10 10v7M14 10v7" /></svg>;
    case 'check':
      return <svg {...common}><path d="m5 12 4 4L19 6" /></svg>;
  }
};

export const AdminShell: React.FC = () => {
  const [activeNav, setActiveNav] = useState<AdminNavKey>('reports');
  const [selectedOutletFilter, setSelectedOutletFilter] = useState<string>('all');


  const [adminNotice, setAdminNotice] = useState<string>('');

  // Outlets state and profile editor
  const [outlets, setOutlets] = useState<OutletItem[]>(INITIAL_OUTLETS);
  const [isAddOutletOpen, setIsAddOutletOpen] = useState<boolean>(false);
  const [editingOutlet, setEditingOutlet] = useState<OutletItem | null>(null);
  const [outletForm, setOutletForm] = useState({ name: '', address: '', isMainOutlet: false });
  const [outletFormError, setOutletFormError] = useState<string>('');
  const [selectedOutletForSettings, setSelectedOutletForSettings] = useState<OutletItem | null>(null);
  const [isOutletSettingsOpen, setIsOutletSettingsOpen] = useState<boolean>(false);

  // Staff state and access editor
  const [staff, setStaff] = useState<StaffItem[]>(INITIAL_STAFF);
  const [staffTab, setStaffTab] = useState<'list' | 'matrix'>('list');
  const [isStaffEditorOpen, setIsStaffEditorOpen] = useState<boolean>(false);
  const [editingStaff, setEditingStaff] = useState<StaffItem | null>(null);
  const [staffForm, setStaffForm] = useState<{ name: string; email: string; role: StaffRole; outletId: string }>({
    name: '',
    email: '',
    role: 'Barista',
    outletId: 'out-bangsar'
  });
  const [staffFormError, setStaffFormError] = useState<string>('');
  const [isResetPinOpen, setIsResetPinOpen] = useState<boolean>(false);
  const [selectedStaffForPin, setSelectedStaffForPin] = useState<StaffItem | null>(null);
  const [newPinCode, setNewPinCode] = useState<string>('');
  const [pinFormError, setPinFormError] = useState<string>('');

  // Device state and configuration editor
  const [devices, setDevices] = useState<DeviceItem[]>(INITIAL_DEVICES);
  const [isEnrollDeviceOpen, setIsEnrollDeviceOpen] = useState<boolean>(false);
  const [newDeviceName, setNewDeviceName] = useState<string>('');
  const [newDeviceType, setNewDeviceType] = useState<'POS' | 'KDS'>('POS');
  const [newDeviceOutletId, setNewDeviceOutletId] = useState<string>('out-bangsar');
  const [generatedCode, setGeneratedCode] = useState<string | null>(null);
  const [isDeviceConfigOpen, setIsDeviceConfigOpen] = useState<boolean>(false);
  const [editingDevice, setEditingDevice] = useState<DeviceItem | null>(null);
  const [deviceForm, setDeviceForm] = useState<{ name: string; deviceType: 'POS' | 'KDS'; outletId: string; isActive: boolean }>({
    name: '',
    deviceType: 'POS',
    outletId: 'out-bangsar',
    isActive: true
  });
  const [deviceFormError, setDeviceFormError] = useState<string>('');

  // Inventory state and safe item metadata editor
  const [stockItems, setStockItems] = useState<AdminStockItem[]>(INITIAL_STOCK_ITEMS);
  const [isStockEditorOpen, setIsStockEditorOpen] = useState<boolean>(false);
  const [editingStockItem, setEditingStockItem] = useState<AdminStockItem | null>(null);
  const [stockForm, setStockForm] = useState({ sku: '', name: '', category: '', unit: 'Grams', reorderThreshold: '' });
  const [stockFormError, setStockFormError] = useState<string>('');

  const [wastageEntries, setWastageEntries] = useState<AdminWastageItem[]>(INITIAL_WASTAGE_ENTRIES);

  // Inventory Modals State
  const [isReceiveStockOpen, setIsReceiveStockOpen] = useState<boolean>(false);
  const [receiveItemId, setReceiveItemId] = useState<string>('stk-3');
  const [receiveQuantity, setReceiveQuantity] = useState<string>('');
  const [receiveInvoice, setReceiveInvoice] = useState<string>('');
  const [receiveUnitCost, setReceiveUnitCost] = useState<string>('0.015');

  const [isRecordWastageOpen, setIsRecordWastageOpen] = useState<boolean>(false);
  const [wastageItemId, setWastageItemId] = useState<string>('stk-1');
  const [wastageQuantity, setWastageQuantity] = useState<string>('');
  const [wastageReason, setWastageReason] = useState<WasteReason>('CalibrationDialIn');
  const [wastageNotes, setWastageNotes] = useState<string>('');

  // MyInvois state remains read-only until the compliance/API workflow is connected.
  const [myInvoisDocuments] = useState<AdminMyInvoisItem[]>(INITIAL_MYINVOIS_DOCUMENTS);

  const activeOutlets = outlets.filter(outlet => outlet.isActive);
  const activeStockItems = stockItems.filter(item => item.isActive);
  const scopeOptions = [
    { value: 'all', label: 'All Stores (Global)' },
    ...outlets.map(outlet => ({ value: outlet.id, label: `${outlet.name}${outlet.isActive ? '' : ' (Inactive)'}` }))
  ];

  const getOutletName = (outletId: string) => {
    if (outletId === 'all') return 'All outlets';
    return outlets.find(outlet => outlet.id === outletId)?.name ?? 'Unknown outlet';
  };

  const getActiveDeviceCount = (outletId: string) =>
    devices.filter(device => device.outletId === outletId && device.isActive).length;

  const showAdminNotice = (message: string) => setAdminNotice(message);

  const openAddOutlet = () => {
    setEditingOutlet(null);
    setOutletForm({ name: '', address: '', isMainOutlet: false });
    setOutletFormError('');
    setAdminNotice('');
    setIsAddOutletOpen(true);
  };

  const openEditOutlet = (outlet: OutletItem) => {
    setEditingOutlet(outlet);
    setOutletForm({ name: outlet.name, address: outlet.address, isMainOutlet: outlet.isMainOutlet });
    setOutletFormError('');
    setAdminNotice('');
    setIsAddOutletOpen(true);
  };

  const closeOutletEditor = () => {
    setIsAddOutletOpen(false);
    setEditingOutlet(null);
    setOutletForm({ name: '', address: '', isMainOutlet: false });
    setOutletFormError('');
  };

  const handleSaveOutlet = () => {
    const name = outletForm.name.trim();
    const address = outletForm.address.trim();
    if (!name) {
      setOutletFormError('Enter an outlet name.');
      return;
    }
    if (outlets.some(outlet => outlet.id !== editingOutlet?.id && outlet.name.toLowerCase() === name.toLowerCase())) {
      setOutletFormError('An outlet with this name already exists.');
      return;
    }
    if (editingOutlet?.isMainOutlet && !outletForm.isMainOutlet) {
      setOutletFormError('Assign another main outlet before removing this designation.');
      return;
    }

    if (editingOutlet) {
      setOutlets(prev => prev.map(outlet => ({
        ...outlet,
        ...(outlet.id === editingOutlet.id
          ? { name, address: address || 'Kuala Lumpur, Malaysia', isMainOutlet: outletForm.isMainOutlet }
          : outletForm.isMainOutlet ? { isMainOutlet: false } : {})
      })));
      showAdminNotice(`${name} profile updated. Linked staff and terminals remain attached by outlet ID.`);
    } else {
      const newOutlet: OutletItem = {
        id: `out-${Date.now()}`,
        name,
        address: address || 'Kuala Lumpur, Malaysia',
        isMainOutlet: outletForm.isMainOutlet,
        isActive: true
      };
      setOutlets(prev => [
        ...prev.map(outlet => outletForm.isMainOutlet ? { ...outlet, isMainOutlet: false } : outlet),
        newOutlet
      ]);
      showAdminNotice(`${name} added. Changes are held in this session only.`);
    }
    closeOutletEditor();
  };

  const toggleOutletActive = (outlet: OutletItem) => {
    if (outlet.isActive) {
      if (outlet.isMainOutlet) {
        showAdminNotice('The main outlet cannot be deactivated. Assign another main outlet first.');
        return;
      }
      if (getActiveDeviceCount(outlet.id) > 0) {
        showAdminNotice('Move or disable active terminals before deactivating this outlet.');
        return;
      }
      if (staff.some(member => member.isActive && member.assignedOutletIds.includes(outlet.id))) {
        showAdminNotice('Reassign active staff before deactivating this outlet.');
        return;
      }
      if (activeOutlets.length <= 1) {
        showAdminNotice('Keep at least one active outlet available.');
        return;
      }
    }
    setOutlets(prev => prev.map(item => item.id === outlet.id ? { ...item, isActive: !item.isActive } : item));
    showAdminNotice(`${outlet.name} ${outlet.isActive ? 'deactivated' : 'reactivated'}.`);
  };

  const openStaffEditor = (member?: StaffItem) => {
    setEditingStaff(member ?? null);
    setStaffForm({
      name: member?.name ?? '',
      email: member?.email ?? '',
      role: member?.role ?? 'Barista',
      outletId: member?.assignedOutletIds[0] && member.assignedOutletIds[0] !== 'all'
        ? member.assignedOutletIds[0]
        : activeOutlets[0]?.id ?? ''
    });
    setStaffFormError('');
    setAdminNotice('');
    setIsStaffEditorOpen(true);
  };

  const closeStaffEditor = () => {
    setIsStaffEditorOpen(false);
    setEditingStaff(null);
    setStaffForm({ name: '', email: '', role: 'Barista', outletId: activeOutlets[0]?.id ?? '' });
    setStaffFormError('');
  };

  const handleSaveStaff = () => {
    const name = staffForm.name.trim();
    const email = staffForm.email.trim();
    if (!name || !email) {
      setStaffFormError('Enter the staff member’s name and email.');
      return;
    }
    if (!email.includes('@')) {
      setStaffFormError('Enter a valid email address.');
      return;
    }
    if (staff.some(member => member.id !== editingStaff?.id && member.email.toLowerCase() === email.toLowerCase())) {
      setStaffFormError('A staff account with this email already exists.');
      return;
    }
    if (staffForm.role !== 'Owner' && !staffForm.outletId) {
      setStaffFormError('Select an active outlet for this role.');
      return;
    }
    if (staffForm.role !== 'Owner' && !outlets.some(outlet => outlet.id === staffForm.outletId && outlet.isActive)) {
      setStaffFormError('Select an active outlet for this role.');
      return;
    }
    if (editingStaff?.id === 'usr-1' && staffForm.role !== 'Owner') {
      setStaffFormError('The signed-in owner cannot be demoted from this session.');
      return;
    }
    if (editingStaff?.role === 'Owner' && staffForm.role !== 'Owner' && staff.filter(member => member.isActive && member.role === 'Owner').length <= 1) {
      setStaffFormError('Keep at least one active owner account.');
      return;
    }

    const assignedOutletIds = staffForm.role === 'Owner' ? ['all'] : [staffForm.outletId];
    if (editingStaff) {
      setStaff(prev => prev.map(member => member.id === editingStaff.id
        ? { ...member, name, email, role: staffForm.role, assignedOutletIds }
        : member));
      showAdminNotice(`${name} access profile updated.`);
    } else {
      setStaff(prev => [...prev, {
        id: `usr-${Date.now()}`,
        name,
        email,
        role: staffForm.role,
        assignedOutletIds,
        hasPinSet: false,
        isActive: true
      }]);
      showAdminNotice(`${name} added. Set a register PIN separately if needed.`);
    }
    closeStaffEditor();
  };

  const toggleStaffActive = (member: StaffItem) => {
    if (member.isActive && member.id === 'usr-1') {
      showAdminNotice('The signed-in owner cannot be deactivated from this session.');
      return;
    }
    if (member.isActive && member.role === 'Owner' && staff.filter(item => item.isActive && item.role === 'Owner').length <= 1) {
      showAdminNotice('Keep at least one active owner account.');
      return;
    }
    setStaff(prev => prev.map(item => item.id === member.id ? { ...item, isActive: !item.isActive } : item));
    showAdminNotice(`${member.name} ${member.isActive ? 'deactivated' : 'reactivated'}.`);
  };

  const handleSavePin = () => {
    if (!/^\d{4}$/.test(newPinCode)) {
      setPinFormError('Enter exactly four numbers. The PIN is not stored in the dashboard state.');
      return;
    }
    if (selectedStaffForPin) {
      setStaff(prev => prev.map(member => member.id === selectedStaffForPin.id ? { ...member, hasPinSet: true } : member));
      showAdminNotice(`PIN status updated for ${selectedStaffForPin.name}.`);
    }
    setIsResetPinOpen(false);
    setSelectedStaffForPin(null);
    setNewPinCode('');
    setPinFormError('');
  };

  const openDeviceConfig = (device: DeviceItem) => {
    setEditingDevice(device);
    setDeviceForm({ name: device.name, deviceType: device.deviceType, outletId: device.outletId, isActive: device.isActive });
    setDeviceFormError('');
    setAdminNotice('');
    setIsDeviceConfigOpen(true);
  };

  const closeDeviceConfig = () => {
    setIsDeviceConfigOpen(false);
    setEditingDevice(null);
    setDeviceFormError('');
  };

  const handleSaveDevice = () => {
    const name = deviceForm.name.trim();
    if (!name) {
      setDeviceFormError('Enter a terminal name.');
      return;
    }
    if (!outlets.some(outlet => outlet.id === deviceForm.outletId && outlet.isActive)) {
      setDeviceFormError('Select an active outlet.');
      return;
    }
    if (!editingDevice) return;
    setDevices(prev => prev.map(device => device.id === editingDevice.id
      ? { ...device, name, deviceType: deviceForm.deviceType, outletId: deviceForm.outletId, isActive: deviceForm.isActive }
      : device));
    showAdminNotice(`${name} configuration updated.`);
    closeDeviceConfig();
  };

  const handleGenerateCode = () => {
    const name = newDeviceName.trim();
    if (!name) {
      showAdminNotice('Enter a terminal identifier before generating a code.');
      return;
    }
    if (!outlets.some(outlet => outlet.id === newDeviceOutletId && outlet.isActive)) {
      showAdminNotice('Select an active outlet before generating a code.');
      return;
    }
    const code = `${newDeviceType}-${Math.floor(1000 + Math.random() * 9000)}`;
    setGeneratedCode(code);

    const newDevice: DeviceItem = {
      id: `dev-${Date.now()}`,
      name,
      outletId: newDeviceOutletId,
      deviceType: newDeviceType,
      status: 'PendingEnrollment',
      enrollmentCode: code,
      lastSeen: 'Never',
      isActive: true
    };
    setDevices(prev => [...prev, newDevice]);
    showAdminNotice(`${name} added as pending enrollment.`);
  };

  const openStockEditor = (item?: AdminStockItem) => {
    setEditingStockItem(item ?? null);
    setStockForm({
      sku: item?.sku ?? '',
      name: item?.name ?? '',
      category: item?.category ?? '',
      unit: item?.unit ?? 'Grams',
      reorderThreshold: item ? String(item.reorderThreshold) : ''
    });
    setStockFormError('');
    setAdminNotice('');
    setIsStockEditorOpen(true);
  };

  const closeStockEditor = () => {
    setIsStockEditorOpen(false);
    setEditingStockItem(null);
    setStockForm({ sku: '', name: '', category: '', unit: 'Grams', reorderThreshold: '' });
    setStockFormError('');
  };

  const handleSaveStockItem = () => {
    const sku = stockForm.sku.trim().toUpperCase();
    const name = stockForm.name.trim();
    const category = stockForm.category.trim();
    const reorderThreshold = Number(stockForm.reorderThreshold);
    if (!sku || !name || !category) {
      setStockFormError('Enter SKU, item name, and category.');
      return;
    }
    if (!Number.isFinite(reorderThreshold) || reorderThreshold < 0) {
      setStockFormError('Reorder level must be zero or greater.');
      return;
    }
    if (stockItems.some(item => item.id !== editingStockItem?.id && item.sku.toLowerCase() === sku.toLowerCase())) {
      setStockFormError('An item with this SKU already exists.');
      return;
    }

    if (editingStockItem) {
      setStockItems(prev => prev.map(item => item.id === editingStockItem.id
        ? { ...item, sku, name, category, reorderThreshold }
        : item));
      showAdminNotice(`${name} metadata updated. Stock quantity and unit cost were preserved.`);
    } else {
      setStockItems(prev => [...prev, {
        id: `stk-${Date.now()}`,
        sku,
        name,
        category,
        unit: stockForm.unit,
        currentStock: 0,
        reorderThreshold,
        costPerUnit: 0,
        isActive: true
      }]);
      showAdminNotice(`${name} added with zero stock. Receive stock to establish its current unit cost.`);
    }
    closeStockEditor();
  };

  const toggleStockItemActive = (item: AdminStockItem) => {
    if (item.isActive && item.currentStock > 0) {
      showAdminNotice('Archive is available after this item reaches zero stock; its movement history stays intact.');
      return;
    }
    setStockItems(prev => prev.map(stockItem => stockItem.id === item.id ? { ...stockItem, isActive: !stockItem.isActive } : stockItem));
    showAdminNotice(`${item.name} ${item.isActive ? 'archived' : 'reactivated'}.`);
  };

  const openReceiveStock = () => {
    const item = activeStockItems.find(stockItem => stockItem.id === receiveItemId) ?? activeStockItems[0];
    if (!item) {
      showAdminNotice('Add an active stock item before receiving stock.');
      return;
    }
    setReceiveItemId(item.id);
    setReceiveQuantity('');
    setReceiveInvoice('');
    setReceiveUnitCost(item.costPerUnit > 0 ? String(item.costPerUnit) : '');
    setAdminNotice('');
    setIsReceiveStockOpen(true);
  };

  const confirmReceiveStock = () => {
    const qty = parseFloat(receiveQuantity);
    const unitCost = parseFloat(receiveUnitCost);
    const targetItem = activeStockItems.find(item => item.id === receiveItemId);
    if (!targetItem) return;
    if (!Number.isFinite(qty) || qty <= 0 || !Number.isFinite(unitCost) || unitCost < 0) {
      showAdminNotice('Enter a positive quantity and a valid unit cost.');
      return;
    }

    setStockItems(prev => prev.map(item => item.id === receiveItemId
      ? { ...item, currentStock: item.currentStock + qty, costPerUnit: unitCost }
      : item));
    showAdminNotice(`${qty.toLocaleString()} ${targetItem.unit} received for ${targetItem.name}.`);
    setIsReceiveStockOpen(false);
    setReceiveQuantity('');
    setReceiveInvoice('');
    setReceiveUnitCost('');
  };

  const confirmRecordWastage = () => {
    const qty = parseFloat(wastageQuantity);
    if (!Number.isFinite(qty) || qty <= 0) {
      showAdminNotice('Enter a positive quantity for the wastage write-down.');
      return;
    }

    const targetItem = activeStockItems.find(item => item.id === wastageItemId);
    if (!targetItem) return;
    if (qty > targetItem.currentStock) {
      showAdminNotice(`Wastage cannot exceed the ${targetItem.currentStock.toLocaleString()} ${targetItem.unit} currently on hand.`);
      return;
    }

    const costImpact = Math.round(qty * targetItem.costPerUnit * 100) / 100;
    setStockItems(prev => prev.map(item => item.id === wastageItemId
      ? { ...item, currentStock: item.currentStock - qty }
      : item));

    const newEntry: AdminWastageItem = {
      id: `wst-${Date.now()}`,
      stockItemId: targetItem.id,
      stockItemName: targetItem.name,
      quantityWasted: qty,
      unit: targetItem.unit,
      reason: wastageReason,
      costImpact,
      notes: wastageNotes.trim() || undefined,
      loggedByName: 'Nurul Manager',
      time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
    };

    setWastageEntries(prev => [newEntry, ...prev]);
    showAdminNotice(`${qty.toLocaleString()} ${targetItem.unit} written down for ${targetItem.name}.`);
    setIsRecordWastageOpen(false);
    setWastageQuantity('');
    setWastageNotes('');
  };

  const openResetPin = (member: StaffItem) => {
    setSelectedStaffForPin(member);
    setNewPinCode('');
    setPinFormError('');
    setAdminNotice('');
    setIsResetPinOpen(true);
  };

  const closeResetPin = () => {
    setIsResetPinOpen(false);
    setSelectedStaffForPin(null);
    setNewPinCode('');
    setPinFormError('');
  };

  // Filtered devices use stable outlet IDs, so outlet names can safely change.
  const filteredDevices = selectedOutletFilter === 'all'
    ? devices
    : devices.filter(device => device.outletId === selectedOutletFilter);

  const pageActions = (
    <>
      {activeNav === 'reports' && (
        <Button variant="primary" onClick={() => alert('Exporting End-of-Day PDF Reconciliation Report...')}>
          <AdminIcon name="download" size={16} />
          Export Z-Report
        </Button>
      )}
      {activeNav === 'inventory' && (
        <>
          <Button variant="outline" onClick={() => openStockEditor()}>
            <AdminIcon name="plus" size={16} />
            Add item
          </Button>
          <Button
            variant="outline"
            onClick={() => {
              const firstItem = activeStockItems[0];
              if (!firstItem) {
                showAdminNotice('Add an active stock item before logging wastage.');
                return;
              }
              setWastageItemId(activeStockItems.some(item => item.id === wastageItemId) ? wastageItemId : firstItem.id);
              setWastageQuantity('');
              setWastageNotes('');
              setAdminNotice('');
              setIsRecordWastageOpen(true);
            }}
          >
            Log Wastage
          </Button>
          <Button
            variant="primary"
            onClick={openReceiveStock}
          >
            <AdminIcon name="plus" size={16} />
            Receive Stock
          </Button>
        </>
      )}
      {activeNav === 'myinvois' && (
        <Button variant="outline" onClick={() => showAdminNotice('MyInvois status refresh requires the connected LHDN integration. No local invoice mutation was made.')}>Refresh Status</Button>
      )}
      {activeNav === 'outlets' && (
        <Button variant="primary" onClick={openAddOutlet}>
          <AdminIcon name="plus" size={16} />
          Add Outlet
        </Button>
      )}
      {activeNav === 'staff' && (
        <Button variant="primary" onClick={() => openStaffEditor()}>
          <AdminIcon name="plus" size={16} />
          Add staff
        </Button>
      )}
      {activeNav === 'devices' && (
        <Button
          variant="primary"
          onClick={() => {
            setGeneratedCode(null);
            setNewDeviceName('');
            setNewDeviceOutletId(activeOutlets[0]?.id ?? '');
            setAdminNotice('');
            setIsEnrollDeviceOpen(true);
          }}
        >
          <AdminIcon name="plus" size={16} />
          Enroll Terminal
        </Button>
      )}
    </>
  );

  return (
    <div className="rl-admin-shell">
      {/* 1. Left Navigation Sidebar */}
      <aside className="rl-admin-sidebar">
        <div className="rl-admin-sidebar__brand">
          <span className="rl-admin-sidebar__logo"><AdminIcon name="leaf" size={28} /></span>
          <div className="rl-admin-sidebar__brand-meta">
            <span className="rl-admin-sidebar__brand-name">ROAST LEDGER</span>
            <span className="rl-admin-sidebar__tenant">Artisan Roast Co.</span>
          </div>
        </div>

        <nav className="rl-admin-sidebar__nav">
          <button
            type="button"
            aria-label="Sales & Analytics"
            className={`rl-admin-sidebar__item ${activeNav === 'reports' ? 'rl-admin-sidebar__item--active' : ''}`}
            onClick={() => { setActiveNav('reports'); setAdminNotice(''); }}
          >
            <span className="rl-admin-sidebar__icon"><AdminIcon name="analytics" size={18} /></span>
            <span>Sales & Analytics</span>
          </button>

          <button
            type="button"
            aria-label="Inventory & Stock"
            className={`rl-admin-sidebar__item ${activeNav === 'inventory' ? 'rl-admin-sidebar__item--active' : ''}`}
            onClick={() => { setActiveNav('inventory'); setAdminNotice(''); }}
          >
            <span className="rl-admin-sidebar__icon"><AdminIcon name="inventory" size={18} /></span>
            <span>Inventory & Stock</span>
          </button>

          <button
            type="button"
            aria-label="MyInvois e-Invoice"
            className={`rl-admin-sidebar__item ${activeNav === 'myinvois' ? 'rl-admin-sidebar__item--active' : ''}`}
            onClick={() => { setActiveNav('myinvois'); setAdminNotice(''); }}
          >
            <span className="rl-admin-sidebar__icon"><AdminIcon name="invoice" size={18} /></span>
            <span>MyInvois e-Invoice</span>
          </button>

          <button
            type="button"
            aria-label="Outlets & Stores"
            className={`rl-admin-sidebar__item ${activeNav === 'outlets' ? 'rl-admin-sidebar__item--active' : ''}`}
            onClick={() => { setActiveNav('outlets'); setAdminNotice(''); }}
          >
            <span className="rl-admin-sidebar__icon"><AdminIcon name="outlet" size={18} /></span>
            <span>Outlets & Stores</span>
          </button>

          <button
            type="button"
            aria-label="Staff & Access"
            className={`rl-admin-sidebar__item ${activeNav === 'staff' ? 'rl-admin-sidebar__item--active' : ''}`}
            onClick={() => { setActiveNav('staff'); setAdminNotice(''); }}
          >
            <span className="rl-admin-sidebar__icon"><AdminIcon name="staff" size={18} /></span>
            <span>Staff & Access</span>
          </button>

          <button
            type="button"
            aria-label="Devices & Terminals"
            className={`rl-admin-sidebar__item ${activeNav === 'devices' ? 'rl-admin-sidebar__item--active' : ''}`}
            onClick={() => { setActiveNav('devices'); setAdminNotice(''); }}
          >
            <span className="rl-admin-sidebar__icon"><AdminIcon name="device" size={18} /></span>
            <span>Devices & Terminals</span>
          </button>
        </nav>

        <div className="rl-admin-sidebar__footer">
          <div className="rl-admin-sidebar__user">
            <span className="rl-admin-sidebar__user-avatar" aria-hidden="true">AO</span>
            <div className="rl-admin-sidebar__user-meta">
              <strong>Azman Owner</strong>
              <span>azman@artisanroast.my</span>
            </div>
          </div>
          <Badge tone="success" isDot>MFA Active</Badge>
        </div>
      </aside>

      {/* 2. Main Administration Workspace */}
      <div className="rl-admin-workspace">
        <AdminPageHeader
          title={PAGE_TITLES[activeNav]}
          scopeValue={selectedOutletFilter}
          onScopeChange={setSelectedOutletFilter}
          scopeOptions={scopeOptions}
          actions={pageActions}
        />
        <div className="rl-admin-local-state-note" role="note">Demo data · changes reset when this page reloads</div>
        {adminNotice && <div className="rl-admin-feedback" role="status" aria-live="polite">{adminNotice}</div>}

        {/* Workspace Body */}
        <main className="rl-admin-main">
          {/* REPORTS & SALES ANALYTICS */}
          {activeNav === 'reports' && (
            <div className="rl-admin-section">
              <AdminSectionHeader
                title="Today’s store performance"
                description="19 August 2026 · All values shown in Malaysian ringgit"
              />

              <AdminMetricRow>
                <AdminMetric label="Gross sales" value={<MoneyDisplay amount={2224.00} size="lg" />} detail="112 orders fulfilled" />
                <AdminMetric label="Net revenue (excl. tax)" value={<MoneyDisplay amount={2098.11} size="lg" />} detail="Discounts: RM 24.00" />
                <AdminMetric label="6% SST collected" value={<MoneyDisplay amount={125.89} size="lg" />} detail="Tax compliance verified" />
                <AdminMetric label="Average ticket (AOV)" value={<MoneyDisplay amount={19.86} size="lg" />} detail="+12% vs last Wednesday" />
              </AdminMetricRow>

              {/* Hourly Velocity & Payment Breakdown */}
              <div className="rl-admin-analytics-row">
                {/* Hourly Velocity Visualizer */}
                <div className="rl-admin-card-panel">
                  <h3>Hourly sales velocity</h3>
                  <p className="rl-admin-card-panel__sub">Peak coffee rush between 08:00–10:00 and 12:00–14:00.</p>
                  
                  <div className="rl-admin-velocity-chart">
                    {[
                      { hour: '08:00', orders: 14, height: '50%' },
                      { hour: '09:00', orders: 26, height: '95%' },
                      { hour: '10:00', orders: 19, height: '70%' },
                      { hour: '11:00', orders: 12, height: '42%' },
                      { hour: '12:00', orders: 22, height: '80%' },
                      { hour: '13:00', orders: 28, height: '100%' },
                      { hour: '14:00', orders: 18, height: '65%' },
                      { hour: '15:00', orders: 15, height: '54%' },
                      { hour: '16:00', orders: 11, height: '38%' }
                    ].map(bar => (
                      <div key={bar.hour} className="rl-admin-velocity-bar-col">
                        <div className="rl-admin-velocity-bar-track">
                          <div className="rl-admin-velocity-bar-fill" style={{ height: bar.height }}>
                            <span className="rl-admin-velocity-bar-tooltip">{bar.orders}</span>
                          </div>
                        </div>
                        <span className="rl-admin-velocity-bar-label">{bar.hour}</span>
                      </div>
                    ))}
                  </div>
                </div>

                {/* Tender Breakdown */}
                <div className="rl-admin-card-panel">
                  <h3>Tender method share</h3>
                  <p className="rl-admin-card-panel__sub">DuitNow QR and contactless cards lead in volume.</p>
                  
                  <div className="rl-admin-tender-breakdown">
                    <div className="rl-admin-tender-item">
                      <div className="rl-admin-tender-item__meta">
                        <strong>DuitNow QR</strong>
                        <span className="rl-admin-tender-item__pct">54.2%</span>
                      </div>
                      <div className="rl-admin-tender-bar">
                        <div className="rl-admin-tender-fill" style={{ width: '54.2%', backgroundColor: 'var(--admin-primary)' }} />
                      </div>
                      <span className="rl-admin-tender-amount">RM 1,205.41 (61 txns)</span>
                    </div>

                    <div className="rl-admin-tender-item">
                      <div className="rl-admin-tender-item__meta">
                        <strong>Cash Tender</strong>
                        <span className="rl-admin-tender-item__pct">28.4%</span>
                      </div>
                      <div className="rl-admin-tender-bar">
                        <div className="rl-admin-tender-fill" style={{ width: '28.4%' }} />
                      </div>
                      <span className="rl-admin-tender-amount">RM 631.62 (32 txns)</span>
                    </div>

                    <div className="rl-admin-tender-item">
                      <div className="rl-admin-tender-item__meta">
                        <strong>Credit / Debit Card</strong>
                        <span className="rl-admin-tender-item__pct">17.4%</span>
                      </div>
                      <div className="rl-admin-tender-bar">
                        <div className="rl-admin-tender-fill" style={{ width: '17.4%' }} />
                      </div>
                      <span className="rl-admin-tender-amount">RM 386.97 (19 txns)</span>
                    </div>
                  </div>
                </div>
              </div>

              {/* Product Mix Table */}
              <div className="rl-admin-data-block">
                <h3 className="rl-admin-table-title">Product performance</h3>
                <AdminTableFrame>
                <table className="rl-admin-table rl-admin-table--products">
                  <thead>
                    <tr>
                      <th>Product / Recipe</th>
                      <th>Category</th>
                      <th>Units Sold</th>
                      <th>Gross Revenue</th>
                      <th>Share of Sales</th>
                    </tr>
                  </thead>
                  <tbody>
                    {[
                      { name: 'Oat Flat White', category: 'Espresso Bar', units: 48, revenue: 768.00, share: '34.5%' },
                      { name: 'Pour Over (Ethiopia Guji)', category: 'Filter Bar', units: 24, revenue: 432.00, share: '19.4%' },
                      { name: 'Espresso (Artisan Blend)', category: 'Espresso Bar', units: 32, revenue: 384.00, share: '17.2%' },
                      { name: 'Almond Croissant', category: 'Bakery', units: 28, revenue: 364.00, share: '16.3%' },
                      { name: 'Iced Spanish Latte', category: 'Specialty', units: 16, revenue: 280.00, share: '12.6%' }
                    ].map(p => (
                      <tr key={p.name}>
                        <td><strong>{p.name}</strong></td>
                        <td>{p.category}</td>
                        <td>{p.units} cups</td>
                        <td><MoneyDisplay amount={p.revenue} size="sm" /></td>
                        <td className="rl-admin-table__numeric"><AdminStatusChip>{p.share}</AdminStatusChip></td>
                      </tr>
                    ))}
                  </tbody>
                </table>
                </AdminTableFrame>
              </div>
            </div>
          )}

          {/* INVENTORY WORKSPACE */}
          {activeNav === 'inventory' && (
            <div className="rl-admin-section">
              <AdminSectionHeader title="Stock levels" description="Automatic recipe depletion and reorder thresholds" />

              <AdminTableFrame>
              <table className="rl-admin-table rl-admin-table--inventory">
                <thead>
                  <tr>
                    <th>SKU / Code</th>
                    <th>Ingredient / Product</th>
                    <th>Category</th>
                    <th>Current Stock</th>
                    <th>Reorder Level</th>
                    <th>Unit Cost</th>
                    <th>Stock Health</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {stockItems.length === 0 && <AdminEmptyTableRow colSpan={8} message="No stock items configured." />}
                  {stockItems.map(item => {
                    const isLow = item.currentStock <= item.reorderThreshold;
                    return (
                      <tr key={item.id}>
                        <td><code>{item.sku}</code></td>
                        <td><strong>{item.name}</strong></td>
                        <td>{item.category}</td>
                        <td>
                          <strong>{item.currentStock.toLocaleString()} {item.unit}</strong>
                        </td>
                        <td>{item.reorderThreshold.toLocaleString()} {item.unit}</td>
                        <td><MoneyDisplay amount={item.costPerUnit} size="sm" /></td>
                        <td>
                          {!item.isActive ? (
                            <AdminStatusChip tone="neutral" dot>Archived</AdminStatusChip>
                          ) : isLow ? (
                            <AdminStatusChip tone="warning" dot>Low stock</AdminStatusChip>
                          ) : (
                            <AdminStatusChip tone="success" dot>Healthy</AdminStatusChip>
                          )}
                        </td>
                        <td>
                          <div className="rl-admin-table__actions">
                            <Button variant="outline" size="sm" onClick={() => openStockEditor(item)}>Edit</Button>
                            <Button variant="secondary" size="sm" onClick={() => toggleStockItemActive(item)}>
                              {item.isActive ? 'Archive' : 'Restore'}
                            </Button>
                          </div>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
              </AdminTableFrame>

              {/* Wastage Logs Section */}
              <div className="rl-admin-data-block">
                <h3 className="rl-admin-table-title">Recent wastage & dial-in logs ({wastageEntries.length})</h3>
                <AdminTableFrame>
                <table className="rl-admin-table rl-admin-table--wastage">
                  <thead>
                    <tr>
                      <th>Time</th>
                      <th>Ingredient</th>
                      <th>Quantity Wasted</th>
                      <th>Reason</th>
                      <th>Cost Impact (RM)</th>
                      <th>Logged By</th>
                      <th>Notes</th>
                    </tr>
                  </thead>
                  <tbody>
                    {wastageEntries.map(w => (
                      <tr key={w.id}>
                        <td>{w.time}</td>
                        <td><strong>{w.stockItemName}</strong></td>
                        <td>{w.quantityWasted} {w.unit}</td>
                        <td>
                          <AdminStatusChip tone={w.reason === 'CalibrationDialIn' ? 'info' : 'warning'}>
                            {w.reason}
                          </AdminStatusChip>
                        </td>
                        <td><MoneyDisplay amount={w.costImpact} size="sm" /></td>
                        <td>{w.loggedByName}</td>
                        <td className="rl-admin-table__sub">{w.notes || '—'}</td>
                      </tr>
                    ))}
                </tbody>
                </table>
                </AdminTableFrame>
              </div>
            </div>
          )}


          {/* MYINVOIS E-INVOICING WORKSPACE */}
          {activeNav === 'myinvois' && (
            <div className="rl-admin-section">
              <AdminSectionHeader title="Compliance status" description="LHDN validation and submission records" />

              <AdminMetricRow>
                <AdminMetric label="Validated e-Invoices" value="2 Documents" detail="LHDN UUID & QR issued" />
                <AdminMetric label="Total e-Invoiced value" value={<MoneyDisplay amount={312.70} size="lg" />} detail="6% SST: RM 17.70" />
                <AdminMetric label="Rejections / errors" value="0 Failed" detail="100% compliant schema" />
              </AdminMetricRow>

              {/* e-Invoice Documents Table */}
              <AdminTableFrame className="rl-admin-data-block">
                <table className="rl-admin-table rl-admin-table--invoices">
                  <thead>
                    <tr>
                      <th>Invoice #</th>
                      <th>Buyer / Company Name</th>
                      <th>Tax ID (TIN)</th>
                      <th>Amount (Excl.)</th>
                      <th>6% SST</th>
                      <th>Total Payable</th>
                      <th>LHDN Status</th>
                      <th>Validated At</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {myInvoisDocuments.map(doc => (
                      <tr key={doc.id}>
                        <td><strong>{doc.invoiceNumber}</strong></td>
                        <td>{doc.buyerName}</td>
                        <td><code>{doc.buyerTin}</code></td>
                        <td><MoneyDisplay amount={doc.totalExcludingTax} size="sm" /></td>
                        <td><MoneyDisplay amount={doc.totalTaxAmount} size="sm" /></td>
                        <td><MoneyDisplay amount={doc.totalPayable} size="sm" /></td>
                        <td>
                          <AdminStatusChip tone={doc.status === 'Valid' ? 'success' : doc.status === 'Submitted' ? 'info' : 'warning'}>
                            {doc.status}
                          </AdminStatusChip>
                        </td>
                        <td className="rl-admin-table__sub">{doc.validatedAt}</td>
                        <td>
                          <Button
                            variant="outline"
                            size="sm"
                            onClick={() => alert(`LHDN Verification URL:\nhttps://myinvois.hasil.gov.my/verify/${doc.uuid}`)}
                          >
                            <AdminIcon name="card" size={15} />
                            Verify QR
                          </Button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </AdminTableFrame>
            </div>
          )}

          {/* OUTLETS WORKSPACE */}

          {activeNav === 'outlets' && (
            <div className="rl-admin-section">
              <AdminSectionHeader
                title={`Configured outlets (${outlets.length})`}
                description="Store profiles, terminal assignments, tax and receipt settings"
              />
              <AdminTableFrame>
                <table className="rl-admin-table rl-admin-table--outlets">
                  <thead>
                    <tr>
                      <th>Outlet & address</th>
                      <th>Active terminals</th>
                      <th>Status</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {outlets.length === 0 && <AdminEmptyTableRow colSpan={4} message="No outlets configured." />}
                    {outlets.map(outlet => (
                      <tr key={outlet.id}>
                        <td>
                          <div className="rl-admin-outlet-name">
                            <strong>{outlet.name}</strong>
                            {outlet.isMainOutlet && <AdminStatusChip tone="info">Main flagship</AdminStatusChip>}
                          </div>
                          <div className="rl-admin-table__sub">{outlet.address}</div>
                        </td>
                        <td>{getActiveDeviceCount(outlet.id)} devices</td>
                        <td>
                          <AdminStatusChip tone={outlet.isActive ? 'success' : 'neutral'} dot>
                            {outlet.isActive ? 'Operational' : 'Inactive'}
                          </AdminStatusChip>
                        </td>
                        <td>
                          <div className="rl-admin-table__actions">
                            <Button variant="outline" size="sm" onClick={() => openEditOutlet(outlet)}>Edit profile</Button>
                            <Button
                              variant="secondary"
                              size="sm"
                              onClick={() => {
                                setSelectedOutletForSettings(outlet);
                                setIsOutletSettingsOpen(true);
                              }}
                            >
                              Tax & receipt
                            </Button>
                            <Button variant="secondary" size="sm" onClick={() => toggleOutletActive(outlet)}>
                              {outlet.isActive ? 'Deactivate' : 'Restore'}
                            </Button>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </AdminTableFrame>
            </div>
          )}

          {/* STAFF & ROLE ACCESS WORKSPACE */}
          {activeNav === 'staff' && (
            <div className="rl-admin-section">
              <AdminToolbar>
                <div className="rl-admin-tabs-toggle">
                  <button
                    type="button"
                    className={`rl-admin-tab-btn ${staffTab === 'list' ? 'rl-admin-tab-btn--active' : ''}`}
                    onClick={() => setStaffTab('list')}
                  >
                    Staff List ({staff.length})
                  </button>
                  <button
                    type="button"
                    className={`rl-admin-tab-btn ${staffTab === 'matrix' ? 'rl-admin-tab-btn--active' : ''}`}
                    onClick={() => setStaffTab('matrix')}
                  >
                    Role Permissions Matrix
                  </button>
                </div>
              </AdminToolbar>

              {staffTab === 'list' ? (
                <AdminTableFrame>
                <table className="rl-admin-table">
                  <thead>
                    <tr>
                      <th>Staff Member</th>
                      <th>Role</th>
                      <th>Assigned Outlets</th>
                      <th>Access</th>
                      <th>PIN Status</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {staff.length === 0 && <AdminEmptyTableRow colSpan={6} message="No staff accounts found." />}
                    {staff.map(member => (
                      <tr key={member.id}>
                        <td>
                          <div className="rl-admin-person">
                            <span className="rl-admin-person__initials" aria-hidden="true">
                              {member.name.split(' ').map(part => part[0]).join('').slice(0, 2)}
                            </span>
                            <div>
                              <strong>{member.name}</strong>
                              {member.email && <div className="rl-admin-table__sub">{member.email}</div>}
                            </div>
                          </div>
                        </td>
                        <td>
                          <AdminStatusChip tone={member.role === 'Owner' ? 'info' : member.role === 'Manager' ? 'warning' : 'neutral'}>
                            {member.role}
                          </AdminStatusChip>
                        </td>
                        <td>{member.assignedOutletIds.map(getOutletName).join(', ')}</td>
                        <td>
                          <AdminStatusChip tone={member.isActive ? 'success' : 'neutral'} dot>
                            {member.isActive ? 'Active' : 'Inactive'}
                          </AdminStatusChip>
                        </td>
                        <td>
                          <AdminStatusChip tone={member.hasPinSet ? 'success' : 'warning'} dot>
                            {member.hasPinSet ? 'PIN set' : 'PIN not set'}
                          </AdminStatusChip>
                        </td>
                        <td>
                          <div className="rl-admin-table__actions">
                            <Button variant="outline" size="sm" onClick={() => openStaffEditor(member)}>Edit</Button>
                            <Button variant="secondary" size="sm" onClick={() => openResetPin(member)}>Reset PIN</Button>
                            <Button variant="secondary" size="sm" onClick={() => toggleStaffActive(member)}>
                              {member.isActive ? 'Deactivate' : 'Restore'}
                            </Button>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
                </AdminTableFrame>
              ) : (
                <AdminTableFrame>
                <table className="rl-admin-table rl-admin-table--matrix">
                  <thead>
                    <tr>
                      <th>Permission / Capability</th>
                      <th>Owner</th>
                      <th>Manager</th>
                      <th>Cashier</th>
                      <th>Barista</th>
                    </tr>
                  </thead>
                  <tbody>
                    {PERMISSION_MATRIX.map((row, idx) => (
                      <tr key={idx}>
                        <td><strong>{row.permission}</strong></td>
                        <td className="rl-admin-table__center"><AdminPermissionMark allowed={row.owner} /></td>
                        <td className="rl-admin-table__center"><AdminPermissionMark allowed={row.manager} /></td>
                        <td className="rl-admin-table__center"><AdminPermissionMark allowed={row.cashier} /></td>
                        <td className="rl-admin-table__center"><AdminPermissionMark allowed={row.barista} /></td>
                      </tr>
                    ))}
                  </tbody>
                </table>
                </AdminTableFrame>
              )}
            </div>
          )}

          {/* DEVICES & HARDWARE WORKSPACE */}
          {activeNav === 'devices' && (
            <div className="rl-admin-section">
              <AdminSectionHeader
                title={`Registered terminals (${filteredDevices.length})`}
                description="POS registers and Kitchen Display System stations"
              />

              <AdminTableFrame>
              <table className="rl-admin-table">
                <thead>
                  <tr>
                    <th>Device Name</th>
                    <th>Terminal Type</th>
                    <th>Assigned Store</th>
                    <th>Status</th>
                    <th>Last Active</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredDevices.length === 0 && (
                    <AdminEmptyTableRow colSpan={6} message="No terminals match the selected store scope." />
                  )}
                  {filteredDevices.map(dev => (
                    <tr key={dev.id}>
                      <td><strong>{dev.name}</strong></td>
                      <td>
                        <AdminStatusChip tone={dev.deviceType === 'POS' ? 'info' : 'warning'}>
                          <AdminIcon name={dev.deviceType === 'POS' ? 'device' : 'ticket'} size={14} />
                          {dev.deviceType === 'POS' ? 'POS Register' : 'KDS Station'}
                        </AdminStatusChip>
                      </td>
                      <td>{getOutletName(dev.outletId)}</td>
                      <td>
                        <AdminStatusChip tone={!dev.isActive ? 'neutral' : dev.status === 'Enrolled' ? 'success' : 'warning'} dot>
                          {!dev.isActive ? 'Disabled' : dev.status === 'Enrolled' ? 'Enrolled' : 'Activation Code Generated'}
                        </AdminStatusChip>
                      </td>
                      <td>{dev.lastSeen}</td>
                      <td>
                        <Button variant="outline" size="sm" onClick={() => openDeviceConfig(dev)}>Configure</Button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
              </AdminTableFrame>
            </div>
          )}
        </main>
      </div>

      {/* Receive Stock Modal */}
      <Modal
        isOpen={isReceiveStockOpen}
        title="Receive Supplier Stock"
        onClose={() => setIsReceiveStockOpen(false)}
        footer={
          <>
            <Button variant="outline" onClick={() => setIsReceiveStockOpen(false)}>Cancel</Button>
            <Button variant="primary" onClick={confirmReceiveStock} disabled={!receiveQuantity || !receiveUnitCost}>
              Confirm Stock Receipt
            </Button>
          </>
        }
      >
        <div className="rl-admin-form-modal">
          <AdminField label="Ingredient / product">
            <select
              className="rl-admin-select-input"
              value={receiveItemId}
              onChange={e => {
                const nextItemId = e.target.value;
                const nextItem = activeStockItems.find(item => item.id === nextItemId);
                setReceiveItemId(nextItemId);
                setReceiveUnitCost(nextItem && nextItem.costPerUnit > 0 ? String(nextItem.costPerUnit) : '');
              }}
            >
              {activeStockItems.map(item => (
                <option key={item.id} value={item.id}>
                  {item.name} ({item.sku}) • Current: {item.currentStock} {item.unit}
                </option>
              ))}
            </select>
          </AdminField>

          <TextInput
            label="Quantity Received"
            placeholder="e.g. 10000 (Grams/Ml) or 20 (Pcs)"
            type="number"
            value={receiveQuantity}
            onChange={e => setReceiveQuantity(e.target.value)}
          />

          <TextInput
            label="Unit cost (RM)"
            placeholder="e.g. 0.085"
            type="number"
            min="0"
            step="0.001"
            inputMode="decimal"
            value={receiveUnitCost}
            onChange={e => setReceiveUnitCost(e.target.value)}
            helperText="The latest received cost is used for future wastage valuation."
          />

          <TextInput
            label="Supplier Invoice / PO Reference"
            placeholder="e.g. INV-20260819-01"
            value={receiveInvoice}
            onChange={e => setReceiveInvoice(e.target.value)}
          />
        </div>
      </Modal>

      {/* Record Wastage Modal */}
      <Modal
        isOpen={isRecordWastageOpen}
        title="Log Ingredient Wastage & Write-Down"
        onClose={() => setIsRecordWastageOpen(false)}
        footer={
          <>
            <Button variant="outline" onClick={() => setIsRecordWastageOpen(false)}>Cancel</Button>
            <Button variant="destructive" onClick={confirmRecordWastage} disabled={!wastageQuantity}>
              Write-Down Wastage
            </Button>
          </>
        }
      >
        <div className="rl-admin-form-modal">
          <AdminField label="Item">
            <select
              className="rl-admin-select-input"
              value={wastageItemId}
              onChange={e => setWastageItemId(e.target.value)}
            >
              {activeStockItems.map(item => (
                <option key={item.id} value={item.id}>
                  {item.name} ({item.sku}) • Stock: {item.currentStock} {item.unit}
                </option>
              ))}
            </select>
          </AdminField>

          <TextInput
            label="Quantity Wasted"
            placeholder="e.g. 150 (Grams)"
            type="number"
            value={wastageQuantity}
            onChange={e => setWastageQuantity(e.target.value)}
          />

          <AdminField label="Wastage reason">
            <select
              className="rl-admin-select-input"
              value={wastageReason}
              onChange={e => setWastageReason(e.target.value as WasteReason)}
            >
              <option value="CalibrationDialIn">Grinder Dial-In & Extraction Calibration</option>
              <option value="Spillage">Accidental Spillage / Dropped Item</option>
              <option value="Expired">Shelf Life Expired</option>
              <option value="QualityDefect">Quality Defect / Bean Defect</option>
              <option value="StaffTraining">Staff Latte Art / Dial-In Training</option>
            </select>
          </AdminField>

          <TextInput
            label="Notes / Barista Comment"
            placeholder="e.g. Morning recalibration for new harvest batch"
            value={wastageNotes}
            onChange={e => setWastageNotes(e.target.value)}
          />
        </div>
      </Modal>

      {/* Stock Item Editor Modal */}
      <Modal
        isOpen={isStockEditorOpen}
        title={editingStockItem ? 'Edit stock item' : 'Add stock item'}
        onClose={closeStockEditor}
        footer={
          <>
            <Button variant="outline" onClick={closeStockEditor}>Cancel</Button>
            <Button variant="primary" onClick={handleSaveStockItem}>
              {editingStockItem ? 'Save item' : 'Add item'}
            </Button>
          </>
        }
      >
        <div className="rl-admin-form-modal">
          <TextInput
            label="SKU / Code"
            placeholder="e.g. BEAN-HOUSE-02"
            value={stockForm.sku}
            onChange={e => setStockForm(prev => ({ ...prev, sku: e.target.value }))}
            error={stockFormError}
          />
          <TextInput
            label="Ingredient / product"
            placeholder="e.g. Colombia Single Origin"
            value={stockForm.name}
            onChange={e => setStockForm(prev => ({ ...prev, name: e.target.value }))}
          />
          <TextInput
            label="Category"
            placeholder="e.g. Coffee Beans"
            value={stockForm.category}
            onChange={e => setStockForm(prev => ({ ...prev, category: e.target.value }))}
          />
          <AdminField label="Unit">
            <select
              className="rl-admin-select-input"
              value={stockForm.unit}
              disabled={Boolean(editingStockItem)}
              onChange={e => setStockForm(prev => ({ ...prev, unit: e.target.value }))}
            >
              <option value="Grams">Grams</option>
              <option value="Milliliters">Milliliters</option>
              <option value="Pieces">Pieces</option>
            </select>
          </AdminField>
          <TextInput
            label="Reorder level"
            placeholder="e.g. 2000"
            type="number"
            min="0"
            step="1"
            value={stockForm.reorderThreshold}
            onChange={e => setStockForm(prev => ({ ...prev, reorderThreshold: e.target.value }))}
          />
          <p className="rl-admin-form-note">
            {editingStockItem
              ? 'Current stock, unit, unit cost, and movement history are protected.'
              : 'New items start at zero stock. Receive stock to set the current unit cost.'}
          </p>
        </div>
      </Modal>

      {/* Add / Edit Outlet Modal */}
      <Modal
        isOpen={isAddOutletOpen}
        title={editingOutlet ? 'Edit outlet profile' : 'Add new coffee store outlet'}
        onClose={closeOutletEditor}
        footer={
          <>
            <Button variant="outline" onClick={closeOutletEditor}>Cancel</Button>
            <Button variant="primary" onClick={handleSaveOutlet}>
              {editingOutlet ? 'Save profile' : 'Create outlet'}
            </Button>
          </>
        }
      >
        <div className="rl-admin-form-modal">
          <TextInput
            label="Outlet name"
            placeholder="e.g. Mont Kiara Branch"
            value={outletForm.name}
            onChange={e => setOutletForm(prev => ({ ...prev, name: e.target.value }))}
            error={outletFormError}
          />
          <TextInput
            label="Physical address"
            placeholder="e.g. 15 Jalan Kiara, Mont Kiara, KL"
            value={outletForm.address}
            onChange={e => setOutletForm(prev => ({ ...prev, address: e.target.value }))}
          />
          <label className="rl-admin-check-row">
            <input
              type="checkbox"
              checked={outletForm.isMainOutlet}
              onChange={e => setOutletForm(prev => ({ ...prev, isMainOutlet: e.target.checked }))}
            />
            <span>Make this the main outlet</span>
          </label>
          <p className="rl-admin-form-note">Outlet relationships use a stable ID, so profile edits do not break staff or terminal assignments.</p>
        </div>
      </Modal>

      {/* Outlet Tax & Receipt Read-only Settings */}
      <Modal
        isOpen={isOutletSettingsOpen}
        title={`Tax & receipt · ${selectedOutletForSettings?.name ?? 'Outlet'}`}
        onClose={() => {
          setIsOutletSettingsOpen(false);
          setSelectedOutletForSettings(null);
        }}
        footer={
          <Button
            variant="outline"
            onClick={() => {
              setIsOutletSettingsOpen(false);
              setSelectedOutletForSettings(null);
            }}
          >
            Close
          </Button>
        }
      >
        <div className="rl-admin-form-modal">
          <div className="rl-admin-readonly-row">
            <span>SST calculation</span>
            <strong>6% Malaysian SST</strong>
          </div>
          <div className="rl-admin-readonly-row">
            <span>Receipt currency</span>
            <strong>MYR (RM)</strong>
          </div>
          <p className="rl-admin-form-note">Tax and receipt mutations stay outside this local prototype until the outlet configuration API is connected.</p>
        </div>
      </Modal>

      {/* Staff Editor Modal */}
      <Modal
        isOpen={isStaffEditorOpen}
        title={editingStaff ? 'Edit staff access' : 'Add staff account'}
        onClose={closeStaffEditor}
        footer={
          <>
            <Button variant="outline" onClick={closeStaffEditor}>Cancel</Button>
            <Button variant="primary" onClick={handleSaveStaff}>
              {editingStaff ? 'Save access' : 'Add staff'}
            </Button>
          </>
        }
      >
        <div className="rl-admin-form-modal">
          <TextInput
            label="Full name"
            placeholder="e.g. Hana Shift Lead"
            value={staffForm.name}
            onChange={e => setStaffForm(prev => ({ ...prev, name: e.target.value }))}
            error={staffFormError}
          />
          <TextInput
            label="Email"
            type="email"
            placeholder="e.g. hana@artisanroast.my"
            value={staffForm.email}
            onChange={e => setStaffForm(prev => ({ ...prev, email: e.target.value }))}
          />
          <AdminField label="Role">
            <select
              className="rl-admin-select-input"
              value={staffForm.role}
              onChange={e => setStaffForm(prev => ({ ...prev, role: e.target.value as StaffRole }))}
            >
              <option value="Owner">Owner</option>
              <option value="Manager">Manager</option>
              <option value="Cashier">Cashier</option>
              <option value="Barista">Barista</option>
            </select>
          </AdminField>
          {staffForm.role === 'Owner' ? (
            <div className="rl-admin-readonly-row">
              <span>Outlet access</span>
              <strong>All outlets</strong>
            </div>
          ) : (
            <AdminField label="Assigned outlet">
              <select
                className="rl-admin-select-input"
                value={staffForm.outletId}
                onChange={e => setStaffForm(prev => ({ ...prev, outletId: e.target.value }))}
              >
                {outlets.filter(outlet => outlet.isActive || outlet.id === staffForm.outletId).map(outlet => (
                  <option key={outlet.id} value={outlet.id}>{outlet.name}</option>
                ))}
              </select>
            </AdminField>
          )}
          <p className="rl-admin-form-note">PINs are set separately and are never shown or stored in this profile form.</p>
        </div>
      </Modal>

      {/* Enroll Device Modal */}
      <Modal
        isOpen={isEnrollDeviceOpen}
        title="Enroll New Terminal Device"
        onClose={() => setIsEnrollDeviceOpen(false)}
        footer={
          !generatedCode ? (
            <>
              <Button variant="outline" onClick={() => setIsEnrollDeviceOpen(false)}>Cancel</Button>
              <Button variant="primary" onClick={handleGenerateCode}>Generate Activation Code</Button>
            </>
          ) : (
            <Button variant="primary" onClick={() => setIsEnrollDeviceOpen(false)}>Done</Button>
          )
        }
      >
        {!generatedCode ? (
          <>
            <TextInput
              label="Terminal Identifier"
              placeholder="e.g. Counter 2 POS"
              value={newDeviceName}
              onChange={e => setNewDeviceName(e.target.value)}
            />
            <AdminField label="Device type">
              <select
                className="rl-admin-select-input"
                value={newDeviceType}
                onChange={e => setNewDeviceType(e.target.value as 'POS' | 'KDS')}
              >
                <option value="POS">POS Register (Cashier)</option>
                <option value="KDS">KDS (Kitchen / Barista Rail)</option>
              </select>
            </AdminField>
            <AdminField label="Assign to store">
              <select
                className="rl-admin-select-input"
                value={newDeviceOutletId}
                onChange={e => setNewDeviceOutletId(e.target.value)}
              >
                {activeOutlets.map(outlet => (
                  <option key={outlet.id} value={outlet.id}>{outlet.name}</option>
                ))}
              </select>
            </AdminField>
          </>
        ) : (
          <div style={{ textAlign: 'center', padding: '16px 0' }}>
            <p>Enter this activation code on the new physical device to complete enrollment:</p>
            <div className="rl-admin-enrollment-code">{generatedCode}</div>
            <p style={{ fontSize: '0.8125rem', color: 'var(--admin-muted)' }}>Valid for 15 minutes. Bound to {getOutletName(newDeviceOutletId)}.</p>
          </div>
        )}
      </Modal>

      {/* Device Configuration Modal */}
      <Modal
        isOpen={isDeviceConfigOpen}
        title={`Configure terminal · ${editingDevice?.name ?? 'Device'}`}
        onClose={closeDeviceConfig}
        footer={
          <>
            <Button variant="outline" onClick={closeDeviceConfig}>Cancel</Button>
            <Button variant="primary" onClick={handleSaveDevice}>Save configuration</Button>
          </>
        }
      >
        <div className="rl-admin-form-modal">
          <TextInput
            label="Terminal name"
            placeholder="e.g. Counter 2 POS"
            value={deviceForm.name}
            onChange={e => setDeviceForm(prev => ({ ...prev, name: e.target.value }))}
            error={deviceFormError}
          />
          <AdminField label="Terminal type">
            <select
              className="rl-admin-select-input"
              value={deviceForm.deviceType}
              onChange={e => setDeviceForm(prev => ({ ...prev, deviceType: e.target.value as 'POS' | 'KDS' }))}
            >
              <option value="POS">POS Register (Cashier)</option>
              <option value="KDS">KDS (Kitchen / Barista Rail)</option>
            </select>
          </AdminField>
          <AdminField label="Assigned outlet">
            <select
              className="rl-admin-select-input"
              value={deviceForm.outletId}
              onChange={e => setDeviceForm(prev => ({ ...prev, outletId: e.target.value }))}
            >
              {outlets.filter(outlet => outlet.isActive || outlet.id === deviceForm.outletId).map(outlet => (
                <option key={outlet.id} value={outlet.id}>{outlet.name}</option>
              ))}
            </select>
          </AdminField>
          <AdminField label="Terminal access">
            <select
              className="rl-admin-select-input"
              value={deviceForm.isActive ? 'active' : 'disabled'}
              onChange={e => setDeviceForm(prev => ({ ...prev, isActive: e.target.value === 'active' }))}
            >
              <option value="active">Enabled</option>
              <option value="disabled">Disabled</option>
            </select>
          </AdminField>
          <p className="rl-admin-form-note">Disabling a terminal is reversible. Enrollment status and activation history are kept.</p>
        </div>
      </Modal>

      {/* Reset PIN Modal */}
      <Modal
        isOpen={isResetPinOpen}
        title={`Reset PIN for ${selectedStaffForPin?.name ?? 'staff member'}`}
        onClose={closeResetPin}
        footer={
          <>
            <Button variant="outline" onClick={closeResetPin}>Cancel</Button>
            <Button variant="primary" onClick={handleSavePin} disabled={!newPinCode}>Save new PIN</Button>
          </>
        }
      >
        <p className="rl-admin-form-note">Set a new 4-digit numeric PIN code for register quick-switching. The value is used only for this form submission and is not stored.</p>
        <TextInput
          label="New 4-digit PIN"
          placeholder="e.g. 4321"
          type="password"
          inputMode="numeric"
          pattern="[0-9]*"
          autoComplete="new-password"
          maxLength={4}
          value={newPinCode}
          onChange={e => setNewPinCode(e.target.value.replace(/\D/g, '').slice(0, 4))}
          error={pinFormError}
        />
      </Modal>
    </div>
  );
};
