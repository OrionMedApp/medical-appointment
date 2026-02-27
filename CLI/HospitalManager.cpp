//
// Created by petrifuj on 2/24/2026.
//

#include "HospitalManager.hpp"

#include <fstream>
#include <iostream>

#include "json.hpp"
#include <windows.h>
#include <winhttp.h>
#include <filesystem>
#include <iomanip>
#include <sstream>
#include <chrono>
#include <set>
#include <thread>
#include <locale>


#include "HttpClient.hpp"
#include "Validator.hpp"

#pragma comment(lib, "winhttp.lib")

using json = nlohmann::json;

void HospitalManager::addSaveDoctor(Doctor &d) {
    std::ifstream inFile(doctors_file);
    json doctors;
    if (inFile.good()) {
        inFile >> doctors;
        inFile.close();
    } else {
        doctors = json::array();
    }

    doctors.push_back(d);

    std::ofstream outFile(doctors_file);
    outFile << doctors.dump(4);
    outFile.close();
}

void HospitalManager::addSavePatient(Patient &p) {
    std::ifstream inFile(patients_file);
    json patients;
    if (inFile.good()) {
        inFile >> patients;
        inFile.close();
    } else {
        patients = json::array();
    }

    patients.push_back(p);

    std::ofstream outFile(patients_file);
    outFile << patients.dump(4);
    outFile.close();
}

void HospitalManager::addSaveAppointment(Appointment &a) {
    std::ifstream inFile(appointments_file);
    json appointments;
    if (inFile.good()) {
        inFile >> appointments;
        inFile.close();
    } else {
        appointments = json::array();
    }

    appointments.push_back(a);

    std::ofstream outFile(appointments_file);
    outFile << appointments.dump(4);
    outFile.close();
}

void HospitalManager::exportResponseToAFile(std::string &response, std::string file_name) {
    std::cout << "CSV fetched - " << response.length() << " bytes!" << std::endl;

    // CSV filename: YYYYMMDD_HHMMSS_doctors.csv
    auto now = std::chrono::system_clock::now();
    auto time_t = std::chrono::system_clock::to_time_t(now);
    std::stringstream ss;
    ss << std::put_time(std::localtime(&time_t), "%Y%m%d_%H%M%S") << "_" << file_name << ".csv";
    std::string timestamp_file = ss.str();

    std::ofstream outFile(timestamp_file, std::ios::binary);
    outFile << response;
    outFile.close();
    std::cout << "File saved" << std::endl << std::endl;
}

std::string HospitalManager::getResponseFromBackend(const std::wstring &appName, const std::wstring &host,
                                                    const int &port, const std::wstring &path, DWORD &statusCode) {
    auto &client = HttpClient::getInstance();
    client.connect(appName, host, port);
    statusCode = 0;
    std::string response = client.getRequest(path, statusCode);
    client.disconnect();


    return response;
}

std::string getValidatedInput(const std::string& prompt, bool (*validator)(const std::string&)) {
    std::string input;
    while (true) {
        std::cout << prompt;
        if (!(std::cin >> std::ws)) return ""; // Handle EOF/Stream error
        std::getline(std::cin, input);

        if (validator(input)) return input;
        std::cout << "Invalid format! Please try again." << std::endl;
    }
}

