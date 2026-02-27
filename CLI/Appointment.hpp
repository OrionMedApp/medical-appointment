//
// Created by tamtegel on 2/26/2026.
//

#ifndef CLI_APPOINTMENT_HPP
#define CLI_APPOINTMENT_HPP
#include <string>
#include "json.hpp"
#include <locale>
#include <codecvt>


class Appointment {
public:
    Appointment(const std::string& patientID,
                const std::string& email,
                const std::string& start,
                const std::string& end,
                const std::string& t,
                const std::string& s,
                const std::string& n) {
        this->patientMedicalId = patientID;
        this->doctorEmail = email;
        this->startTime = start;
        this->endTime = end;
        this->type = t;
        this->status = s;
        this->notes = n;
}
    std::string getPatientMedicalId() const;
    std::string getDoctorEmail() const;
    std::string getStartTime() const;
    std::string getEndTime() const;
    std::string getType() const;
    std::string getStatus() const;
    std::string  getNotes() const;

    NLOHMANN_DEFINE_TYPE_INTRUSIVE(Appointment, patientMedicalId, doctorEmail, startTime, endTime, type, status, notes)

private:
    std::string patientMedicalId;
    std::string doctorEmail;
    std::string startTime;
    std::string endTime;
    std::string type;
    std::string status;
    std::string notes;
};
namespace nlohmann {
    template <>
    struct adl_serializer<std::wstring> {
        static void to_json(json& j, const std::wstring& str) {
            // wstring -> UTF-8 string za JSON
            std::wstring_convert<std::codecvt_utf8<wchar_t>> converter;
            std::string utf8_str = converter.to_bytes(str);
            j = utf8_str;
        }

        static void from_json(const json& j, std::wstring& str) {
            // JSON string -> wstring
            std::wstring_convert<std::codecvt_utf8<wchar_t>> converter;
            str = converter.from_bytes(j.get<std::string>());
        }
    };
}
#endif //CLI_APPOINTMENT_HPP