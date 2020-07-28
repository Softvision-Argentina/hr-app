/// <reference types="Cypress" />
var chai = require('chai');
var expect = chai.expect;

let settingsSearchBar = 'input[placeholder="Search for a setting"]';
let subMenus = 'div.ant-layout-sider-children a';
let referenceButton = 'button[nzshape="round"]';
let gridHeaders = 'tr th';

var SettingsPage = function () {

    this.validateSubMenus = function (submenu) {

        cy.get(settingsSearchBar).should('be.visible');
        let submenutitle = [];

        for (let i = 0; i < submenu.length; i++) {
            submenutitle.push(submenu[i].SubMenu);
        }

        cy.get(subMenus).each(($el, index, $list) => {
            cy.log($el.text());
            expect($el.text()).to.contains(submenutitle[index]);
        });
    }

    this.clickOnSubmenu = function (submenu) {

        let subMenu = 'div.ant-layout-sider-children a:contains("' + submenu + '")';

        cy.get(subMenu).should('be.visible');
        cy.get(subMenu).click();

        // please don't delete the below code 

        //cy.get(referenceButton).should('be.visible');
        // cy.get(subMenus).each(($el, index, $list) => {
        //     // if (($el.text()) == submenu) {
        //     //     // cy.log('element text ', $el.text());
        //     //     // cy.log('element index ', $el[index]);
        //     //     // cy.get($el[index]).click();
        //     //     // cy.get(referenceButton).should('be.visible');
        //     // }
        // });
    }

    this.validateGridHeaders = function (hdr) {

        cy.get(gridHeaders).should('be.visible');
        let headers = [];

        for (let i = 0; i < hdr.length; i++) {
            headers.push(hdr[i].gridHeaders);
        }
        cy.get(gridHeaders).should('be.visible');
        cy.get(gridHeaders).each(($el, index, $list) => {

            cy.get($el).should('be.visible');
            expect($el.text()).to.contains(headers[index]);
        });
    }

    this.clickOnButton = function (buttonname) {
        let button = 'button[nzshape="round"] span:contains("' + buttonname + '")';
        cy.get(button).should('be.visible');
        cy.get(referenceButton).click();
    }

    this.validateAttributesinPopUp = function (popup) {

        let popupHeader = 'div.ant-modal-title div';
        cy.get(popupHeader).should('be.visible');

        cy.get(popupHeader).invoke('text').then((hdr) => {
            expect(hdr.toLowerCase()).to.equal(popup.toLowerCase());
        });

        if (popup == 'Add new projection') {

            let dateElement = 'input[placeholder="Select a month"]';
            cy.get(dateElement).should('be.visible');

            let valueElement = 'input#value';
            cy.get(valueElement).should('be.visible');
        }

        else if (popup == 'Add new Candidate Profile') {

            let nameElement = ' input[placeholder = "Please input the Name"]';
            cy.get(nameElement).should('be.visible');

            let descriptionElement = 'input[placeholder="Please input the description"]';
            cy.get(descriptionElement).should('be.visible');
        }

        else if (popup == 'Add new Role') {
            let nameElement = ' input[placeholder = "Please input the Name"]';
            cy.get(nameElement).should('be.visible');

            let statusDropdown = 'nz-select-search input';
            cy.get(statusDropdown).should('be.visible');
        }



        let closePopUpButton = 'button span.ant-modal-close-x';
        cy.get(closePopUpButton).should('be.visible');
        cy.get(closePopUpButton).click();
        cy.get(closePopUpButton).should('not.be.visible');
    }


}

module.exports = new SettingsPage();