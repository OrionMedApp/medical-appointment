//
// Created by petrifuj on 2/24/2026.
//

#ifndef CLI_HOSPITALMANAGER_HPP
#define CLI_HOSPITALMANAGER_HPP
#include <vector>
#include <string>

#include "Doctor.hpp"
#include "Patient.hpp"
#include "Appointment.hpp"

class HospitalManager {

public:
    void addSaveDoctor(Doctor& d);
    void addSavePatient(Patient& p);

    std::string getResponseFromBackend(const std::wstring& appName, const std::wstring& host, const int& port, const std::wstring& path, DWORD& statusCode);

    void trackAppointments();

    void exportResponseToAFile(std::string& response, std::string file_name);

    bool fetchDoctorsFromBackend();

    void addSaveAppointment(Appointment& a);

    bool getDoctorFromBackend(const std::string& email, const std::string& medicalID);


private:
    std::vector<Doctor> doctors;
    const std::string doctors_file = "doctors.json";
    const std::string patients_file = "patients.json";
    const std::string appointments_file = "appointments.json";
};


#endif //CLI_HOSPITALMANAGER_HPP