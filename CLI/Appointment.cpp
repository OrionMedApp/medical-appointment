//
// Created by tamtegel on 2/26/2026.
//

#include "Appointment.hpp"

std::string Appointment::getPatientMedicalID() const {
    return patientMedicalID;
};

std::string Appointment::getDoctorEmail() const {
    return doctorEmail;
};

std::string Appointment::getStartDateTime() const {
    return startDateTime;
};
std::string Appointment::getEndDateTime() const {
    return endDateTime;
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