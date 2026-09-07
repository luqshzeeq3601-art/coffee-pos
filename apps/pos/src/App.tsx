import React, { useState } from 'react';
import { PinPad } from '@coffee-pos/ui';
import { PosShell } from './components/PosShell';
import './App.css';

interface StaffSession {
  id: string;
  name: string;
  role: string;
}

const LoginCoffeeMark: React.FC = () => (
  <svg viewBox="0 0 88 92" aria-hidden="true" focusable="false">
    <circle cx="44" cy="42" r="36" />
    <path d="M29 38h31v11.5A13.5 13.5 0 0 1 46.5 63h-4A13.5 13.5 0 0 1 29 49.5V38Z" />
    <path d="M60 42h5.5a7 7 0 0 1 0 14H60M25 68h39" />
    <path d="M37 31c-4-5 4-6 0-11M45 31c-4-5 4-6 0-11M53 31c-4-5 4-6 0-11" />
    <circle cx="44" cy="80" r="5.5" />
    <path d="M42 77.5c3 1.2 3 3.8 0 5M49.5 81c8.5-.3 13.5-3.5 17-10.5-8-.2-13.5 3-17 10.5Z" />
  </svg>
);

export const App: React.FC = () => {
  const [session, setSession] = useState<StaffSession | null>({
    id: 'emp-cashier-01',
    name: 'Ahmad Cashier',
    role: 'Cashier'
  });
  const [isLocked, setIsLocked] = useState<boolean>(false);
  const [pinError, setPinError] = useState<string | null>(null);

  const handlePinUnlock = (pin: string) => {
    if (pin === '5678') {
      setSession({
        id: 'emp-cashier-01',
        name: 'Ahmad Cashier',
        role: 'Cashier'
      });
      setIsLocked(false);
      setPinError(null);
    } else if (pin === '1234') {
      setSession({
        id: 'emp-manager-01',
        name: 'Siti Manager',
        role: 'Manager'
      });
      setIsLocked(false);
      setPinError(null);
    } else {
      setPinError('Invalid PIN. Please re-enter 4-digit code.');
    }
  };

  return (
    <div className="rl-pos-app">
      {isLocked ? (
        <div className="rl-pos-lockscreen">
          <div className="rl-pos-lockscreen__card">
            <div className="rl-pos-lockscreen__brand">
              <span className="rl-pos-lockscreen__logo" aria-hidden="true">
                <LoginCoffeeMark />
              </span>
              <h1 className="rl-pos-lockscreen__title">Roast Ledger POS</h1>
              <p className="rl-pos-lockscreen__outlet">Bangsar Flagship • Register #01</p>
            </div>

            <PinPad
              variant="premium"
              title="Punch Staff PIN"
              subtitle="Cashier (5678) or Manager (1234)"
              error={pinError}
              onComplete={handlePinUnlock}
            />
          </div>
        </div>
      ) : (
        <PosShell
          currentCashierName={session?.name}
          currentCashierRole={session?.role}
          outletName="Bangsar Flagship"
          onLockSession={() => setIsLocked(true)}
        />
      )}
    </div>
  );
};
