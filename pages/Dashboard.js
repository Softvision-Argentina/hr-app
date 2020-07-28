/// <reference types="Cypress" />
var chai = require('chai');
var expect = chai.expect;

let homePageUserNanme = 'div.user-info';
let appTitles = 'div.side-menu';
let homePageLogo = 'div#logo img';
let dashboard = 'div.side-menu div.container-item:nth-child(2) div.item > p:nth-child(2)';
let dashboardSearchBar = 'input[placeholder="Search for a dashboard"]';

var DashboardPage = function () {

    this.verifyPageAttributes = function () {
        cy.get(dashboardSearchBar).should('be.visible');
        cy.get(dashboardSearchBar).type('testing');
    }

}
module.exports = new DashboardPage();