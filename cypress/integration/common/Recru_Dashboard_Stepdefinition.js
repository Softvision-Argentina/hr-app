/// <reference types="Cypress" />
var DashboardPage = require('../../../pages/Dashboard.js');
var HomePage = require('../../../pages/HomePage.js');
var ProcessesPage = require('../../../pages/ProcessesPage.js');
var PeoplePage = require('../../../pages/PeoplePage.js');
var DaysOffPage = require('../../../pages/DaysOffPage.js');
var TasksPage = require('../../../pages/TasksPage.js');
var ReportsPage = require('../../../pages/ReportsPage.js');
var ReferralsPage = require('../../../pages/ReferralsPage.js');
import { Before, After, Given, When, Then } from "cypress-cucumber-preprocessor/steps"


Then(/^"(.*)" validates page attributes in "(.*)" page$/, function (Role, app) {

    if (Role == "Admin") {

        if (app == "Dashboard") {
            DashboardPage.verifyPageAttributes();
        }

        else if (app == "Processes") {
            ProcessesPage.validatePageAttributes();
        }

        else if (app == "People") {
            PeoplePage.validateGridHeaders();
        }

        else if (app == "DaysOff") {
            DaysOffPage.validatePageAttributes();
        }

        else if (app == "Tasks") {
            TasksPage.validateTasksPageAttributes();
        }

        else if (app == "Reports") {
            ReportsPage.validateReportsPageAttributes();
        }

        else if (app == "Referrals") {
            ReferralsPage.validatePageAttributes();
        }

    }

});
