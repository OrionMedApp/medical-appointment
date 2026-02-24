import { useState } from "react";
import { Appointment, AppointmentStatus } from "../models/Appointment.model";

interface Props {
  appointments: Appointment[];
  onClose: () => void;
  onCreate: (appointment: Appointment) => void;
}

const CreateAppointmentModal = ({ appointments, onClose, onCreate }: Props) => {
  const [doctor, setDoctor] = useState("");
  const [patient, setPatient] = useState("");
  const [dateTime, setDateTime] = useState("");
  const [type, setType] = useState("");
  const [status, setStatus] = useState("");
  const [notes, setNotes] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (!doctor || !patient || !dateTime || !type || !status || !notes) {
      setError("All fields are mandatory.");
      return;
    }

    const newStart = new Date(dateTime);
    const newEnd = new Date(newStart.getTime() + 60 * 60 * 1000);

    const isOverlapping = appointments.some(
      (appointment) =>
        appointment.doctor === doctor &&
        newStart < appointment.end &&
        newEnd > appointment.start
    );

    if (isOverlapping) {
      setError("Doctor already has appointment in this time slot.");
      return;
    }

    const newAppointment: Appointment = {
      id: Date.now(),
      doctor,
      patient,
      type,
      status: status as AppointmentStatus,
      notes,
      start: newStart,
      end: newEnd,
    };

    onCreate(newAppointment);
    onClose();
  };

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h3>Kreiranje termina</h3>

        <form onSubmit={handleSubmit} className="modal-form">
          <select value={doctor} onChange={(e) => setDoctor(e.target.value)}>
            <option value="">Doktor</option>
            <option value="Dr Marković">Dr Marković</option>
            <option value="Dr Jovanović">Dr Jovanović</option>
          </select>

          <input
            type="text"
            placeholder="Pacijent"
            value={patient}
            onChange={(e) => setPatient(e.target.value)}
          />

          <input
            type="datetime-local"
            value={dateTime}
            onChange={(e) => setDateTime(e.target.value)}
          />

          <select value={type} onChange={(e) => setType(e.target.value)}>
            <option value="">Tip</option>
            <option value="Consultation">Consultation</option>
            <option value="Follow-up">Follow-up</option>
            <option value="Emergency">Emergency</option>
          </select>

          <select value={status} onChange={(e) => setStatus(e.target.value)}>
            <option value="">Status</option>
            <option value={AppointmentStatus.Scheduled}>Scheduled</option>
            <option value={AppointmentStatus.Completed}>Completed</option>
            <option value={AppointmentStatus.Cancelled}>Cancelled</option>
          </select>

          <textarea
            placeholder="Notes"
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
          />

          {error && <p className="error">{error}</p>}

          <div className="modal-actions">
            <button type="button" onClick={onClose}>
              Cancel
            </button>
            <button type="submit" className="primary-btn">
              Save
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateAppointmentModal;