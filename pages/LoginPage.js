var chai = require('chai');
var expect = chai.expect;

var encyptDecrypt = require('../utilities/EncryptDecrypt.js');
let usernameElement = 'input[formcontrolname="userName"]';
let passwordElement = 'input[formcontrolname="password"]';
let loginButton = 'nz-form-item.login-input-container.ant-form-item.ant-row button';
let recruLogo = 'img.recru-logo-black';
let homePageUserNanme = 'div.user-info';
let homePageLogo = 'div#logo img';
let myProcesses = 'div.ant-tabs-tab.ant-tabs-tab-active.ng-star-inserted';
let statusCode = 'app-unauthorized div h1';
let noUsernameMessage = "nz-form-control div:contains(' Please input your username! ')";
let noPasswordMessage = "nz-form-control div:contains(' Please input your Password! ')";
let profilebutton = '.ant-avatar > .anticon > svg';
let logoutButton = '.ant-list-items > :nth-child(3)';

var RecruLoginPage = function () {

    this.testDataSetUp = function () {

    }

    this.NavigatetoURL = function (URL) {
        cy.visit(URL);
    }

    this.NavigatetoLoginPage = function (URL) {
        cy.get(profilebutton).click();
        cy.get(logoutButton).should('be.visible');
        cy.get(logoutButton).click();
        cy.get(recruLogo).should('be.visible');

    }

    this.verifyRecruLogo = function () {
        cy.get(recruLogo).should('be.visible');
    }

    this.inputLoginDetails = function (username, password) {

        if (username != '') {
            cy.get(usernameElement).type(username);
        }
        if (password != '') {
            cy.get(passwordElement).type(encyptDecrypt.decrypt(password));
        }

        cy.get(loginButton).click();
    }

    this.validateUserNameinHomePage = function (username) {

        cy.get(homePageLogo).should('be.visible');
        cy.get(myProcesses).should('be.visible');

        let b = username.split('@');
        let c = b[0].split('.');
        let name = ' ' + c[0] + ' ' + c[1] + ' ';

        cy.get(homePageUserNanme).invoke('text').then((UN) => {
            expect(UN.toLowerCase()).to.equal(name);
        });
    }

    this.validateMessage = function (context) {

        if (context == 'Wrong Password' || context == 'Wrong Username') {
            cy.get(statusCode).invoke('text').then((SC) => {
                expect(SC).to.equal('403');
            });
        }

        else if (context == 'No Username') {
            cy.get(noUsernameMessage).should('be.visible');
        }

        else if (context == 'No Password') {
            cy.get(noPasswordMessage).should('be.visible');
        }
    }

}

module.exports = new RecruLoginPage();