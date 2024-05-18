<template>
  <div class="recharge_wrap" style="height: 1000px">
     <van-nav-bar :title="$t('recharge.title')" left-text   left-arrow @click-left="$router.go(-1)" />
    <div class="content">
        <div class="service" @click="CustomerService">
            <img src="../assets/images/common/ic_contract_service_small.png" alt="">
            <div>
                <div>
                    <span>{{$t('recharge.serve')}}</span>
                </div>
                <div>
                    <img src="../assets/images/common/back_right.png" alt="">
                </div>

            </div>
        </div>
        <div class="up_img">
            <p class="title">{{$t('recharge.upImg')}}</p>
            <div class="pic">
                <img ref="img" src="../assets/images/common/ic_pz.png" alt="">
                <input type="file"  @change="upimg($event)" class="fils" name="" id="">
            </div>
        </div>
        <div class="tips">
            <p class="title">{{$t('recharge.rule')}}</p>
            <p>{{$t('recharge.tips')}}</p>
            <p>{{$t('recharge.tips1')}}</p>
            <p>{{$t('recharge.tips2')}}</p>
        </div>
        <div class="btns">
            <button class="main_btn" @click="Certificate">{{$t('recharge.confirm')}}</button>
        </div>
    </div>
  </div>
</template>

<script>
import {UploadDepositCertificate,UpLoadImg} from "@/api"
import {
    setToken,
    getToken,
    encryptByDES,
    decryptByDES,
} from "@/libs/util";
import {
  Icon,
  NavBar,
  IndexBar,
} from "vant";
export default {
    name:'recharge',
     components:{
        [Icon.name]:Icon,
        [IndexBar.name]:IndexBar,
        [NavBar.name]:NavBar,
    },
    data(){
        return{
            MINT_auth:'',
            FilePath:'',
            lang:""
        }
    },
    mounted(){
        this.lang  = localStorage.getItem('MINT_lang')
         let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
    },
    methods:{
         // https://124.221.191.238:9080/News/CustomerService/{语言版本标识zh-CN/zh-hk/en/jpn}
     CustomerService(){
      let url =  this.$config.imgPath+`/News/CustomerService/${this.lang}?token=${this.MINT_auth.UserId}`;
      location.href = url;
    },
        upimg(e) {

				let file = e.target.files[0];
				// var exp = /png|gif|jpg|jpeg/;
				// if (!exp.test(file.name)) {
				// 	return;
				// }
				let param = new FormData(); //创建form对象
                param.append("file", file); //通过append向form对象添加数据
                 let EnCode = `${ encodeURIComponent(encryptByDES(JSON.stringify({EnCode:this.MINT_auth.EnCode})))}`
				param.append("EnCode", this.MINT_auth.EnCode); //通过append向form对象添加数据
				param.append("LanguageVersion", this.lang); //通过append向form对象添加数据
				// console.log(param.get("file")); //FormData私有类对象，访问不到，可以通过get判断值是否传进去
				let config = {
					headers: {
						"Content-Type": "multipart/form-data"
					}
                }; //添加请求头
                 this.$toast.loading({
                    duration: 0, // 持续展示 toast
                    forbidClick: true,
                });
				this.axios.post(this.$config.baseUrl.pro+"/Index.asmx/UpLoadImg", param, config).then(res => {
                    console.log(JSON.parse(this.$decryptByDES(res.data)) );
                    let r =JSON.parse(this.$decryptByDES(res.data));
                            // {"Code":200,"Message":"添加成功","Data":{"FilePath":"/Resource/WebUserImage/HJ231057/20200810005058500.png"}}
					if (r.Code == 200) {
                        this.FilePath = r.Data.FilePath;
                        this.$toast(this.$t('base.upSeccess'))
						// this.UpdateUserEntity('HeadIcon',res.data.Data.FilePath).then(r=>{
						// 	console.log(r);
						// this.$layer.msg("修改成功");
						// setTimeout(() => {
						// 	// 刷新用户信息
						// 	this.GetEntityUser();
							// {"Code":200,"Message":"添加成功","Data":{"FilePath":"/Resource/WebUserImage/HJ231057/20200810005058500.png"}}
						// }, 800)
						// })
					}else{
                        this.$toast(r.Message);
                        }
				});

				var objUrl = getObjectURL(file);
				console.log("objUrl = " + objUrl);
				if (objUrl) {
					this.$refs.img.src=objUrl;
				}

				function getObjectURL(file) {
					var url = null;
					if (window.createObjectURL != undefined) {
						// basic
						url = window.createObjectURL(file);
					} else if (window.URL != undefined) {
						// mozilla(firefox)
						url = window.URL.createObjectURL(file);
					} else if (window.webkitURL != undefined) {
						// webkit or chrome
						url = window.webkitURL.createObjectURL(file);
					}
					return url;
				}
            },
            //UserId：用户token</br>
//FilePath：图片凭证路径(必填)</br>
Certificate(){
    let data = {
       FilePath:this.FilePath,
        UserId:this.MINT_auth.UserId,
    };
    if(!this.FilePath){
        this.$toast(this.$t('recharge.imgTips'))
        // this.$toast('请添加凭证!');
        return; 
    }
    UploadDepositCertificate(data).then(r=>{
        if(r.data.Code == 200 ){
             this.$toast(r.data.Message);
        }
    })
}
    }
}
</script>
<style lang="less" >
.recharge_wrap{
     .van-hairline--bottom{
          &::after{
            content: '';
            border-bottom-width:0px;
            border: none !important;
          }
        }
} 


</style>
<style lang="less" >
    .recharge_wrap{
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
    .recharge_wrap_main {
        background-color: @background-color;
    }
    .recharge_wrap{
        background-color: @background-color;
        .content{
            padding: 20px;
            .service{
                display: flex;
                // justify-content: space-between;
                // background:rgba(247,246,252,1);
                padding:0 20px ;
                height: 80px;
                align-items: center;
                border-radius:10px;
                img{
                    height: 40px;
                    margin-right: 15px;
                }
                >div{
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    width: 100%;
                    height: 100%;
                    .bot_br;
                    &:nth-of-type(1){

                        span{
                            font-size: 30px;
                            color: @font_1_color;
                        }

                    }
                    &:nth-of-type(2){
                        img{
                            height: 30px;
                        }
                    }
                }
            }
            .up_img{
                margin-top: 20px;
                .title{
                    font-size:30px ;
                    padding: 20px;
                    color:@font_f_color;
                }
                .pic{
                    position: relative;
                    height: 200px;
                    line-height: 200px;
                    width: 90%;
                    padding:0 40px;
                    margin: 20px auto ;
                    background-color: @panel_color;
                    border-radius: 20px;
                    .fils{
                        position: absolute;
                        width: 100%;
                        height: 100%;
                        top: 0;
                        left: 0;
                        opacity: 0;
                    }
                    img{
                        height: 120px;
                        width:120px;
                    }
                    input {
                        background-color: @panel_color;
                    }
                }
            }
            .tips{
                margin-top: 40px;
                padding: 20px;
                border-top: 5px solid rgba(247,246,252,1);
                .title{
                    font-size: 28px;
                    color: @font_f_color;
                    padding: 10px 0;
                    &~p{
                        padding: 10px;
                        color:@font_f_color;
                        font-size: 26px;
                    }
                }
            }
            .btns{
                width: 90%;
                text-align: center;
                margin: 10px  auto;
            }
        }
    }
</style>