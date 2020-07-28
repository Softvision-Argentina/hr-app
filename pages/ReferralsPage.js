/// <reference types="Cypress" />
var chai = require('chai');
var expect = chai.expect;

let referralsSearchBar = 'input[placeholder="Search for your own referrals"]';
let myReferralsTab = 'div.ant-tabs-tab.ant-tabs-tab-active.ng-star-inserted:nth-child(1)';
let openPositionsTab = 'div.ant-tabs-tab.ng-star-inserted:nth-child(2)';
let myReferralsGridHeaders = 'tr th';
let openPositionsGridHeaders = 'tr th';
let addPositionButton = 'button.referralsButton.ant-btn.ng-star-inserted';
let openPositiontitleField = 'input#title';
let openPositionsCommunityField = 'nz-select[formcontrolname="community"] input';
let openPositionsSeniorityField = 'nz-select[formcontrolname="seniority"] input';
let openPositionsStudioField = 'nz-select[formcontrolname="studio"] input';
let openPostitionsHotField = 'nz-select[formcontrolname="priority"] input';
let openPositionsaveButton = 'button.saveButton.ant-btn.ant-btn-primary:nth-child(1)';
let openPositionCancelButton = 'button.cancelButton.ant-btn:nth-child(3)';
let popupHeader = 'div.cdk-overlay-container:nth-child(10) strong:nth-child(1)';
let openPositionEditPopupEditButton = 'button.saveButton.ant-btn.ant-btn-primary:nth-child(2)';

var ReferralsPage = function () {

    this.validatePageAttributes = function () {
        cy.get(referralsSearchBar).should('be.visible');
        cy.get(myReferralsTab).should('be.visible');
        cy.get(openPositionsTab).should('be.visible');
    }
    this.validateGridHeaders = function (tab, headers) {

        if (tab == 'REFERRALS') {
            cy.get(myReferralsGridHeaders).should('be.visible');
            let gridheaders = [];

            for (let i = 0; i < headers.length; i++) {
                gridheaders.push(headers[i].Headers);
            }

            cy.get(myReferralsGridHeaders).each(($el, index, $list) => {
                expect($el.text()).to.contains(gridheaders[index]);
            });
        }

        else if (tab == 'OPEN POSITIONS') {

            cy.get(openPositionsTab).should('be.visible');
            cy.get(openPositionsTab).click();
            cy.get(openPositionsGridHeaders).should('be.visible');

            let gridheaders = [];

            for (let i = 0; i < headers.length; i++) {
                gridheaders.push(headers[i].Headers);
            }

            cy.get(openPositionsGridHeaders).each(($el, index, $list) => {
                expect($el.text()).to.contains(gridheaders[index]);
            });
        }
    }
    this.clickOnOpenPositions = function () {
        cy.get(openPositionsTab).should('be.visible');
        cy.get(openPositionsTab).click();
        cy.get(addPositionButton).should('be.visible');
    }
    this.validateAttributesInPopUp = function (popup) {

        cy.get(popupHeader).should('be.visible');
        cy.get(popupHeader).invoke('text').then((header) => {
            if (popup == 'Edit Position') {
                expect(header).to.equal(' Edit open position');
            }
            else {
                expect(header).to.equal(' Add open position');
            }
        });

        cy.get(openPositiontitleField).should('be.visible');
        cy.get(openPositionsCommunityField).should('be.visible');
        cy.get(openPositionsSeniorityField).should('be.visible');
        cy.get(openPositionsStudioField).should('be.visible');
        cy.get(openPostitionsHotField).should('be.visible');
        if (popup == 'Edit Position') {
            cy.get(openPositionEditPopupEditButton).should('be.visible');
        }
        else {
            cy.get(openPositionsaveButton).should('be.visible');
        }

        cy.get(openPositionCancelButton).should('be.visible');
        cy.get(openPositionCancelButton).click();
        cy.get(openPositionCancelButton).should('not.be.visible');
    }
}
module.exports = new ReferralsPage();