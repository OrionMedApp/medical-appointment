
import React, { useState } from "react";

function CreateAppointment() {
  const [formData, setFormData] = useState({
    patient: "",
    doctor: "",
    datetime: "",
    type: "",
    status: "Scheduled",
    notes: "",
    customField: ""
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // validacija
    if (!formData.patient || !formData.doctor || !formData.datetime || !formData.type) {
      alert("Please fill in all mandatory fields!");
      return;
    }
    console.log("Form data:", formData);
    // ovde kasnije ide API poziv ka backend-u
  };

  return (
    <form onSubmit={handleSubmit} style={{ maxWidth: "500px", margin: "20px auto" }}>
      <div>
        <label>Patient*</label>
        <input type="text" name="patient" value={formData.patient} onChange={handleChange} />
      </div>
      <div>
        <label>Doctor*</label>
        <input type="text" name="doctor" value={formData.doctor} onChange={handleChange} />
      </div>
      <div>
        <label>Date & Time*</label>
        <input type="datetime-local" name="datetime" value={formData.datetime} onChange={handleChange} />
      </div>
      <div>
        <label>Type*</label>
        <select name="type" value={formData.type} onChange={handleChange}>
          <option value="">Select type</option>
          <option value="Check-up">Check-up</option>
          <option value="Surgery">Surgery</option>
          <option value="Consultation">Consultation</option>
        </select>
      </div>
      <div>
        <label>Status*</label>
        <select name="status" value={formData.status} onChange={handleChange}>
          <option value="Scheduled">Scheduled</option>
          <option value="Completed">Completed</option>
          <option value="Cancelled">Cancelled</option>
        </select>
      </div>
      <div>
        <label>Notes</label>
        <textarea name="notes" value={formData.notes} onChange={handleChange}></textarea>
      </div>
      <div>
        <label>Custom Field</label>
        <input type="text" name="customField" value={formData.customField} onChange={handleChange} />
      </div>
      <button type="submit">Create Appointment</button>
    </form>
  );
}

export default CreateAppointment;