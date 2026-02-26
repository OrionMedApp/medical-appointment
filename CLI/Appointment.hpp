//
// Created by tamtegel on 2/26/2026.
//

#ifndef CLI_APPOINTMENT_HPP
#define CLI_APPOINTMENT_HPP
#include <string>
#include "json.hpp"

class Appointment {
public:
    Appointment(const std::wstring& patientID,
                const std::wstring& doctorID,
                const std::wstring& start,
                const std::wstring& end,
                const std::wstring& t,
                const std::wstring& s,
                const std::wstring& n) {
        this->patientID = patientID;
        this->doctorID = doctorID;
        this->startDateTime = start;
        this->endDateTime = end;
        this->type = t;
        this->status = s;
        this->notes = n;
}
    std::wstring getPatientID() const;
    std::wstring getDoctorID() const;
    std::wstring getStartDateTime() const;
    std::wstring getEndDateTime() const;
    std::wstring getType() const;
    std::wstring getStatus() const;
    std::wstring  getNotes() const;

    NLOHMANN_DEFINE_TYPE_INTRUSIVE(Appointment, patientID, doctorID, startDateTime, endDateTime, type, status, notes)

private:
    std::wstring patientID;
    std::wstring doctorID;
    std::wstring startDateTime;
    std::wstring endDateTime;
    std::wstring type;
    std::wstring status;
    std::wstring notes;
};

#endif //CLI_APPOINTMENT_HPP