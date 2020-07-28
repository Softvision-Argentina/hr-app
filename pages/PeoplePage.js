/// <reference types="Cypress" />
var chai = require('chai');
var expect = chai.expect;

let nameHeader = "th span:contains('Name')";
let statusHeader = "th span:contains('Status')";
let dniHeader = "th:contains('DNI')";
let emailHeader = "th:contains('E-mail')";
let phoneHeader = "th:contains('Phone')";
let actionHeader = "th:contains('Action')";

var PeoplePage = function () {

    this.validateGridHeaders = function () {
        cy.get(nameHeader).should('be.visible');
        cy.get(dniHeader).should('be.visible');
        cy.get(emailHeader).should('be.visible');
        cy.get(phoneHeader).should('be.visible');
        cy.get(actionHeader).should('be.visible');
        cy.get(statusHeader).should('be.visible');
    }

}
module.exports = new PeoplePage();