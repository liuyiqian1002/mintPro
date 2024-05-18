<template>
  <div class="setLan_wrap">
    <van-nav-bar
      :title="$t('safety.codeAuth')"
      left-text
      left-arrow
      @click-left="$router.go(-1)"
    />
    <div class="cont">
      <ul>
        <li
          v-for="(i, j) in dataList"
          :class="{ active: selIndex == j }"
          @click="selIndex = j"
        >
          <p>{{ i.label }}</p>
          <p>{{ i.subLabel }}</p>
        </li>
      </ul>
      <div class="btns">
        <button @click="SaveAuth" class="main_btn">
          {{ $t("setLan.btnSave") }}
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { Icon, NavBar, IndexBar } from "vant";
import { UpdateUserEntity, GetEntityUser } from "@/api";
export default {
  name: "MINT_auth",
  components: {
    [Icon.name]: Icon,
    [IndexBar.name]: IndexBar,
    [NavBar.name]: NavBar,
  },
  data() {
    return {
      selIndex: -1,
      dataList: [
        {
          label:  this.$t('login.navPhone'),
          subLabel: this.$t('authorization.phoneTips'),
          val: "Mobile",
        },
        {
          label: this.$t('login.navEmail'),
          subLabel:this.$t('authorization.emailTips'),
          val: "Email",
        },
      ],
      MINT_auth: { VerifCodeAccount: "" },
      defaultIndex:''
    };
  },
  mounted() {
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    console.log(this.MINT_auth);
    this.selIndex = this.MINT_auth.VerifCodeAccount == "Mobile" ? 0 : 1;
    this.defaultIndex = this.MINT_auth.VerifCodeAccount == "Mobile" ? 0 : 1;
    
    document.body.style.background = "#f5f5f5";
  },
  methods: {
    SaveAuth() {
        if(this.defaultIndex == this.selIndex){
            return
        }
      let data = {
        UserId: this.MINT_auth.UserId, //用户编号</br>?
        Action: "ChangeVerifCodeAccount", //修改信息:<br/>
        ActionValue: this.dataList[this.selIndex].val,
      };
      console.log(data);
      UpdateUserEntity(data).then((r) => {
        console.log(r);
        if (r.data.Code == 200) {
            this.getEntityUser()
          this.$toast({
            message: r.data.Message,
          });
          // console.log(r.Data)
        }
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
          this.selIndex = this.UserInfo.VerifCodeAccount == "Mobile" ? 0 : 1;
        }
      });
    },
  },
  destroyed() {
    document.body.style.background = "#fff";
  },
};
</script>
<style lang="less"  >
.setLan_wrap {
  .van-nav-bar {
    background: transparent;
  }
}
</style>
<style lang="less" scoped>
                   .setLan_wrap {
                     background-color: @background-color;
                     height: 100vh;
                     .cont {
                       margin-top: 30px;
                       padding: 0 30px;
                       li {
                         margin-bottom: 20px;
                         border-radius: 24px;
                         .bot_br;
                         padding: 10px 40px;
                         // line-height: 80px;
                         background: url(../assets/images/common/ic_langue_unselected.png)
                         no-repeat;
                         background-color: @panel_color;
                         background-position: 95% center;
                         background-size: 50px 50px;
                         p {
                           padding: 10px 0;
                           margin: 0;
                           &:nth-of-type(1) {
                             font-size: 28px;
                             color: @font_1_color;
                             font-weight: 400;
                           }
                           &:nth-of-type(2) {
                             font-size: 24px;
                             color: @font_f_color;
                             font-weight: 400;
                           }
                         }
                         &.active {
                           background: url(../assets/images/common/ic_langue_selected.png)
                           no-repeat;
                           background-color: @panel_color;
                           background-position: 95% center;
                           background-size: 50px 50px;
                         }
                       }
                     }
                     .btns {
                       width: 100%;
                       margin: 50px auto;
                     }
                     .bot {
                       width: 80%;
                       margin: 80px auto;
                       .main_btn {
                         height: 90px;
                         font-size: 28px;
                       }
                       .nav {
                         margin: 20px 0;
                         justify-content: space-between;
                         display: flex;
                       }
                     }
                   }
</style>