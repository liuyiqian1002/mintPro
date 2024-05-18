<template>
  <div>
    <van-nav-bar
      :title="$t('transaction.title')"
      left-text
      left-arrow
      @click-left="$router.go(-1)"
      @click-right="$router.push('/transaction_record')"
    >
    <template #right>
        <van-icon
          :name="require('../assets/images/common/ic_withdrawl_coin_details.png')"
          size="18"
        />
      </template>
</van-nav-bar>
    <div class="transaction_wrap">
      <div class="contnet">
        <label>{{$t('transaction.ReceiverEnCode')}}:</label>
        <ul>
          <li>
            <input
              type="text"
              v-model="ReceiverEnCode"
              :placeholder="$t('transaction.ReceiverEnCodeTxt')"
            />
          </li>
        </ul>
        <label>{{$t('transaction.Coin')}}</label>
        <ul @click="showPopup">
          <li>
            <input type="text" readonly v-model="Coin" :placeholder="$t('transaction.CoinTxt')" />
          </li>
          <li>
            <img src="../assets/images/common/back_right.png" alt="" />
          </li>
        </ul>
        <label>{{$t('transaction.Quantity')}}</label>
        <ul>
          <li>
            <input
              type="text"
              v-model="Quantity"
              :placeholder="$t('transaction.QuantityTxt')"
            />
          </li>
        </ul>
        <label>{{$t('withdraw.WithdrawPassword')}}</label
            >
        <ul @click="showKeyboard = true">
          <li>
            <input
              type="password"
              v-model="WithdrawPassword"
              readonly
              :placeholder="$t('withdraw.pwdTxt')"
            />
          </li>
        </ul>
        <label>{{$t("transaction.vcode")}}</label>
        <ul>
          <li>
            <input
              type="text"
              v-model="Code"
              :placeholder="$t('transactionPwd.vcode')"
            />
          </li>
          <li>
            <button class="vcode_bnt" :disabled="!isGetCode" @click="getCode">
              {{ btnTips }}
            </button>
          </li>
        </ul>
      </div>
      <div class="btns">
        <button class="main_btn" @click="userTransfer">{{$t('base.confirm')}}</button>
      </div>
    </div>
    <van-number-keyboard
      :show="showKeyboard"
      @input="onInput"
      @delete="onDelete"
      @blur="showKeyboard = false"
    />
    <van-popup v-model="showPicker" position="bottom" round>
      <van-picker
        show-toolbar
        :columns="Symbols"
        @cancel="showPicker = false"
        @confirm="onConfirm"
      />
    </van-popup>
  </div>
</template>

