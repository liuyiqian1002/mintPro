<template>
  <div class="property_wrap">
    <div class="top">
      <div class="header-content">
        <header>
          <span>{{ $t("property.title") }}</span>
          <img
            @click="showMeMenu = true"
            src="../assets/images/common/me_user@2x.png"
            alt=""
          />
          <!-- <span @click="$router.push('/mine')">{{$t('property.rightTitle')}}</span> -->
        </header>
      </div>
      <div class="acc_info">
        <p>{{ Total | decimals(8) }}</p>
        <p>≈{{ CNYTotal | decimals(2) }} USD</p>
        <p>{{ $t("property.total") }}(USDT)</p>
      </div>
      <div class="search">
        <ul>
          <li>
            <div class="" @click="petty">
              <div :class="['checkbox', { checked: isPetty }]"></div>
              <p>{{ $t("loginPwd.smallCurrency") }}</p>
            </div>
          </li>
          <li>
            <div>
              <input
                v-model="KeyWord"
                @keydown.enter="search"
                @blur="getUserCommodityAccountsInfo"
                type="text"
                :placeholder="$t('property.seach')"
              />
            </div>
          </li>
        </ul>
      </div>
    </div>
    <!-- <div class="empty"></div> -->
    <div class="contnet">
      <div>
        <van-empty
          v-show="isEmpty"
          :description="$t('base.noData')"
          class="custom-image"
          :image="emptyImg"
        />
        <div class="card" v-for="i,index in DataList" @click="toRecord(i)" :key="index">
          <p>{{ i.Coin }}</p>
          <ul>
            <li>
              <p>{{ $t("property.usable") }}</p>
              <p>{{ i.Balance | decimals(8) }}</p>
            </li>
            <li>
              <p>{{ $t("property.Freeze") }}</p>
              <p>{{ i.Freeze | decimals(8) }}</p>
            </li>
            <li>
              <p>{{ $t("property.CNYBalance") }}(USD)</p>
              <p>{{ i.CNYBalance | decimals(2) }}</p>
            </li>
          </ul>
        </div>
      </div>
    </div>
    <mainFooter />
    <van-popup
      v-model="showMeMenu"
      position="left"
      :style="{ height: '100%', width: '80%' }"
    >
      <div class="me_menu">
        <div class="my_code" @click="toQrCode">
          <div>
            <div class="pic">
              <img src="../assets/images/common/me_code.png" alt="" />
            </div>
            <p>{{ $t("mine.QrCode") }}</p>
          </div>
        </div>

        <div class="avatar">
          <div class="pic" @click="$router.push('/user_info')">
            <img :src="avatar" alt="" />
          </div>
          <div>
            <p>
              {{ UserInfo.EnCode }}
              <span class="tag">V{{ UserInfo.UserGrade + 1 }}</span>
            </p>
            <p>UID: {{ UserInfo.EnCode }}</p>
          </div>
        </div>
        <div class="content">
          <div class="cell" @click="pay">
            <div class="cion">
              <img src="../assets/images/common/cjwt.png" alt="" />
            </div>
            <div>
              <span>{{ $t("mine.pay") }}</span>
              <!-- <img src="../assets/images/common/back_right.png" alt="" /> -->
            </div>
          </div>
          <div class="cell" @click="$router.push('/recommend_friend')">
            <div class="cion">
              <img src="../assets/images/common/swhz.png" alt="" />
            </div>
            <div>
              <span>{{ $t("mine.inviteFriends") }}</span>
              <!-- <img src="../assets/images/common/back_right.png" alt="" /> -->
            </div>
          </div>
          <div class="cell" @click="$router.push('/safety')">
            <div class="cion">
              <img src="../assets/images/common/ic_anquan.png" alt="" />
            </div>
            <div>
              <span>{{ $t("mine.securityCenter") }}</span>
              <!-- <img src="../assets/images/common/back_right.png" alt="" /> -->
            </div>
          </div>
          <div
            class="cell"
            @click="(showMeMenu = false), $router.push('/set_lan')"
          >
            <div class="cion">
              <img src="../assets/images/common/jcgx.png" alt="" />
            </div>
            <div>
              <span>{{ $t("mine.language") }}</span>
              <!-- <img src="../assets/images/common/back_right.png" alt="" /> -->
            </div>
          </div>
          <div class="cell" @click="toSevice">
            <div class="cion">
              <img src="../assets/images/common/gywm.png" alt="" />
            </div>
            <div>
              <span>{{ $t("mine.help") }}</span>
              <!-- <img src="../assets/images/common/back_right.png" alt="" /> -->
            </div>
          </div>

          <div class="cell" @click="outSign">
            <div class="cion">
              <img src="../assets/images/common/out_login.png" alt="" />
            </div>
            <div>
              <span>{{ $t("mine.out") }}</span>
              <!-- <img src="../assets/images/common/back_right.png" alt="" /> -->
            </div>
          </div>
        </div>
      </div>
    </van-popup>
  </div>
