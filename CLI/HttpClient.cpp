//
// Created by petrifuj on 2/26/2026.
//

#include "HttpClient.hpp"

#include <iostream>
#include <vector>
#include <windows.h>
#include <winhttp.h>


bool HttpClient::connect(const std::wstring &appName, const std::wstring &host, const int& port) {
    if (hSession || hConnect) disconnect();
    this->hSession = WinHttpOpen(appName.c_str(), WINHTTP_ACCESS_TYPE_DEFAULT_PROXY,
                                 nullptr, nullptr, 0);
    if (!hSession) {
        std::cout << "WinHTTP session failed!" << std::endl;
        return false;
    }

    this->hConnect = WinHttpConnect(this->hSession, host.c_str(), port, 0);

    if (!this->hConnect) {
        WinHttpCloseHandle(this->hSession);
        std::cout << "Connection!" << std::endl;
        return false;
    }

    return true;
}

std::string HttpClient::getRequest(const std::wstring& path, DWORD& outStatusCode) {
    outStatusCode = 0; // Reset status code immediately
    if (!hConnect) {
        outStatusCode = -1;
        return "";
    }

    HINTERNET hRequest = WinHttpOpenRequest(hConnect, L"GET", path.c_str(),
                                            nullptr, WINHTTP_NO_REFERER,
                                            WINHTTP_DEFAULT_ACCEPT_TYPES, 0);
    if (!hRequest) {
        outStatusCode = -1;
        return "";
    }

    std::string responseData;

    if (WinHttpSendRequest(hRequest, WINHTTP_NO_ADDITIONAL_HEADERS, 0, nullptr, 0, 0, 0) &&
        WinHttpReceiveResponse(hRequest, nullptr)) {

        DWORD size = sizeof(outStatusCode);
        WinHttpQueryHeaders(hRequest, WINHTTP_QUERY_STATUS_CODE | WINHTTP_QUERY_FLAG_NUMBER,
                            nullptr, &outStatusCode, &size, nullptr);

        DWORD bytesAvailable = 0;
        // Optimization: Create the buffer once
        std::vector<char> buffer;

        while (WinHttpQueryDataAvailable(hRequest, &bytesAvailable) && bytesAvailable > 0) {
            buffer.resize(bytesAvailable);
            DWORD bytesRead = 0;

            if (WinHttpReadData(hRequest, buffer.data(), bytesAvailable, &bytesRead)) {
                if (bytesRead > 0) {
                    responseData.append(buffer.data(), bytesRead);
                }
            } else {
                // If reading fails, break to avoid infinite loop
                break;
            }
        }
        }

    // Clean up handle
    WinHttpCloseHandle(hRequest);
    return responseData;
}

void HttpClient::disconnect() {
    // Order matters: Close the connection before the session
    if (this->hConnect) {
        WinHttpCloseHandle(this->hConnect);
        this->hConnect = nullptr;
    }
    if (this->hSession) {
        WinHttpCloseHandle(this->hSession);
        this->hSession = nullptr;
    }
}
