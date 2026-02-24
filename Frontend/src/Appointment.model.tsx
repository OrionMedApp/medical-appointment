
export interface Appointment{
     id: number,
     doctor: string,
     patient: string,
     type: string,
     start: Date,
     end: Date
}
export interface CreateAppointmentDto{
     id: number,
     doctor: string,
     patient: string,
     type: string,
     start: Date,
     end: Date,
}
