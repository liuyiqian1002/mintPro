<template>
  <div class="exchaneg_wrap_main">
    <van-nav-bar
      :title="($t('exchaneg.title')+(type == 1?$t('exchaneg.type'):$t('exchaneg.type1')))"
      left-text
      left-arrow
      fixed
      @click-left="$router.go(-1)"
    />
    <div class="exchaneg_wrap">
      <div class="con">
          <p>{{$t('exchaneg.account')}}</p>
            <ul class="exchaneg">
              <li>
                {{ type == 1 ? $t("exchaneg.acc") : $t("exchaneg.acc1") }}
                <img src="../assets/images/common/exchange_to.png" alt="" />
                {{ type == 1 ? $t("exchaneg.acc1") : $t("exchaneg.acc") }}

              </li>
              <li @click="change">
                  <img src="../assets/images/common/ic_transfer_exchang.png" alt="" />{{$t('exchaneg.exchaneg')}}
              </li>
            </ul>
          <p>{{ $t("exchaneg.contractType") }}</p>
          <ul>
            <li>{{ BaseCurrency }}</li>
          </ul>
        <p>{{ $t("exchaneg.Amount") }}</p>
        <ul>
          <li>
            <input
              type="text"
              v-model="Quantity"
              :placeholder="$t('exchaneg.AmountText')"
            />
          </li>
          <li>
            <span>USDT</span>
            <span @click="Quantity = Balance">{{ $t("exchaneg.all") }}</span>
          </li>
        </ul>
        <p class="tips"><span>{{ $t("exchaneg.Available") }}</span> <span>{{ Balance }} USDT</span> </p>
        <p class="tips">
          {{ $t("exchaneg.tips") }}
        </p>
      </div>
      <div class="btns">
        <button class="main_btn" @click="showPwd">
          {{$t("recharge.confirm") }}
          <!-- {{ type == 1 ? $t("recharge.confirm") : $t("exchaneg.type1") }} -->
        </button>
      </div>
    </div>
    <van-dialog
      v-model="show"
      :title="$t('login.password')"
      @confirm="submitFuturesTransfer"
      show-cancel-button
    >
      <!-- <p
        class="pwd_tips"
        @click="showKeyboard = !showKeyboard"
        v-show="!Password"
      >
        {{ $t("login.password") }}
      </p> -->
      <ul
        class="pwd_num"
        @click="showKeyboard = !showKeyboard"
      >
        <li v-for="i in Password"></li>
        <li class="unent" v-for="i in (6-Password.length)"></li>

      </ul>
    </van-dialog>
    <van-number-keyboard
      :show="showKeyboard"
      @input="onInput"
      @delete="onDelete"
      @blur="showKeyboard = false"
    />
  </div>
</template>

