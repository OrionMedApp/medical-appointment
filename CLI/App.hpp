//
// Created by petrifuj on 2/23/2026.
//

#ifndef PROJECT_APP_HPP
#define PROJECT_APP_HPP
#include <iostream>

using namespace std;

enum Operations {
    SCHEDULE = 1, ADD_DOCTOR,
    ADD_PATIENT, STORE_ENTRIES,
    TRACK_APPOINTMENTS, SYNC_DOCTORS, EXIT
};


class App {
public:
    void run();

private:
    bool isRunning = false;
    void writeMenu();
    void stateMachine();
    int chooseOption();
};


#endif //PROJECT_APP_HPP