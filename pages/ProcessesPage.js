/// <reference types="Cypress" />
var chai = require('chai');
var expect = chai.expect;
var processesSearchBar = '.main-nav__input > .ng-untouched';
var myProcessesTab = 'div.ant-tabs-tab.ant-tabs-tab-active.ng-star-inserted:nth-child(1)';
var baStudiotab = 'div.ant-tabs-tab.ng-star-inserted:nth-child(2)';
var myPrcoessesGridHeader = 'th.tableHead.ant-table-cell.ng-star-inserted';
var actions = '.ant-table-row > :nth-child(7)';
var newCandidateButton = 'button.buttonStyle.buttonFont.ant-btn.ant-btn-primary';
var hrStageTitle = "span:contains('HR STAGE')";
var processesActionsEditButton = 'td.ant-table-cell:nth-child(7) i.anticon.anticon-edit:nth-child(1) > svg:nth-child(1)';
var firstNameField = 'input#name';
var lastnameField = 'input#lastName';
var emailField = 'input#email';
var phoneNumber = 'input#phoneNumber';
var linkedIn = 'input#linkedin';
var communityDropdown = 'nz-select[formcontrolname="community"]';
var preferredOffericeDropdown = 'nz-select[formcontrolname="preferredOffice"] input';
var profile = 'nz-select[formcontrolname="profile"] input';
var hrStageFeedBack = 'hr-stage div#recruEdit-editor';
var hrStageStatus = 'div#recruEdit-editor';
var hrStageDate = 'hr-stage input[placeholder="Select date"]';
var hrStageEnglishLevel = 'hr-stage label.ant-radio-button-wrapper.ng-star-inserted:nth-child(2)';
var hrStageSaveButton = 'div.ant-col > button.ant-btn.ant-btn-primary:nth-child(2)';

var technicalStage = "div.ant-steps-item-container  span:contains('2')";
var clientStage = "div.ant-steps-item-container  span:contains('3')";
var preOfferStage = "div.ant-steps-item-container  span:contains('4')";
var offerStage = "div.ant-steps-item-container  span:contains('5')";

var tecchnicalStageTitle = "span:contains('TECHNICAL STAGE')";
var technicalStageStatus = 'technical-stage [formcontrolname="status"]';
var technicalStageDate = 'technical-stage input[placeholder="Select date"]';
var technicalStageInterviewer = 'technical-stage nz-select[nzplaceholder="Select the owner of the process"]';
var technicalStageDelegate = 'technical-stage nz-select[nzplaceholder="Select a delegate for the process"]';
var technicalStageSeniority = 'technical-stage nz-select[formcontrolname="seniority"]';
var technicalStageAlternateSeniority = 'technical-stage nz-select[formcontrolname="alternativeSeniority"]';
var technicalStageFeedBack = 'technical-stage div#recruEdit-editor';
var technicalStageEnglishLevel = 'technical-stage label:contains(" English Level ")';

var clientStageTitle = "span:contains('CLIENT STAGE')";
var clientStageStatus = 'client-stage input.ant-select-selection-search-input.ng-untouched.ng-pristine.ng-valid';
var clientStageDate = 'client-stage [formcontrolname="date"]';
var interviewsHeader = 'span:contains("INTERVIEWS")';
var interviewSectionHeaders = 'client-stage th';

var preOfferStageTitle = "span:contains('PRE OFFER STAGE')";
var preOfferStageStatus = 'pre-offer-stage [formcontrolname="status"]';
var preOfferStageDNI = 'input#dni';

var offerStageTitle = "span:contains('OFFER STAGE')";
var offerStageStatus = 'offer-stage [formcontrolname="status"]';
var offerStageHireDate = 'offer-stage [placeholder="Select date"]';
var offerStageFinalGS = 'offer-stage input#remunerationOffer';
var offerStageVacations = 'offer-stage input#vacationDays';
var offerStageHealthInsurance = 'offer-stage [formcontrolname="healthInsurance"]';



