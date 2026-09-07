export type TimecardStatus = 'ClockedIn' | 'ClockedOut' | 'Approved';

export interface StaffTimecardDto {
  id: string;
  tenantId: string;
  outletId: string;
  outletName: string;
  employeeId: string;
  employeeName: string;
  clockInUtc: string;
  clockOutUtc?: string;
  durationMinutes: number;
  regularHours: number;
  overtimeHours: number;
  status: TimecardStatus;
  notes?: string;
}

export interface ClockInRequest {
  outletId: string;
  pin: string;
}

export interface ClockOutRequest {
  pin: string;
  notes?: string;
}

export interface ApproveTimecardRequest {
  notes?: string;
}
