<template>
   <div>
     <van-nav-bar
  title="关于(oko)"
  left-text="back"
  
  left-arrow
  @click-left="onClickLeft"
  
/>

   <div class="divbox" v-for="(item,index) in txt" :key="index">
     {{item}}
   </div>
   </div>
</template>

<script>
export default {
    name:'about',
    data(){
        return {
          txt:[]
        }
    },
    mounted(){
      var lang=localStorage.getItem('lang')
      this.$fetch('About/GetAboutTxt',{par:lang}).then(r=>{
        this.txt=r.cont
      })
    },
    methods:{
     onClickLeft(){
       this.$router.go(-1)
     }
    }
}
</script>

<style>
    .divbox{
            word-break:break-all;
            margin-left: 5px;
      }
</style>