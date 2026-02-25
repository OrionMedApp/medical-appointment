
export interface Appointment{
     id: number,
     doctor: string,
     patient: string,
     type: string,
     status:AppointmentStatus,
     start: Date,
     notes:string,
     end: Date
}
export interface CreateAppointmentDto{
     doctor: string,
     patient: string,
     type: string,
     start: Date,
     end: Date,
     status: AppointmentStatus,
     notes:string,
}
export enum AppointmentStatus {
  Scheduled = "Scheduled",
  Completed = "Completed",
  Cancelled = "Cancelled",
}