<template>
  <div>
       <van-nav-bar fixed
   :left-text="$t('sumbymonth.lefttext')"
  left-arrow
  @click-left="onClickLeft" 
  />
   <van-card  
  
  :title="user"
  thumb="../assets/FzOwl8ITIf.jpg"
>
   <!-- <template #desc>
       <span>{{$t('seller.Expiredate')}}: {{lastdate}}</span><span>,{{$t('seller.remainingdays')}}{{days}}</span>
   </template> -->
   <template #price>
      <span>{{$t('seller.merchandisequantity')}}:{{productcount}}</span>
   </template>
   </van-card>

 
   <van-grid :column-num="3">
      <van-grid-item icon="add-o" :text="$t('seller.product')" @click="getprdouct"/>
      <van-grid-item icon="add-o" :text="$t('seller.Order')" @click="goorder"/>
       <van-grid-item icon="add-o" :text="$t('seller.client')" @click="gooclient"/>
      <van-grid-item icon="add-o" :text="$t('seller.salesstatistics')" to="/statistics"/>
       
   </van-grid>
  </div>
</template>

<script>
export default {
   name:'seller',
   data(){
       return {
          user:'',
          lastdate:'',
          days:'',
          productcount:0
       }
   },
   mounted(){
       if(!sessionStorage.getItem("token")) this.$router.replace({path:'/index'})
     this.getdata()
   },
   methods:{
    onClickLeft(){
     this.$router.go(-1)
    },
    getdata(){
         var userid=localStorage.getItem('userid')
          this.$fetch('Shop/QueryShopInfo',{UserId:userid}).then(x=>{
          console.log(x)
          if(x.code==1){
              var sid=x.data.id
              this.user=x.data.shopname 
              this.lastdate=x.data.lastDate 
              this.days=x.data.usdays 
              this.productcount=x.countmsg          
          }else
          {
              this.$toast(x.msg)
          }
        })
    },
     getprdouct(){
        var userid=localStorage.getItem('userid')
        this.$fetch('Shop/QueryShopInfo',{UserId:userid}).then(x=>{
          console.log(x)
          if(x.code==1){
              var sid=x.data.id
              this.$router.push({path:'/productlist',query:{ shopid:sid }})
          }else
          {
              this.$toast(x.msg)
          }
        })
      },
      goorder(){
         var userid=localStorage.getItem('userid')
        this.$fetch('Shop/QueryShopInfo',{UserId:userid}).then(x=>{
          console.log(x)
          if(x.code==1){
              var sid=x.data.id
              this.$router.push({path:'/shoporder',query:{ shopid:sid }})
              
          }else
          {
              this.$toast(x.msg)
          }
        })
      },
      gooclient(){
          var userid=localStorage.getItem('userid')
        this.$fetch('Shop/QueryShopInfo',{UserId:userid}).then(x=>{
          console.log(x)
          if(x.code==1){
              var sid=x.data.id
              this.$router.push({path:'/clientage',query:{ shopid:sid }})
              
          }else
          {
              this.$toast(x.msg)
          }
        })
      }
   }
}
</script>

<style>

</style>