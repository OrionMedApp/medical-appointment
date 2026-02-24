//
// Created by petrifuj on 2/24/2026.
//

#include "HospitalManager.hpp"

#include <fstream>
#include <iostream>

#include "json.hpp"

using json  = nlohmann::json;

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
