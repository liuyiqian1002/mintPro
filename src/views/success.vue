<template>
  <div class="success_wrap">
          <van-nav-bar
          v-show="!token"
      title=""
      left-text
      left-arrow
      :placeholder="true" 
      fixed
      @click-left="$router.go(-1)"
    />
      <div>
          <img src="../assets/images/common/success.png" alt="">
      </div>
      <p>{{txt}}</p>
      <div class="btns" v-show="flag">
          <button class="base_btn" @click="toOrder">{{$t('base.checkOrder')}}</button>
      </div>
      <div class="btns" v-show="type == 'bind'">
          <button class="base_btn" @click="back">{{$t('bindAccount.back')}}</button>
      </div>
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
  Empty,
  List,
  Sticky
} from "vant";
// import {GetInformationPageList} from "@/api"
   
export default {
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
     [List.name]:List,
  },
  data(){
    return{
      txt:"",
      flag:'',
      token:"",
      type:'',
      redirect:""
    }
  },
  mounted(){
    this.token=this.$route.query.token;
      // this.txt = this.$route.query.title
      this.flag = this.$route.query.flag
      this.type = this.$route.query.type
      this.redirect = this.$route.query.redirect
      if(this.flag){
       this.txt = this.$t('productDetails.successTips')
      }else{
       this.txt = this.$t('financial.success')
      }
      if(this.type == 'bind'){
       this.txt = this.$t('bindAccount.bindTips')
      }
  },
  methods:{
    back(){
      let path = this.redirect
      this.$router.push({
        path:path,
      })
    },
    toOrder(){
      let query = this.token?{token:this.token}:{}
      this.$router.push({
        path:'/order',
        query
      })
    }
  }

}
</script>
<style lang="less" scoped>
    .success_wrap{
        text-align: center;
        padding-top: 200px;

        >div{
            img{
                width: 134px;
            }
        }
        p{
            margin-top: 48px;
            font-size: 32px;
        }
        .btns{
            width: 60%;
            text-align: center;
            margin: 0 auto;
            button{
                color: #fff;
                margin: 20px auto;
            }
        }
    }
</style>