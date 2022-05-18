<template>
   <div>
     <van-search
    v-model="par.productname"
    show-action
    placeholder="请输入搜索关键词"
    @search="onSearch"
    @cancel="onCancel"
  />
     <van-dropdown-menu>
    <van-dropdown-item v-model="par.kindsid" :options="option1" @change="onClick" />
    <!-- <van-dropdown-item v-model="value2" :options="option2" /> -->
   </van-dropdown-menu>
   <van-pull-refresh v-model="refreshing" @refresh="onRefresh">
  <van-list
    v-model="loading"
    :finished="finished"
    finished-text="没有更多了"
    @load="onLoad"
  >
    
     <van-card v-for="(item,index) in tabslist" :key="index"
        currency="RSD"
        :price="item.price"       
        :title="item.productName"
        :thumb="item.images"
        desc="  "
        >
  <template #num>
    <van-stepper v-model="item.num" theme="round" button-size="22"/>
  </template>
  <template #tags>
     <span>店铺号:{{item.shopname}}</span>
  </template>
    <template #price>
      <span>RSD{{item.price}}/{{item.unit}}</span>
   </template>
  <template #footer>
    
    <van-button size="small" @click="imports(item.id,item.shopid,item.num)">加入购物车</van-button>
  </template>
   </van-card>
  </van-list>
</van-pull-refresh>
   </div>
</template>

<script>
export default {
   name:'home',
   data(){
       return{
           refreshing:false,
           loading:false,
           finished:false,
           totlepage:0,
          tabslist:[],
          option1:[{text:'推荐',value:0}],
          par:{
              kindsid:0,
              productname:'',
              page:1,
              pagesize:10
          },
          cartpar:{
             id:0,
             shopid:0,
             userid:0,
             proid:0,
             num:0
          }
       }
   },
   created(){
       this.getkindslist()
   },
   methods:{
       onClick(value) {
         // this.$toast(value)
         this.onRefresh()
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
      onRefresh(){
        console.log(this.par.page)
        this.finished=false
        this.loading=true
        this.par.page=1
        this.totlepage=0
        this.tabslist=[]
        this.onLoad()
      },
      
      onLoad(){
       
       
       setTimeout(()=>{

            if (this.refreshing) {
                this.tabslist = []; 
                this.refreshing = false;
              }
              if(this.finished) return;              

                  this.$post('Product/QueryProductList',this.par).then(r=>{
                  console.log(r)
                  this.totlepage=r.totalPage 
                  this.par.page++
                  this.tabslist= this.tabslist.concat(r.data)
                 // console.log(this.tabslist)
                  this.loading=false
                    if(this.tabslist.length>=this.totlepage) 
                     {                        
                        this.finished=true
                        console.log("finished:"+this.finished)
                     }
                   })                                      
                

         },1000)       
        
       
      },
      onSearch(){
       if(this.par.productname=='') return
       this.onRefresh()
      },
      onCancel()
      {

      },
      imports(id,shopid,num){
        if(num>0){
           this.cartpar.proid=id
           this.cartpar.shopid=shopid
           this.cartpar.userid=localStorage.getItem('userid')
           this.cartpar.num=num
           this.$post('Shoppingcart/ShopCartAdd',this.cartpar).then(r=>{
              console.log(r)
              this.$toast(r.msg)
           })
        }else{
           this.$toast('数量不能为零')
        }
      }
   }
}
</script>

<style>

</style>