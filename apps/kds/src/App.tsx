import React from 'react';
import { KdsShell } from './components/KdsShell';
import './App.css';

export const App: React.FC = () => {
  return (
    <div className="rl-kds-app">
      <KdsShell />
    </div>
  );
};
