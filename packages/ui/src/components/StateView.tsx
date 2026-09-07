import React from 'react';
import { Button } from './Button';
import './StateView.css';

export type StateViewKind = 'loading' | 'empty' | 'error' | 'offline' | 'review-required' | 'permission-denied';

export interface StateViewProps {
  kind: StateViewKind;
  title: string;
  description: string;
  actionLabel?: string;
  onAction?: () => void;
  secondaryActionLabel?: string;
  onSecondaryAction?: () => void;
  className?: string;
}

export const StateView: React.FC<StateViewProps> = ({
  kind,
  title,
  description,
  actionLabel,
  onAction,
  secondaryActionLabel,
  onSecondaryAction,
  className = ''
}) => {
  const getIcon = () => {
    switch (kind) {
      case 'loading':
        return <span className="rl-stateview__spinner" aria-hidden="true" />;
      case 'offline':
        return <span className="rl-stateview__symbol" aria-hidden="true">📡</span>;
      case 'review-required':
        return <span className="rl-stateview__symbol" aria-hidden="true">⚠️</span>;
      case 'error':
      case 'permission-denied':
        return <span className="rl-stateview__symbol" aria-hidden="true">🚫</span>;
      case 'empty':
      default:
        return <span className="rl-stateview__symbol" aria-hidden="true">☕</span>;
    }
  };

  return (
    <div className={`rl-stateview rl-stateview--${kind} ${className}`} role="status">
      <div className="rl-stateview__icon-box">
        {getIcon()}
      </div>
      <h3 className="rl-stateview__title">{title}</h3>
      <p className="rl-stateview__description">{description}</p>

      {(actionLabel || secondaryActionLabel) && (
        <div className="rl-stateview__actions">
          {actionLabel && onAction && (
            <Button
              variant={kind === 'error' ? 'destructive' : kind === 'review-required' ? 'secondary' : 'primary'}
              size="md"
              onClick={onAction}
            >
              {actionLabel}
            </Button>
          )}
          {secondaryActionLabel && onSecondaryAction && (
            <Button
              variant="outline"
              size="md"
              onClick={onSecondaryAction}
            >
              {secondaryActionLabel}
            </Button>
          )}
        </div>
      )}
    </div>
  );
};
