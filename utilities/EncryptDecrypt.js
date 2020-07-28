var crypto = require('crypto');
var encryptionkey = "jIn0NXMOaPHua4gkDyMkTp1Tzq4K7UgJ";

var EncryptDecrypt = function () {

    this.encrypt = function encrypt(data) {
        var text = JSON.stringify(data);
        var textBuffer = new Buffer(text, 'utf8');
        var cipher = crypto.createCipher('aes-256-ecb', encryptionkey);
        cipher.write(textBuffer);
        cipher.end();
        return cipher.read().toString('hex');
    };

    this.decrypt = function decrypt(hexString) {
        var hexBuffer = new Buffer(hexString, 'hex');
        var decipher = crypto.createDecipher('aes-256-ecb', encryptionkey);
        decipher.write(hexBuffer);
        decipher.end();
        var data = decipher.read().toString('utf8');
        return JSON.parse(data);
    };
}
module.exports = new EncryptDecrypt();