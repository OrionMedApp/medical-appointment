import { useEffect, useMemo, useState } from "react";
import { formatDateTime } from "../utils/dateUtils";
import CreateAppointmentModal from "./CreateAppointmentModal";
import type { Appointment } from "../models/Appointment.model";
import { AppointmentStatus } from "../models/Appointment.model";

// ===== DTO shape koji stvarno dobijaš iz GET /api/Appointment =====
type PersonDto = {
  id: string;
  firstName: string;
  lastName: string;
  fullName?: string;
};

type AppointmentApiDto = {
  id: string;
  type: number;       // 0/1/2
  status: number;     // 0/1/2
  startTime: string;  // ISO string
  endTime: string;    // ISO string
  notes: string;
  doctor: PersonDto;
  patient: PersonDto;
};

// ===== Helpers =====
const typeLabel = (t: number) => {
  switch (t) {
    case 0:
      return "Konsultacija";
    case 1:
      return "Kontrola";
    case 2:
      return "Hitno";
    default:
      return "Nepoznato";
  }
};

const statusLabel = (s: number): AppointmentStatus => {
  switch (s) {
    case 0:
      return AppointmentStatus.Scheduled;
    case 1:
      return AppointmentStatus.Completed;
    case 2:
      return AppointmentStatus.Cancelled;
    default:
      return AppointmentStatus.Scheduled;
  }
};

const getStatusClass = (status: AppointmentStatus) => {
  switch (status) {
    case AppointmentStatus.Scheduled:
      return "status-scheduled";
    case AppointmentStatus.Completed:
      return "status-completed";
    case AppointmentStatus.Cancelled:
      return "status-cancelled";
    default:
      return "";
  }
};

const getFullName = (p?: PersonDto) => {
  if (!p) return "";
  return (p.fullName && p.fullName.trim()) || `${p.firstName} ${p.lastName}`.trim();
};

