import Vue from 'vue'
import Router from 'vue-router'
import routes from './routers'
import store from '@/store'
import {
    setToken,
    getToken,
    canTurnTo,
    setTitle
} from '@/libs/util'
import config from '@/config'
const {
    homeName
} = config
Vue.use(Router)
const router = new Router({
        // mode: 'history',
        routes
    })
    /**
     * 重写路由的push方法
     */
const routerPush = Router.prototype.push
Router.prototype.push = function push(location) {
    return routerPush.call(this, location).catch(error => error)
}
const LOGIN_PAGE_NAME = 'login'

const turnTo = (to, access, next) => {
        if (canTurnTo(to.name, access, routes)) next() // 有权限，可访问
        else next({
                replace: true,
                name: 'error_401'
            }) // 无权限，重定向到401页面
    }
    // router.afterEach((to, from) => {     
    //   // window.__wxjs_is_wkwebview
    //     // true 时 为 IOS 设备
    //     // false时 为 安卓 设备
    //   if (window.__wxjs_is_wkwebview) {  // IOS
    //     if (window.entryUrl == '' || window.entryUrl == undefined) {
    //       var url = `${FRONT_BASE}${to.fullPath}`
    //       window.entryUrl = url    // 将后面的参数去除
    //     }
    //     getWxAuth(to.fullPath,"ios")
    //   }else {       // 安卓
    //     setTimeout(function () {
    //       getWxAuth(to.fullPath,"android")
    //     }, 500);
    //   }
    // })
    // router.beforeEach((to, from, next) => {
    //   let url = window.location.href;
    //   console.log(url)
    //   if (url.includes('?from=')) { // 判断是否携带了 from 参数，这一步灵活变通，只要能判断出是从微信分享链接进来的就 OK
    //     url = url.replace(/com.+.#/, 'com/#'); // 利用正则表达式去掉微信携带的 ?from=singlemessage&isappinstalled=0 这串参数，如果这串参数对于你当前的页面有用处的话，可以重新拼接到你正常的链接后面去
    //     console.log('url is '+url)
    //     window.location.href = url; // 重定向到正常链接
    //   } else {
    //     next()
    //   }

// })
// router.beforeEach((to, from, next) => {
//   // iView.LoadingBar.start()
//   const token = getToken()
//   if (!token && to.name !== LOGIN_PAGE_NAME) {
//     // 未登录且要跳转的页面不是登录页
//     next({
//       name: LOGIN_PAGE_NAME // 跳转到登录页
//     })
//   } else if (!token && to.name === LOGIN_PAGE_NAME) {
//     // 未登陆且要跳转的页面是登录页
//     next() // 跳转
//   } else if (token && to.name === LOGIN_PAGE_NAME) {
//     // 已登录且要跳转的页面是登录页
//     next({
//       name: homeName // 跳转到homeName页
//     })
//   } else {
//     if (store.state.user.hasGetInfo) {
//       turnTo(to, store.state.user.access, next)
//     } else {
//       store.dispatch('getUserInfo').then(user => {
//         // 拉取用户信息，通过用户权限和跳转的页面的name来判断是否有权限访问;access必须是一个数组，如：['super_admin'] ['super_admin', 'admin']
//         turnTo(to, user.access, next)
//       }).catch(() => {
//         setToken('')
//         next({
//           name: 'login'
//         })
//       })
//     }
//   }
// })
const whiteList = ['/login', '/', '/market', '/H5', '/set_lan', '/register', '/trad', '/news', '/forget_pwd', '/financial', '/mining', '/guess','/coins', '/blind_box'];
router.beforeEach((to, from, next) => {
    console.log('to', to.path);
    var hideServiceList = ['/coins','/buyCoinsRecord','/coinsOrder','/sellCoinsRecord'] ;
    if(hideServiceList.includes(to.path)){
        localStorage.setItem('hideService',true)
    }else{
        localStorage.removeItem('hideService')
    }
    console.log('from', from);
    setTitle(to, router.app)
    console.log(to.path)
        //新开页面自动滚到顶部
    if (!to.meta.keepAlive && !from.meta.keepAlive) {
        console.log('scroll')
        window.scrollTo(0, 0)
    }
    const flag = localStorage.getItem("MINT_auth") ? true : false; //鉴权
    if (flag) {
        next();
    } else {
        if (whiteList.indexOf(to.path) !== -1) { // 在免登录白名单，直接进入
            next(); //记得当所有程序执行完毕后要进行next()，不然是无法继续进行的;
        } else {
            console.log('to', to);
            console.log('from', from);
            next({
                path: '/login',
                replace: true,
                query: {
                    // redirect: from.fullPath
                }
            })
        }
    }
})

export default router