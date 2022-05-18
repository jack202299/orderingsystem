<template>
  <div>
          <van-nav-bar fixed
  :title="$t('shporder.title')"
  :left-text="$t('cart.backtl')"  
  left-arrow
  @click-left="onClickLeft"      
/>
    <van-grid :column-num="3">
           <van-grid-item  icon="photo-o" :text="$t('ord.myord')" to='/order' />
           <!-- <van-grid-item  icon="photo-o" text="收货地址" @click="ongoaddress" /> -->
    </van-grid>  

  </div>
</template>

<script>
export default {
   name:'custmoter',
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
     ongoaddress(){
       var uid=localStorage.getItem('userid')
       this.$router.push({path:'/address',query:{uid}})
     }
   }
}
</script>

<style>

</style>