const AppointmentsPage = () => {
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const [selectedDoctor, setSelectedDoctor] = useState("");
  const [selectedType, setSelectedType] = useState("");
  const [selectedPatient, setSelectedPatient] = useState("");
  const [dateFrom, setDateFrom] = useState("");
  const [dateTo, setDateTo] = useState("");

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [editingAppointment, setEditingAppointment] = useState<Appointment | null>(null);


  const handleEdit = (appointment: Appointment) => {
  setEditingAppointment(appointment);
  setIsModalOpen(true);
};

const handleDelete = async (id: string) => {
  if (!window.confirm("Da li ste sigurni da želite da obrišete termin?")) return;

  try {
    const res = await fetch(`/api/Appointment/${id}`, {
      method: "DELETE",
    });

    if (!res.ok) throw new Error("Delete failed");

    setAppointments((prev) => prev.filter((a) => a.id !== id));
  } catch (e) {
    alert("Greška pri brisanju termina");
  }
};

  useEffect(() => {
    const load = async () => {
      try {
        setLoading(true);
        setError("");

        const res = await fetch("/api/Appointment");
        if (!res.ok) throw new Error(`Appointments error: ${res.status}`);

        const aDto = (await res.json()) as AppointmentApiDto[];

        const mapped: Appointment[] = (aDto ?? []).map((a) => ({
          id: a.id, // GUID string
          doctor: getFullName(a.doctor),
          patient: getFullName(a.patient),
          type: typeLabel(a.type),
          status: statusLabel(a.status),
          start: new Date(a.startTime),
          end: new Date(a.endTime),
          notes: a.notes ?? "",
        }));

        setAppointments(mapped);
      } catch (e) {
        setError(e instanceof Error ? e.message : "Load failed");
        setAppointments([]);
      } finally {
        setLoading(false);
      }
    };

    load();
  }, []);

  // Dinamičke opcije za filtere
  const doctorOptions = useMemo(
    () => Array.from(new Set(appointments.map((a) => a.doctor))).sort(),
    [appointments]
  );

  const patientOptions = useMemo(
    () => Array.from(new Set(appointments.map((a) => a.patient))).sort(),
    [appointments]
  );

  const typeOptions = useMemo(() => ["Konsultacija", "Kontrola", "Hitno"], []);

  const filteredEvents = useMemo(() => {
    return appointments.filter((event) => {
      if (selectedDoctor && event.doctor !== selectedDoctor) return false;
      if (selectedType && event.type !== selectedType) return false;
      if (selectedPatient && event.patient !== selectedPatient) return false;

      if (dateFrom) {
        const fromDate = new Date(dateFrom);
        if (event.start < fromDate) return false;
      }

      if (dateTo) {
        const toDate = new Date(dateTo);
        toDate.setHours(23, 59, 59);
        if (event.start > toDate) return false;
      }

      return true;
    });
  }, [appointments, selectedDoctor, selectedType, selectedPatient, dateFrom, dateTo]);

  return (
    <div style={{ padding: "20px" }}>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <h2 className="title">Appointments Overview</h2>
        <div style={{ display: "flex", gap: "10px" }}>
          <button className="secondary-btn" onClick={() => window.print()}>
            Download as PDF
          </button>
          <button className="primary-btn" onClick={() => setIsModalOpen(true)}>
            + New Appointment
          </button>
        </div>
      </div>

      <div className="filter-bar">
        <select
          className="filter-input"
          value={selectedDoctor}
          onChange={(e) => setSelectedDoctor(e.target.value)}
        >
          <option value="">All doctors</option>
          {doctorOptions.map((d) => (
            <option key={d} value={d}>
              {d}
            </option>
          ))}
        </select>

        <select
          className="filter-input"
          value={selectedType}
          onChange={(e) => setSelectedType(e.target.value)}
        >
          <option value="">All types</option>
          {typeOptions.map((t) => (
            <option key={t} value={t}>
              {t}
            </option>
          ))}
        </select>

        <select
          className="filter-input"
          value={selectedPatient}
          onChange={(e) => setSelectedPatient(e.target.value)}
        >
          <option value="">All patients</option>
          {patientOptions.map((p) => (
            <option key={p} value={p}>
              {p}
            </option>
          ))}
        </select>

        <input
          className="filter-input"
          type="date"
          value={dateFrom}
          onChange={(e) => setDateFrom(e.target.value)}
        />

        <input
          className="filter-input"
          type="date"
          value={dateTo}
          onChange={(e) => setDateTo(e.target.value)}
        />
      </div>

      {loading && <div style={{ padding: 12 }}>Loading...</div>}
      {!loading && error && <div style={{ padding: 12, color: "crimson" }}>{error}</div>}

      <div className="card">
        <table className="table">
          <thead>
  <tr>
    <th className="col-doctor">Doctor</th>
    <th className="col-patient">Patient</th>
    <th className="col-type">Type</th>
    <th className="col-status">Status</th>
    <th className="col-start">Start</th>
    <th className="col-end">End</th>
    <th className="col-action">Actions</th>
  </tr>
</thead>
          <tbody>
            {!loading && filteredEvents.length === 0 ? (
              <tr>
                <td colSpan={6} style={{ textAlign: "center" }}>
                  No appointments found
                </td>
              </tr>
            ) : (
              filteredEvents.map((event) => (
                <tr key={event.id}>
  <td className="col-doctor">{event.doctor}</td>
  <td className="col-patient">{event.patient}</td>
  <td className="col-type">{event.type}</td>
  <td className="col-status">
    <span className={`status-badge ${getStatusClass(event.status)}`}>{event.status}</span>
  </td>
  <td className="col-start">{formatDateTime(event.start)}</td>
  <td className="col-end">{formatDateTime(event.end)}</td>
  <td className="col-actions" style={{ display: "flex", gap: "8px" }}>
  <button
    className="icon-btn"
    onClick={() => handleEdit(event)}
    title="Izmeni termin"
  >
    ✏️
  </button>

  <button
    className="icon-btn danger"
    onClick={() => handleDelete(event.id)}
    title="Obriši termin"
  >
    🗑️
  </button>
</td>
  
</tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {isModalOpen && (
        <CreateAppointmentModal
          appointments={appointments}
          onClose={() => setIsModalOpen(false)}
          onCreate={(newAppointment) => setAppointments((prev) => [...prev, newAppointment])}
        />
      )}
    </div>
  );
};

export default AppointmentsPage;