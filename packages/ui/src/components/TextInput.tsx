import React, { InputHTMLAttributes, forwardRef } from 'react';
import './TextInput.css';

export interface TextInputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
  helperText?: string;
  leftAddon?: React.ReactNode;
  rightAddon?: React.ReactNode;
}

export const TextInput = forwardRef<HTMLInputElement, TextInputProps>(({
  label,
  error,
  helperText,
  leftAddon,
  rightAddon,
  id,
  className = '',
  disabled,
  ...props
}, ref) => {
  const inputId = id || (label ? `input-${label.toLowerCase().replace(/\s+/g, '-')}` : undefined);

  return (
    <div className={`rl-input-group ${disabled ? 'rl-input-group--disabled' : ''} ${className}`}>
      {label && (
        <label htmlFor={inputId} className="rl-input-label">
          {label}
        </label>
      )}
      <div className={`rl-input-wrapper ${error ? 'rl-input-wrapper--error' : ''}`}>
        {leftAddon && <span className="rl-input-addon rl-input-addon--left">{leftAddon}</span>}
        <input
          ref={ref}
          id={inputId}
          disabled={disabled}
          className="rl-input"
          aria-invalid={Boolean(error)}
          aria-describedby={error ? `${inputId}-error` : helperText ? `${inputId}-helper` : undefined}
          {...props}
        />
        {rightAddon && <span className="rl-input-addon rl-input-addon--right">{rightAddon}</span>}
      </div>
      {error && <span id={`${inputId}-error`} className="rl-input-error" role="alert">{error}</span>}
      {!error && helperText && <span id={`${inputId}-helper`} className="rl-input-helper">{helperText}</span>}
    </div>
  );
});

TextInput.displayName = 'TextInput';
