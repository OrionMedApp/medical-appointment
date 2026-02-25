import { useMemo, useState } from "react";
import { events } from "../services/AppointmentMockService";
import { formatDateTime } from "../utils/dateUtils";
import CreateAppointmentModal from "./CreateAppointmentModal";
import { AppointmentStatus } from "../models/Appointment.model";
const AppointmentsPage = () => {

  const [appointments, setAppointments] = useState(events);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const [selectedDoctor, setSelectedDoctor] = useState("");
  const [selectedType, setSelectedType] = useState("");
  const [selectedPatient, setSelectedPatient] = useState("");
  const [dateFrom, setDateFrom] = useState("");
  const [dateTo, setDateTo] = useState("");

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


    //delete

    const handleDelete = async (id: number) => {
      const confirmed = window.confirm("Da li ste sigurni da želite da obrišete termin?");
      if (!confirmed) return;

      try {
        const response = await fetch(`http://localhost:7001/api/Appointment/${id}`, {
          method: "DELETE",
        });

        if (!response.ok) {
          throw new Error("Greška prilikom brisanja termina");
        }
        
        setAppointments((prev) => prev.filter((a) => a.id !== id));
      } catch (error) {
        console.error(error);
        alert("Došlo je do greške prilikom brisanja termina.");
      }
    };



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
              <th>Status</th>
              <th>Početak</th>
              <th>Kraj</th>
              <th>Akcije</th>
            </tr>
          </thead>
          <tbody>
            {filteredEvents.length === 0 ? (
              <tr>
                <td colSpan={7} style={{ textAlign: "center" }}>
                  Nema termina
                </td>
              </tr>
            ) : (
              filteredEvents.map((event) => (
                <tr key={event.id}>
                  <td>{event.doctor}</td>
                  <td>{event.patient}</td>
                  <td>{event.type}</td>
                  <td><span className={`status-badge ${getStatusClass(event.status)}`}>
                      {event.status}
                      </span>
                  </td>
                  <td>{formatDateTime(event.start)}</td>
                  <td>{formatDateTime(event.end)}</td>
                  <td>
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
    onCreate={(newAppointment) =>
      setAppointments((prev) => [...prev, newAppointment])
    }
  />
)}
    </div>

  );
};

export default AppointmentsPage;