<template>
<div class="h5-wrap">
    <div class="notice_wrap">
      <van-nav-bar :title="title" fixed placeholder left-arrow @click-left="$router.go(-1)" />
      <div >
      <iframe ref="iframe"  src="" frameborder="0"></iframe>
      </div>
    </div>

</div>
</template>

<script>
import { Popup,NavBar } from "vant";
import { UpdateUserEntity } from "@/api";
export default {
  name: "Instrusctions",
  components: {
    [NavBar.name]: NavBar,
    [Popup.name]: Popup
  },
  props: {
    nShow: {
      type: Boolean,
      default: true
    }
  },
  data(){
    return {
      checked:true,
      MINT_auth:{},
      url:'',
      title:''
    }
  },
  mounted(){
    this.url = this.$route.query.url;
    this.title = this.$route.query.title;
    document.title=this.title;
    console.log(this.url)
    let lang = localStorage.getItem('MINT_lang');
      setTimeout(_=>{
        var _clientHeight=document.documentElement.clientHeight;
        console.log(_clientHeight)
        this.$refs.iframe.src= this.url//this.$config.imgPath+`/News/AppSynopsis/16515ae1-eb5b-4aa3-99cc-fc1ace43ede4?LanguageVersion=${lang}`;
        this.$refs.iframe.style.height= _clientHeight-46+'px';
        this.$refs.iframe.style.width='100%';
      },500)
  },
  methods:{
    close(){
      this.$emit('closeH5',false)
    },
    // /region [6]用户 => 变更用户基本信息
    
  },
  watch:{
    nShow(){
      if(this.nShow){
       
      }

    }
  }
};
</script>
<style lang="less" >
    .search_wrap{
        .van-popup{
            overflow-y: unset;
        }
        .btns{
            background-color: #fff;
            z-index: 100;
            position: fixed;
            bottom: 10px;
            left: 0;
            right: 0;
            width: 100%;
            text-align: center;
            margin: 0 auto;
            .main_btn{
                width: 90%;
            }
        }
    }
</style>
<style lang="less" scoped>
    .notice_wrap{
        height: 100vh;
        background-color: @background-color;
        overflow: scroll;
        color:@font_1_color;
        padding-bottom: 120px;
        .txt{
            p{
                font-size: 38px;
                color:#1CED34;
                margin-bottom: 10px;
            }
        }
    }
    .agree{
        width: 90%;
        margin:  auto;
        padding: 20px 0;
        display: flex;
        align-items: center;
        label{
            align-items: center;
            display: flex;
            b{
                display: block;
                width: 30px;
                height: 30px;
                background: url(../assets/images/common/reg_tologin_disagree.png) no-repeat center;
                background-size:contain;
                margin-right: 20px;
                &.active{
                    background: url(../assets/images/common/reg_tologin_agree.png) no-repeat center;
                    background-size:contain;
                }
            }
        }
        span{
            color: @font_1_color;
        }
    }
</style>