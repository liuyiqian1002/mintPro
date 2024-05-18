export default [
    // {
    // path: '/',
    // redirect: '/index'
    // },
    {
        path: '/',
        name: 'Home',
        meta: {
            title: '{{base.title}}',
        },
        component: () =>
            import ('@/views/index.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/property',
        name: 'property',
        meta: {
            title: '{{property.title}}',
        },
        component: () =>
            import ('@/views/property.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/mine',
        name: 'mine',
        meta: {
            title: '{{mine.title}}',
        },
        component: () =>
            import ('@/views/mine.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/receiving_qrcode',
        name: 'receivingQRCode',
        meta: {
            title: '{{receivingQRCode.title}}',
        },
        component: () =>
            import ('@/views/receivingQRCode.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/transaction_pwd',
        name: 'transactionPwd',
        meta: {
            title: '{{transactionPwd.title}}',
        },
        component: () =>
            import ('@/views/transactionPwd.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/login_pwd',
        name: 'loginPwd',
        meta: {
            title: '{{loginPwd.title}}',
        },
        component: () =>
            import ('@/views/loginPwd.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/contract',
        name: 'contract',
        meta: {
            title: '{{contract.title}}',
        },
        component: () =>
            import ('@/views/contract.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/acc_record',
        name: 'accRecord',
        meta: {
            title: '{{accRecord.title}}',
        },
        component: () =>
            import ('@/views/accRecord.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/withdraw_coin',
        name: 'withdrawCoin',
        meta: {
            title: '{{withdrawCoin.title}}',
        },
        component: () =>
            import ('@/views/withdrawCoin.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/reference_asset',
        name: 'referenceAsset',
        meta: {
            title: '{{referenceAsset.title}}',
        },
        component: () =>
            import ('@/views/referenceAsset.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/user_info',
        name: 'userInfo',
        meta: {
            title: '{{userInfo.title}}',
        },
        component: () =>
            import ('@/views/user/userInfo.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/nickname',
        name: 'nickname',
        meta: {
            title: '{{nickname.title}}',
        },
        component: () =>
            import ('@/views/user/Nickname.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/transaction',
        name: 'transaction',
        meta: {
            title: '{{transaction.title}}',
        },
        component: () =>
            import ('@/views/transaction.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/recommend_friend',
        name: 'recommendFriend',
        meta: {
            title: '{{recommendFriend.title}}',
        },
        component: () =>
            import ('@/views/recommendFriend.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/forget_pwd',
        name: 'forgetPwd',
        meta: {
            title: '{{forgetPwd.title}}',
        },
        component: () =>
            import ('@/views/forgetPwd.vue')
            // component: resolve => require(['@/views/RetPassword/RetPassword.vue'],resolve)
    },
    {
        path: '/login',
        name: 'login',
        meta: {
            title: '{{login.title}}',
        },
        component: () =>
            import ('@/views/login.vue')
    },
    {
        path: '/register',
        name: 'register',
        meta: {
            title: '{{register.title}}',
        },
        component: () =>
            import ('@/views/register.vue')
    },
    {
        path: '/set_lan',
        name: 'setLan',
        meta: {
            title: '{{setLan.title}}',
        },
        component: () =>
            import ('@/views/setLan.vue')
    },
    {
        path: '/safety',
        name: 'safety',
        meta: {
            title: '{{safety.title}}',
        },
        component: () =>
            import ('@/views/safety.vue')
    },
    {
        path: '/recharge',
        name: 'recharge',
        meta: {
            title: '{{recharge.title}}',
        },
        component: () =>
            import ('@/views/recharge.vue')
    },
    {
        path: '/add_address',
        name: 'add_address',
        meta: {
            title: '{{addAddress.title}}',
        },
        component: () =>
            import ('@/views/addAddress.vue')
    },
    {
        path: '/address',
        name: 'address',
        meta: {
            title: '{{Address.title}}',
        },
        component: () =>
            import ('@/views/Address.vue')
    },
    {
        path: '/market',
        name: 'market',
        meta: {
            title: '{{market.title}}',
        },
        component: () =>
            import ('@/views/market.vue')
    },
    {
        path: '/deal',
        name: 'deal',
        meta: {
            title: '{{base.deal}}',
        },
        component: () =>
            import ('@/views/deal.vue')
    },
    {
        path: '/contract_trad',
        name: 'contractTrad',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/contractTrad.vue')
    },
    {
        path: '/exchange',
        name: 'exchange',
        meta: {
            title: '{{exchange.title}}',
        },
        component: () =>
            import ('@/views/exchange.vue')
    },
    {
        path: '/entrust',
        name: 'entrust',
        meta: {
            title: '{{entrust.title}}',
        },
        component: () =>
            import ('@/views/entrust.vue')
    },
    {
        path: '/history',
        name: 'history',
        meta: {
            title: '{{history.title}}',
        },
        component: () =>
            import ('@/views/history.vue')
    },
    {
        path: '/news',
        name: 'news',
        meta: {
            title: '{{news.title}}',
        },
        component: () =>
            import ('@/views/News.vue')
    },
    {
        path: '/transaction_record',
        name: 'transactionRecord',
        meta: {
            title: '{{transactionRecord.title}}',
        },
        component: () =>
            import ('@/views/transactionRecord.vue')
    },
    {
        path: '/trad',
        name: 'trad',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/trad.vue')
    },
    {
        path: '/withdraw_record',
        name: 'withdrawRecord',
        meta: {
            // title: '{{withdrawRecord.title}}',
        },
        component: () =>
            import ('@/views/withdrawRecord.vue')
    },
    {
        path: '/mining',
        name: 'mining',
        meta: {
            title: '{{mining.title}}',
        },
        component: () =>
            import ('@/views/mining.vue')
    },
    {
        path: '/product_details',
        name: 'product_details',
        meta: {
            title: '{{productDetails.title}}',
        },
        component: () =>
            import ('@/views/productDetails.vue')
    },
    {
        path: '/success',
        name: 'success',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/success.vue'),
    },
    {
        path: '/order',
        name: 'order',
        meta: {
            title: 'Order',
        },
        component: () =>
            import ('@/views/order.vue'),
    },
    {
        path: '/financial',
        name: 'financial',
        meta: {
            title: '{{financial.title}}',
        },
        component: () =>
            import ('@/views/financial.vue'),
    },
    {
        path: '/financial_admin',
        name: 'financialAdmin',
        meta: {
            title: '{{financialAdmin.title}}',
        },
        component: () =>
            import ('@/views/financialAdmin.vue'),
    },
    {
        path: '/mineral_record',
        name: 'MineralRecord',
        meta: {
            title: '{{mineralRecord.title}}',
        },
        component: () =>
            import ('@/views/MineralRecord.vue'),
    },
    {
        path: '/guess',
        name: 'Guess',
        meta: {
            title: '{{guess.title}}',
        },
        component: () =>
            import ('@/views/guess.vue'),
    },
    {
        path: '/blind_box',
        name: 'blindBox',
        meta: {
            title: '{{BlindBox.title}}',
        },
        component: () =>
            import ('@/views/game/BlindBox.vue'),
    },
    {
        path: '/blind_box_record',
        name: 'BlindBoxRecord',
        meta: {
            title: '{{BlindBoxRecord.title}}',
        },
        component: () =>
            import ('@/views/game/BlindBoxRecord.vue'),
    },
    {
        path: '/blind_box_voucher',
        name: 'BlindBoxVoucher',
        meta: {
            title: '{{BlindBoxVoucher.title}}',
        },
        component: () =>
            import ('@/views/game/BlindBoxVoucher.vue'),
    },
    {
        path: '/recharge_qrcode',
        name: 'recharge_qrcode',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/rechargeQrcode.vue'),
    },
    {
        path: '/account',
        name: 'account',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/account.vue'),
    },
    {
        path: '/coins',
        name: 'coins',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/coins.vue'),
    },
    {
        path: '/buyCoinsRecord',
        name: 'buyCoinsRecord',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/buyCoinsRecord.vue'),
    },
    {
        path: '/coinsOrder',
        name: 'buyCoinsOrder',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/coinsOrder.vue'),
    },
    {
        path: '/sellCoinsRecord',
        name: 'sellCoinsRecord',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/sellCoinsRecord.vue'),
    },
    {
        path: '/authorization',
        name: 'authorization',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/authorization.vue'),
    },
    {
        path: '/bind_account',
        name: 'bindAccount',
        meta: {
            title: '',
        },
        component: () =>
            import ('@/views/bindAccount.vue'),
    },
    {
        path: '/h5',
        name: 'h5',
        meta: {
            // title: '{{withdrawRecord.title}}',
        },
        component: () =>
            import ('@/components/H5.vue')
    },
]