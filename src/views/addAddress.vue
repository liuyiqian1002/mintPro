<template>
  <div class="add_address_wrap">
    <van-nav-bar
      :title="$t('addAddress.title')"
      left-text
      left-arrow
      @click-left="$router.go(-1)"
    />
    <div class="contnet">
      <p>{{$t('addAddress.address')}}:</p>
      <ul>
        <li>
           <textarea type="text"  v-model="CoinAddress" :placeholder="$t('addAddress.addressTxt')" />
        </li>
        
      </ul>
      <p>{{$t('addAddress.Remark')}}:</p>
      <ul>
       <li>
           <input type="text" v-model="Remark" :placeholder="$t('addAddress.RemarkTxt')" />
        </li>
        
      </ul>
      <div class="btns">
        <button class="main_btn" @click="add">{{$t('base.confirm')}}</button>
      </div>
    </div>
  </div>
</template>

<script>
import { Icon, NavBar, IndexBar, Picker, Popup, PasswordInput } from "vant";
import {AddUserCoinAddress} from "@/api"
export default {
  name: "Address",
  components: {
    [Icon.name]: Icon,
    [IndexBar.name]: IndexBar,
    [NavBar.name]: NavBar,
    [Picker.name]: Picker,
    [Popup.name]: Popup
  },
  data(){
    return {
        Remark: "",
        CoinAddress:"",
        MINT_auth:{}
    }
  },
  mounted(){
        let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
        this.emptyImg = require('../assets/images/common/empty_img.png');
  },
  methods:{
    // AddUserCoinAddress()
    add(){
      let data = {
        UserId:this.MINT_auth.UserId,//用户编号</br>
        Remark: this.Remark,//备注</br>
        CoinAddress:this.CoinAddress,//钱包地址</br>
      };
      if(!this.CoinAddress){
        
        this.$toast(this.$t('addAddress.addressTxt'));
        return;
      }
      if(!this.Remark){
     this.$toast(this.$t('addAddress.RemarkTxt'));
        return;
      }
      this.$toast.loading({
         duration: 0, // 持续展示 toast
          forbidClick: true,
      })
      AddUserCoinAddress(data).then(r=>{
        let that = this;
        if(r.data.Code == 200 ){
          this.$toast({
            message:r.data.Message,
            onClose(){
              that.$router.go(-1)
            }
          })
        }
      })
    }
  }
};
</script>

<style lang="less" >
.add_address_wrap{
  .van-hairline--bottom{
               &::after{
                 content: '';
                 border-bottom-width:0px;
                 border: none !important;
               }
             }

}

</style>
<style lang="less" scoped>
                   .add_address_wrap {
                     .contnet {
                       padding: 20px;
                       background-color: #fff;
                       p{
                         color: #999999;
                         margin: 20px 0;
                       }
                       ul {
                         background-color: #F5F6F7;
                         display: flex;
                         // height: 80px;
                         border-radius: 16px;
                         align-items: center;
                         justify-content: space-between;
                         position: relative;
                         padding:0 30px;
                         width: 100%;
                         margin-bottom: 30px;
                         &:not(:last-of-type) {
                           .bot_br;
                         }
                         li {
                           height: 80px;
                           display: flex;
                           align-items: center;
                           label {
                             font-size: 28px;
                             color: @important-font-color;
                             margin-right: 20px;
                           }
                           &:nth-of-type(1) {
                             width: 100%;
                           }
                           textarea{
                             width: 100%;
                             background: transparent;
                             resize: none;
                           }
                           input {
                             height: 100%;
                             width: 100%;
                             font-size: 26px;
                             background-color:transparent;
                           }
                           img {
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
                     .tips {
                       padding: 20px;
                       height: 50px;
                       line-height: 50px;
                       font-size: 28px;
                       color: @sub-font-color;
                     }
                     .btns {
                       margin-top: 50px;
                       padding: 20px;
                     }
                   }
</style>