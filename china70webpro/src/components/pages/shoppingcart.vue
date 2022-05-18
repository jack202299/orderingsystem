<template>
  <div>
     <van-nav-bar fixed
  :title="$t('cart.crttil')"
  :left-text="$t('cart.backtl')"  
  left-arrow
  @click-left="onClickLeft"
 
/>
     <div >
        <van-field
          v-model="message"
          rows="5"
          autosize
          :label="$t('cart.message')"
          type="textarea"
          :placeholder="$t('cart.plc')"
          maxlength="350"
          show-word-limit
          border
        />
     </div>
     <!-- <div>
         <van-field name="uploader" :label="$t('cart.uptl')">
              <template #input>
                <van-uploader 
                v-model="uploader" 
                :after-read="afterRead"
                 :before-read="beforeRead"  
                  :before-delete ="beforedelete"           
                   max-count="1" 
                   capture="camera" />
              </template>
            </van-field>

     </div> -->
    
     <van-cell-group inset v-for="(k,index) in tabslist" :key="index" >
        <template #title>
           <span>{{$t("cart.shopname")}}:{{k.shopcd}}</span>
        </template>
            
          <van-cell >
  <!-- 使用 title 插槽来自定义标题 -->
  <template #title>
    <span class="custom-title">{{$t('cart.total')}}:RSD{{k.meny}}</span>   
  </template>
   <template #default>
        <van-button :loading="showload" size="small" @click="onSubmit(index)">{{$t('cart.Submitorders')}}</van-button>
  </template>
    </van-cell>
        <div style="height: 100%;OVERFLOW-Y:scroll; width: 100%">
        <van-card v-for="(item,index) in k.plist" :key="index"
         currency="RSD"
        :price="item.price"       
        :title="item.productName"
        desc=""
        :thumb="item.images"
        >
  <template #num>
    <van-stepper @change="onChange(item.id,item.num)" v-model="item.num" theme="round" button-size="22"/>
  </template>
   
   <template #price>
      <span>RSD{{item.price}}/{{item.unit}}</span>
   </template>
  <template #footer>    
     <van-button size="small" @click="deleterow(item.id)">{{$t('cart.del')}}</van-button>
  </template>
   </van-card>
        </div>
   
   </van-cell-group>
       
  </div>
</template>

<script>
import Compressor from 'compressorjs';
export default {
  name:'shoppingcart',
  data(){
      return{
        showload:false,
        message:'',
        list:[],
        tabslist:[],
         uploader:[],
        ship:{
          id:0,
          userid:'',
          images:''       
        }
      }
  },
  activated(){
    this.onload()  
       this.getaddresslist()
  },
  mounted(){
       if(!sessionStorage.getItem("token")) this.$router.replace({path:'/index'})
       if(localStorage.getItem('remark')) this.message=localStorage.getItem('remark')
       this.onload()  
       this.getaddresslist()
  },
  methods:{
   getaddresslist(){
      this.$fetch('Ships/QueryUserRemark',{UserId:localStorage.getItem('userid')}).then(x=>{
        
        if(x.data!=null){
        this.ship.id=x.data.id;
        this.ship.userid=x.data.userid
        this.ship.images=x.data.images
        this.uploader.push({url:x.data.images})
        }
      })
   },
   onload(){
     this.$fetch('Shoppingcart/QueryProductShopcarts',{UserId:localStorage.getItem('userid')}).then(x=>{
         console.log(x)
         this.tabslist=x.products
     })
   },

   deleterow(id){
     this.$fetch('Shoppingcart/DelProductShopcart',{id}).then(x=>{
        if(x.code==0) this.onload()
        this.$toast(x.msg)
     })
   },
   onSubmit(index){
      //  if(this.ship.id==0){
      //    this.$toast('请拍照上传收货地址等信息，才能提交订单(Prije slanja narudžbe snimite fotografiju i prenesite adresu za dostavu i ostale podatke)')
      //    return;
      //  }
      

      this.showload=true
      console.log(index)
      console.log(this.tabslist[index])
      var row=this.tabslist[index]
      var orderDeTails=[]
      var productcount=0
      row.plist.forEach(e => {
         var patk={id:0,orderid:0,productID:e.proid,quantity:e.num,discount:'4',discountname:'无折扣'}
         productcount+=e.num
         orderDeTails.push(patk)
      });
      var date=new Date();
      var mon=date.getMonth()+1
      var dat=date.getFullYear()+'-'+mon+'-'+date.getDate()+' '+date.getHours()+':'+date.getMinutes()
      var kk= ''
      
      var uk=parseInt(localStorage.getItem('userid'))
      var park={id:0,
      ordername:kk,
      shopid:row.shopid,
      userid:uk,
      shipsid:this.ship.id,
     
      blfalge:0,
      remark:this.message,
      meney:row.meny,
      productcount,
      paymethod:1,
      ordertype:1,
      orderDeTails}
      console.log(park)
      this.$post('Order/AddOrEditOrder',park).then(x=>{
          localStorage.setItem('remark',this.message)
          this.$toast(x.msg)
          this.onload()
          this.showload=false
      })    
   },
   onChange(id,num) {
     var par={id:id,num:num}
     this.$post('Shoppingcart/UpateProductNum',par).then(x=>{
        this.onload()
     })
   },
   onClickLeft(){
     this.$router.go(-1)
   },
  
    afterRead(file){
       console.log(file)
        file.status = 'uploading';
        file.message = '上传中...';
         let config = {
         width: 500, // 压缩后图片的宽
         height: 400, // 压缩后图片的高
         quality: 0.6 // 压缩后图片的清晰度，取值0-1，值越小，所绘制出的图像越模糊
      }
        this.$tools.compressImage(file.file, config).then(img=>{
             this.$post('Ships/UploadImg',{imgbase64:img,userid:localStorage.getItem('userid')}).then(x=>{
           console.log(x)
            this.ship.id=x.data.id
            this.ship.userid=x.data.userid            
            this.ship.images=x.data.images
           file.message="上传完成"
           file.status="done"
           
          
        })
        })
        
     },

      beforeRead(file) {
           return new Promise((resolve) => {
        // compressorjs 默认开启 checkOrientation 选项
        // 会将图片修正为正确方向
        new Compressor(file, {
          success: resolve,
          error(err) {
            console.log(err.message);
          },
        });
      });
      },
     beforedelete(file){
      console.log(file.url)
      var vk=file.url
      if (file.url==undefined) vk= this.ship.images
       this.$fetch('Product/DelImgByUrl',{url:vk}).then(x=>{
         console.log(x)
         this.$toast(x.msg)
       })
      return true
     },
  }
}
</script> 

<style>
 .dd{
   position:fixed;
   top:0px;
   
 }
</style>