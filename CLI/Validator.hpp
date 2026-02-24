//
// Created by petrifuj on 2/24/2026.
//

#ifndef CLI_VALIDATOR_HPP
#define CLI_VALIDATOR_HPP
#include <regex>
#include <string>


class Validator {
public:
    static bool isValidEmail(const std::string& email) {
        const std::regex pattern(R"((\w+)(\.{1}\w+)*@(\w+)(\.\w+)+)");
        return std::regex_match(email, pattern);
    }

    // Checks for: exactly 10 digits (adjust for your country)
    // Or use: R"(^\+?[0-9]{10,15}$)" for international formats
    static bool isValidPhone(const std::string& phone) {
        const std::regex pattern(R"(^\+[0-9]{10,15}$)");
        return std::regex_match(phone, pattern);
    }
};


#endif //CLI_VALIDATOR_HPP