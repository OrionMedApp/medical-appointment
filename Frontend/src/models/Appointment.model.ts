export interface Appointment {
  id: string;                // GUID
  doctor: string;            // "Ime Prezime"
  patient: string;           // "Ime Prezime"
  type: string;              // "Konsultacija" / "Kontrola" / "Hitno"
  status: AppointmentStatus; // "Scheduled" | "Completed" | "Cancelled"
  start: Date;
  end: Date;
  notes: string;
}

export interface CreateAppointmentDto {
  doctorId: string;   // GUID
  patientId: string;  // GUID
  type: number;       // 0/1/2 (backend enum)
  startTime: string;  // ISO string
  endTime: string;    // ISO string
  notes: string;
}

export enum AppointmentStatus {
  Scheduled = "Scheduled",
  Completed = "Completed",
  Cancelled = "Cancelled",
}