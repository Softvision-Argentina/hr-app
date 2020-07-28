/// <reference types="Cypress" />
var chai = require('chai');
var expect = chai.expect;

let tasks = 'div[routerlink="/tasks"] svg';
let tasksSearchBard = 'input[placeholder="Search for a Task"]';
let BAStudioTasksHeader = 'div.studio.ant-col';
let newTasksButton = 'button.newTask';
let orderByDropdown = 'button.orderBy.ant-btn.ant-dropdown-trigger';
let allFilter = "nz-button-group.ant-btn-group button span:contains('All')";
let openFilter = "nz-button-group.ant-btn-group button span:contains('Open')";
let closedFilter = "nz-button-group.ant-btn-group button span:contains('Closed')";
let showingSwitch = 'nz-switch';



var TasksPage = function () {

    this.validateTasksPageAttributes = function () {
        cy.get(tasksSearchBard).should('be.visible');
        cy.get(BAStudioTasksHeader).should('be.visible');
        cy.get(BAStudioTasksHeader).invoke('text').then((txt) => {
            expect(txt).to.equal('B.A Studio Tasks');
        });
        cy.get(orderByDropdown).should('be.visible');
        cy.get(allFilter).should('be.visible');
        cy.get(openFilter).should('be.visible');
        cy.get(closedFilter).should('be.visible');
        cy.get(newTasksButton).should('be.visible');
        cy.get(showingSwitch).should('be.visible');

    }

}
module.exports = new TasksPage();