<template>
  <div>
  <van-nav-bar fixed
  title="店铺管理"
  left-text="返回"  
  right-text="添加店铺"
  left-arrow
  @click-left="onClickLeft"  
  @click-right="onClickRight"
/>
 <van-search v-model="par.shopname" placeholder="请输入搜索商店名称" @search="onsearch" />
        <van-dropdown-menu>
    <van-dropdown-item v-model="par.kindsid" :options="option1" @change="onchange" />
    <!-- <van-dropdown-item v-model="value2" :options="option2" /> -->
   </van-dropdown-menu>
       <van-list
  v-model="loading"
  :finished="finished"
  finished-text="没有更多了"
  @load="onLoad"
>
  

   <van-card  v-for="(item,index) in list" :key="index" 
   
  :desc="item.shopaddress"
  :title="item.shopname"
  :thumb="item.shopimg"
>
   <template #price>
    <span>电话:{{item.tel}}</span>
  </template>
  <template #num>
    <span>联系人:{{item.person}}</span><span>到期:{{item.lastDate}}</span>
  </template>
  <template #tags>
    <van-button size="mini">按钮</van-button>
    <van-button size="mini" @click="ongosumproduct(item.id)">拍照统计</van-button>
  </template>
  <template #footer>
    <van-button size="mini" @click="onedit(item.id)">修改</van-button>
    <van-button size="mini" @click="onclick(item.id)">产品管理</van-button>
  </template>
</van-card>

</van-list>

  </div>
</template>

<script>
export default {
   name:'shopsadmin',
   data(){
       return {
           option1:[{text:"全部",value:0}],
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
   created(){
       var uid=localStorage.getItem("userid")
     this.$fetch('User/QueryUserInfo',{Id:uid}).then(r=>{
       console.log(r.result)
       if(r.code==0){
         if(r.result.identitytype!="admin")  this.$router.replace({path:'/myshop'})
       }
     })
       this.getkindslist()
       this.par.page=1
       this.loading=true
       this.list=[]
       this.onLoad()
   },
   methods:{
     onedit(id){
        this.$router.push({path:'/myshop',query:{b:"edit",shopid:id}})
     },
     ongosumproduct(id){
       this.$router.push({path:'/sumproduct',query:{shopid:id}})
     },

     onClickLeft(){
        this.$router.go(-1)
     },
     onClickRight(){
        // this.$router.push({path : '/shopproduct', query : { shopid:id }}) 
        this.$router.push({path:'/myshop',query:{b:"add"}})
     },
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
         this.$router.push({path : '/productlist', query : { shopid:id }}) 
          //this.$router.push({path:'/myshop',query:{b:"edit",shopid:id}})
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