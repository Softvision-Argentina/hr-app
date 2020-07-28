/// <reference types="Cypress" />
var settingsPage = require('../../../pages/settingsPage.js');
import { Before, After, Given, When, Then, And } from "cypress-cucumber-preprocessor/steps"


When(/^"(.*)" validates attributes and the submenu$/, function (Role, submenu) {
    if (Role == "Admin") {
        let sm = submenu.hashes();
        settingsPage.validateSubMenus(sm);
    }
});

And(/^"(.*)" click on "(.*)" submenu$/, function (Role, submenu) {
    if (Role == "Admin") {
        settingsPage.clickOnSubmenu(submenu);
    }
});

Then(/^"(.*)" validate attributes and grid headers in "(.*)"$/, function (Role, submenu, headers) {
    if (Role == "Admin") {
        let hdr = headers.hashes();
        settingsPage.validateGridHeaders(hdr);
    }
});

Then(/^"(.*)" validate attributes in "(.*)" pop up$/, function (Role, popup, attributes) {
    if (Role == "Admin") {
        let attr = attributes.hashes();
        settingsPage.validateAttributesinPopUp(popup, attr);
    }
});

And(/^"(.*)" click on "(.*)" button in "(.*)"$/, function (Role, button, popup) {
    if (Role == "Admin") {
        settingsPage.clickOnButton(button);
    }
});

