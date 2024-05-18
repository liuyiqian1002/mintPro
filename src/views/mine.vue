<template>
  <div class="mine_wrap">
    <van-nav-bar
      :title="$t('mine.title')"
      left-text
      left-arrow
      @click-left="$router.go(-1)"
    />
    <div class="top">
      <div class="avatar">
        <div class="pic" @click="$router.push('/user_info')">
          <img :src="avatar" alt="" />
        </div>
        <div>
          <p class="tag">
            {{ UserInfo.EnCode }} <img :src="UserGradeFilePath" alt="" />
          </p>
          <p class="uid">UID: {{  UserInfo.EnCode }}</p>
          
          <!-- <p class="uid">UID: {{ base.userInfo }}</p> -->
        </div>
      </div>
      <div class="code" @click="toQrCode">
        <img src="../assets/images/common/ic_shoukuanma.png" alt="" />
        <p>{{ $t("mine.QrCode") }}</p>
      </div>
    </div>
    <div class="content">
      <div class="cell" @click="pay">
        <div class="cion">
          <img src="../assets/images/common/cjwt.png" alt="" />
        </div>
        <div>
          <span>{{ $t("mine.pay") }}</span>
          <img src="../assets/images/common/back_right.png" alt="" />
        </div>
      </div>
      <div class="cell" @click="$router.push('/recommend_friend')">
        <div class="cion">
          <img src="../assets/images/common/swhz.png" alt="" />
        </div>
        <div>
          <span>{{ $t("mine.inviteFriends") }}</span>
          <img src="../assets/images/common/back_right.png" alt="" />
        </div>
      </div>
      <div class="cell" @click="$router.push('/safety')">
        <div class="cion">
          <img src="../assets/images/common/ic_anquan.png" alt="" />
        </div>
        <div>
          <span>{{ $t("mine.securityCenter") }}</span>
          <img src="../assets/images/common/back_right.png" alt="" />
        </div>
      </div>
      <div class="cell" @click="$router.push('/set_lan')">
        <div class="cion">
          <img src="../assets/images/common/jcgx.png" alt="" />
        </div>
        <div>
          <span>{{ $t("mine.language") }}</span>
          <img src="../assets/images/common/back_right.png" alt="" />
        </div>
      </div>
      <div class="cell" @click="toSevice">
        <div class="cion">
          <img src="../assets/images/common/gywm.png" alt="" />
        </div>
        <div>
          <span>{{ $t("mine.help") }}</span>
          <img src="../assets/images/common/back_right.png" alt="" />
        </div>
      </div>
    </div>
    <div class="btns">
      <button class="btn" @click="outSign">{{ $t("mine.out") }}</button>
    </div>
  </div>
</template>

<script>
import { Icon, NavBar, IndexBar, Dialog } from "vant";
import { GetEntityUser, SignOut } from "@/api";
import { mapState , mapActions} from "vuex";
export default {
  name: "mine",
  components: {
    [Icon.name]: Icon,
    [NavBar.name]: NavBar,
    [IndexBar.name]: IndexBar,
    [Dialog.name]: Dialog,
  },
  data() {
    return {
      hasPayPwd: true, // 是否设置交易密码
      MINT_auth: {},
      UserInfo: {},
      avatar: "",
      UserGradeFilePath: "",
    };
  },
  mounted() {
    // this.avatar = require('../assets/images/common/logo.png')
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    console.log(this.MINT_auth);
    //   history.replaceState('state', 'title', '.#/transaction')
    console.log(this.$route.query);
    this.getEntityUser();
  },
  methods: {
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
          this.$store.commit('setUserInfo',this.UserInfo);
          this.UserInfo.WithdrawPassword == "" ?this.hasPayPwd = false :this.hasPayPwd= true;
          let num = UserGrade + 1;
          this.avatar = r.data.Data.HeadIcon
            ? this.$config.imgPath + r.data.Data.HeadIcon
            : require("../assets/images/common/logo.png");
          this.UserGradeFilePath = this.$config.imgPath + r.data.Data.UserGradeFilePath//require(`../assets/images/common/vip/vip${num}@2x.png`);
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
              localStorage.removeItem("userInfo");
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
    toSevice(){
      let lang = localStorage.getItem('MINT_lang')?localStorage.getItem('MINT_lang'):'zh-CN';
       let href = this.$config.imgPath+`/News/AppReplicate/c8ebc9cc-8d27-46a5-a57e-c54d1287c4d4?LanguageVersion=${lang}`
        this.$router.push({
          path:'/h5',
          query:{
            url:href,
            title:this.$t("mine.help")
          }
        })
    }
  },
   computed: {
    ...mapState(["base"]),
  },
};
</script>
<style lang="less" scoped>
  .mine_wrap {
    .top {
      color: #fff;
      height: 260px;
      background-size: 100% 100%;
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 0px 50px 20px;
      .avatar {
        display: flex;
        align-items: center;
        .pic {
          margin-right: 20px;
          width: 110px;
          height: 110px;
          border-radius: 50%;
          overflow: hidden;
          img {
            width: 100%;
            height: 100%;
          }
          & + div {
            p {
              margin-bottom: 10px;
              display: flex;
              align-items: center;

              img {
                margin-left: 20px;
                height: 30px;
              }
            }
          }
        }
      }
      .code {
        text-align: center;
        img {
          width: 50px;
        }
        p {
          margin-top: 10px;
          font-size: 20px;
        }
      }
    }
    .content {
      .cell {
        padding: 10px 20px;
        display: flex;
        align-items: center;
        .cion {
          height: 45px;
          margin-right: 10px;
          img {
            height: 100%;
          }
          & + div {
            // .t;
            width: 90%;
            .bot_br;
            padding: 15px 0;
            display: flex;
            justify-content: space-between;
            align-items: center;
            color: @sub-font-color;
            font-size: 28px;
            img {
              height: 25px;
            }
          }
        }
      }
    }
    .btns {
      text-align: center;
      margin-top: 20px;
      padding: 20px;
      .btn {
        border: 2px solid @main_color;
        height: 90px;
        background-color: transparent;
        font-size: 28px;
        width: 100%;
        border-radius: 4px;
        color: @main_color;
      }
    }
  }
</style>