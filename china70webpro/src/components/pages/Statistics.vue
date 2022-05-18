<template>
  <div>
    <van-nav-bar fixed
  :title="$t('sumbymonth.title')"
  :left-text="$t('sumbymonth.lefttext')"
  left-arrow
  @click-left="onClickLeft"
 
/>

<van-grid>
  <van-grid-item icon="after-sale" :text="$t('sumbymonth.title')" @click="ongosumbymonth"/>
  <!-- <van-grid-item icon="photo-o" text="文字" />
  <van-grid-item icon="photo-o" text="文字" />
  <van-grid-item icon="photo-o" text="文字" /> -->
</van-grid>

  </div>
</template>

<script>
export default {
   name:'statistic',
   data(){
       return{

       }
   },
   created(){

   },
   methods:{
     onClickLeft(){
       this.$router.go(-1)
     },
     ongosumbymonth(){
         var userid=localStorage.getItem('userid')
        this.$fetch('Shop/QueryShopInfo',{UserId:userid}).then(x=>{
          console.log(x)
          if(x.code==1){
              var sid=x.data.id
              this.$router.push({path:'/sumbymonth',query:{ shopid:sid }})
               //this.$router.push({path:'/sumbymonth'})
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