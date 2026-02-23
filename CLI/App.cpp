//
// Created by petrifuj on 2/23/2026.
//

#include "App.hpp"

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

void App::stateMachine() {
    try {
        switch (chooseOption()) {
            case SCHEDULE:
            case ADD_DOCTOR:
            case ADD_PATIENT:
            case STORE_ENTRIES:
            case TRACK_APPOINTMENTS:
            case SYNC_DOCTORS:
                break;
            case EXIT:
                isRunning = false;
                return;
            default:
                cout <<"Unknown option"<<endl;
                break;
        }
    }catch (const invalid_argument& e) {
        cout << "Not a number!" << endl;
    }catch (const out_of_range& e) {
        cout << "Out of range!" << endl;
    }
}
