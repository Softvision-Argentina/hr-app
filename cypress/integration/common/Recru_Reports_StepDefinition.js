/// <reference types="Cypress" />
var ReportsPage = require('../../../pages/ReportsPage.js');
import { Before, After, Given, When, Then } from "cypress-cucumber-preprocessor/steps"

When(/^"(.*)" clicks on "(.*)"$/, function (Role, section) {
    if (Role == "Admin") {
        ReportsPage.clickOnSection(section);
    }
});

Then(/^"(.*)" validates page attributes in "(.*)" section$/, function (Role, section) {
    if (Role == "Admin") {

        if (section == "Candidate Filter") {
            ReportsPage.verifyAttributesInCandidateFilterSection();
        }

        else if (section == "Processes report") {
            ReportsPage.verifyChartPresenceinProcessreport();
        }
        else if (section == "Skills Report") {
            ReportsPage.verifyChartPresenceinSkillsreport();
        }
    }
});