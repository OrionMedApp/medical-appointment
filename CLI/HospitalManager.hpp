//
// Created by petrifuj on 2/24/2026.
//

#ifndef CLI_HOSPITALMANAGER_HPP
#define CLI_HOSPITALMANAGER_HPP
#include <vector>

#include "Doctor.hpp"
#include "Patient.hpp"


class HospitalManager {

public:
    void addSaveDoctor(Doctor& d);
    void addSavePatient(Patient& p);
    bool fetchDoctorsFromBackend();
private:
    std::vector<Doctor> doctors;
    const std::string doctors_file = "doctors.json";
    const std::string patients_file = "patients.json";
    const std::string appointments_file = "appointments.json";
};


#endif //CLI_HOSPITALMANAGER_HPP