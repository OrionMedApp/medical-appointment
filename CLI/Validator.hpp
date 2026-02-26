//
// Created by petrifuj on 2/24/2026.
//

#ifndef CLI_VALIDATOR_HPP
#define CLI_VALIDATOR_HPP
#include <objbase.h>
#include <string>
#include <regex>
#include <iomanip>
#include <sstream>



class Validator {
public:
    static bool isValidEmail(const std::string& email) {
        const std::regex pattern(R"((\w+)(\.{1}\w+)*@(\w+)(\.\w+)+)");
        return std::regex_match(email, pattern);
    }


    // stavljeno isto kao kolege iz C#
    static bool isValidPhone(const std::string& phone) {
        const std::regex pattern(R"((?:\+3816\d{7,8}|06\d{7,8}))");
        return std::regex_match(phone, pattern);
    }
    static bool isValidName(const std::string& name) {
        return !name.empty() && name.length() >= 3;
    }

    static bool isValidMedicalID(const std::string& id) {
        return id.length() == 36;
    }
    static std::string generateMedicalID() {
        GUID guid;
        CoCreateGuid(&guid);
        wchar_t wbuf[64];
        StringFromGUID2(guid, wbuf, 64);
        std::wstring ws(wbuf);
        std::string result(ws.begin(), ws.end());
        return result.substr(1, result.length() - 2);
    }
    static bool isValidAppointmentType(const std::string& type) {
        return type == "Consultation" || type == "Follow-up" || type == "Emergency";
    }

    static bool isValidGuid(const std::wstring& guid) {
        const std::wregex guidPattern(L"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
        return std::regex_match(guid, guidPattern);
    }

    static bool isValidAppointmentStatus(const std::string& status) {
        return status == "Scheduled" || status == "Completed" || status == "Cancelled";
    }

    static bool isValidISO8601(const std::wstring& ts) {
        const std::wregex pattern(L"^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}\\.\\d{3}Z$");

        return std::regex_match(ts, pattern);
    }

    static bool isMandatory(const std::string& field) {
        return !field.empty();
    }

};


#endif //CLI_VALIDATOR_HPP