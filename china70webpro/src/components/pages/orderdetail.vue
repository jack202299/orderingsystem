<template>
   <div>
     <van-nav-bar fixed
  :title="$t('ord.title')"
  :left-text="$t('ord.lefttext')"
  :right-text="$t('ord.print')"
  left-arrow
  @click-left="onClickLeft"
   @click-right="onClickRight(tabledata.id)"
/>
<van-cell-group>
  <van-field v-model="tabledata.ordername" :label="$t('ord.orno')" readonly />
  <van-field v-model="tabledata.shopname" :label="$t('ord.shopname')" readonly />
    <van-field v-model="tabledata.person" :label="$t('ord.buyser')" readonly />  
  <van-field v-model="tabledata.meney" :label="$t('ord.oramount')" readonly />
  <van-field v-model="tabledata.remark" :label="$t('ord.remark')" readonly type="textarea" />
  
   <!-- <van-image
  width="10rem"
  height="10rem"
  fit="contain"
 :src="tabledata.images"
 :alt="$t('ord.imgerr')"
  @click="show=true"
/> -->
   <van-image-preview v-model="show" :images="images" closeable />
</van-cell-group>
   <van-card v-for="(item,index) in tabledata.sublist" :key="index"
  :num="item.quantity"  
  :title="item.productName"
  :thumb="item.images"
>
  <template #price>
    <span>{{$t('ord.price')}}:RSD{{item.price}}</span>
  </template>
   </van-card>
   </div>
</template>

<script>
export default {
  name:'orderdetail',
  data(){
      return {
        tabledata:{},
        images:[],
        show:false
      }
  },
  mounted(){
       if(!sessionStorage.getItem("token")) this.$router.replace({path:'/index'})
    this.getdata()
  },
  methods:{
     getdata(){
       console.log(this.$route.query.orderid); 
       var id=this.$route.query.orderid
       this.$fetch('OrderDeTail/QueryOrderDelTail',{orderid:id}).then(x=>{
         console.log(x)
         this.tabledata=x
         this.images.push(x.images)
       })
     },
     onClickLeft(){
           this.$router.go(-1)
     },
     onClickRight(){
       this.$fetch('Order/QueryPrintPage',{Id:this.tabledata.id}).then(r=>{
           console.log(r)
           window.open(r)
       })
      // var url="http://www.china70.tk/printpages.html?id="+this.tabledata.id
       //window.open(url)
     }
  }
}
</script>

<style>

</style>