//
// Created by tamtegel on 2/26/2026.
//

#include "Appointment.hpp"

std::wstring Appointment::getPatientID() const {
    return patientID;
};

std::wstring Appointment::getDoctorID() const {
    return doctorID;
};

std::wstring Appointment::getStartDateTime() const {
    return startDateTime;
};
std::wstring Appointment::getEndDateTime() const {
    return endDateTime;
}

std::wstring Appointment::getType() const {
    return type;
};

std::wstring Appointment::getStatus() const {
    return status;
};

std::wstring Appointment::getNotes() const {
    return notes;
};