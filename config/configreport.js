var reporter = require('cucumber-html-reporter');

var options = {
    theme: 'bootstrap',
    jsonFile: 'reports/Recru_Smoke.cucumber.json',
    output: 'reports/cucumber_report.html',
    screenshotsDirectory: "../cypress/screenshots/",
    storeScreenshots: true,
    reportSuiteAsScenarios: true,
    scenarioTimestamp: true,
    launchReport: false,
    metadata: {
        "App Version": "1.0.0",
        "Test Environment": "Test Environment",
        "Browser": "Chrome  54.0.2840.98",
        "Platform": "Windows 10",
        "Parallel": "Scenarios",
        "Executed": "Remote"
    }
};

reporter.generate(options);