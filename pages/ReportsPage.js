/// <reference types="Cypress" />
var chai = require('chai');
var expect = chai.expect;
let reports = 'div.side-menu div.container-item.ng-star-inserted.active:nth-child(8) > div.item';
let reportsSearchBar = 'input[placeholder="Search for a report"]';
let sectionHeaders = 'nz-collapse div[role="tab"]';
let preferredOfficeDropdown = 'nz-select[formcontrolname="preferredOffice"] input';
let communityDropdown = 'nz-select[formcontrolname="community"] input';
let skillsDropdown = 'nz-select[nzplaceholder="Select a skill"] input';
let addSkillButton = "div.ant-row button span:contains('Add Skill')";
let searchButton = "div.ant-row button span:contains('Search')";
let removeSkillButton = 'nz-form-item i[nztype="minus-circle-o"]';
let abovefiftypercentSection = "div:contains('Above 50% rate')";
let belowfiftypercentSection = "div:contains('Below 50%')";
let phoneHeader = "tr.ant-table-row.ng-star-inserted th:contains('Phone')";
let emailHeader = "tr.ant-table-row.ng-star-inserted th:contains('E-mail')";
let ActionHeader = "tr.ant-table-row.ng-star-inserted th:contains('Action')";
let nameHeader = "th span:contains('Name')";
let processReportSection = 'div.ant-row.ant-row-center:nth-child(2) canvas.chartjs-render-monitor';
let skillsReportSection = 'div.ant-row.ant-row-center:nth-child(3) canvas.chartjs-render-monitor';



var ReportsPage = function () {

    this.validateReportsPageAttributes = function () {
        cy.get(reportsSearchBar).should('be.visible');
        cy.get(sectionHeaders).should('be.visible');

    }

    this.verifyAttributesInCandidateFilterSection = function () {
        cy.get(preferredOfficeDropdown).should('be.visible');
        cy.get(communityDropdown).should('be.visible');
        cy.get(skillsDropdown).should('be.visible');
        cy.get(addSkillButton).should('be.visible');
        cy.get(searchButton).should('be.visible');
        cy.get(removeSkillButton).should('be.visible');
        cy.get(abovefiftypercentSection).should('be.visible');
        cy.get(belowfiftypercentSection).should('be.visible');
        cy.get(phoneHeader).should('be.visible');
        cy.get(emailHeader).should('be.visible');
        cy.get(ActionHeader).should('be.visible');
        cy.get(nameHeader).should('be.visible');
    }

    this.clickOnSection = function (section) {
        let candidateFilterSection = "nz-collapse div[role='tab']:contains('" + section + "')";
        cy.get(candidateFilterSection).should('be.visible');
        cy.get(candidateFilterSection).click();
    }

    this.verifyChartPresenceinProcessreport = function () {
        cy.get(processReportSection).should('be.visible');
    }

    this.verifyChartPresenceinSkillsreport = function () {
        cy.get(skillsReportSection).should('be.visible');
    }
}
module.exports = new ReportsPage();