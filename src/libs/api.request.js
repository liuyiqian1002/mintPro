import HttpRequest from '@/libs/axios'
import config from '@/config'
const baseUrl = process.env.NODE_ENV === 'development' ? config.baseUrl.dev : config.baseUrl.pro

const axios = new HttpRequest(baseUrl)
export default axios

// import CryptoJS from 'crypto-js';
export function AjaxFunc(method, formData) {
    let config = {
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        }
    }
    const key = "8GfE5qsL";
    //CBC模式加密
    function encryptByDES(message, key) {
        var keyHex = CryptoJS.enc.Utf8.parse(key);
        var ivHex = CryptoJS.enc.Utf8.parse(key);
        let encrypted = CryptoJS.DES.encrypt(message, keyHex, {
            iv: ivHex,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });
        console.log();
        // encrypted.ciphertext.toString();
        return CryptoJS.enc.Base64.stringify(encrypted.ciphertext)
    }

    //CBC模式解密
    function decryptByDES(ciphertext, key) {
        const keyHex = CryptoJS.enc.Utf8.parse(key);
        const iv = CryptoJS.enc.Utf8.parse(key);
        // direct decrypt ciphertext
        const decrypted = CryptoJS.DES.decrypt({
            ciphertext: CryptoJS.enc.Base64.parse(ciphertext)
        }, keyHex, {
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });
        return decrypted.toString(CryptoJS.enc.Utf8);
    }

    const path = '/Index.asmx';
    return new Promise((resolve, reject) => {
        console.log('=========>' + JSON.stringify(formData))
        formData = encryptByDES(JSON.stringify(formData), key);
        // console.log('=========>' + formData)
        axios.post(path + method, `InJson=${ encodeURIComponent(formData)}`, config)
            .then(response => {
                let res = decryptByDES(response.data, key);
                let data = JSON.parse(res);
                console.log(data);
                if (data.Code == 200) {
                    resolve(data, {
                        headers: {
                            'Content-Type': 'Content-Type: application/json'
                        }
                    });

                } else {
                    this.$layer.msg(data.Message);
                    resolve(data, {
                        headers: {
                            'Content-Type': 'Content-Type: application/json'
                        }
                    });
                    // this.$router.push('/login')
                }
            }, err => {
                // this.$layer.msg('网络错误!请联系管理员');
                reject(err)
            })
    })
}