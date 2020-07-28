/// <reference types="Cypress" />
var HomePage = require('../../../pages/HomePage.js');
var ProcessesPage = require('../../../pages/ProcessesPage.js');
import { Before, After, Given, When, Then, And } from "cypress-cucumber-preprocessor/steps"



Then(/^"(.*)" validate grid headers in "(.*)" tab$/, function (Role, Tab, headers) {
    let hdr = headers.hashes();
    if (Role == "Admin") {
        ProcessesPage.validateGridHeaders(Tab, hdr);
    }

});

And(/^"(.*)" click on new candidate button$/, function (Role) {
    if (Role == "Admin") {
        ProcessesPage.clickOnNewCandidateButton();
    }
});

Then(/^"(.*)" validate page attributes in "(.*)" stage$/, function (Role, Tab, headers) {

    if (Role == "Admin") {
        if (Tab == 'HR') {
            ProcessesPage.validateHRStagePageAttributes();
        }
        else if (Tab == 'Technical') {
            ProcessesPage.validateTechnicalStagePageAttributes();
        }

        else if (Tab == 'Client') {
            ProcessesPage.validateClientStagePageAttributes();
        }
        else if (Tab == 'PreOffer') {
            ProcessesPage.validatePreOfferStagePageAttributes();
        }
        else if (Tab == 'Offer') {
            ProcessesPage.validateOfferStagePageAttributes();
        }

    }

});