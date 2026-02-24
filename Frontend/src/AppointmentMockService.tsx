import { Appointment } from "./Appointment.model";

export const events: Appointment[] = [
  {
    id: 1,
    doctor: "Dr Marković",
    patient: "Petar Petrović",
    type: "Pregled",
    start: new Date("2026-02-25T13:00"),
    end: new Date("2026-02-25T14:00"),
  },
  {
    id: 3,
    doctor: "Dr Jovanović",
    patient: "Janko Janković",
    type: "Kontrola",
    start: new Date("2026-02-24T16:00"),
    end: new Date("2026-02-24T17:00"),
  },
  {
    id: 4,
    doctor: "Dr Jovanović",
    patient: "Marko Marković",
    type: "Kontrola",
    start: new Date("2026-02-28T16:00"),
    end: new Date("2026-02-28T17:00"),
  },
  {
    id: 5,
    doctor: "Dr Jovanović",
    patient: "Milos Milosević",
    type: "Kontrola",
    start: new Date("2026-02-24T13:00"),
    end: new Date("2026-02-24T14:00"),
  },
  {
    id: 6,
    
    doctor: "Dr Jovanović",
    patient: "Ivan Ivanović",
    type: "Konsultacija",
    start: new Date("2026-02-23T12:00"),
    end: new Date("2026-02-23T13:00"),
  },
];

