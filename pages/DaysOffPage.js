/// <reference types="Cypress" />
var chai = require('chai');
var expect = chai.expect;

let dayOffSearchBar = ".main-nav__input > .ng-untouched";
let addDaysOffBuutton = 'button.ant-btn.ant-btn-primary.ant-btn-round';
let DNIfield = 'nz-form-control input#DNI';
let reasonField = 'nz-select#idType input';
let fromDateField = 'nz-date-picker[formcontrolname="date"] input';
let endDateField = 'nz-date-picker[formcontrolname="endDate"] input';
let saveButton = ".ant-modal-footer > .ant-btn-primary";
let closePopUpButton = 'button[aria-label="Close"] svg';

// tasks page selectors
let newTasksButton = 'button.newTask';
let newTaskstitleField = 'input#title';
let newTasksuserField = 'input.ant-select-selection-search-input.ng-untouched.ng-pristine.ng-valid';
let newTasksDateField = 'input[placeholder="Select date"]';
let newTasksAddFieldButton = 'button[nztype="dashed"]';
let newTasksSaveButton = 'button.ant-btn.ant-btn-primary.ng-star-inserted:nth-child(2)';
let newTasksclosePopUpButton = 'button[aria-label="Close"] svg';
let addPositionButton = 'button.referralsButton.ant-btn.ng-star-inserted';
let editPositionButton = ':nth-child(1) > :nth-child(6) > .ng-star-inserted > .anticon-edit > svg';


var DaysOffPage = function () {

    this.validatePageAttributes = function () {
        cy.get(dayOffSearchBar).should('be.visible');
        cy.get(addDaysOffBuutton).should('be.visible');
    }

    this.validatePopUpAttributes = function (popup) {

        if (popup == 'Add new daysOff') {
            cy.get(DNIfield).should('be.visible');
            cy.get(reasonField).should('be.visible');
            cy.get(fromDateField).should('be.visible');
            cy.get(endDateField).should('be.visible');
            cy.get(saveButton).should('be.visible');
            cy.get(closePopUpButton).should('be.visible');
            cy.get(closePopUpButton).click();
            cy.get(closePopUpButton).should('not.be.visible');
        }
        else if (popup == 'New Tasks') {
            cy.get(newTaskstitleField).should('be.visible');
            cy.get(newTasksuserField).should('be.visible');
            cy.get(newTasksDateField).should('be.visible');
            cy.get(newTasksAddFieldButton).should('be.visible');
            cy.get(newTasksSaveButton).should('be.visible');
            cy.get(newTasksclosePopUpButton).should('be.visible');
            cy.get(newTasksclosePopUpButton).click();
            cy.get(newTasksclosePopUpButton).should('not.be.visible');
        }

    }

    this.clickOnButton = function (Button) {
        if (Button == 'Add new daysOff') {
            cy.get(addDaysOffBuutton).should('be.visible');
            cy.get(addDaysOffBuutton).click();
        }

        else if (Button == 'New Tasks') {
            cy.get(newTasksButton).should('be.visible');
            cy.get(newTasksButton).click();
        }

        else if (Button == 'Add Position') {
            cy.get(addPositionButton).should('be.visible');
            cy.get(addPositionButton).click();
        }

        else if (Button == 'Edit Position') {
            cy.get(editPositionButton).should('be.visible');
            cy.get(editPositionButton).click();
        }

    }

}

module.exports = new DaysOffPage();