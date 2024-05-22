<template>
  <div class="deal_wrap">
    <van-nav-bar :placeholder="true" fixed>
      <template #right>
        <van-icon
          :name="require('../assets/images/common/ic_kline.png')"
          size="18"
          @click="toKline"
        />
      </template>
      <template #left>
        <div @click="test">
          <span style="color: #fff; margin-right: 5px">{{ title }}</span>
          <van-icon
            :name="require('../assets/images/trade/组 6139.png')"
            size="18"
          />
        </div>
      </template>
    </van-nav-bar>
    <div class="more_symbols" v-show="showMore" @click.stop="showMore = false">
      <div class="more-con">
        <ul
          v-for="(i, index) in search"
          :class="{ active: i.SymbolsId == SymbolsId }"
          @click="selSymbol(i)"
          :key="index"
        >
          <li>{{ i.BaseCurrency }}/USDT</li>
        </ul>
      </div>
    </div>
    <div class="content">
      <div class="con_left">
        <div class="title">
          <div
            @click="tradType = 1"
            :class="['bay', { active: tradType == 1 }]"
          >
            {{ $t("base.bay") }}
          </div>
          <!-- <div
            :class="[
              'reverse',
              { 'reverse-bay': tradType == 1 },
              { 'reverse-sell': tradType == 2 }
            ]"
          ></div> -->
          <div
            @click="tradType = 2"
            :class="['sell', { active: tradType == 2 }]"
          >
            {{ $t("base.sell") }}
          </div>
        </div>
        <div :class="[{ sell: tradType == 1 }, { bay: tradType == 2 }, 'txt']">
          <div class="minus" @click="crtPrice('minus')"></div>
          <div class="in">
            <input type="text" v-model="Price" />
          </div>
          <div class="add" @click="crtPrice('add')"></div>
        </div>
        <div :class="[{ sell: tradType == 1 }, { bay: tradType == 2 }, 'txt']">
          <div class="minus" @click="crtQuantity('min')"></div>
          <div class="in">
            <input
              type="text"
              v-model="Quantity"
              @blur="changeQuantity"
              :placeholder="$t('base.quantity')"
            />
          </div>
          <div class="add" @click="crtQuantity('add')"></div>
        </div>
        <div class="bot">
          <p class="tips" v-show="tradType == 1">
            <span>{{ $t("deal.usable") }}:</span>
            <span> {{ CombinationSymbols.QuoteCurrencyBalance }} USDT</span>
          </p>
          <p class="tips" v-show="tradType == 2">
            <span>{{ $t("deal.usable") }}:</span
            ><span
              >{{ CombinationSymbols.BaseCurrencyBalance }}
              {{ this.BaseCurrency }}</span
            >
          </p>
          <ul :class="[{ sell: tradType == 1 }, { bay: tradType == 2 }]">
            <li
              @click="clickQuantity(index)"
              :class="{ active: index == QuantityIndex }"
              v-for="(i, index) in quantityLinst"
              :key="index"
            >
              {{ i.label }}
            </li>
          </ul>
          <div>
            <button
              :class="[
                'main_btn',
                { sell: tradType == 1 },
                { bay: tradType == 2 },
              ]"
              @click="showPwd"
            >
              {{
                (tradType == 1 ? $t("base.bay") : $t("base.sell")) +
                "   " +
                BaseCurrency
              }}
            </button>
          </div>
        </div>
      </div>
      <div class="con_right">
        <ul class="title">
          <li>{{ $t("base.strikePrice") }}</li>
          <li>{{ $t("property.quantity") }}</li>
        </ul>
        <ul v-for="i,index in tradJson.bids" :key="index">
          <li class="sell">{{ i[0] | decimals(4) }}</li>
          <li>{{ i[1] }}K</li>
        </ul>
        <p class="bay">{{ SymbolsMarket.ClosePrice | decimals(4) }}</p>
        <p class="sub-color">
          ≈{{ SymbolsMarket.ClosePriceCNY | decimals(2) }} USD
        </p>
        <ul v-for="i,index in tradJson.asks" :key="index">
          <li class="bay">{{ i[0] | decimals(4) }}</li>
          <li>{{ i[1] }}K</li>
        </ul>
      </div>
    </div>
    <div class="record">
      <div class="title">
        <span><img src="../assets/images/trade/组 6058.png" alt="" /> {{ $t("deal.entrust") }}</span>
        <span @click="toHistory"
          ><img src="../assets/images/trade/路径 19108.png" alt="" />
          {{ $t("deal.history") }}</span
        >
      </div>
      <div class="con">
        <van-empty
          v-show="isEmpty"
          :description="$t('base.noData')"
          class="custom-image"
          :image="emptyImg"
        />

        <div class="card" v-for="i,index in OrdersPageList" :key="index">
          <div>
            <p
              :class="[
                'tag',
                { bay: i.category == 1 },
                { sell: i.category == 2 },
              ]"
            >
              {{ i.category == 1 ? $t("base.bay") : $t("base.sell") }}
            </p>
            <p class="time">
              {{ $t("deal.createdate") }} :
              {{ $moment(i.createdate).format("YYYY-MM-DD HH:mm:ss") }}
            </p>
            <p>
              <span
                >{{ $t("deal.category") }} : {{ i.basecurrency }}/{{
                  i.quotecurrency
                }}</span
              >
            </p>
            <p>
              <span>{{ $t("deal.price") }} : {{ i.price }}</span>
            </p>
            <p>{{ $t("base.quantity") }} : {{ i.quantity }}</p>
          </div>
          <div>
            <button class="main_btn" @click="cancelOrder(i.delegateordersid)">
              {{ $t("deal.cancel") }}
            </button>
          </div>
        </div>
        <!-- <div class="card">
            <div>
              <p class="tag sell">买入</p>
              <p>创建时间:2019-07-18 11:19</p>
              <p> <span>类别:BOS/USDT</span> <span>价格:0.5</span> </p>
              <p>数量:6.34545</p>
            </div>
             <div>
                <button class="main_btn">撤销订单</button>
            </div>
        </div> -->
      </div>
    </div>
    <mainFooter />
    <van-dialog
      v-model="show"
      :title="$t('login.password')"
      @confirm="submitDelegateOrders"
      show-cancel-button
    >
      <!-- <p class="pwd_tips" v-show="!Password">{{$t('login.password')}}</p> -->
      <ul class="pwd_num">
        <li v-for="i,index in Password" :key="index"></li>
        <li class="unent" v-for="i,index in 6 - Password.length" :key="index.toString()"></li>
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
} from "vant";
import Vue from "vue";
Vue.use(PasswordInput);
Vue.use(NumberKeyboard);
import mainFooter from "@/components/mainFooter";
import {
  SubmitDelegateOrders,
  GetSymbolsMarketDepth,
  GetCombinationSymbolsEntity,
  GetDelegateOrdersPageList,
  CancelDelegateOrders,
} from "@/api";
import contractVue from "./contract.vue";
export default {
  name: "deal",
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
    mainFooter,
  },
  data() {
    return {
      tradType: 1, //委托方向(1-买入、2-卖出)
      SymbolsId: "",
      Quantity: "", //数量</br>
      Price: "", //价格<br/>
      Password: "", //支付密码<br/>
      showKeyboard: false,
      show: false,
      Password: "",
      Quantity: "",
      tradJson: [],
      SymbolsMarket: "",
      CombinationSymbols: {},
      QuantityIndex: -1,
      quantityLinst: [],
      emptyImg: "",
      isEmpty: false,
      OrdersPageList: [],
      title: "",
      showMore: false,
      search: [],
      time: "",
      BaseCurrency: "",
    };
  },
  mounted() {
    this.quantityLinst = [
      { label: this.$t("deal.allWarehouse"), val: 1 },
      { label: "1/2" + this.$t("deal.warehouse"), val: 1 / 2 },
      { label: "1/3" + this.$t("deal.warehouse"), val: 1 / 3 },
      { label: "1/4" + this.$t("deal.warehouse"), val: 1 / 4 },
    ];
    this.tradType = this.$route.query.type ? this.$route.query.type : 1;
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    let SymbolsIdTiem = localStorage.getItem("SymbolsIdTiem")
      ? JSON.parse(localStorage.getItem("SymbolsIdTiem"))
      : "";
    let search = localStorage.getItem("search")
      ? localStorage.getItem("search")
      : "";
    search = JSON.parse(atob(search));
    this.search = search;
    console.log(search);
    this.SymbolsId = this.$route.query.SymbolsId
      ? this.$route.query.SymbolsId
      : SymbolsIdTiem.SymbolsId;
    search.map((_) => {
      if (_.SymbolsId == SymbolsIdTiem.SymbolsId) {
        this.BaseCurrency = _.BaseCurrency;
        this.title = _.BaseCurrency + "/" + _.QuoteCurrency;
      }
    });
    this.getSymbolsMarketDepth();
    this.getCombinationSymbolsEntity();
    this.getDelegateOrdersPageList();
    this.emptyImg = require("../assets/images/common/empty_img.png");
    this.time = setInterval((_) => {
      this.getSymbolsMarketDepth();
      this.getCombinationSymbolsEntity();
      this.getDelegateOrdersPageList();
    }, 6000);
  },
  methods: {
    test() {
      // alert(11);
      this.showMore = true;
    },
    // 获取账户针对Symbol的详情信息
    getCombinationSymbolsEntity() {
      let data = {
        UserId: this.MINT_auth.UserId, //用户编号</br>
        SymbolsId: this.SymbolsId, //交易对主键编号<br/>
      };
      GetCombinationSymbolsEntity(data).then((r) => {
        if (r.data.Code == 200) {
          console.log("=============");
          console.log(JSON.stringify(r.data));
          this.CombinationSymbols = r.data.Data;
        }
      });
    },
    getSymbolsMarketDepth() {
      let data = {
        SymbolsId: this.SymbolsId,
      };
      GetSymbolsMarketDepth(data).then((r) => {
        if (r.data.Code == 200) {
          this.SymbolsMarket = r.data.Data;
          this.tradJson = JSON.parse(r.data.Data.JsonString);
          this.Price = this.Price ? this.Price : r.data.Data.ClosePrice;
        }
        console.log(r.data);
      });
    },
    crtQuantity(type) {
      console.log(type);
      let num = 0;
      if (this.QuantityIndex != -1) {
        this.QuantityIndex = -1;
        num = 0; //this.Quantity ?0+ this.Quantity:0;
      } else {
        num = this.Quantity ? 0 + this.Quantity : 0;
      }
      if (type == "add") {
        num++;
      }
      if (type == "min") {
        num > 1 ? num-- : (num = 1);
      }
      // if(num>=1 ){
      //   type == 'add' ? num++ :num--
      // }else{
      //   type == 'add' ?num++:""
      // }
      this.Quantity = num;
    },
    crtPrice(type) {
      console.log(type);
      let num = this.Price;

      if (type == "add") {
        num++;
      }
      if (type == "minus") {
        num > 1 ? num-- : "";
      }
      console.log(num);
      this.Price = Number(num).toFixed(4);
      if (this.QuantityIndex != -1) {
        if (this.tradType == 1) {
          let num = this.quantityLinst[this.QuantityIndex].val;
          let q =
            (this.CombinationSymbols.QuoteCurrencyBalance * num) / this.Price;
          q = Math.floor(q * 10000);
          this.Quantity = q / 10000;
          // this.Quantity = ((this.CombinationSymbols.QuoteCurrencyBalance*num)/this.Price).toFixed(4);
        }
        if (this.tradType == 2) {
          let num = this.quantityLinst[this.QuantityIndex].val;
          // this.Quantity = (this.CombinationSymbols.BaseCurrencyBalance*num).toFixed(4);
          let n = this.CombinationSymbols.BaseCurrencyBalance * num;
          this.Quantity = Math.floor(n * 10000) / 10000;
        }
      }
    },
    changeQuantity() {
      this.QuantityIndex = -1;
      this.Quantity < 1 ? (this.Quantity = 1) : "";
    },
    submitDelegateOrders(action, done) {
      if (!this.Password) {
        // this.$toast('密码不格式不正确');
        return;
      }
      let data = {
        UserId: this.MINT_auth.UserId, //用户ID</br>
        Category: this.tradType, //委托方向(1-买入、2-卖出)<br/>
        SymbolsId: this.SymbolsId, //交易对主键编号<br/>
        Quantity: this.Quantity, //数量</br>
        Price: this.Price, //价格<br/>
        Password: this.Password, //支付密码<br/>
      };
      SubmitDelegateOrders(data).then((r) => {
        if (r.data.Code == 200) {
          console.log(r.data);
          this.$toast(r.data.Message);
          this.getDelegateOrdersPageList();
        }
      });
    },
    showPwd() {
      if (this.Quantity <= 0) {
        this.$toast(this.$t("deal.quantityErr"));
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
    clickQuantity(index) {
      this.QuantityIndex = index;
      if (this.tradType == 1) {
        let num = this.quantityLinst[index].val;
        let q =
          (this.CombinationSymbols.QuoteCurrencyBalance * num) / this.Price;
        q = Math.floor(q * 10000);
        this.Quantity = q / 10000;
        // this.Quantity = ((this.CombinationSymbols.QuoteCurrencyBalance*num)/this.Price).toFixed(4);
      }
      if (this.tradType == 2) {
        let num = this.quantityLinst[index].val;
        let n = this.CombinationSymbols.BaseCurrencyBalance * num;
        this.Quantity = Math.floor(n * 10000) / 10000;
      }
    },
    // 撮合交易=> 获取用户当前委托列表
    getDelegateOrdersPageList() {
      let data = {
        UserId: this.MINT_auth.UserId, //用户主键<br/>
        PageSize: 10, //每页数</br>
        PageIndex: 1, //当前页</br>
      };
      GetDelegateOrdersPageList(data).then((r) => {
        console.log(JSON.stringify(r.data));
        if (r.data.Code == 200) {
          this.OrdersPageList = r.data.Data; //[{"delegateordersid":"3d1d45c3503f4a1f83ab292cdaa71e24","category":1,"createdate":"2020-08-16 01:50:44.263","basecurrency":"YUANBI","quotecurrency":"USDT","price":1.422,"quantity":1}]
          // console.log(JSON.stringify(this.OrdersPageList))
          this.isEmpty = false;
        } else {
          this.isEmpty = true;
        }
      });
    },
    cancelOrder(id) {
      let data = {
        DelegateOrdersId: id,
        UserId: this.MINT_auth.UserId,
      };
      console.log(data);
      CancelDelegateOrders(data).then((r) => {
        if (r.data.Code == 200) {
          this.$toast(r.data.Message);
          this.getDelegateOrdersPageList();
        }
      });
    },
    toKline() {
      localStorage.setItem("path", "deal");
      this.$router.push({
        path: "/trad",
        query: {
          SymbolsId: this.SymbolsId,
        },
      });
    },
    toHistory() {
      this.$router.push({
        path: "/history",
        query: {
          SymbolsId: this.SymbolsId,
        },
      });
    },
    selSymbol(i) {
      this.Price = "";
      this.QuantityIndex = -1;
      this.Quantity = "";
      localStorage.setItem("SymbolsIdTiem", JSON.stringify(i));
      this.SymbolsId = i.SymbolsId;
      this.BaseCurrency = i.BaseCurrency;
      this.title = i.BaseCurrency + "/" + i.QuoteCurrency;
      (this.showMore = false), this.getSymbolsMarketDepth();
      this.getCombinationSymbolsEntity();
      this.getDelegateOrdersPageList();
    },
  },
  watch: {
    tradType() {
      this.QuantityIndex = -1;
      this.Quantity = "";
    },
  },
  destroyed() {
    console.log("定时器关");
    clearInterval(this.time);
  },
};
</script>
<style lang="less" >
.deal_wrap {
  background-color: @bg_color;
  height: 100vh;
  .custom-image .van-empty__image {
    width: 480px;
    height: 298px;

    margin-top: unset;
  }

  .van-empty__description {
    color: @font_1_color;
    font-size: 30px;
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
    &.unent {
      margin: 0 10px;
      border: none;
      background-color: transparent;
      position: relative;
      &:before {
        position: absolute;
        content: "";
        display: block;
        width: 30px;
        height: 2px;
        background-color: #000;
        top: 50%;
        left: 0;
        transform: translate(0, -50%);
      }
    }
  }
}
.deal_wrap {
  background-color: @background-color;
  .more_symbols {
    position: absolute;
    top: 88px;
    bottom: 0;
    z-index: 1000000;
    width: 100%;
    .more-con {
      width: 100%;
      height: 560px;
      overflow: scroll;
      background-color: @panel_color;
      background: @background-linear; // linear-gradient( 145deg, #03C1A7 0%, #30AED4 100%);
      border-radius: 0px 0px 19px 19px;
      position: absolute;
      // right: 0;
      // left: 20px;
      margin: 0 auto;
      z-index: 1000;
      // border-radius: 10px;
      ul {
        width: 100%;
        .bot_br;
        &.active {
          li {
            color: @font_1_color;
          }
        }
        li {
          color: #555;
          text-align: center;
          height: 90px;
          line-height: 90px;
          display: flex;
          align-items: center;
          justify-content: center;
          font-size: 28px;
          padding: 10px;
          img {
            height: 30px;
            margin-right: 20px;
          }
        }
      }
    }
  }
  .content {
    display: flex;
    padding: 20px;
    > div {
      // .t;

      input {
        background-color: @bg_color;
        color: @sub-font-color;
      }

      &.con_left {
        width: 65%;
        .title {
          display: flex;
          justify-content: space-between;
          height: 80px;
          > div {
            &.bay {
              border-top-left-radius: 20px;
              border-bottom-left-radius: 20px;
              width: 50%;
              line-height: 80px;
              // border: 1px solid #ccc;
              text-align: center;
              // background: url(../assets/images/common/ic_buy_false.png)
              // no-repeat center;
              background-color: @bg_btn_clor;
              color: @sub-font-color;
              &.active {
                // background: url(../assets/images/common/ic_buy_ture.png)
                // no-repeat center;
                background-color: @up-color;
                background-size: contain;
                color: #fff;
              }
            }
            // &.reverse {
            //   .t;
            //   width: 20%;
            //   &.reverse-sell {
            //     background: url(../assets/images/common/ic_exchange_red.png)
            //       no-repeat center;
            //     background-size: 50%;
            //   }
            //   &.reverse-bay {
            //     background: url(../assets/images/common/ic_exchange_green.png)
            //       no-repeat center;
            //     background-size: 50%;
            //   }
            // }
            &.sell {
              width: 50%;
              text-align: center;
              line-height: 80px;
              background-color: @bg_btn_clor;
              // border: 1px solid #ccc;
              border-top-right-radius: 20px;
              border-bottom-right-radius: 20px;
              // background: url(../assets/images/common/ic_sell_false.png)
              // no-repeat center;
              background-size: contain;
              color: @sub-font-color;
              &.active {
                background-color: @down-color;
                // background: url(../assets/images/common/ic_sell_ture.png)
                // no-repeat center;
                background-size: contain;
                color: #fff;
              }
            }
          }
        }
        .txt {
          margin: 20px 0;
          height: 80px;
          width: 100%;
          border-radius: 4px;
          display: flex;
          justify-content: space-between;
          .in {
            input {
              height: 100%;
              text-align: center;
            }
          }
          .add,
          .minus {
            width: 80px;
            height: 80px;
          }
          &.sell {
            border: 1px solid @up-color;
            .minus {
              // border-right: 1px solid @up-color;
              background: url(../assets/images/common/minus_green.png) no-repeat
                center;
              background-size: 100% 100%;
            }
            .add {
              // border-left: 1px solid @up-color;
              background: url(../assets/images/common/plus_green.png) no-repeat
                center;
              background-size: 100% 100%;
            }
          }
          &.bay {
            border: 1px solid @down-color;
            .minus {
              // border-right: 1px solid @down-color;
              background: url(../assets/images/common/minus_red.png) no-repeat
                center;
              background-size: 100% 100%;
            }
            .add {
              // border-left: 1px solid @down-color;
              background: url(../assets/images/common/plus_red.png) no-repeat
                center;
              background-size: 100% 100%;
            }
          }
        }
        .bot {
          .tips {
            color: @sub-font-color;
            display: flex;
            justify-content: space-between;
            padding: 10px 0;
          }
          ul {
            display: flex;
            justify-content: space-between;
            li {
              width: 19.5%;
              margin: 10px 0;
              color: @font_2_color;
              font-weight: bold;
              border-radius: 4px;
              text-align: center;
              padding: 10px 0;
            }
            &.sell {
              li {
                // border: 1px solid @up-color;
                background-color: #f5f6fa;
                &.active {
                  color: #fff;
                  background-color: @up-color;
                }
              }
            }
            &.bay {
              li {
                // border: 1px solid @down-color;
                background-color: #f5f6fa;
                &.active {
                  color: #fff;
                  background-color: @down-color;
                }
              }
            }
          }
          .main_btn {
            height: 80px;
            line-height: 80px;
            &.sell {
              background-color: @up-color;
            }
            &.bay {
              background-color: @down-color;
            }
          }
        }
      }
      &.con_right {
        width: 35%;
        padding: 10px 0 10px 25px;
        p.bay {
          color: @up-color;
          font-size: 26px;
          font-weight: 500;
          text-align: center;
        }
        p.sell {
          color: @down-color;
          text-align: center;
        }
        p.sub-color {
          color: @sub-font-color;
          text-align: center;
        }
        ul {
          display: flex;
          justify-content: space-between;
          margin: 7px;
          &.title {
            color: @sub-font-color;
          }
          .bay {
            color: @up-color;
          }
          .sell {
            color: @down-color;
          }
          li {
            &:nth-of-type(2) {
              color: @sub-font-color;
            }
          }
        }
      }
    }
  }
  .record {
    margin-top: 10px;
    border-top: 10px solid @panel_color;
    .title {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 20px;
      color: @font_2_color;
      .bot_br;
      span {
        font-size: 28px;
        &:nth-of-type(2) {
          color: @font_2_color;
        }
        img {
          width: 28px;
        }
      }
    }
    .con {
      padding: 20px;
      padding-bottom: 180px;
      .card {
        padding: 20px;
        // display: flex;
        margin: 20px 0;
        // justify-content: space-between;
        position: relative;
        .bot_br;
        // box-shadow: 0px -2px 2px 2px #eee,   /*上边阴影  红色*/
        // -2px 0px 2px 2px #eee,   /*左边阴影  绿色*/
        // 2px 0px 2px 2px #eee,    /*右边阴影  蓝色*/
        // 0px 2px 2px 2px #eee;    /*下边阴影  黄色*/
        .bay {
          color: @up-color;
        }
        .sell {
          color: @down-color;
        }
        .tag {
          font-size: 30px;
          font-weight: 500;
        }
        p {
          padding: 10px 0;
          color: @sub-font-color;
          font-size: 28px;
          display: flex;
          justify-content: space-between;
          span {
            color: @sub-font-color;
            font-size: 28px;
          }
        }

        > div {
          &:nth-of-type(1) {
            width: 100%;
          }
          &:nth-of-type(2) {
            width: 30%;
            position: absolute;
            right: 20px;
            top: 20px;
            text-align: right;
            .main_btn {
              height: 60px;
              line-height: 60px;
              width: 200px;
              background-color: @ornament-color-4;
            }
          }
        }
      }
    }
  }
}
</style>