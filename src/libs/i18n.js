import Vue from 'vue'
import VueI18n from 'vue-i18n'
Vue.use(VueI18n)

// 以下为语言包单独设置的场景，单独设置时语言包需单独引入
// 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
const messages = {
    'zh-CN': require('../assets/i18n/zh-CN'), // 中文语言包
    'en': require('../assets/i18n/en'), // 英文语言包
    'zh-hk': require('../assets/i18n/zh-hk'),
    'jpn': require('../assets/i18n/jpn')
        // 'en_US': require('../assets/i18n/en') // 英文语言包
}
let lang = localStorage.getItem('MINT_lang') ? localStorage.getItem('MINT_lang') : 'en';
// 最后 export default，这一步肯定要写的。
export default new VueI18n({
    locale: lang, // set locale 默认显示英文
    messages: messages // set locale messages
})