<template>
<div class="safety_wrap_main">
<van-nav-bar :title="$t('safety.AccountManagement')" left-text   left-arrow @click-left="$router.push('/safety')" />
  <div class="safety_wrap">
    <div class="contnet">
       <ul @click="toBindAcc(i)" v-for="(i ,index) in navList">
           <li>
               {{i.label}}
           </li>
           <li>
            {{i.val||$t('bindAccount.bind')}} <img src="../assets/images/common/back_right.png" alt="">
           </li>
       </ul>
    </div>
  </div>

</div>
</template>

<script>
import {
  Icon,
  NavBar,
  IndexBar,
} from "vant";
import { UpdateUserEntity, GetEntityUser } from "@/api";
export default {
    name:'account',
    components:{
        [Icon.name]:Icon,
        [IndexBar.name]:IndexBar,
        [NavBar.name]:NavBar,
    },
    data(){
        return{
            navList:[
                {
                    type:'mobile',
                    label:this.$t("login.navPhone"),
                    val:''
                },
                {
                    type:"email",
                    label:this.$t("login.navEmail"),
                    val:''
                },
            ],
            MINT_auth:{}

        }
    },
    mounted(){
        document.body.style.background='#f5f5f5';
        let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
        console.log(this.MINT_auth);
        this.navList[0].val= this.MINT_auth.Mobile?('+'+this.MINT_auth.AreaCode+'  '+this.MINT_auth.Mobile):'';
        this.navList[1].val= this.MINT_auth.Email
    },
    methods:{
        toBindAcc(i){
            if(i.val){
                this.$toast(this.$t('bindAccount.tips'))
                return
            }
            this.$router.push({
                path:'/bind_account',
                query:{
                    type:i.type
                }
            })
        },
    // getEntityUser() {
    //   let data = {
    //     UserId: this.MINT_auth.UserId,
    //   };
    //   GetEntityUser(data).then((r) => {
    //     // console.log(JSON.stringify(r.data.Data) );
    //     if (r.data.Code == 200) {
    //       this.UserInfo = r.data.Data;
    //       const { UserGrade } = r.data.Data;
    //       console.log(this.UserInfo);
    //       this.$store.commit("setUserInfo", this.UserInfo);
    //       this.selIndex = this.UserInfo.VerifCodeAccount == "Mobile" ? 0 : 1;
    //     }
    //   });
    // },
    },
    destroyed(){
          document.body.style.background='#fff';
    }

}
</script>
<style lang="less" >
.safety_wrap_main{
     .van-hairline--bottom{
          &::after{
            content: '';
            border-bottom-width:0px;
            border: none !important;
          }
        }
         .van-nav-bar{
            background-color:transparent;
        }
}
</style>
<style lang="less" scoped>
                   .safety_wrap{
                       background-color: @background-color;
                       height: 100vh;
                       .contnet{
                           margin: 20px;
                           ul{
                               background-color: @panel_color;
                               border-radius: 16px;
                               padding: 20px;
                               margin-bottom: 20px;
                               display: flex;
                               // height: 80px;
                               align-items: center;
                               justify-content: space-between;
                               position: relative;
                               padding: 20px;
                               width: 100%;
                               color: @font_1_color;
                               &:not(:last-of-type){
                                   .bot_br;
                               }
                               li{
                                   display: flex;
                                   align-items: center;
                                   height: 80px;
                                   // padding: 10px 0;
                                   &:nth-of-type(1){
                                       width: 70%;
                                       font-size: 28px;
                                   }
                                   &:nth-of-type(2){
                                       display: flex;
                                       align-items: center;
                                       img{
                                           height: 25px;
                                           margin-left: 10px;
                                       }
                                   }
                                   input{
                                       height: 100%;
                                       width: 100%;
                                       font-size: 26px;
                                   }
                                   .vcode_bnt{
                                       position: absolute;
                                       top: 50%;
                                       right: 10px;
                                       border: 1px solid @main_color;
                                       transform: translate(0,-50%,);
                                       background: transparent;
                                       color: @main_color;
                                       padding:5px 10px;
                                       width: 160px;
                                       &:disabled{
                                           color:  @minor-font-color;;
                                           border: 1px solid @minor-font-color;;
                                       }
                                   }
                               }
                           }
                       }
                       .btns{
                           margin-top: 50px;
                           padding: 20px;
                       }
                   }
</style>