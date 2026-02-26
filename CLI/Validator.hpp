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

    static bool isValidAppointmentStatus(const std::string& status) {
        return status == "Scheduled" || status == "Completed" || status == "Cancelled";
    }

    static bool isValidISO8601(const std::string& dt) {
        if (dt.length() != 24) return false;

        if (dt[4] != '-' || dt[7] != '-' || dt[10] != 'T' ||
            dt[13] != ':' || dt[16] != ':' || dt[19] != '.' || dt[23] != 'Z') {
            return false;
            }
        for (size_t i = 0; i < 24; ++i) {
            if (i == 4 || i == 7 || i == 10 || i == 13 || i == 16 || i == 19 || i == 23) continue;
            if (!isdigit(static_cast<unsigned char>(dt[i]))) return false;
        }

        return true;
    }

    static bool isMandatory(const std::string& field) {
        return !field.empty();
    }

};


#endif //CLI_VALIDATOR_HPP