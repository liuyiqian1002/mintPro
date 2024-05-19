<template>
<div class="search_wrap">
  <van-popup v-model="nShow" position="right" :style="{ height: '100%',    width: '100%'}">
      <van-search
    v-model="keyWord"
    show-action
    background="linear-gradient( 145deg, #03C1A7 0%, #30AED4 100%)"
    :placeholder="$t('search.keyWord')" 
    @search="onSearch"
    @cancel="onCancel"
  />
  <div class="content">
      <van-empty v-show="isEmpty" :description="$t('base.noData')"    class="custom-image" :image="emptyImg" />
        <ul >
            <li v-for=" (i ) in searchRes" @click="search(i)"> {{i.BaseCurrency}}/<span class="mc">{{i.QuoteCurrency}}</span></li>
        </ul>
  </div>
      <!-- <van-nav-bar title="预订须知" fixed placeholder left-arrow @click-left="close" /> -->
  </van-popup>

</div>
</template>

<script>
import { Popup,NavBar,Search ,Empty,Image} from "vant";
import {GetSymbolsPageList} from '@/api'
export default {
  name: "Instrusctions",
  components: {
    [NavBar.name]: NavBar,
    [Popup.name]: Popup,
    [Search.name]: Search,
    [Empty.name]: Empty,
    [Image.name]: Image,
  },
  props: {
    nShow: {
      type: Boolean,
      default: false
    },
    Flag:{
      type:String,
    }
  },
  data(){
      return{
          keyWord:'',
          emptyImg:'',
          isEmpty:true,
          KeyWord:'',
          PageIndex:1,
          SymbolsList:[],
          searchRes:[],
      }
  },
mounted(){
  this.emptyImg = require('../assets/images/common/empty_img.png');
  // this.SymbolsList = localStorage.getItem("search")?JSON.parse(atob(localStorage.getItem("search")) ) :[];
},
  methods:{
    close(){
      this.SymbolsList = [];
      this.searchRes = [];
      this.$emit('changeSt',false)
    },
    search(i){
      this.SymbolsList = [];
      this.$emit('search',i)
    },
     onSearch(val) {
      // this.$toast(val);
      console.log(val)
      let arr = [];
      this.SymbolsList.map(_=>{
        let BaseCurrency = _.BaseCurrency.toLocaleLowerCase();
        console.log(BaseCurrency)
        val = val.toLocaleLowerCase();
        console.log(val);
        if( BaseCurrency.includes(val)){
          arr.push(_);
        }
      })
      console.log(this.SymbolsList);
      this.searchRes = arr;
      if(this.searchRes.length == 0 ){
                        this.isEmpty = true;
                    }else{
                        this.isEmpty = false;
                    }
    },
    onCancel() {
        this.close();
    },
       //region 行情 => 获取全部symbol的交易行情列表
      getSymbolsPageList(Flag){
        let data = {
          PageIndex:this.PageIndex,
          PageSize:100,
          AreaId:'Default',
          Sidx:'SortCode',
          Sord:'desc',
          KeyWord:'Default',
          Flag
        };
        GetSymbolsPageList(data).then(r=>{
          console.log(r.data)
          if(r.data.Code == 200 ){
            let arr = [];
            r.data.Data.DataList.map(_=>{
              arr.push(
                {SymbolsId:_.SymbolsId,BaseCurrency:_.BaseCurrency,QuoteCurrency:_.QuoteCurrency}
                )
            })
            this.SymbolsList = arr
            // this.SymbolsList.push(...r.data.Data.DataList) ;
            let Symbols =  btoa(JSON.stringify(arr))
            localStorage.setItem("search" ,Symbols)
                this.loading = false;
                if(this.SymbolsList.length == 0 ){
                  //       this.isEmpty = true;
                  //   }else{
                  //     alert(12)
                  //       this.isEmpty = false;
                    }
                }else{
                    this.finished = true;
                }
        })
      },
  },
  watch:{
    keyWord(){
      this.onSearch(this.keyWord)
    },
    nShow(){
      if(this.nShow){
       this.getSymbolsPageList(this.Flag);
      }

    }
  }
};
</script>
<style lang="less" >
    .search_wrap{
        .van-search__content{
            .van-cell {
                color: #000;
            }
        }

        .van-search__action{
            color: @font_1_color;
        }

        .van-popup{
            overflow-y: unset;
        }
        .custom-image .van-empty__image {
            width: 480px;
            height: 298px;
            margin-top: unset;
        }
    }
</style>
<style lang="less">
    .search_wrap{
        .van-popup {


        }
        .van-cell {


        }
        .content{
            overflow: scroll;
            padding: 20px;
            height: 90vh;
            ul{
                li{
                    height: 120px;
                    line-height: 120px;
                    background: @panel_color;
                    font-size: 30px;
                    margin: 20px 0;
                    padding:0 30px;
                    .bot_br;
                    border-radius: 24px;

                }
            }
            .txt{
                p{
                    font-size: 28px;
                    color: @important-font-color;
                    margin-bottom: 10px;
                }
            }
        }

    }
</style>