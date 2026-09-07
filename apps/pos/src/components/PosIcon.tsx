import React from 'react';

export type PosIconName =
  | 'map-pin'
  | 'cart'
  | 'ticket'
  | 'clock'
  | 'lock'
  | 'search'
  | 'sliders'
  | 'heart'
  | 'cup'
  | 'bag'
  | 'user'
  | 'table'
  | 'minus'
  | 'plus'
  | 'trash'
  | 'pause'
  | 'card'
  | 'cash'
  | 'arrows'
  | 'cloud'
  | 'monitor'
  | 'check'
  | 'warning'
  | 'receipt'
  | 'printer'
  | 'chevron-down'
  | 'close';

export interface PosIconProps {
  name: PosIconName;
  size?: number;
  strokeWidth?: number;
  className?: string;
}

export const PosIcon: React.FC<PosIconProps> = ({
  name,
  size = 22,
  strokeWidth = 1.8,
  className = ''
}) => {
  const common = {
    fill: 'none',
    stroke: 'currentColor',
    strokeWidth,
    strokeLinecap: 'round' as const,
    strokeLinejoin: 'round' as const
  };

  let shape: React.ReactNode;

  switch (name) {
    case 'map-pin':
      shape = <><path {...common} d="M19 10c0 5-7 11-7 11S5 15 5 10a7 7 0 1 1 14 0Z" /><circle {...common} cx="12" cy="10" r="2.2" /></>;
      break;
    case 'cart':
      shape = <><path {...common} d="M3.5 4h2l1.4 9.2a2 2 0 0 0 2 1.7h6.7a2 2 0 0 0 1.9-1.4L19 7H6.2" /><circle {...common} cx="9" cy="18.3" r="1" /><circle {...common} cx="16.5" cy="18.3" r="1" /><path {...common} d="M8 10.2h8.4" /></>;
      break;
    case 'ticket':
      shape = <><path {...common} d="M4 5.5A2.5 2.5 0 0 1 6.5 3h11A2.5 2.5 0 0 1 20 5.5v2a2.5 2.5 0 0 0 0 5v2a2.5 2.5 0 0 1-2.5 2.5h-11A2.5 2.5 0 0 1 4 14.5v-2a2.5 2.5 0 0 0 0-5v-2Z" /><path {...common} d="M9 6v2m0 3v2m0 3v1M14 7h3m-3 4h3m-3 4h3" /></>;
      break;
    case 'clock':
      shape = <><circle {...common} cx="12" cy="12" r="8.8" /><path {...common} d="M12 7v5l3.5 2" /></>;
      break;
    case 'lock':
      shape = <><rect {...common} x="5.2" y="10" width="13.6" height="10" rx="1.7" /><path {...common} d="M8 10V7.6a4 4 0 0 1 8 0V10" /><circle {...common} cx="12" cy="15" r="1" /></>;
      break;
    case 'search':
      shape = <><circle {...common} cx="10.5" cy="10.5" r="6.5" /><path {...common} d="m16 16 5 5" /></>;
      break;
    case 'sliders':
      shape = <><path {...common} d="M4 6h16M4 12h16M4 18h16" /><circle {...common} cx="8" cy="6" r="1.8" /><circle {...common} cx="15" cy="12" r="1.8" /><circle {...common} cx="10" cy="18" r="1.8" /></>;
      break;
    case 'heart':
      shape = <path {...common} d="M20.7 8.7c0 5.1-8.7 10.3-8.7 10.3S3.3 13.8 3.3 8.7A4.5 4.5 0 0 1 12 6.5a4.5 4.5 0 0 1 8.7 2.2Z" />;
      break;
    case 'cup':
      shape = <><path {...common} d="M4 7h13v5.5A4.5 4.5 0 0 1 12.5 17h-4A4.5 4.5 0 0 1 4 12.5V7Z" /><path {...common} d="M17 9h1.5a2.5 2.5 0 0 1 0 5H17M3 20h15" /></>;
      break;
    case 'bag':
      shape = <><path {...common} d="M5 8h14l1 12H4L5 8Z" /><path {...common} d="M8 8V6a4 4 0 0 1 8 0v2" /></>;
      break;
    case 'user':
      shape = <><circle {...common} cx="12" cy="8" r="3.3" /><path {...common} d="M5 20a7 7 0 0 1 14 0" /></>;
      break;
    case 'table':
      shape = <><path {...common} d="M4 8h16M6 8v7m12-7v7M8 15v5m8-5v5M3 20h18" /><path {...common} d="M6 4h12l1 4H5l1-4Z" /></>;
      break;
    case 'minus':
      shape = <path {...common} d="M5 12h14" />;
      break;
    case 'plus':
      shape = <><path {...common} d="M5 12h14M12 5v14" /></>;
      break;
    case 'trash':
      shape = <><path {...common} d="M5 7h14M9 7V4h6v3m-8 0 1 13h8l1-13M10 10v7m4-7v7" /></>;
      break;
    case 'pause':
      shape = <><circle {...common} cx="12" cy="12" r="8.8" /><path {...common} d="M10 9v6m4-6v6" /></>;
      break;
    case 'card':
      shape = <><rect {...common} x="3" y="5.5" width="18" height="13" rx="2" /><path {...common} d="M3 9.5h18M7 15h3" /></>;
      break;
    case 'cash':
      shape = <><rect {...common} x="3" y="6" width="18" height="12" rx="1.5" /><circle {...common} cx="12" cy="12" r="2.4" /><path {...common} d="M6 9v0m12 6v0" /></>;
      break;
    case 'arrows':
      shape = <><path {...common} d="M4 8h15l-3-3m3 3-3 3M20 16H5l3-3m-3 3 3 3" /></>;
      break;
    case 'cloud':
      shape = <><path {...common} d="M7.2 18h9.6a4.2 4.2 0 0 0 .5-8.4A6 6 0 0 0 6 8.3 4.8 4.8 0 0 0 7.2 18Z" /><path {...common} d="m12 9.5 0 6m0-6-2 2m2-2 2 2" /></>;
      break;
    case 'monitor':
      shape = <><rect {...common} x="3" y="4.5" width="18" height="12" rx="1.5" /><path {...common} d="M8 20h8m-4-3.5V20" /></>;
      break;
    case 'check':
      shape = <path {...common} d="m5 12 4.5 4.5L19 7" />;
      break;
    case 'warning':
      shape = <><path {...common} d="m12 3 9 17H3L12 3Z" /><path {...common} d="M12 9v4m0 3.1v.1" /></>;
      break;
    case 'receipt':
      shape = <><path {...common} d="M6 3.5h12v17l-2-1.5-2 1.5-2-1.5-2 1.5-2-1.5-2 1.5v-17Z" /><path {...common} d="M9 8h6m-6 3h6m-6 3h4" /></>;
      break;
    case 'printer':
      shape = <><path {...common} d="M7 8V4h10v4M6 17H4V9.5A1.5 1.5 0 0 1 5.5 8h13A1.5 1.5 0 0 1 20 9.5V17h-2" /><path {...common} d="M7 14h10v6H7zM17 11h.1" /></>;
      break;
    case 'chevron-down':
      shape = <path {...common} d="m7 9 5 5 5-5" />;
      break;
    case 'close':
      shape = <><path {...common} d="m6 6 12 12M18 6 6 18" /></>;
      break;
    default:
      shape = null;
  }

  return (
    <svg
      className={`rl-pos-icon ${className}`}
      width={size}
      height={size}
      viewBox="0 0 24 24"
      aria-hidden="true"
      focusable="false"
    >
      {shape}
    </svg>
  );
};

