import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import 'lib-flexible/flexible'
import config from '@/config'
import '@/assets/icons/iconfont.css'
import 'swiper/css/swiper.css';
import i18n from '@/libs/i18n'
import axios from 'axios'
import { decryptByDES } from "@/libs/util.js";
// import FastClick from 'fastclick'
import * as filters from '@/filters';
//复制到粘贴板插件
Object.keys(filters).forEach((key) => {
    Vue.filter(key, filters[key]);
});
import {
    Lazyload,
    Toast,
    Dialog
} from 'vant';
// FastClick.attach(document.body);
// Vue.use(Lazyload, {
//     preLoad: 1.3,
//     error: 'http://m.huizhi-intl.com/StaticFile/img/fangji/fangji2.jpg',
//     loading: 'http://m.huizhi-intl.com/StaticFile/img/fangji/fangji2.jpg'
// });
Vue.use(Toast);
Vue.use(Dialog);
import waterfall from 'vue-waterfall2'
Vue.use(waterfall)
import VueClipboard from 'vue-clipboard2'
Vue.use(VueClipboard)
    /** 
     * 使用方法
     * const createDate = this.$moment(this.createTime).format('YYYY-MM-DD')
     */
import moment from 'moment'
Vue.prototype.$moment = moment;
Vue.prototype.axios = axios;
Vue.config.productionTip = false
axios.defaults.baseURL = process.env.NODE_ENV === 'development' ? '/api' : '',

    /**
     * @description 全局注册应用配置
     */
    Vue.prototype.$config = config
Vue.prototype.$decryptByDES = decryptByDES
    // import VConsole from "vconsole/dist/vconsole.min.js"; //import vconsole
    // let vConsole = new VConsole();

new Vue({
    i18n, //挂载i18n
    router,
    store,
    render: h => h(App)
}).$mount('#app')