var RecruHomePage = function () {

    this.validatePageAttributes = function () {

        cy.get(newCandidateButton).should('be.visible');
        cy.get(myProcessesTab).should('be.visible');
        cy.get(baStudiotab).should('be.visible');
        cy.get(processesSearchBar).should('be.visible');
    }

    this.validateGridHeaders = function (Tab, headersHashed) {

        if (Tab == "BA Studio") {
            cy.get(baStudiotab).should('be.visible');
            cy.get(baStudiotab).click();
        }
        let gridheaders = [];

        for (let i = 0; i < headersHashed.length; i++) {
            gridheaders.push(headersHashed[i].Headers);
        }
        cy.get(myPrcoessesGridHeader).should('be.visible');
        cy.get(myPrcoessesGridHeader).each(($el, index, $list) => {
            expect($el.text()).to.contains(gridheaders[index]);
        });

    }

    this.clickOnNewCandidateButton = function () {

        cy.get(processesActionsEditButton).should('be.visible');
        cy.get(newCandidateButton).should('be.visible');
        cy.get(newCandidateButton).click();
        cy.get(hrStageTitle).should('be.visible');
    }

    this.validateHRStagePageAttributes = function () {
        cy.get(firstNameField).should('be.visible');
        cy.get(lastnameField).should('be.visible');
        cy.get(emailField).should('be.visible');
        cy.get(phoneNumber).should('be.visible');
        cy.get(linkedIn).should('be.visible');
        cy.get(communityDropdown).should('be.visible');
        cy.get(preferredOffericeDropdown).should('be.visible');
        cy.get(profile).should('be.visible');
        cy.get(hrStageDate).scrollIntoView();
        cy.get(hrStageFeedBack).should('be.visible');
        cy.get(hrStageStatus).should('be.visible');
        cy.get(hrStageDate).scrollIntoView();
        cy.get(hrStageDate).should('be.visible');
        cy.get(hrStageEnglishLevel).scrollIntoView();
        cy.get(hrStageEnglishLevel).should('be.visible');
        cy.get(hrStageSaveButton).should('be.visible');
    }

    this.validatingCommunityDropdown = function () {

        let expectedCommunityValues = [''];
        cy.get(communityDropdown).should('be.visible');
        cy.get(communityDropdown).click();

        let dropdownValues = 'div.ant-select-item-option-content';
        cy.get(dropdownValues).should('be.visible');

        cy.get(dropdownValues).each(($el, index, $list) => {
            cy.log($el.text());
            expect($el.text()).to.contains(expectedCommunityValues[index]);
        });

    }

    this.validateTechnicalStagePageAttributes = function () {

        cy.get(technicalStage).should('be.visible');
        cy.get(technicalStage).click();
        cy.get(tecchnicalStageTitle).should('be.visible');
        cy.get(technicalStageStatus).should('be.visible');
        cy.get(technicalStageInterviewer).should('be.visible');
        cy.get(technicalStageDelegate).should('be.visible');
        cy.get(technicalStageDate).should('be.visible');
        cy.get(technicalStageSeniority).should('be.visible');
        cy.get(technicalStageAlternateSeniority).should('be.visible');
        cy.get(technicalStageFeedBack).should('be.visible');
        cy.get(technicalStageEnglishLevel).should('be.visible');
    }

    this.validateClientStagePageAttributes = function () {
        cy.get(clientStage).should('be.visible');
        cy.get(clientStage).click();
        cy.get(clientStageTitle).should('be.visible');
        cy.get(clientStageStatus).should('be.visible');
        cy.get(clientStageDate).should('be.visible');
        cy.get(clientStageDate).should('be.visible');
        cy.get(interviewsHeader).should('be.visible');
        cy.get(interviewSectionHeaders).should('be.visible');
        cy.get(interviewSectionHeaders).each(($el, index, $list) => {
            cy.log($el.text());
            //expect($el.text()).to.contains(interviewSectionHeaders[index]);
        });

    }

    this.validatePreOfferStagePageAttributes = function () {
        cy.get(preOfferStage).should('be.visible');
        cy.get(preOfferStage).click();
        cy.get(preOfferStageTitle).should('be.visible');
        cy.get(preOfferStageStatus).should('be.visible');
        cy.get(preOfferStageDNI).should('be.visible');
    }

    this.validateOfferStagePageAttributes = function () {

        cy.get(offerStage).should('be.visible');
        cy.get(offerStage).click();
        cy.get(offerStageTitle).should('be.visible');
        cy.get(offerStageStatus).should('be.visible');
        cy.get(offerStageHireDate).should('be.visible');
        cy.get(offerStageFinalGS).should('be.visible');
        cy.get(offerStageHealthInsurance).should('be.visible');
        cy.get(offerStageVacations).should('be.visible');

    }

}
module.exports = new RecruHomePage();