bool HospitalManager::scheduleAppointment() {
    DWORD statusCode = 0;

    std::string did = getValidatedInput("Enter Doctor GUID: ", Validator::isValidGuid);


    std::string pathDoctor = "/api/Doctor/" + did;
    std::string response = getResponseFromBackend(L"CLI-DoctorApp", L"localhost", 5085,
                                                std::filesystem::path(pathDoctor).wstring(), statusCode);

    if (statusCode != 200) {
        std::cout << "Doctor doesn't exist in database!" << std::endl << std::endl;
        return false;
    }

    std::string doctorEmail, patientMedicalId;
    try {
        auto j = json::parse(response);

        // 2. Extract the email field
        // (Ensure your backend uses the key "email", or change this to "Email")
        if (j.contains("email")) {
            doctorEmail = j.at("email").get<std::string>();
        } else {
            std::cout << "Error: Doctor record found but email is missing!" << std::endl;
            return false;
        }
    } catch (json::parse_error& e) {
        std::cout << "Failed to parse doctor data: " << e.what() << std::endl;
        return false;
    }

    std::string pid = getValidatedInput("Enter Patient GUID: ", Validator::isValidGuid);
    std::string pathPatient = "/api/Patient/" + pid;
    response = getResponseFromBackend(L"CLI-DoctorApp", L"localhost", 5085,
                                     std::filesystem::path(pathPatient).wstring(), statusCode);

    try {
        auto j = json::parse(response);

        // 2. Extract the email field
        // (Ensure your backend uses the key "email", or change this to "Email")
        if (j.contains("medicalId")) {
            patientMedicalId = j.at("medicalId").get<std::string>();
        } else {
            std::cout << "Error: Patient record found but medicalId is missing!" << std::endl;
            return false;
        }
    } catch (json::parse_error& e) {
        std::cout << "Failed to parse doctor data: " << e.what() << std::endl;
        return false;
    }

    if (statusCode != 200) {
        std::cout << "Patient doesn't exist in database!" << std::endl << std::endl;
        return false;
    }

    std::string sdate = getValidatedInput("Enter start-date (YYYY-MM-DDTHH:mm:ss.xxxZ): ", Validator::isValidISO8601);
    std::string edate = getValidatedInput("Enter end-date (YYYY-MM-DDTHH:mm:ss.xxxZ): ", Validator::isValidISO8601);

    std::set<std::string> allowedTypes = { "Consultation", "Follow-up", "Emergency" };
    std::set<std::string> allowedStatuses = { "Scheduled", "Completed", "Cancelled" };

    std::string type, status, notes;

    std::cout << "Enter Type (Consultation/Follow-up/Emergency): ";
    std::getline(std::cin >> std::ws, type);
    if (allowedTypes.find(type) == allowedTypes.end()) {
        std::cout << "Error: Invalid appointment type." << std::endl;
        return false;
    }

    std::cout << "Enter Status (Scheduled/Completed/Cancelled): ";
    std::getline(std::cin >> std::ws, status);
    if (allowedStatuses.find(status) == allowedStatuses.end()) {
        std::cout << "Error: Invalid appointment status." << std::endl;
        return false;
    }

    std::cout << "Enter notes: ";
    std::getline(std::cin >> std::ws, notes);

    Appointment a(patientMedicalId, doctorEmail, sdate, edate, type, status, notes);
    addSaveAppointment(a);

    return true;
}


void HospitalManager::trackAppointments() {
    DWORD statusCode = 0;

    // Create the filename ONCE when the command starts
    auto now = std::chrono::system_clock::now();
    auto time_t = std::chrono::system_clock::to_time_t(now);
    std::stringstream ss;
    ss << "Report_" << std::put_time(std::localtime(&time_t), "%Y%m%d_%H%M%S") << ".txt";
    std::string reportFile = ss.str();

    std::set<std::string> seenLines;
    std::cout << "Monitoring... Saving to " << reportFile << std::endl;
    int cnt = 1;
    bool running = true;
    while (running) {
        std::string response = getResponseFromBackend(L"CLI-DoctorApp", L"localhost", 5085, L"/api/Appointment/export", statusCode);

        if (statusCode == 200) {
            std::stringstream responseStream(response);
            std::string line;

            std::getline(responseStream, line); // Skip header
            while (std::getline(responseStream, line)) {
                if (line.empty()) continue;

                if (seenLines.find(line) == seenLines.end()) {
                    seenLines.insert(line);

                    std::cout << "\n--- New Appointment "  << cnt << "---\n" << line << std::endl;
                    cnt++;
                    std::ofstream outFile(reportFile, std::ios::app);
                    outFile << line << std::endl;
                }
            }
        } else {
            std::cout << "API Error! " << statusCode << std::endl;
        }

        std::cout << "Press ESC to exit... " << std::endl << std::endl;

        for (int i = 0; i < 250; i++) {
            if (GetAsyncKeyState(VK_ESCAPE) & 0x8000) {
                running = false; // Break out of the sleep loop if a key is pressed
                break;
            }

            std::this_thread::sleep_for(std::chrono::milliseconds(20));
        }
    }
}


