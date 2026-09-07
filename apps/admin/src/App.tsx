import React from 'react';
import { AdminShell } from './components/AdminShell';
import './App.css';

export const App: React.FC = () => {
  return (
    <div className="rl-admin-app">
      <AdminShell />
    </div>
  );
};
