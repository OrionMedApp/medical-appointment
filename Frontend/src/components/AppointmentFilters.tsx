import { AppointmentType } from "../models/Appointment.model";
import "../style/Filters.css";

type AppointmentFiltersProps = {
  doctorOptions: string[];
  patientOptions: string[];
  typeOptions: AppointmentType[];
  selectedDoctor: string;
  selectedPatient: string;
  selectedType: string;
  dateFrom: string;
  dateTo: string;
  onDoctorChange: (v: string) => void;
  onPatientChange: (v: string) => void;
  onTypeChange: (v: string) => void;
  onDateFromChange: (v: string) => void;
  onDateToChange: (v: string) => void;
  onNewAppointment: () => void;
  onDownloadPdf: () => void;
};

const AppointmentFilters = ({
  doctorOptions,
  patientOptions,
  typeOptions,
  selectedDoctor,
  selectedPatient,
  selectedType,
  dateFrom,
  dateTo,
  onDoctorChange,
  onPatientChange,
  onTypeChange,
  onDateFromChange,
  onDateToChange,
  onNewAppointment,
  onDownloadPdf,
}: AppointmentFiltersProps) => {
  const hasActiveFilters =
    selectedDoctor || selectedPatient || selectedType || dateFrom || dateTo;

  return (
    <div className="filter-bar">
      <div className="filter-bar__label">
        <span className="filter-bar__icon">⚙︎</span>
        Filters
        {hasActiveFilters && (
          <span className="filter-bar__active-dot" title="Active filters" />
        )}
      </div>

      <div className="filter-bar__inputs">
        <div className={`filter-chip ${selectedDoctor ? "filter-chip--active" : ""}`}>
          <label className="filter-chip__label">Doctor</label>
          <select className="filter-chip__input" value={selectedDoctor} onChange={(e) => onDoctorChange(e.target.value)}>
            <option value="">All</option>
            {doctorOptions.map((d) => <option key={d} value={d}>{d}</option>)}
          </select>
        </div>

        <div className={`filter-chip ${selectedType ? "filter-chip--active" : ""}`}>
          <label className="filter-chip__label">Type</label>
          <select className="filter-chip__input" value={selectedType} onChange={(e) => onTypeChange(e.target.value)}>
            <option value="">All</option>
            {typeOptions.map((t) => <option key={t} value={t}>{t}</option>)}
          </select>
        </div>

        <div className={`filter-chip ${selectedPatient ? "filter-chip--active" : ""}`}>
          <label className="filter-chip__label">Patient</label>
          <select className="filter-chip__input" value={selectedPatient} onChange={(e) => onPatientChange(e.target.value)}>
            <option value="">All</option>
            {patientOptions.map((p) => <option key={p} value={p}>{p}</option>)}
          </select>
        </div>

        <div className={`filter-chip ${dateFrom ? "filter-chip--active" : ""}`}>
          <label className="filter-chip__label">From</label>
          <input className="filter-chip__input" type="date" value={dateFrom} onChange={(e) => onDateFromChange(e.target.value)} />
        </div>

        <div className={`filter-chip ${dateTo ? "filter-chip--active" : ""}`}>
          <label className="filter-chip__label">To</label>
          <input className="filter-chip__input" type="date" value={dateTo} onChange={(e) => onDateToChange(e.target.value)} />
        </div>

        {hasActiveFilters && (
          <button className="filter-clear-btn" onClick={() => {
            onDoctorChange("");
            onPatientChange("");
            onTypeChange("");
            onDateFromChange("");
            onDateToChange("");
          }}>
            ✕ Clear
          </button>
        )}
      </div>

      <div className="filter-bar__actions">
        <button className="header-btn header-btn--secondary" onClick={onDownloadPdf}>
          <span>↓</span> Download PDF
        </button>
        <button className="header-btn header-btn--primary" onClick={onNewAppointment}>
          <span>+</span> New Appointment
        </button>
      </div>
    </div>
  );
};

export default AppointmentFilters;