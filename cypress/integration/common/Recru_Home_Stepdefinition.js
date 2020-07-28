
/// <reference types="Cypress" />
var LoginPage = require('../../../pages/LoginPage.js');
var HomePage = require('../../../pages/HomePage.js');
import { Before, After, Given, When, Then } from "cypress-cucumber-preprocessor/steps"

Given(/^"(.*)" is provided with Recru "(.*)"$/, function (Role, URL) {
    if (Role == "Admin") {
        LoginPage.NavigatetoURL(URL);
    }
});

When(/^"(.*)" should see Recru Logo$/, function (Role) {
    if (Role == "Admin") {
        LoginPage.verifyRecruLogo();
    }
});

Then(/^"(.*)" enters UserName "(.*)" and Password "(.*)" and clicks on login button$/, function (Role, username, password) {
    if (Role == "Admin") {
        LoginPage.inputLoginDetails(username, password);
    }
});

Then(/^"(.*)" should check the displayed "(.*)" in HomePage$/, function (Role, username) {
    if (Role == "Admin") {
        LoginPage.validateUserNameinHomePage(username);
    }
});


Given(/^"(.*)" is present in Recru homepage$/, function (Role) {

    if (Role == "Admin") {
        HomePage.verifyPresence();
    }

});

Then(/^"(.*)" validates the Apps in app navigation bar$/, function (Role, apps) {

    let appsHashed = apps.hashes();
    if (Role == "Admin") {
        HomePage.validateApps(appsHashed);
    }

});


When(/^"(.*)" clicks on "(.*)" in App navigator$/, function (Role, app) {

    if (Role == "Admin") {
        HomePage.NavigateToApp(app);
    }

});

Then(/^"(.*)" validates the error message for "(.*)"$/, function (Role, context) {

    if (Role == "Admin") {
        LoginPage.validateMessage(context);
    }



});

Given(/^"(.*)" is provided with the URL "(.*)"$/, function (Role, URL) {
    if (Role == "Admin") {
        LoginPage.NavigatetoLoginPage(URL);
    }
});