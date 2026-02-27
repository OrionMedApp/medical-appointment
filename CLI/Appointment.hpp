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