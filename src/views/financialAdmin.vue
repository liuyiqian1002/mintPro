<template>
  <div class="financial_admin">
          <van-nav-bar
      :title="$t('financialAdmin.title')"
      left-text
      left-arrow
      :placeholder="true" 
      fixed
      @click-left="$router.go(-1)"
    />
    <!-- <div class="calendar">
        <div class="sel" @click="show = true">
            <img src="../assets/images/common/Calendar.png" alt="">
            <p>{{startTime}}-{{endTime}}</p>
        </div>
        <div class="btns">
            <button class="base_btn" @click="delCalendar">cancel</button>
        </div>
    </div> -->
    <div class="content">
         <van-list
          v-model="loading"
          :finished="finished"
          :finished-text="$t('base.noMore')"
          @load="onLoad"
        >
        <div class="card" v-for="i in dataList">
            <div class="l">
                <p class="time"> <span>{{$t('financialAdmin.Time')}}</span>{{ $moment(i.createdate).format('YYYY-MM-DD HH:mm')}}</p>
                <p class="price"><span>{{i.quantity}}</span> USDT</p>
                <p class="day">{{i.days }}{{$t('mining.Days')}} + {{i.rate*100}}%/{{$t('financial.day')}}</p>
                <p class="day" v-show="i.enddate">{{$t('financialAdmin.releaseTime')}} {{i.enddate?$moment(i.enddate).format('YYYY-MM-DD HH:mm'):''}}</p>
            </div>
            <div class="r">
                <button :disabled='i.mark!=1' @click="endOrderTask(i.dogeid)">{{withdrawmark(i.mark )}}</button>
            </div>
        </div>
         </van-list>
    </div>
    <van-calendar v-model="show" :show-confirm="false" first-day-of-week="1" :default-date= [defaultStartDate,defaultEndDate] :min-date='minDate' type="range"  @confirm="onConfirm"/>
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
  Sticky,
  Calendar,
} from "vant";
import {GetOrderList,EndOrderTask} from "@/api"
   
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
     [Calendar.name]:Calendar,
  },
  data(){
      return{
        dataList:[],
        PageIndex:1,
        startTime:'',
        endTime:'',
        show:!true,
        loading: false,
        finished: false,
        refreshing: false,
        minDate: new Date(2010,0,1),
        defaultStartDate: new Date(),
        defaultEndDate: new Date(),
        MINT_auth:{},
        token:'',

      }
  },
  mounted(){
      let token=this.$route.query.token;
      this.token = token;
      let MINT_auth = localStorage.getItem("MINT_auth");
        this.MINT_auth = JSON.parse(this.$decryptByDES(MINT_auth));
         if(token){
          console.log('url token')
          this.MINT_auth.UserId=token;
          localStorage.setItem('isApp',true);
           let MINT_auth= encryptByDES(JSON.stringify(this.MINT_auth));
          localStorage.setItem('MINT_auth',MINT_auth)
        }
      this.getData()
  },
  methods:{
      onLoad() {
        setTimeout(() => {
            this.PageIndex++;
            this.getData();
        }, 1000);
        },
      getData(){
          let TimeInterval = this.startTime?`${this.startTime}#${this.endTime}`:'default'
          let data = {
            UserId:this.MINT_auth.UserId,
            PageSize:20,//每页数
            PageIndex:this.PageIndex,//当前页            
            // TimeInterval//时间区间(默认default,若根据时间区间查询：开始时间#结束时间)
          }
          GetOrderList(data).then(r=>{
              console.log(r.data)
            //   if(r.data.Code == 200 ){
            //       this.dataList = r.data.Data

            //   }
            if (r.data.Code == 200) {
                this.dataList.push(...r.data.Data);
                this.loading = false;
                console.log(this.dataList)
            } else {
                this.finished = true;
                if (this.dataList.length == 0) {
                this.isEmpty = true;
                } else {
                this.isEmpty = false;
                }
            }
          })
      },
      onConfirm(date){
            this.show = false;
             const [start, end] = date;
            this.show = false;
            this.startTime =`${this.$moment(start).format('YYYY-MM-DD')}`;
             this.endTime =`${this.$moment(end).format('YYYY-MM-DD')}`;
             this.PageIndex = 0;
             this.dataList=[]
             this.onLoad()
      },
      delCalendar(){
        this.startTime =''
        this.endTime ='';
        this.PageIndex = 0;
        this.dataList=[];
        this.onLoad()
      },
      endOrderTask(id){
          this.$toast.loading({
                duration: 0, // 持续展示 toast
                forbidClick: true,
                // message: this.$t('login.loading'),
            });
            
          EndOrderTask({DogeId:id,UserId:this.MINT_auth.UserId,}).then(r=>{
              if(r.data.Code == 200 ){
                  this.$toast(r.data.Message);
                   this.startTime =''
                    this.endTime ='';
                    this.PageIndex = 0;
                    this.dataList=[];
                  this.onLoad()
              }
          })
      },
      withdrawmark(val){
        //   理财状态(0-待解锁(不能领取)、1-待领取(已解锁)、2-已转出(已完结))
          let str = '';
           switch (val){
                case 0:
                     str = this.$t('financialAdmin.unlocked')
                 break;
                case 1:
                     str = this.$t('financialAdmin.unclaimed')
                 break;
                case 2:
                     str = this.$t('financialAdmin.out')
                 break;
           }
           return str
      }
  },
  filters:{
      
  }

}
</script>
<style lang="less" scoped>
    .financial_admin{
        background-color: @background-color;
        height: 100vh;
        .calendar{
            padding: 10px 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: #fff;
            .sel{
                display: flex;
                align-items: center;
                padding: 10px 20px;
                background: @item_bg;
                img{
                    width: 24px;
                    margin-right: 20px;

                }
            }
            .base_btn{
                color: #fff;
                width: 120px;
                height: 48px;
            }

        }
        .content{
            padding: 20px 18px;
            .card{
                background: #fff;
                padding: 20px;
                border-radius: 5px;
                display: flex;
                justify-content: space-between;
                align-items: center;
                margin-bottom: 20px;
                >div{
                    &:nth-of-type(1){
                        p{
                            &.time{
                                span{
                                    color: @sub-font-color;
                                }
                                color: @font_color;
                            }
                            &.price{
                                span{
                                    font-size: 36px;
                                    margin-right: 10px;
                                }
                                font-size: 28px;
                                font-weight: 500;
                                // color: @font_f_color;
                            }
                            &.day{
                                margin-bottom: 20px;
                                color: @sub_f_color;
                            }
                        }
                    }
                    &.r{
                        height: 100%;
                        button{
                            .base_btn;
                            width: 200px;
                            border-radius: 50px;
                            height: 80px;
                            color:#fff;
                            &:disabled{
                                background: #999;
                                color: #fff;
                            }
                        }
                        .mc{
                            color:@main_color;
                        }

                    }
                }
            }
        }
    }

</style>