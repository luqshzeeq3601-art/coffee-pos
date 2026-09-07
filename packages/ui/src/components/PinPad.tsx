import React, { useEffect, useState } from 'react';
import './PinPad.css';

export interface PinPadProps {
  pinLength?: number;
  variant?: 'default' | 'premium';
  title?: string;
  subtitle?: string;
  error?: string | null;
  isLoading?: boolean;
  onComplete: (pin: string) => void;
  onCancel?: () => void;
}

export const PinPad: React.FC<PinPadProps> = ({
  pinLength = 4,
  variant = 'default',
  title = 'Enter Staff PIN',
  subtitle = 'Punch your 4-digit PIN to unlock register',
  error = null,
  isLoading = false,
  onComplete,
  onCancel
}) => {
  const [pin, setPin] = useState<string>('');

  const handleDigit = (digit: string) => {
    if (isLoading || pin.length >= pinLength) return;
    const newPin = pin + digit;
    setPin(newPin);

    if (newPin.length === pinLength) {
      onComplete(newPin);
    }
  };

  const handleBackspace = () => {
    if (isLoading) return;
    setPin(prev => prev.slice(0, -1));
  };

  const handleClear = () => {
    if (isLoading) return;
    setPin('');
  };

  useEffect(() => {
    if (variant !== 'premium') return;

    const handleKeyboardInput = (event: KeyboardEvent) => {
      const target = event.target as HTMLElement | null;
      if (target?.matches('input, textarea, select, [contenteditable="true"]')) return;

      if (/^\d$/.test(event.key)) {
        event.preventDefault();
        handleDigit(event.key);
      } else if (event.key === 'Backspace') {
        event.preventDefault();
        handleBackspace();
      } else if (event.key === 'Delete' || event.key === 'Escape') {
        event.preventDefault();
        handleClear();
      }
    };

    window.addEventListener('keydown', handleKeyboardInput);
    return () => window.removeEventListener('keydown', handleKeyboardInput);
  }, [isLoading, onComplete, pin, pinLength, variant]);

  return (
    <div
      className={`rl-pinpad ${variant === 'premium' ? 'rl-pinpad--premium' : ''}`}
      role="region"
      aria-label="Staff PIN Pad"
      aria-busy={isLoading}
    >
      <div className="rl-pinpad__header">
        {variant === 'premium' && (
          <div className="rl-pinpad__ornament" aria-hidden="true">
            <span />
            <svg viewBox="0 0 24 24" focusable="false">
              <path d="M12 20c-1.5-5.8.4-11 5.8-15.5.5 6.5-1.4 11.6-5.8 15.5Z" />
              <path d="M12 20C8 16.8 6 12.4 6.4 6.8c4.7 3.2 6.6 7.7 5.6 13.2ZM12 20c-2.6-.2-5.1.5-7.5 2 2.7-4.3 5.2-5 7.5-2Z" />
            </svg>
            <span />
          </div>
        )}
        <h3 className="rl-pinpad__title">{title}</h3>
        {subtitle && <p className="rl-pinpad__subtitle">{subtitle}</p>}
      </div>

      {/* Masked Dots */}
      <div className="rl-pinpad__dots" role="status" aria-label={`Entered ${pin.length} of ${pinLength} digits`}>
        {Array.from({ length: pinLength }).map((_, index) => (
          <span
            key={index}
            className={`rl-pinpad__dot ${index < pin.length ? 'rl-pinpad__dot--filled' : ''} ${error ? 'rl-pinpad__dot--error' : ''}`}
          />
        ))}
      </div>

      {error && <div className="rl-pinpad__error" role="alert">{error}</div>}

      {/* 3x4 Grid */}
      <div className="rl-pinpad__grid">
        {['1', '2', '3', '4', '5', '6', '7', '8', '9'].map(digit => (
          <button
            key={digit}
            type="button"
            className="rl-pinpad__key"
            disabled={isLoading}
            onClick={() => handleDigit(digit)}
          >
            {digit}
          </button>
        ))}

        <button
          type="button"
          className="rl-pinpad__key rl-pinpad__key--util"
          disabled={isLoading || pin.length === 0}
          onClick={handleClear}
          aria-label="Clear PIN"
        >
          C
        </button>

        <button
          type="button"
          className="rl-pinpad__key"
          disabled={isLoading}
          onClick={() => handleDigit('0')}
        >
          0
        </button>

        <button
          type="button"
          className="rl-pinpad__key rl-pinpad__key--util"
          disabled={isLoading || pin.length === 0}
          onClick={handleBackspace}
        aria-label="Backspace"
      >
        {variant === 'premium' ? (
          <svg className="rl-pinpad__backspace-icon" viewBox="0 0 28 20" aria-hidden="true" focusable="false">
            <path d="M10 2h14.25A1.75 1.75 0 0 1 26 3.75v12.5A1.75 1.75 0 0 1 24.25 18H10L2 10l8-8Z" />
            <path d="m15 7 6 6m0-6-6 6" />
          </svg>
        ) : '⌫'}
        </button>
      </div>

      {onCancel && (
        <button
          type="button"
          className="rl-pinpad__cancel"
          disabled={isLoading}
          onClick={onCancel}
        >
          Cancel
        </button>
      )}
    </div>
  );
};
