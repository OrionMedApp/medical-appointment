//
// Created by petrifuj on 2/23/2026.
//

#include "App.hpp"

#include "Validator.hpp"

using namespace std;


void App::run() {
    isRunning = true;
    while (isRunning) {
        writeMenu();
        stateMachine();
        if (!isRunning) {
            return;
        }
    }
}

void App::writeMenu() {
    cout << "Please choose one of the options:" << endl;
    cout << "1. Schedule Appointment" << endl;
    cout << "2. Add Doctor" << endl;
    cout << "3. Add Patient" << endl;
    cout << "4. Store entries in DB" << endl;
    cout << "5. Track new appointments in real-time" << endl;
    cout << "6. Sync Doctors with backend" << endl;
    cout << "7. Exit" << endl;
}

int App::chooseOption() {
    string input;
    getline(cin,input);
    size_t pos;
    int option = stoi(input, &pos);
    if (pos != input.length()) {
        throw invalid_argument("Wrong input!");
    }
    return option;
}

void App::addDoctor() {
    string email, name,spec,phone;
    std::cout << "--- Add New Doctor ---" << endl;
    std::cout << "Email: "; std::getline(std::cin >> std::ws, email);
    if (!Validator::isValidEmail(email)) {
        std::cout << "Invalid email!" << endl << endl;
        return;
    }
    std::cout << "Name: "; std::getline(std::cin >> std::ws, name);
    std::cout << "Specialization: "; std::getline(std::cin, spec);
    std::cout << "Phone: "; std::getline(std::cin, phone);
    if (!Validator::isValidPhone(phone)) {
        std::cout << "Invalid phone!" << endl << endl;
        return;
    }
    Doctor d = Doctor(name,spec,phone,email);
    hospitalManager->addSaveDoctor(d);
    cout << "Doctor added" << endl;
}

void App::stateMachine() {
    try {
        switch (chooseOption()) {
            case SCHEDULE:
            case ADD_DOCTOR:
                addDoctor();
                break;
            case ADD_PATIENT:
            case STORE_ENTRIES:
            case TRACK_APPOINTMENTS:
            case SYNC_DOCTORS:
                break;
            case EXIT:
                isRunning = false;
                return;
            default:
                cout <<"Unknown option"<<endl << endl;
                break;
        }
    }catch (const invalid_argument& e) {
        cout << "Not a number!" << endl << endl;
    }catch (const out_of_range& e) {
        cout << "Out of range!" << endl << endl;
    }
}
