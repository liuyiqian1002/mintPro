<template>
  <div class="nickname_wrap">
    <van-nav-bar :title="$t('nickname.title')" left-text :placeholder='true' fixed left-arrow @click-left="$router.go(-1)" />
        <div class="content">
            <ul>
                <li><input type="text" v-model="nickname" :placeholder="$t('nickname.label')"></li>
            </ul>
            </div>
      <div class="btns">
        <button class="main_btn" @click="submit">{{$t('base.submit')}}</button>
    </div>
  </div>
</template>

<script>
import { Icon, NavBar, IndexBar, } from 'vant'
import { UpdateUserEntity, } from "@/api";
export default {
    name:'nickname',
     components:{
    [NavBar.name]: NavBar,
    [Icon.name]: Icon,
    },
    data(){
        return{
            nickname:'',
            MINT_auth:{}
        }
    },
    mounted(){
          let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    console.log(MINT_auth)
    },
    methods:{
        submit(){
            if(!this.nickname){
                this.$toast(this.$t('nickname.label'))
                return;
            }
            this.updateUserEntity('NickName',this.nickname).then(r=>{
                if(r.Code == 200 ){
                    this.$toast(r.Message);
                    setTimeout(_=>{
                        this.$router.replace('/user_info')
                    },2000)
                }
            })
        },
        async updateUserEntity  (Action,ActionValue){
            let data = {
                UserId: this.MINT_auth.UserId,
                Action,
                ActionValue,
            };
           let res = await  UpdateUserEntity(data).then(r=>{
               return r.data;
            });
            return res;
        }

    }
}
</script>
<style lang="less" scoped>
    .nickname_wrap{
        background-color: @background-color;
        min-height: 100vh;
        .content{
            background-color: @panel_color;
            ul{
                display: flex;
                // height: 80px;
                align-items: center;
                justify-content: space-between;
                position: relative;
                padding: 30px;
                width: 100%;
                li{
                    height: 80px;
                    &:nth-of-type(1){
                        width: 70%;
                    }
                    input{
                        height: 100%;
                        background-color: @panel_color;
                        width: 100%;
                        color: @font_1_color;
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