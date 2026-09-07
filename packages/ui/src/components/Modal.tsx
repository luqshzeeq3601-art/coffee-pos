import React, { useEffect } from 'react';
import './Modal.css';

export interface ModalProps {
  isOpen: boolean;
  title: string;
  onClose: () => void;
  children: React.ReactNode;
  footer?: React.ReactNode;
  maxWidth?: 'sm' | 'md' | 'lg';
  className?: string;
}

export const Modal: React.FC<ModalProps> = ({
  isOpen,
  title,
  onClose,
  children,
  footer,
  maxWidth = 'md',
  className = ''
}) => {
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape' && isOpen) {
        onClose();
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  return (
    <div className="rl-modal-backdrop" onClick={onClose} role="presentation">
      <div
        className={`rl-modal rl-modal--${maxWidth} ${className}`.trim()}
        onClick={e => e.stopPropagation()}
        role="dialog"
        aria-modal="true"
        aria-labelledby="rl-modal-title"
      >
        <header className="rl-modal__header">
          <h2 id="rl-modal-title" className="rl-modal__title">{title}</h2>
          <button
            type="button"
            className="rl-modal__close"
            onClick={onClose}
            aria-label="Close dialog"
          >
            <svg className="rl-modal__close-icon" viewBox="0 0 24 24" fill="none" aria-hidden="true" focusable="false">
              <path d="m6 6 12 12M18 6 6 18" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" />
            </svg>
          </button>
        </header>

        <div className="rl-modal__content">
          {children}
        </div>

        {footer && (
          <footer className="rl-modal__footer">
            {footer}
          </footer>
        )}
      </div>
    </div>
  );
};
