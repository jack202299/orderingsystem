<template>
   <div>
       <van-search v-model="par.shopname" :placeholder="$t('shops.searchtip')" @search="onsearch" />
        <van-dropdown-menu>
    <van-dropdown-item v-model="par.kindsid" :options="option1" @change="onchange" />
    <!-- <van-dropdown-item v-model="value2" :options="option2" /> -->
   </van-dropdown-menu>
       <van-list
  v-model="loading"
  :finished="finished"
  finished-text="没有更多了(ne više)"
  @load="onLoad"
>
 

  <van-card  v-for="(item,index) in list" :key="index" 
  
  :desc="item.shopaddress"
  :title="item.shopname"
  :thumb="item.shopimg"
  @click="onclick(item.id)"
>
  <template #price>
    <span>{{$t('shops.tel')}}:{{item.tel}}</span>
  </template>
  <template #num>
    <span>{{$t('shops.contact')}}:{{item.person}}</span>
  </template>
</van-card>

</van-list>
   <van-empty v-if="showempty" description="暂时没有记录(za sada nema rekorda)" />
   </div>
</template>

<script>
export default {
   name:'shops',
   data(){
       return {
         showempty:false,
         option1:[{text:"全部(all)",value:0}],
         loading:false,
         finished:false,
         list:[],
         totlepage:0,
         par:{
           kindsid:0,
            shopname: "",
            page: 1,
            pagesize: 10
         }
       }
   },
  mounted(){
       if(!sessionStorage.getItem("token")) this.$router.replace({path:'/index'})
       this.getkindslist()
       this.par.page=1
       this.loading=true
       this.list=[]
     this.onLoad()
   },
   methods:{
     onsearch(value){
       this.par.page=1
       this.loading=true
       this.list=[]
       this.par.kindsid=0
       this.onLoad()
     },
     onLoad(){
         
        this.$post('Shop/QueryShopList',this.par).then(x=>{
            console.log(x)
            if(x.result.length==0) this.showempty=true
            this.totlepage=x.icount 
                  this.par.page++
                 this.list=this.list.concat(x.result)
                 // console.log(this.tabslist)
                  this.loading=false
                    if(this.list.length>=this.totlepage) 
                     {                        
                        this.finished=true
                        console.log("finished:"+this.finished)
                     }
        })
     },
     onclick(id){
         this.$router.push({path : '/shopproduct', query : { shopid:id }}) 
     }, 
     onchange(value) {
         // this.$toast(value)
       this.par.page=1
       this.loading=true
       this.list=[]
       this.onLoad()
       },
      getkindslist(){
           var url="Kinds/QuerKindsList"; 
           this.$fetch(url).then(x=>{
              console.log(x)
              x.forEach(e => {
                 var kk={text:e.kindsname,value:e.id}
                 this.option1.push(kk)
              });
           });
       },
   }
}
</script>

<style>

</style>