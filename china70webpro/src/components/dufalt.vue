<template>
   <div>
       <!-- <van-nav-bar
  :title="$t('index.login')" 
 
>
   <template #left>
     <span>{{$t('index.lang')}}</span>
     <van-dropdown-menu>
      <van-dropdown-item v-model="value1" :options="option1" @change="onchang" />  
      </van-dropdown-menu>
   </template>
    </van-nav-bar> -->
  <!-- <van-swipe :autoplay="3000">
  <van-swipe-item v-for="(image, index) in images" :key="index">
   
     <van-image :src="image" width="100%" height="100%">
      <template v-slot:error>加载失败</template>
      </van-image>
  </van-swipe-item>
</van-swipe> -->
<div style="height:3rem;"></div>

<router-view />


<van-tabbar route fixed placeholder >
  <!-- <van-tabbar-item  to="/home" icon="home-o">首页</van-tabbar-item>   -->
  <van-tabbar-item  to="/shopproduct?shopid=2" icon="shop">{{$t('dufalt.shop')}}</van-tabbar-item>
  <van-tabbar-item  to="/shoppingcart" icon="shopping-cart">{{$t('dufalt.shopcart')}}</van-tabbar-item>
  <van-tabbar-item  to="/person"  icon="setting-o">{{$t('dufalt.personalcenter')}}</van-tabbar-item>
</van-tabbar>
 
   </div> 

</template>

<script>


export default {
   name:'dufalt',
   data(){
       return{
          option1:[{icon:'',text:'China',value:'zh-CN'},{icon:'',text:'English',value:'en-US'},{icon:'',text:'Serbia',value:'sr-SR'}],
       }
   },
   
  
   created(){
     this.$i18n.locale='zh-CN'
   },
 
   
   methods:{
      onchang(val){
      localStorage.setItem('lang',val)
      this.$i18n.locale=val
    },
      
   }
}
</script>

<style>

</style>