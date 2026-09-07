import React, { ButtonHTMLAttributes, forwardRef } from 'react';
import './Button.css';

export type ButtonVariant = 'primary' | 'secondary' | 'destructive' | 'outline' | 'ghost';
export type ButtonSize = 'sm' | 'md' | 'lg' | 'pos-touch';

export interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant;
  size?: ButtonSize;
  isLoading?: boolean;
  leftIcon?: React.ReactNode;
  rightIcon?: React.ReactNode;
}

export const Button = forwardRef<HTMLButtonElement, ButtonProps>(({
  children,
  variant = 'primary',
  size = 'md',
  isLoading = false,
  disabled = false,
  leftIcon,
  rightIcon,
  className = '',
  ...props
}, ref) => {
  const classes = [
    'rl-button',
    `rl-button--${variant}`,
    `rl-button--${size}`,
    isLoading ? 'rl-button--loading' : '',
    className
  ].filter(Boolean).join(' ');

  return (
    <button
      ref={ref}
      className={classes}
      disabled={disabled || isLoading}
      aria-busy={isLoading}
      {...props}
    >
      {isLoading ? (
        <span className="rl-button__spinner" aria-hidden="true" />
      ) : (
        leftIcon && <span className="rl-button__icon rl-button__icon--left">{leftIcon}</span>
      )}
      <span className="rl-button__label">{children}</span>
      {!isLoading && rightIcon && (
        <span className="rl-button__icon rl-button__icon--right">{rightIcon}</span>
      )}
    </button>
  );
});

Button.displayName = 'Button';
