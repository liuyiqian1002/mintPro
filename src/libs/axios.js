import axios from 'axios'
import store from '@/store'
import i18n from '@/libs/i18n'
import Vuex from 'vuex'
import {
    setToken,
    getToken,
    encryptByDES,
    decryptByDES,
} from "@/libs/util";
// import { Spin } from 'iview'
import {
    Toast
} from 'vant';
import router from '@/router';
import { lang } from 'moment';
const toLogin = () => {
        router.replace({
            path: '/login',
            // query: {
            //     redirect: router.currentRoute.fullPath
            // }
        });
    }
    /**
     * 请求失败后的错误统一处理
     * @param {Number} status 请求失败的状态码
     */
const errorHandle = (res, other) => {
    let lang = localStorage.getItem('MINT_lang') ? localStorage.getItem('MINT_lang') : 'zh-CN'
        // Toast(i18n.messages[lang].base.err)
        // 状态码判断
    switch (other.Code) {
        case 600:
            // if (res.data.status == "notlogin") {
            setToken();
            localStorage.removeItem('MINT_auth')
            localStorage.removeItem('userInfo')
            localStorage.removeItem("isRead");
            localStorage.removeItem("isBindPhone");
            Toast(i18n.messages[lang].base.readLogin)
            setTimeout(() => {
                    toLogin()
                }, 1000)
                // }
                // Toast(res.data.msg?res.data.msg:'系统错误，请联系管理员');
            break;
            //     // 401: 未登录状态，跳转登录页
        case 400:
            // toLogin();
            break;
            // 403 token过期
            // 清除token并跳转登录页
            // case 403:
            //     Toast('登录过期，请重新登录');
            //     setToken('');
            //     break;
            //     // 404请求不存在
            // case 404:
            //     Toast('请求的资源不存在');
            //     break;
            // default:
        case 200:
            // Toast('请求的资源不存在');
            break;
        default:
            console.log(res)
            console.log(other);
            Toast(other.Message);
    }
}
const addErrorLog = errorInfo => {
    const {
        statusText,
        status,
        request: {
            responseURL
        }
    } = errorInfo
    let info = {
            type: 'ajax',
            code: status,
            mes: statusText,
            url: responseURL
        }
        // if (!responseURL.includes('save_error_logger')) store.dispatch('addErrorLog', info)
}

class HttpRequest {
    constructor(baseUrl = baseURL) {
        this.baseUrl = baseUrl
        this.queue = {}
    }
    getInsideConfig() {
        const config = {
            baseURL: this.baseUrl,
            headers: {
                //
            }
        }
        return config
    }
    destroy(url) {
        delete this.queue[url]
        if (!Object.keys(this.queue).length) {
            // Spin.hide()
        }
    }
    interceptors(instance, url) {
        // 请求拦截
        instance.interceptors.request.use(config => {
                //请求添加token
                // const token = getToken();
                // token && (config.headers.Authorization = token);
                // 添加全局的loading...
                if (!Object.keys(this.queue).length) {
                    // Spin.show() // 不建议开启，因为界面不友好
                }
                this.queue[url] = true
                return config
            }, error => {
                return Promise.reject(error)
            })
            // 响应拦截
        instance.interceptors.response.use(res => {
            this.destroy(url)
            const {
                data,
                status
            } = res
            errorHandle(res, JSON.parse(decryptByDES(data)));
            return {
                data: JSON.parse(decryptByDES(data)),
                status
            }
        }, error => {
            this.destroy(url)
            let errorInfo = error.response
            if (!errorInfo) {
                const {
                    request: {
                        statusText,
                        status
                    },
                    config
                } = JSON.parse(JSON.stringify(error))
                errorInfo = {
                    statusText,
                    status,
                    request: {
                        responseURL: config.url
                    }
                }
            }
            addErrorLog(errorInfo)
            console.log(i18n.messages)
            let lang = localStorage.getItem('MINT_lang') ? localStorage.getItem('MINT_lang') : 'zh-CN'
                // Toast(i18n.messages[lang].base.err)
            return Promise.reject(error)
        })
    }
    request(options) {
        const { data, url } = options;
        let LanguageVersion = localStorage.getItem('MINT_lang') ? localStorage.getItem('MINT_lang') : 'zh-CN';
        console.log('LanguageVersion>>>>>', LanguageVersion)
        let isLogin = !!localStorage.getItem('MINT_auth')
        let isApp = !!localStorage.getItem('isApp')
        let hideService = !!localStorage.getItem('hideService')
        let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = MINT_auth ? decryptByDES(MINT_auth) : "";
        MINT_auth = MINT_auth ? JSON.parse(MINT_auth) : "";
        console.log(MINT_auth)
        if(isApp||hideService){
            isLogin = false
        }
        store.dispatch('setIsLogin', isLogin);
        store.dispatch('setLang', LanguageVersion);
        if (MINT_auth) {
            store.dispatch('setUserInfo', MINT_auth);
        }
        data.LanguageVersion = LanguageVersion;
        console.log('>>>>>>>>>', data);
        let params = `InJson=${ encodeURIComponent(encryptByDES(JSON.stringify(data)))}`;
        options.data = params;
        options.url = '/Index.asmx' + url;
        const instance = axios.create()
        options = Object.assign(this.getInsideConfig(), options)
        this.interceptors(instance, options.url)
        return instance(options)
    }
}
export default HttpRequest