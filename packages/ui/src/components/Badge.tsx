import React, { HTMLAttributes } from 'react';
import './Badge.css';

export type BadgeTone = 'neutral' | 'success' | 'warning' | 'danger' | 'info' | 'syncing';

export interface BadgeProps extends HTMLAttributes<HTMLSpanElement> {
  tone?: BadgeTone;
  icon?: React.ReactNode;
  isDot?: boolean;
}

export const Badge: React.FC<BadgeProps> = ({
  children,
  tone = 'neutral',
  icon,
  isDot = false,
  className = '',
  ...props
}) => {
  const classes = [
    'rl-badge',
    `rl-badge--${tone}`,
    className
  ].filter(Boolean).join(' ');

  return (
    <span className={classes} {...props}>
      {isDot && <span className="rl-badge__dot" aria-hidden="true" />}
      {icon && <span className="rl-badge__icon">{icon}</span>}
      <span className="rl-badge__text">{children}</span>
    </span>
  );
};
