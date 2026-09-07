import React from 'react';
import './MoneyDisplay.css';

export interface MoneyDisplayProps {
  amount: number | string;
  currency?: string;
  size?: 'sm' | 'md' | 'lg' | 'hero';
  showSign?: boolean;
  className?: string;
}

export const MoneyDisplay: React.FC<MoneyDisplayProps> = ({
  amount,
  currency = 'RM',
  size = 'md',
  showSign = false,
  className = ''
}) => {
  const numericAmount = typeof amount === 'string' ? parseFloat(amount) : amount;
  const isNegative = numericAmount < 0;
  const absAmount = Math.abs(isNaN(numericAmount) ? 0 : numericAmount);

  const formattedNumber = absAmount.toLocaleString('en-MY', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  });

  const [whole, decimals] = formattedNumber.split('.');

  const classes = [
    'rl-money',
    `rl-money--${size}`,
    isNegative ? 'rl-money--negative' : '',
    className
  ].filter(Boolean).join(' ');

  return (
    <span className={classes}>
      {isNegative && <span className="rl-money__sign">-</span>}
      {!isNegative && showSign && numericAmount > 0 && <span className="rl-money__sign">+</span>}
      <span className="rl-money__currency">{currency}&nbsp;</span>
      <span className="rl-money__whole">{whole}</span>
      <span className="rl-money__decimal">.{decimals}</span>
    </span>
  );
};