export const RoastMark: React.FC<{ size?: number; className?: string }> = ({ size = 54, className = '' }) => (
  <svg
    className={`rl-roast-mark ${className}`}
    width={size}
    height={size}
    viewBox="0 0 54 62"
    fill="none"
    aria-hidden="true"
    focusable="false"
  >
    <path d="M27 2.5c11.2 6.2 17.3 15.3 17.3 26.4C44.3 44.1 36.8 53.7 27 59.5 17.2 53.7 9.7 44.1 9.7 28.9 9.7 17.8 15.8 8.7 27 2.5Z" stroke="currentColor" strokeWidth="1.6" />
    <path d="M27 12v36M27 24c-5.1-2.4-8.6-5.7-10.5-9.7M27 31c5.2-2.4 8.7-5.8 10.7-9.9M27 39c-4.4-2-7.5-4.8-9.2-8.2M27 45c4.1-1.8 7.1-4.3 8.8-7.7" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
  </svg>
);

export const BotanicalSprig: React.FC<{ className?: string }> = ({ className = '' }) => (
  <svg className={`rl-botanical-sprig ${className}`} viewBox="0 0 230 250" fill="none" aria-hidden="true" focusable="false">
    <path d="M34 238C72 184 112 120 190 20" stroke="currentColor" strokeWidth="1.4" strokeLinecap="round" />
    <path d="M72 182c-19-5-34-18-43-37 21 0 39 10 50 26M88 157c-16-12-23-28-23-47 18 7 30 21 31 39M107 130c-7-18-4-34 6-49 12 13 15 29 8 45M127 103c-1-17 7-31 21-42 6 16 2 30-12 42M148 77c5-16 17-27 33-33 1 16-8 28-24 36M64 194c10 13 24 21 42 23-8-17-19-27-35-31M95 149c12 10 27 15 44 13-10-14-23-22-39-23M119 116c14 4 27 1 40-9-14-8-28-7-42 1M143 84c14-2 26-9 35-22-15-2-27 4-37 15" stroke="currentColor" strokeWidth="1.1" strokeLinecap="round" strokeLinejoin="round" />
    <circle cx="43" cy="218" r="5" stroke="currentColor" strokeWidth="1.1" />
    <circle cx="54" cy="229" r="3.5" stroke="currentColor" strokeWidth="1.1" />
  </svg>
);
