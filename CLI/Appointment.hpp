//
// Created by tamtegel on 2/26/2026.
//

#ifndef CLI_APPOINTMENT_HPP
#define CLI_APPOINTMENT_HPP
#include <string>
#include "json.hpp"

class Appointment {
public:
    Appointment(const std::string& patientMedicalID,
                const std::string& doctorEmail,
                const std::string& start,
                const std::string& end,
                const std::string& t,
                const std::string& s,
                const std::string& n) {
        this->patientMedicalID = patientMedicalID;
        this->doctorEmail = doctorEmail;
        this->startDateTime = start;
        this->endDateTime = end;
        this->type = t;
        this->status = s;
        this->notes = n;
}
    std::string getPatientMedicalID() const;
    std::string getDoctorEmail() const;
    std::string getStartDateTime() const;
    std::string getEndDateTime() const;
    std::string getType() const;
    std::string getStatus() const;
    std::string getNotes() const;

    NLOHMANN_DEFINE_TYPE_INTRUSIVE(Appointment, patientMedicalID, doctorEmail, startDateTime, endDateTime, type, status, notes)

private:
    std::string patientMedicalID;
    std::string doctorEmail;
    std::string startDateTime;
    std::string endDateTime;
    std::string type;
    std::string status;
    std::string notes;
};

#endif //CLI_APPOINTMENT_HPP