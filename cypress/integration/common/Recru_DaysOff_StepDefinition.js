
/// <reference types="Cypress" />
var DaysOffPage = require('../../../pages/DaysOffPage.js');
import { Before, After, Given, When, Then } from "cypress-cucumber-preprocessor/steps"

When(/^"(.*)" clicks on "(.*)" button$/, function (Role, button) {
    if (Role == "Admin") {
        DaysOffPage.clickOnButton(button);
    }
});

Then(/^"(.*)" validate attributes in "(.*)" popup$/, function (Role, popup) {
    if (Role == "Admin") {
        DaysOffPage.validatePopUpAttributes(popup);
    }
});