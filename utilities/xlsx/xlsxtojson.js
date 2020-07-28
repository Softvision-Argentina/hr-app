
//'use strict';

Data = require("xlsx-to-json");

module.exports = {
  setdupata: () => {
    Data({
      input: "./RunManager_Test.xlsx",  // input xls 
      output: ".//json/Authentication/Authentication.json", // output json 
      sheet: "Authentication"  // specific sheetname 
    }, function (err, result) {
      if (err) {
        console.error(err);
      } else {
        //console.log(result);
      }
    });
  }
}




