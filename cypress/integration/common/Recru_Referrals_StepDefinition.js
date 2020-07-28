/// <reference types="Cypress" />
var ReferralsPage = require('../../../pages/ReferralsPage.js');
import { Before, After, Given, When, Then } from "cypress-cucumber-preprocessor/steps"


When(/^"(.*)" validate grid headers in "(.*)" section$/, function (Role, tab, headers) {
    if (Role == "Admin") {
        let hdr = headers.hashes();
        ReferralsPage.validateGridHeaders(tab, hdr);
    }
});

When(/^"(.*)" click on "(.*)" section$/, function (Role, section) {
    if (Role == "Admin") {
        ReferralsPage.clickOnOpenPositions();
    }
});

When(/^"(.*)" validates the attributes in the "(.*)" popup$/, function (Role, popup) {
    if (Role == "Admin") {
        ReferralsPage.validateAttributesInPopUp(popup);
    }
});