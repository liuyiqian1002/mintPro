import { encryptByDES } from "@/libs/util"
export default {
    state: {
        cityCode: "111111",
        cityTxt: "",
        fromPath: "",
        userInfo: {},
        coinInfo: {},
        userCoinAddress: {},
        isLogin: false,
        lang: ''
    },
    mutations: {
        setUserInfo(state, data) {
            state.userInfo = data;
            localStorage.setItem('userInfo', encryptByDES(JSON.stringify(data)))
            localStorage.setItem('MINT_auth', encryptByDES(JSON.stringify(data)))
        },
        setIsLogin(state, data) {
            state.isLogin = data;
        },
        setLang(state, data) {
            state.lang = data;
        },
        setUserCoinAddress(state, data) {
            state.userCoinAddress = data;
        },
        setCoinInfo(state, data) {
            state.coinInfo = data;
            localStorage.setItem('coinInfo', encryptByDES(JSON.stringify(data)))
        },
        setCityCode(state, data) {
            state.cityCode = data;
            // localStorage.setItem("cityCode", JSON.stringify(state.cityCode));
        },
        setcityTxt(state, data) {
            state.cityTxt = data;
            // localStorage.setItem("cityTxt", JSON.stringify(state.cityTxt));
        },
        setPath(state, data) {
            state.fromPath = data;
        },

    },
    getters: {},
    actions: {
        setIsLogin({ commit }, data) {
            commit('setIsLogin', data)
        },
        setUserInfo({ commit }, data) {
            commit('setUserInfo', data)
        },
        setLang({ commit }, data) {
            commit('setLang', data)
        },
    }
}