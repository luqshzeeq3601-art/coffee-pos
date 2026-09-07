import React, { type ReactNode } from 'react';
import { Badge } from '@coffee-pos/ui';

type StatusTone = 'neutral' | 'success' | 'warning' | 'danger' | 'info';

interface AdminPageHeaderProps {
  title: string;
  scopeValue: string;
  onScopeChange: (value: string) => void;
  scopeOptions?: Array<{ value: string; label: string }>;
  actions?: ReactNode;
}

export const AdminPageHeader: React.FC<AdminPageHeaderProps> = ({
  title,
  scopeValue,
  onScopeChange,
  scopeOptions,
  actions
}) => {
  const options = scopeOptions ?? [
    { value: 'all', label: 'All Stores (Global)' },
    { value: 'Bangsar Flagship', label: 'Bangsar Flagship' },
    { value: 'Damansara Heights', label: 'Damansara Heights' }
  ];

  return (
    <header className="rl-admin-topbar">
      <h1 className="rl-admin-topbar__title">{title}</h1>
      <div className="rl-admin-topbar__actions">
        <label className="rl-admin-outlet-select">
          <span className="rl-admin-outlet-select__label">Store scope</span>
          <select
            aria-label="Store scope"
            className="rl-admin-outlet-select__dropdown"
            value={scopeValue}
            onChange={event => onScopeChange(event.target.value)}
          >
            {options.map(option => (
              <option key={option.value} value={option.value}>{option.label}</option>
            ))}
          </select>
        </label>
        {actions && <div className="rl-admin-topbar__page-actions">{actions}</div>}
      </div>
    </header>
  );
};

interface AdminSectionHeaderProps {
  title: string;
  description?: string;
  actions?: ReactNode;
}

export const AdminSectionHeader: React.FC<AdminSectionHeaderProps> = ({ title, description, actions }) => (
  <header className="rl-admin-section__header">
    <div>
      <h2>{title}</h2>
      {description && <p>{description}</p>}
    </div>
    {actions && <div className="rl-admin-section__actions">{actions}</div>}
  </header>
);

export const AdminToolbar: React.FC<{ children: ReactNode }> = ({ children }) => (
  <div className="rl-admin-toolbar">{children}</div>
);

export const AdminMetricRow: React.FC<{ children: ReactNode }> = ({ children }) => (
  <div className="rl-admin-kpi-grid">{children}</div>
);

interface AdminMetricProps {
  label: string;
  value: ReactNode;
  detail: string;
}

export const AdminMetric: React.FC<AdminMetricProps> = ({ label, value, detail }) => (
  <div className="rl-admin-kpi-card">
    <span className="rl-admin-kpi-card__label">{label}</span>
    <div className="rl-admin-kpi-card__val">{value}</div>
    <span className="rl-admin-kpi-card__sub">{detail}</span>
  </div>
);

export const AdminTableFrame: React.FC<{ children: ReactNode; className?: string }> = ({ children, className = '' }) => (
  <div className={`rl-admin-table-container ${className}`.trim()}>{children}</div>
);

export const AdminStatusChip: React.FC<{ children: ReactNode; tone?: StatusTone; dot?: boolean }> = ({
  children,
  tone = 'neutral',
  dot = false
}) => (
  <Badge tone={tone} isDot={dot}>{children}</Badge>
);

export const AdminEmptyTableRow: React.FC<{ colSpan: number; message: string }> = ({ colSpan, message }) => (
  <tr>
    <td className="rl-admin-table__empty" colSpan={colSpan}>{message}</td>
  </tr>
);

export const AdminPermissionMark: React.FC<{ allowed: boolean }> = ({ allowed }) => (
  <span className={allowed ? 'rl-admin-permission rl-admin-permission--allowed' : 'rl-admin-permission'}>
    <span aria-hidden="true">{allowed ? '\u2713' : '\u2014'}</span>
    <span className="rl-admin-sr-only">{allowed ? 'Allowed' : 'Not allowed'}</span>
  </span>
);

interface AdminFieldProps {
  label: string;
  children: ReactNode;
}

export const AdminField: React.FC<AdminFieldProps> = ({ label, children }) => (
  <label className="rl-admin-field">
    <span className="rl-admin-field__label">{label}</span>
    {children}
  </label>
);
