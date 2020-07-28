/// <reference types="Cypress" />
var chai = require('chai');
var expect = chai.expect;

var encyptDecrypt = require('../utilities/EncryptDecrypt.js');
let homePageUserNanme = 'div.user-info';
let appTitles = 'div.side-menu';
let homePageLogo = 'div#logo img';
let dashboard = 'div.side-menu div.container-item:nth-child(2) div.item > p:nth-child(2)';
let dashboardSearchBar = 'input[placeholder="Search for a dashboard"]';
let processes = 'div.side-menu div.container-item.active:nth-child(3) div.item > p:nth-child(2)';
let processesSearchBar = '.main-nav__input > .ng-untouched';
let People = 'div.side-menu div.container-item.ng-star-inserted:nth-child(4) > div.item';
let peopleSearcBar = 'input[placeholder="Search for a candidate"]';
let daysOff = '[routerlink="/daysOff"] > .item';
let dayOffSearchBar = ".main-nav__input > .ng-untouched";
let tasks = 'div[routerlink="/tasks"] svg';
let tasksSearchBar = 'input[placeholder="Search for a Task"]';
let reports = 'div[routerlink="/reports"]';
let reportsSearchBar = 'input[placeholder="Search for a report"]';
let referrals = 'div[routerlink="/referrals"]';
let referralsSearchBar = 'input[placeholder="Search for your own referrals"]';
let settings = 'div[routerlink="/settings"]';
let settingsSearchBar = 'input[placeholder="Search for a setting"]';



var RecruHomePage = function () {

    this.validateApps = function (appsHashed) {
        cy.get(appTitles).should('be.visible');
        let apps = [];

        for (let i = 0; i < appsHashed.length; i++) {
            apps.push(appsHashed[i].Apps);
        }

        cy.get('div.item p').each(($el, index, $list) => {
            expect(apps.includes($el.text())).to.be.true;
        });

    }
    this.verifyPresence = function () {
        cy.get(homePageUserNanme).should('be.visible');
        cy.get(homePageLogo).should('be.visible');
    }
    this.NavigateToApp = function (app) {

        if (app == "Dashboard") {
            cy.get(dashboard).should('be.visible');
            cy.get(dashboard).click();
            cy.get(dashboardSearchBar).should('be.visible');
        }

        else if (app == "Processes") {
            cy.get(processes).should('be.visible');
            cy.get(processes).click();
            cy.get(processesSearchBar).should('be.visible');
        }

        else if (app == "People") {
            cy.get(People).should('be.visible');
            cy.get(People).click();
            cy.get(peopleSearcBar).should('be.visible');
        }

        else if (app == "DaysOff") {
            cy.get(daysOff).should('be.visible');
            cy.get(daysOff).click();
            cy.get(dayOffSearchBar).should('be.visible');
        }

        else if (app == "Tasks") {
            cy.get(tasks).should('be.visible');
            cy.get(tasks).click();
            cy.get(tasksSearchBar).should('be.visible');
        }

        else if (app == "Reports") {
            cy.get(reports).should('be.visible');
            cy.get(reports).click();
            cy.get(reportsSearchBar).should('be.visible');
        }

        else if (app == "Referrals") {
            cy.get(referrals).should('be.visible');
            cy.get(referrals).click();
            cy.get(referralsSearchBar).should('be.visible');
        }

        else if (app == "Settings") {
            cy.get(settings).should('be.visible');
            cy.get(settings).click();
            cy.get(settingsSearchBar).should('be.visible');
        }
    }

}
module.exports = new RecruHomePage();