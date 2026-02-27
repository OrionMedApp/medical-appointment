//
// Created by tamtegel on 2/26/2026.
//

#include "Appointment.hpp"

std::string Appointment::getPatientMedicalId() const {
    return patientMedicalId;
};

std::string Appointment::getDoctorEmail() const {
    return doctorEmail;
};

std::string Appointment::getStartTime() const {
    return startTime;
};
std::string Appointment::getEndTime() const {
    return endTime;
}

std::string Appointment::getType() const {
    return type;
};

std::string Appointment::getStatus() const {
    return status;
};

std::string Appointment::getNotes() const {
    return notes;
};