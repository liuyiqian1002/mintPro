<template>
<div class="userInstructions_wrap">
  <van-popup v-model="nShow" position="right" :style="{ height: '100%' ,width:'100%'}">
    <div class="notice_wrap">
      <van-nav-bar title="" fixed placeholder left-arrow :title="$t('base.protocol')" @click-left="close" />
      <div class="txt">
        <div class="doc">
          <div class="doc_top">
            <div>
              <p>{{$t('search.openCon')}}</p>
              <p>{{$t('search.doc')}}
              </p>
            </div>
            <div>
              <img src="../assets/images/common/userInstructions_top.png" alt="">
            </div>
          </div>
          <iframe ref="iframe" src="" frameborder="0"></iframe>
        </div>
      </div>
    <div class="btns">
       <div class="agree">
           <label @click="checked = !checked"> <b :class="{active:checked}" > </b> {{$t('register.agree')}} </label>  <span>《{{$t('base.protocol')}}》</span>  
          </div>
      <button class="main_btn" @click="updateUserEntity">{{$t('base.confirm')}}</button>
    </div>
    </div>
  </van-popup>

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
      lang:''
    }
  },
  mounted(){
     let MINT_auth = localStorage.getItem("MINT_auth");
     this.lang = localStorage.getItem('MINT_lang');
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
  },
  methods:{
    close(){
      this.$emit('closeH5',false)
    },
    // /region [6]用户 => 变更用户基本信息
    updateUserEntity(){
      let data = {
      UserId:this.MINT_auth.UserId,//用户编号</br>?
      Action:'OpenFutures',//修改信息:<br/>
      ActionValue:1,
      };
      UpdateUserEntity(data).then(r=>{
        if(r.data.Code == 200 ){
          console.log(r.data);
          this.close()
        }
      })
    }
  },
  watch:{
    nShow(){
      if(this.nShow){
         setTimeout(_=>{
        var _clientHeight=document.documentElement.clientHeight;
        console.log(_clientHeight)
        this.$refs.iframe.src= this.$config.imgPath+`/News/AppSynopsis/16515ae1-eb5b-4aa3-99cc-fc1ace43ede4?LanguageVersion=${this.lang}`;
        this.$refs.iframe.style.height=56+'vh' //_clientHeight+'px';
        this.$refs.iframe.style.width='90%';
      },500)
      }

    }
  }
};
</script>
<style lang="less" >
.userInstructions_wrap{
  .van-icon,
  .van-nav-bar__title{
    color: #fff !important;
  }
  .van-ellipsis{
    height: 46PX !important;
    line-height: 46PX !important;

  }
  .van-nav-bar{
    background-color:transparent;
  }
  .van-hairline--top-bottom,
        .van-hairline--bottom{
          &::after{
            content: '';
            border-bottom-width:0px;
            border: none !important;
          }
        }
    .van-popup{
        overflow-y: unset;
    }
  .btns{
    padding: 20px 0;
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
        background-color: #2A63FF;
        overflow: scroll;
        padding-bottom: 120px;
        .txt{
          height: 83%;
          padding-top: 10px;
          // padding-bottom: 80px;
          overflow: scroll;
          // margin: 20px;

          // background-color: #fff;
          // userInstructions_bg
          background:url(../assets/images/common/userInstructions_bg.png) no-repeat;
          background-size: 100% 100%;
          .doc{
            margin-top: 40px;
            height: 93%;
            overflow: hidden;
            padding-bottom: 800px;
            .doc_top{
              width: 90%;
              margin:0 auto;
              padding: 30px 40px 20px;
               box-shadow: 
              //  0px -2px 2px 2px #eee,   /*上边阴影  红色*/
                // -2px 0px 2px 2px #eee,   /*左边阴影  绿色*/
                // 2px 0px 2px 2px #eee,    /*右边阴影  蓝色*/
                0px 2px 2px 2px #eee; 
              p{
                &:nth-of-type(1){
                  color:#333333;
                  font-size: 52px;
                  font-weight: 500;

                }
              }
              img{
                width: 200px;
              }
              display: flex;
              align-items: center;
              justify-content: space-between;
            }
             iframe{
                  margin: 0 auto;
                }
          }
            // p{
            //     font-size: 28px;
            //     color: @important-font-color;
            //     margin-bottom: 10px;
            // }
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
                color: @main_color;
            }
        }
</style>