<script>
import { UserTransfer, GetSymbolsPageList,GetMobileCode } from "@/api";
import {
  Icon,
  NavBar,
  IndexBar,
  Picker,
  Popup,
  PasswordInput,
  NumberKeyboard,
} from "vant";
import Vue from "vue";
Vue.use(PasswordInput);
Vue.use(NumberKeyboard);
export default {
  name: "transaction",
  components: {
    [Icon.name]: Icon,
    [IndexBar.name]: IndexBar,
    [NavBar.name]: NavBar,
    [Picker.name]: Picker,
    [Popup.name]: Popup,
  },
  data() {
    return {
      showPicker: false,
      Symbols: [],
      UserId: "", //发起人用户主键</br>
      Coin: "", //币<br/>
      Quantity: "", //数量(0.01起转)</br>
      ReceiverEnCode: "", //目标账户EnCode</br>
      WithdrawPassword: "", //支付密码</br>
      Code: "",
      MINT_auth: {},
      showKeyboard: false,
       isGetCode:true,
        btnTips:'',
    };
  },
  mounted() {
    document.body.style.background = "#f5f5f5";
    this.getSymbolsPageList();
    this.btnTips = this.$t("forgetPwd.btnGetCode");
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
  },
  methods: {
    userTransfer() {
      let data = {
        UserId: this.MINT_auth.UserId, //发起人用户主键</br>
        Coin: this.Coin, //币<br/>
        Quantity: this.Quantity, //数量(0.01起转)</br>
        ReceiverEnCode: this.ReceiverEnCode, //目标账户EnCode</br>
        WithdrawPassword: this.WithdrawPassword, //支付密码</br>
        Code: this.Code, //验证码<br/>
      };
      if(!this.ReceiverEnCode){
          this.$toast(this.$t('transaction.ReceiverEnCodeTxt'));
          return;
      }
      if(!this.Coin){
          this.$toast(this.$t('transaction.CoinTxt'));
          return;
      }
      if(!this.Quantity){
          this.$toast(this.$t('transaction.QuantityTxt'));
          return;
      }
      if(!this.WithdrawPassword){
          this.$toast(this.$t('withdraw.pwdTxt'));
          return;
      }
      if(!this.Code){
          this.$toast(this.$t('transactionPwd.vcode'));
          return;
      }
      console.log(data);
      UserTransfer(data).then((r) => {
        if (r.data.Code == 200) {
          this.$toast(r.data.Message);
        }
      });
    },
    //region 行情 => 获取全部symbol的交易行情列表
    getSymbolsPageList() {
      let data = {
        PageIndex: 1,
        PageSize: 100,
        AreaId: "Default",
        Sidx: "SortCode",
        Sord: "desc",
        KeyWord: "Default",
        Flag: "2",
      };
      GetSymbolsPageList(data).then((r) => {
        console.log(r.data);
        if (r.data.Code == 200) {
          let arr = [];
          r.data.Data.DataList.map((_) => {
            arr.push({
              value: _.SymbolsId,
              text: _.BaseCurrency,
              QuoteCurrency: _.QuoteCurrency,
            });
          });
          this.Symbols = arr;
        }
      });
    },
    getCode() {
         let Mobile = ''
            //  let Mobile = this.VerifCodeAccount=="Email">
            if(this.MINT_auth.VerifCodeAccount){
                Mobile=  this.MINT_auth.VerifCodeAccount=="Email"?this.MINT_auth.Email:(this.MINT_auth.AreaCode+"|"+this.MINT_auth.Mobile)
                }else{
                    Mobile=  this.MINT_auth.Email?this.MINT_auth.Email:(this.MINT_auth.AreaCode+"|"+this.MINT_auth.Mobile)
            }
      let data = {
        Title: "转账",
        Mobile: Mobile//this.MINT_auth.Email?this.MINT_auth.Email:(this.MINT_auth.AreaCode+"|"+this.MINT_auth.Mobile)
      };
      this.$toast.loading({
        duration: 0, // 持续展示 toast
        forbidClick: true,
      });
      this.isGetCode = false;
      GetMobileCode(data).then((r) => {
        console.log(r);
        if (r.data.Code == 200) {
          this.$toast(this.$t("register.sendSeccess"));
          let wait = 60;
          let time = setInterval((_) => {
            wait--;
            if (wait == 0) {
              clearInterval(time);
              this.btnTips = this.$t("forgetPwd.btnGetCode");
              this.isGetCode = true;
            } else {
              this.btnTips = wait + "S";
            }
          }, 1000);
        } else {
          this.isGetCode = true;
        }
      });
    },
    showPopup() {
      this.showPicker = true;
    },
    onInput(key) {
      this.WithdrawPassword = (this.WithdrawPassword + key).slice(0, 6);
      console.log(this.WithdrawPassword);
    },
    onDelete() {
      this.WithdrawPassword = this.WithdrawPassword.slice(
        0,
        this.WithdrawPassword.length - 1
      );
    },
    onConfirm(value, index) {
      console.log(value);
      this.Coin = value.text;
      this.showPicker = false;
      //   this.$toast(`当前值：${value}, 当前索引：${index}`);
    },
    onChange(picker, value, index) {
      console.log(value);
      this.Coin = value.text;
      //   this.$toast(`当前值：${value}, 当前索引：${index}`);
    },
    onCancel() {
      //   this.$toast('取消');
    },
  },
  destroyed() {
    document.body.style.background = "#fff";
  },
};
</script>
<style lang="less" >
</style>
<style lang="less" scoped>
  .transaction_wrap {
    background-color: @background-color;
    // min-height: 100vh;
    .contnet {
      padding: 20px;
      label{
        color: @sub-font-color2;
        margin:5px 20px ;
        display: block;
      }
      ul {
        display: flex;
        // height: 80px;
        align-items: center;
        justify-content: space-between;
        position: relative;
        padding:10px 20px;
        width: 100%;
        &:not(:last-of-type) {
          // .bot_br;
        }
        li {
          height: 80px;
          display: flex;
          align-items: center;
          /* label {
            font-size: 28px;
            color: @important-font-color;
            margin-right: 20px;
          } */
          &:nth-of-type(1) {
            width: 1000%;
          }
          input {
            height: 100%;
            width: 100%;
            font-size: 26px;
            padding-left: 10px;
            background-color: @panel_color;
            color: @font_1_color;
            border-radius: 10px;
          }
          position: relative;
          img {
            position: absolute;
            right: 20px;
            height: 35px;
          }
          .vcode_bnt {
            position: absolute;
            top: 50%;
            right: 10px;
            padding: 10px;
            border: 1px solid @main_color;
            transform: translate(0, -50%);
            background: transparent;
            color: @main_color;
            width: 160px;
            border-radius: 8px;
            &:disabled {
              color: @minor-font-color;
              border: 1px solid @minor-font-color;
            }
          }
        }
      }
    }
    .btns {
      margin-top: 50px;
      padding: 20px;
    }
  }
</style>