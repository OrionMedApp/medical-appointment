import { useMemo, useState } from "react";
import { events } from "../services/AppointmentMockService";
import { formatDateTime } from "../utils/dateUtils";
import CreateAppointmentModal from "./CreateAppointmentModal";
const AppointmentsPage = () => {

  const [appointments, setAppointments] = useState(events);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const [selectedDoctor, setSelectedDoctor] = useState("");
  const [selectedType, setSelectedType] = useState("");
  const [selectedPatient, setSelectedPatient] = useState("");
  const [dateFrom, setDateFrom] = useState("");
  const [dateTo, setDateTo] = useState("");

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
  <h2 className="title">Pregled termina</h2>
  <button className="primary-btn" onClick={() => setIsModalOpen(true)}>
    + Novi termin
  </button>
</div>
      <div className="filter-bar">
        <select className="filter-input" onChange={(e) => setSelectedDoctor(e.target.value)}>
          <option value="">Svi doktori</option>
          <option value="Dr Marković">Dr Marković</option>
          <option value="Dr Jovanović">Dr Jovanović</option>
        </select>

        <select className="filter-input" onChange={(e) => setSelectedType(e.target.value)}>
          <option value="">Svi tipovi</option>
          <option value="Pregled">Pregled</option>
          <option value="Kontrola">Kontrola</option>
          <option value="Konsultacija">Konsultacija</option>
        </select>

        <select className="filter-input" onChange={(e) => setSelectedPatient(e.target.value)}>
          <option value="">Svi pacijenti</option>
          <option value="Petar Petrović">Petar Petrović</option>
          <option value="Marko Marković">Marko Marković</option>
          <option value="Janko Janković">Janko Janković</option>
          <option value="Ivan Ivanović">Ivan Ivanović</option>
          <option value="Milos Milosević">Milos Milosević</option>
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

      <div className="card">
        <table className="table">
          <thead>
            <tr>
              <th>Doktor</th>
              <th>Pacijent</th>
              <th>Tip</th>
              <th>Početak</th>
              <th>Kraj</th>
            </tr>
          </thead>
          <tbody>
            {filteredEvents.length === 0 ? (
              <tr>
                <td colSpan={5} style={{ textAlign: "center" }}>
                  Nema termina
                </td>
              </tr>
            ) : (
              filteredEvents.map((event) => (
                <tr key={event.id}>
                  <td>{event.doctor}</td>
                  <td>{event.patient}</td>
                  <td>{event.type}</td>
                  <td>{formatDateTime(event.start)}</td>
                  <td>{formatDateTime(event.end)}</td>
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
    onCreate={(newAppointment) =>
      setAppointments((prev) => [...prev, newAppointment])
    }
  />
)}
    </div>

  );
};

export default AppointmentsPage;