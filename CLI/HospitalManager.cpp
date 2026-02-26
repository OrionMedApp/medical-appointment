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

#include "HttpClient.hpp"

#pragma comment(lib, "winhttp.lib")

using json = nlohmann::json;



void HospitalManager::addSaveDoctor(Doctor& d) {
    std::ifstream inFile(doctors_file);
    json doctors;
    if (inFile.good()) {
        inFile >> doctors;
        inFile.close();
    }else {
        doctors = json::array();
    }

    doctors.push_back(d);

    std::ofstream outFile(doctors_file);
    outFile << doctors.dump(4);
    outFile.close();
}
void HospitalManager::addSavePatient(Patient& p) {
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
void exportResponseToAFile(std::string& response, std::string file_name) {
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

bool HospitalManager::fetchFileFromBackend(const std::wstring& appName, const std::wstring& host, const int& port, const std::wstring& path, std::string file_name) {
    auto& client = HttpClient::getInstance();
    client.connect(appName, host, port);
    DWORD statusCode = 0;
    std::string response = client.getRequest(path, statusCode);
    client.disconnect();


    if (statusCode == 200) {
        exportResponseToAFile(response, file_name);
        return true;
    }

    return false;
}


