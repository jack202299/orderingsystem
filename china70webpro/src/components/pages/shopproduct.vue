<template>
   <div>
     <van-nav-bar fixed
  :title="shoptitle"
  :left-text="$t('sumbymonth.lefttext')" 
  left-arrow
  @click-left="onClickLeft" 
  />
    <van-search v-model="par.productName" :placeholder="$t('sp.searchtip')" @search="onsearch" />
         <van-dropdown-menu>
          <van-dropdown-item v-model="par.producttypeid" :options="option1" @change="onchange" />                 
         </van-dropdown-menu>
   <div class="box1">
    <van-pull-refresh v-model="refreshing" @refresh="onRefresh">

      <van-list
    v-model="loading"
    :finished="finished"
    :finished-text="$t('sp.nomore')"
    @load="onload"
  >
     
     <van-card v-for="(item,index) in list" :key="index"
        currency="RSD"
        :price="item.price"       
        :title="item.productName"
        :thumb="item.images"
         @click-thumb="clickthumb(item.images)"
        >
  <template #num>
    <van-stepper v-model="item.num" theme="round" button-size="22"/>
  </template>
   <template #desc>
      <span>{{item.alias}}</span>
   </template>
    <template v-if="item.hotsort" #tag>
         <van-tag mark type="danger">新品</van-tag>
    </template>

   
    <template #price>
      <span>RSD{{item.price}}/{{item.unit}}</span>
   </template>
  <template #footer>
    
    <van-button size="small" @click="imports(item.id,item.shopid,item.num)">{{$t('sp.cartbut')}}</van-button>
  </template>
   </van-card>
  </van-list>
   </van-pull-refresh> 
     <van-empty v-if="showempty" :description="$t('sumbymonth.emptip')" />

</div>

 <van-image-preview v-model="show1" :images="images" closeable  />
  

<van-dialog v-model="show" :title="$t('sp.digti')" showCancelButton :confirmButtonText="$t('sp.digbut')" @confirm="gotel">
   {{$t('sp.digcont')}}
</van-dialog>

   </div>
</template>

<script>

export default {
  name:'shopproduct',
  data(){
      return{
          show1:false,
          showempty:false,
           icount:0,
           shoptitle:'',
           show:false,
           refreshing:false,
           loading:false,
           finished:false,
         list:[],
         images:[],
         option1:[{text:"全部(all)",value:0}],
         irows:0,
         par:{
             shopid:0,
             producttypeid:0,
             productName:'',
             page:1,
             pageSize:10
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

  activated(){
    this.getproducttype()
    
    this.data()
  },
  created(){
     this.getproducttype()
      // if(!localStorage.getItem("userid")) this.$router.replace({path:'/index'})
      this.par.shopid=this.$route.query.shopid
      this.$fetch('Shop/QueryShopInfWithID',{Id:this.par.shopid}).then(r=>{
           this.shoptitle=r.data.shopname
      })
      //this.getdata()
   },
   methods:{
       getproducttype(){
        this.option1=[{text:"全部(all)",value:0}]
        this.$fetch('ProductType/QueryProductTypeListByShopId',{shopid:this.$route.query.shopid}).then(x=>{
            console.log(x)
            x.data.forEach(r=>{
              var vk={text:r.protype,value:r.id}
              this.option1.push(vk)
            })
        })

      },
     onRefresh(){
        this.finished = false;
         this.par.page=1
      // 重新加载数据
      // 将 loading 设置为 true，表示处于加载状态
      this.loading = true;
      this.irows=0
      this.getdata();
     },
     getdata(){

      if (this.refreshing) {
          this.list = [];
          this.refreshing = false;
        }

       this.$post('Product/QueryProductWithShopId',this.par).then(r=>{
                if(r.code==-1){
                   this.show=true
                }else{
                 this.irows=r.icount 
                  if(r.data.length==0) this.showempty=true
                  console.log(r.data.length)
                  this.list= this.list.concat(r.data)
                 // console.log(this.tabslist)
                  this.loading=false
                    if(this.list.length>=this.irows) 
                     {                        
                        this.finished=true
                        console.log("finished:"+this.finished)
                     }else
                     {
                        this.par.page++
                     }
                }
       })
     },
     gotel(){
        window.location.href="Tel:0638209044"
        this.show=false
     },
    onsearch(value){
      this.par.productName=value
      this.par.page=1
      this.list=[]
      this.loading=true
      this.getdata()     
    },
    onClickLeft(){
      this.$router.go(-1)
    },
    onload(){
       console.log(this.loading)
       this.getdata()
    },
      imports(id,shopid,num){

         if(!sessionStorage.getItem("token")) {
             this.$toast('请先登录(molimo vas da se prvo prijavite)')
             this.$router.replace({path:'/index'})
         }
        
        if(num>0){
           this.cartpar.proid=id
           this.cartpar.shopid=shopid
           this.cartpar.userid=localStorage.getItem('userid')
           this.cartpar.num=num
           this.$post('Shoppingcart/ShopCartAdd',this.cartpar).then(r=>{
              this.icount=this.icount+num             

              console.log(r)
              this.$toast(r.msg)
           })
        }else{
           this.$toast('数量不能为零(Količina ne može biti nula)')
        }
      },
      onchange(val){
         this.par.producttypeid=val
         this.par.page=1
         this.loading = true;
          this.irows=0
         this.list=[]
         this.getdata()
      },
      clickthumb(val){
        this.images=[]
        this.images.push(val)
        this.show1=true
      }
   }
}
</script>

<style>
  .box1{
            min-height: 3rem;
            max-height: 100%;           
            overflow-x: hidden;
            overflow-y: auto;

       }
</style>