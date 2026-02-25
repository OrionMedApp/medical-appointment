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
// Callback funkcija za CURL (globalna)
static size_t WriteCallback(void* contents, size_t size, size_t nmemb, std::string* response) {
    size_t totalSize = size * nmemb;
    response->append((char*)contents, totalSize);
    return totalSize;
}

bool HospitalManager::fetchDoctorsFromBackend() {
    HINTERNET hSession = WinHttpOpen(L"CLI-DoctorApp", WINHTTP_ACCESS_TYPE_DEFAULT_PROXY,
                                     WINHTTP_NO_PROXY_NAME, WINHTTP_NO_PROXY_BYPASS, 0);
    if (!hSession) {
        std::cout << "WinHTTP session failed!" << std::endl;
        return false;
    }

    HINTERNET hConnect = WinHttpConnect(hSession, L"localhost", 5085, 0);
    HINTERNET hRequest = WinHttpOpenRequest(hConnect, L"GET", L"/api/Doctor/export",
                                            NULL, WINHTTP_NO_REFERER, WINHTTP_DEFAULT_ACCEPT_TYPES, 0);

    if (!hRequest) {
        WinHttpCloseHandle(hConnect);
        WinHttpCloseHandle(hSession);
        std::cout << "WinHTTP request failed!" << std::endl;
        return false;
    }

    BOOL sent = WinHttpSendRequest(hRequest, WINHTTP_NO_ADDITIONAL_HEADERS, 0,
                                   WINHTTP_NO_REQUEST_DATA, 0, 0, 0);
    if (!sent) {
        WinHttpCloseHandle(hRequest);
        WinHttpCloseHandle(hConnect);
        WinHttpCloseHandle(hSession);
        return false;
    }

    BOOL received = WinHttpReceiveResponse(hRequest, NULL);
    if (!received) {
        WinHttpCloseHandle(hRequest);
        WinHttpCloseHandle(hConnect);
        WinHttpCloseHandle(hSession);
        return false;
    }

    DWORD statusCode = 0;
    DWORD size = sizeof(statusCode);
    WinHttpQueryHeaders(hRequest, WINHTTP_QUERY_STATUS_CODE | WINHTTP_QUERY_FLAG_NUMBER,
                        NULL, &statusCode, &size, NULL);

    std::string response;
    DWORD bytesAvailable = 0;
    do {
        WinHttpQueryDataAvailable(hRequest, &bytesAvailable);
        if (bytesAvailable > 0) {
            std::vector<char> buffer(bytesAvailable + 1);
            DWORD bytesRead = 0;
            WinHttpReadData(hRequest, buffer.data(), bytesAvailable, &bytesRead);
            response.append(buffer.data(), bytesRead);
        }
    } while (bytesAvailable > 0);

    WinHttpCloseHandle(hRequest);
    WinHttpCloseHandle(hConnect);
    WinHttpCloseHandle(hSession);

    if (statusCode == 200) {
        std::cout << "CSV doctors fetched (" << response.length() << " bytes)!" << std::endl;

        // CSV filename: YYYYMMDD_HHMMSS_doctors.csv
        auto now = std::chrono::system_clock::now();
        auto time_t = std::chrono::system_clock::to_time_t(now);
        std::stringstream ss;
        ss << std::put_time(std::localtime(&time_t), "%Y%m%d_%H%M%S") << "_doctors.csv";
        std::string timestamp_file = ss.str();

        std::ofstream outFile(timestamp_file, std::ios::binary);
        outFile << response;
        outFile.close();

        std::cout << "ABSOLUTNA PUTANJA: " << std::filesystem::absolute(timestamp_file) << std::endl;
        std::cout << "Saved to " << timestamp_file << std::endl;

        std::cout << "RAW PREVIEW (first 200 bytes):" << std::endl;
        std::cout << response.substr(0, std::min<size_t>(200, response.length())) << "..." << std::endl;
        return true;
    }



    std::cout << "HTTP " << statusCode << std::endl;
    return false;
}