<script>
import {
  Icon,
  NavBar,
  IndexBar,
  Picker,
  Popup,
  Tab,
  Tabs,
  PasswordInput,
  NumberKeyboard,
  Empty,
  Sticky,
  Dialog,
} from "vant";
import Vue from "vue";
Vue.use(PasswordInput);
Vue.use(NumberKeyboard);
import { GetTransferBalanceByDirection, SubmitFuturesTransfer } from "@/api";
export default {
  name: "exchange",
  components: {
    [Icon.name]: Icon,
    [IndexBar.name]: IndexBar,
    [NavBar.name]: NavBar,
    [Picker.name]: Picker,
    [Popup.name]: Popup,
    [Empty.name]: Empty,
    [Tab.name]: Tab,
    [Tabs.name]: Tabs,
    [Popup.name]: Popup,
    [Sticky.name]: Sticky,
    [Dialog.name]: Dialog,
  },
  data() {
    return {
      type: "",
      SymbolsId: "",
      MINT_auth: {},
      Balance: "",
      Password: "",
      showKeyboard: false,
      show: false,
      Password: "",
      Quantity: "",
      BaseCurrency: "",
    };
  },
  mounted() {
    document.body.style.background = "#F5F5F5";
    this.type = this.$route.query.type;
    this.BaseCurrency = this.$route.query.BaseCurrency;
    this.SymbolsId = this.$route.query.SymbolsId;
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    this.getTransferBalanceByDirection();
    document.title =
      this.$t("exchaneg.title") +
      (this.type == 1 ? this.$t("exchaneg.type") : this.$t("exchaneg.type1"));
  },
  methods: {
    // 合约交易 => 获取合约账户划入、划出页面可用余额
    change() {
      this.type == 1 ? (this.type = 2) : (this.type = 1);
    },
    getTransferBalanceByDirection() {
      let data = {
        UserId: this.MINT_auth.UserId, //用户ID</br>
        SymbolsId: this.SymbolsId, //交易对主键编号<br/>
        Direction: this.type == 1 ? "Into" : "DrawOut", //操作方向(Into-划入页面可用余额、DrawOut-划出页面可用余额)
      };
      console.log(data);
      GetTransferBalanceByDirection(data).then((r) => {
        if (r.data.Code == 200) {
          console.log(r.data);
          this.Balance = r.data.Data;
        }
      });
    },
    submitFuturesTransfer(action, done) {
      if (!this.Password) {
        // this.$toast('密码不格式不正确');w
        return;
      }
      let data = {
        UserId: this.MINT_auth.UserId, //用户ID</br>
        SymbolsId: this.SymbolsId, //交易对主键编号<br/>
        Direction: this.type == 1 ? "Into" : "DrawOut", //操作方向(Into-币币账户划入到合约账户、DrawOut-合约账户划出到币币账户)</br>
        Quantity: this.Quantity, //数量<br/>
        Password: this.Password, //支付密码<br/>
      };
      this.$toast.loading({
        duration: 0, // 持续展示 toast
        forbidClick: true,
      });
      SubmitFuturesTransfer(data).then((r) => {
        this.$toast.clear();
        if (r.data.Code == 200) {
          Dialog.confirm({
            title: this.$t("base.tips"),
            message: r.data.Message,
            confirmButtonText: this.$t("base.confirm"),
            cancelButtonText: this.$t("base.cancel"),
          })
            .then(() => {
              this.$router.replace({
                path: "/contract_trad",
                query: {
                  SymbolsId: this.SymbolsId,
                },
              });
            })
            .catch(() => {
              // on cancel
            });
        } 
      });
    },
    showPwd() {
      if (!this.Quantity) {
        this.$toast(this.$t("exchaneg.AmountText"));
        return;
      }
      this.show = true;
      this.showKeyboard = true;
      this.Password = "";
    },
    onInput(key) {
      this.Password = (this.Password + key).slice(0, 6);
      console.log(this.Password);
    },
    onDelete() {
      this.Password = this.Password.slice(0, this.Password.length - 1);
    },
  },
  destroyed() {
    document.body.style.background = "#fff";
  },
};
</script>
<style lang="less" >

  .exchaneg_wrap_main{
    background-color: @bg_color;
    .van-hairline--bottom::after{
      border:none;
    }
  }
  .van-overlay {
    z-index: 10 !important;
  }
</style>
<style lang="less" scoped>
  .pwd_tips {
    text-align: center;
    height: 90px;
    // margin: 10px;/*  */
    line-height: 90px;
  }
  .pwd_num {
    display: flex;
    height: 90px;
    justify-content: center;
    align-items: center;
    li {
      border-radius: 50%;
      width: 30px;
      height: 30px;
      background-color: #000;
      margin: 0 10px;
      &.unent{
        margin: 0 10px;
        border: none;
        background-color: transparent;
        position: relative;
        &:before{
          position: absolute;
          content: '';
          display: block;
          width: 30px;
          height: 2px;
          background-color: #000;
          top: 50%;
          left: 0;
          transform: translate(0,-50%);
        }
      }
    }
  }
  .exchaneg_wrap {
    height: 100vh;
    padding:0 20px;
    padding-top: 46PX;
    .con {
      border-radius: 4px;
      margin-top:10px;
      // padding: 20px;
      p {
        color: @sub-font-color;
        padding: 15px 0;
        &.tips {
          color: @font_f_color;
          display: flex;
          justify-content: space-between;
        }
      }
      ul {
        display: flex;
        justify-content: space-between;
        margin: 10px 0;
        background-color: @panel_color;
        border-radius: 12px;
        padding: 0 30px;
        height: 120px;
        align-items: center;
        input{
          height: 100px;
          background-color: @panel_color;
        }

        li {
          color: @sub-font-color;
          img {
            height: 60px;
          }
          span {
            display: inline-block;
            padding: 5px 20px;
            &:nth-of-type(2) {
              .mc;
              border-left: 1px solid #eee;
            }
          }
        }
        &.exchaneg{
          li{
            &:nth-of-type(1){
              display: flex;
              align-items: center;
              color: @font_1_color;
              img{
                width: 30px;
                height: 30px;
                margin:0 20px;
              }

            }
            &:nth-of-type(2){
              display: flex;
              align-items: center;
              .mc;
              img{
                width: 28px;
                height: 25px;
                margin-right: 10px;
              }
            }

          }
        }
      }
    }
    .btns {
      width: 100%;
      margin: 120px auto 0;
    }
  }
</style>