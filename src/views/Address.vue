<template>
<div class="address_wrap">
    <van-nav-bar :title="$t('address.title')" left-text right-text='+' @click-right="$router.push('/add_address')"  left-arrow @click-left="$router.go(-1)" />
  <div class="address_wrap">
      <div class="content">
           <van-empty v-show="isEmpty" :description="$t('base.noData')"    class="custom-image" :image="emptyImg" />
            <!-- <van-swipe-cell> -->
        <div class="card" @click="selAdd(i)"  v-for="(i, index) in dataList " :key="index">
                <div class="del" @click.stop="delet(i)" >
                    <img src="../assets/images/common/address_del.png" alt="">
                </div>
                <ul>
                    <li>
                        <p > <img src="../assets/images/common/address_site.png" alt=""> {{$t('addAddress.address')}}</p>
                        <p >{{i.coinaddress}}</p>
                    </li>
                    <li>
                        <p > <img src="../assets/images/common/address_edit.png" alt=""> {{$t('addAddress.Remark')}}:</p>
                        <p >{{i.remark}}</p>
                    </li>
                </ul>
        </div>
                  <!-- <template #right>
    <van-button square :text="$t('base.del')" @click="delet(i)" type="danger" class="delete-button" />
  </template>
</van-swipe-cell> -->
      </div>
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
  PasswordInput,
  Empty,
  SwipeCell,
  Button,
  Dialog,
} from "vant";
import{mapState} from 'vuex'
import {GetPageUserCoinAddress,DeleteUserCoinAddress} from '@/api'
export default {
    name:'Address',
     components:{
        [Icon.name]:Icon,
        [IndexBar.name]:IndexBar,
        [NavBar.name]:NavBar,
        [Picker.name]:Picker,
        [Popup.name]:Popup,
        [Empty.name]: Empty,
        [SwipeCell.name]: SwipeCell,
        [Button.name]: Button,
        [Dialog.name]: Dialog,
    },
    data(){
        return {
            emptyImg:'',
            isEmpty:false,
            MINT_auth:{},
            dataList:[],
            redirect:''
        }
    },
    mounted(){
        this.redirect = this.$route.query.redirect;
         let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
            this.emptyImg = require('../assets/images/common/empty_img.png');
            this.getData();

    },
    methods:{
        getData(){
            let data = {
                    UserId:this.MINT_auth.UserId,//用户编号</br>
                    PageSize:100,//每页数</br>
                    PageIndex:1//:当前页</br>
                };
                GetPageUserCoinAddress(data).then(r=>{
                    console.log(r)
                    if(r.data.Code == 200 ){
                        this.dataList = r.data.Data.rows;
                    }else{
                        this.dataList = [];
                        this.isEmpty = true;
                    }
                })
        },
        delet(i){
            let data = {
                UserCoinAddressId:i.usercoinaddressid,//主键编号</br>
                UserId:this.MINT_auth.UserId,//用户编号</br>
            }
             Dialog.confirm({
            message: this.$t('base.confirm')+this.$t('base.del'),
          }).then(() => {
              DeleteUserCoinAddress(data).then(r=>{
                  if(r.data.Code == 200 ){
                      this.$toast(r.data.Message);
                      this.getData();
                  }
              })
          });
        },
        selAdd(i){
            this.$store.commit('setUserCoinAddress',i)
            this.$router.replace(this.redirect);
        }
    },
    computed: {
        ...mapState(["base"]),
    },
    
}
</script>
<style lang="less" >
 .goods-card {
    margin: 0;
  }
  .address_wrap{
      background-color: @background-color;
      .custom-image .van-empty__image {
         width: 480px;
         height: 298px;
         margin-top: unset;
       }
  }

  .delete-button {
    height: 100%;
    width: 120px;
  }
 </style>
 <style lang="less" scoped>
                    .address_wrap{
                        padding: 20px;
                        .card{
                            background-color: @main_color;
                            border-radius: 10px;
                            color: #fff;
                            margin-bottom: 10px;
                            position: relative;
                            .del{
                                position: absolute;
                                right: 30px;
                                top: 30px;
                                img{
                                    width: 36px;
                                    height: 36px;
                                }
                            }
                            li{
                                img{
                                    width: 36px;
                                    height: 36px;
                                }
                                p{
                                    &:nth-of-type(1){
                                        font-size: 28px;
                                        margin-bottom: 20px;
                                    }
                                }
                                // border-bottom: 1px solid #eee;
                                padding: 20px;
                            }
                        }
                    }

 </style>