</template>

<script>
import { mapState } from "vuex";
import mainFooter from "@/components/mainFooter";
import { GetUserCommodityAccountsInfo, GetEntityUser, SignOut } from "@/api";
import {
  Icon,
  NavBar,
  IndexBar,
  Picker,
  Popup,
  Tab,
  Tabs,
  PasswordInput,
  Empty,
  Sticky,
  Dialog,
  List,
  CountDown,
} from "vant";
export default {
  name: "property",
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
    [List.name]: List,
    [Dialog.name]: Dialog,
    [CountDown.name]: CountDown,
    mainFooter,
  },
  data() {
    return {
      MINT_auth: {},
      CNYTotal: "",
      DataList: [],
      Total: "",
      isPetty: false, //小额
      emptyImg: "",
      isEmpty: false,
      KeyWord: "",
      showMeMenu: false,
      avatar: "",
      UserInfo: {},
      loading: false,
    };
  },
  mounted() {
    this.emptyImg = require("../assets/images/common/empty_img.png");
    console.log(this.$route.query);
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    this.getUserCommodityAccountsInfo();
    //   history.replaceState('state', 'title', '.#/transaction')
    this.getEntityUser();
  },
  methods: {
    search() {
      this.loading = true;
      this.$toast.loading({
        duration: 0, // 持续展示 toast
        forbidClick: true,
      });
      this.getUserCommodityAccountsInfo();
    },
    getUserCommodityAccountsInfo() {
      let data = { UserId: this.MINT_auth.UserId };
      GetUserCommodityAccountsInfo(data).then((r) => {
        console.log(r.data);
        const { Total, CNYTotal, DataList } = r.data.Data;
        console.log(DataList);
        let arr = [];
        let res = [];
        if (this.isPetty) {
          DataList.map((_) => {
            _.Balance != 0 ? arr.push(_) : "";
          });
        } else {
          arr = DataList;
        }
        if (this.KeyWord) {
          arr.map((_) => {
            let KeyWord = this.KeyWord.toLowerCase();
            let Coin = _.Coin.toLowerCase();

            Coin.includes(KeyWord) ? res.push(_) : "";
          });
        } else {
          res = arr;
        }
        if (arr.length == 0) {
          this.isEmpty = true;
        } else {
          this.isEmpty = false;
        }
        if (this.loading) {
          this.$toast.clear();
          this.loading = false;
        }
        this.DataList = res;
        console.log(this.DataList);
        this.CNYTotal = CNYTotal;
        this.Total = Total;
      });
    },
    petty() {
      this.isPetty = !this.isPetty;
      this.loading = true;
      this.$toast.loading({
        duration: 0, // 持续展示 toast
        forbidClick: true,
      });
      this.getUserCommodityAccountsInfo();
    },
    toRecord(i) {
      this.$store.commit("setCoinInfo", i);
      this.$router.push({
        path: "/acc_record",
        query: {
          coin: i.Coin,
        },
      });
    },
    getEntityUser() {
      let data = {
        UserId: this.MINT_auth.UserId,
      };
      GetEntityUser(data).then((r) => {
        // console.log(JSON.stringify(r.data.Data) );
        if (r.data.Code == 200) {
          this.UserInfo = r.data.Data;
          const { UserGrade } = r.data.Data;
          console.log(this.UserInfo);
          this.$store.commit("setUserInfo", this.UserInfo);
          this.UserInfo.WithdrawPassword == ""
            ? (this.hasPayPwd = false)
            : (this.hasPayPwd = true);
          let num = UserGrade + 1;
          this.avatar = r.data.Data.HeadIcon
            ? this.$config.imgPath + r.data.Data.HeadIcon
            : require("../assets/images/common/logo.png");
          this.UserGradeFilePath =
            this.$config.imgPath + r.data.Data.UserGradeFilePath; //require(`../assets/images/common/vip/vip${num}@2x.png`);
        }
      });
    },
    pay() {
      //   let hasPayPwd = true; // 是否设置交易密码
      if (!this.hasPayPwd) {
        this.$router.push("/transaction_pwd");
      } else {
        this.$router.push("/transaction");
      }
    },
    outSign() {
      // //region 用户 => 退出登录
      this.showMeMenu = false;
      let data = {
        UserId: this.UserInfo.UserId,
      };
      Dialog.confirm({
        title: this.$t("mine.outTips"),
        confirmButtonText: this.$t("base.confirm"),
        cancelButtonText: this.$t("base.cancel"),
      })
        .then(() => {
          SignOut(data).then((r) => {
            let that = this;
            if (r.data.Code == 200) {
              localStorage.removeItem("MINT_auth");
              localStorage.removeItem("isRead");
              localStorage.removeItem("userInfo");
              localStorage.removeItem("isBindPhone");
              this.$toast({
                message: that.$t("mine.seccess"),
                onClose() {
                  that.$router.replace("/");
                },
              });
            }
          });
        })
        .catch(() => {
          // on cancel
        });
    },
    toQrCode() {
      this.$router.push({
        path: "/receiving_qrcode",
        query: {
          code: this.UserInfo.EnCode,
        },
      });
    },
    toSevice() {
      this.showMeMenu = false;
      let lang = localStorage.getItem("MINT_lang")
        ? localStorage.getItem("MINT_lang")
        : "zh-CN";
      let href =
        this.$config.imgPath +
        `/News/AppReplicate/c8ebc9cc-8d27-46a5-a57e-c54d1287c4d4?LanguageVersion=${lang}`;
      this.$router.push({
        path: "/h5",
        query: {
          url: href,
          title: this.$t("mine.help"),
        },
      });
    },
  },
  computed: {
    ...mapState(["base"]),
  },
};
</script>
<style lang="less" >
.property_wrap {
  background-color: @background-color;
  color: @font_1_color;

  .custom-image .van-empty__image {
    width: 480px;
    height: 298px;
    margin-top: unset;
  }
}
</style>
<style lang="less" scoped>
.property_wrap {
  .top {
    background-color: @background-color;
    //    background-color: @main_color;
    //    color: #fff;
    position: fixed;
    top: 0;
    width: 100%;
    z-index: 10;
    .header-content {
      position: fixed;
      left: 0;
      top: 0;
      z-index: 11;
      width: 100vw;
      header {
        display: flex;
        width: 100%;
        height: 80px;
        align-items: center;
        justify-content: center;
        // background: transparent;
        span {
          color: @font_3_color;
          font-size: 30px;
          font-weight: 500;
        }
        span {
          position: absolute;
          left: 40px;
          font-size: 28px;
          font-weight: 500;
        }
        img {
          position: absolute;
          right: 40px;
          font-size: 28px;
          width: 56px;
          height: 56px;

          font-weight: 500;
        }
      }
    }
    .acc_info {
      background: url(../assets/images/common/property_top_bg@2x.png) no-repeat;
      padding: 110px 40px 50px 40px;
      height: 440px;
      // margin: 20px 20px;
      background-size: cover;
      position: relative;
      p {
        color: #fff;
        padding: 5px 0;
        font-size: 26;
        &:nth-of-type(1) {
          font-size: 60px;
          font-weight: bold;
        }
        &:nth-of-type(2) {
          font-weight: 500;
          font-size: 28px;
        }
        &:nth-of-type(3) {
          color: @font_3_color;
          position: absolute;
          bottom: 40px;
          font-weight: 500;
          font-size: 28px;
        }
      }
    }
    .search {
      margin: 20px 30px;
      border-radius: 40px;
      background-color: #fff;
      // margin-bottom: 30px;
      // padding: 20px;
      box-shadow: 0px -2px 2px 2px #eee,
        /*上边阴影  红色*/ -2px 0px 2px 2px #eee,
        /*左边阴影  绿色*/ 2px 0px 2px 2px #eee,
        /*右边阴影  蓝色*/ 0px 2px 2px 2px #eee;
      ul {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 0 20px;
        li {
          > div {
            height: 90px;
            display: flex;
            align-items: center;
            color: #909399;
            input {
              width: 164px;
              height: 44px;
              border: 1px solid @main_color; // #e5e5e5
              border-radius: 10px;
              padding-left: 40px;
              background: url(../assets/images/common/properth_search.png)
                no-repeat 5px center;
              background-size: 30px 30px;
            }
            .checkbox {
              margin-right: 10px;
              width: 30px;
              height: 30px;
              background: url(../assets/images/common/uncheck.png) no-repeat;
              background-size: cover;
              &.checked {
                background: url(../assets/images/common/checked.png) no-repeat;
                background-size: cover;
              }
            }
          }
        }
      }
    }
  }
  .empty {
    background-color: @main_color;
    width: 100%;
    height: 80px;
    position: fixed;
    top: 230px;
  }
  .contnet {
    background-color: @background-color;
    > div {
      padding-bottom: 520px;
      margin: 30px;
    }
    position: fixed;
    overflow: auto;
    width: 100%;
    height: 100%;
    top: 560px;
    z-index: 1;
    padding-bottom: 120px;
    .card {
      border-radius: 10px;
      background-color: @panel_color;
      margin-bottom: 30px;
      padding: 20px;
      p {
        font-weight: 500;
        margin-bottom: 10px;
        font-size: 32px;
      }
      ul {
        display: flex;
        li {
          width: 33.33%;
          p {
            font-size: 26px;
            &:nth-of-type(1) {
              color: @minor-font-color;
              white-space: nowrap;
            }
          }
          &:nth-of-type(1) {
            p{
              &:nth-of-type(2) {
                color: #000;
              }
            }
          }
          &:nth-of-type(2) {
            text-align: center;
            justify-content: center;
            color: #000;
          }
          &:nth-of-type(3) {
            text-align: right;
            justify-content: center;
            color:@btn_main_color !important;
          }
        }
      }
    }
  }
  .me_menu {
    padding: 20px;
    .my_code {
      display: flex;
      justify-content: flex-end;
      align-items: center;
      text-align: center;
      > div {
        // width: 100px;
        text-align: center;
        .pic {
          width: 60px;
          height: 60px;
          text-align: center;
          margin: 0 auto;
          img {
            margin: 0 auto;
            display: block;
            width: 60px;
            height: 60px;
          }
        }
      }
    }
    .avatar {
      display: flex;
      align-items: center;
      .pic {
        width: 120px;
        height: 120px;
        border-radius: 50%;
        overflow: hidden;
        img {
          width: 100%;
          height: 100%;
          transform: scale(.8);
        }
        margin-right: 10px;
        & + div {
          p {
            &:nth-of-type(1) {
              /*color: #272E3F;*/
              font-weight: 600;
              .tag {
                width: 60px;
                height: 30px;
                font-size: 20px;
                display: inline-block;
                background: url(../assets/images/common/me_tag_bg.png) no-repeat;
                background-size: cover;
                text-align: center;
                color: #916517;
                line-height: 30px;
              }
            }
          }
        }
      }
    }
    .content {
      margin-top: 30px;
      .cell {
        display: flex;
        align-items: center;
        height: 120px;
        font-size: 34px;
        font-weight: 500;
        img {
          width: 35px;
          height: 35px;
          margin-right: 20px;
        }
      }
    }
  }
}
</style>