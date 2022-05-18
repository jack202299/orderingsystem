<template>
  <div>
         <van-nav-bar fixed
  title="添加店铺"
  left-text="返回"  
  left-arrow
  @click-left="onClickLeft"  
/>


    <van-form @submit="onSubmit">
      

   <van-field name="uploader" label="店铺照片">
              <template #input>
                <van-uploader v-model="uploader" :after-read="afterRead" :before-read="beforeRead"  max-count="1" capture="camera" />
              </template>
            </van-field>
  <van-field
    v-model="par.shopname"
    name="店铺名称"
    label="店铺名称"
    placeholder="店铺名称"
    :rules="[{ required: true, message: '请填写店铺名称' }]"
  />
  <van-field
    v-model="par.shopaddress"    
    name="店铺地址"
    label="店铺地址"
    placeholder="店铺地址"
    :rules="[{ required: true, message: '请填写店铺地址' }]"
  />
  <van-field
    v-model="par.tel"    
    name="联系电话"
    label="联系电话"
    placeholder="联系电话"
    :rules="[{ required: true, message: '请填写联系电话' }]"
  />
  <van-field
    v-model="par.person"    
    name="联系人"
    label="联系人"
    placeholder="联系人"
    :rules="[{ required: true, message: '请填写联系人' }]"
  >
  <template #button>
    <van-button size="small" type="primary" @click="show=true">选择联系人</van-button>
  </template>
  </van-field>
 <van-field
  readonly
  clickable
  name="calendar"
  :value="par.lastDate"
  label="到期日期"
  placeholder="点击选择日期"
  @click="showCalendar = true"
/>
<van-calendar :min-date="mind" v-model="showCalendar" @confirm="onConfirm" />

 <van-dropdown-menu direction="up">
    <van-dropdown-item v-model="par.kindid" :options="option1" @change="onchange" />
    <!-- <van-dropdown-item v-model="value2" :options="option2" /> -->
   </van-dropdown-menu>
  <van-radio-group v-model="par.shoptypid" direction="horizontal">
  <van-radio v-for="(item,index) in shoptypelist" :key="index" 
  :name="item.id"><span>{{item.typename}}({{item.icount}})</span></van-radio>
 
  </van-radio-group>

  <div style="margin: 16px;">
    <van-button round block type="info" native-type="submit">提交</van-button>
  </div>
</van-form>

<van-dialog v-model="show" title="标题" show-cancel-button>
  <form action="/">
  <van-search
    v-model="username"
    show-action
    placeholder="请输入搜索关键词"
    @search="onSearch"
    
  />
</form>
  <van-contact-list
  v-model="par.userid"
  :list="list"
  default-tag-text="默认"  
  add-text="提交"
  @add="onadd"
  @select="onselect"
/>
</van-dialog>


  </div>
</template>

<script>
import Compressor from 'compressorjs';
export default {
   name:'myshop',
   data(){
      return{
       option1:[],
       mind:new Date(2020,1,1),
       showCalendar:false,
       uploader:[],
       show:false,
       list:[],
       username:'',
       shoptypelist:[],
       par:{
         id:0,
         userid:0,        
         shopname:'',
         shopaddress:'',
         tel:'',
         person:'',
         usdays:0,
         lastDate:'',
         kindid:0,
         shoptypid:0,
         shopimg:''
       }
      }
   },
  mounted(){
       if(!sessionStorage.getItem("token")) this.$router.replace({path:'/index'})
       this.getkindslist()
       this.getshoptype()
      var bb=this.$route.query.b
      if(bb=="add")
         {
            
         }
      else{
         console.log(bb)
         this.par.id=this.$route.query.shopid
         this.$fetch('Shop/QueryShopInfWithID',{Id:this.par.id}).then(r=>{
           if(r.code==1){
               this.par.shopname=r.data.shopname
               this.par.userid=r.data.userid
               this.par.shopaddress=r.data.shopaddress
               this.par.tel=r.data.tel
               this.par.person=r.data.person
               this.par.lastDate=r.data.lastDate
               this.par.kindid=r.data.kindid
               this.par.shopimg=r.data.shopimg
               this.par.shoptypid=r.data.shoptypid
               this.uploader.push({url:r.data.shopimg})
           }
         })
      }
   },
   methods:{
     getshoptype(){
       this.$fetch('ShopType/QueryShopTypeList').then(r=>{
           this.shoptypelist=r.data;
       })
     },
     onConfirm(date){
        this.par.lastDate= `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;   
        this.showCalendar=false     
     },
     onSearch(val){
        this.$post('User/QueryUsers',{username:this.username}).then(r=>{
           if(r.length>0){
             r.forEach(item=>{
                var ji={id:item.id,name:item.name,tel:item.useremail}
                this.list.push(ji)
             })
           }
        })
     },
     onSubmit(values) {
      console.log('submit', values);
       this.$post('Shop/AddOrEditShop',this.par).then(r=>{
          this.$toast(r.msg)
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
         width: 400, // 压缩后图片的宽
         height: 400, // 压缩后图片的高
         quality: 0.6 // 压缩后图片的清晰度，取值0-1，值越小，所绘制出的图像越模糊
      }
        this.$tools.compressImage(file.file, config).then(img=>{
             this.$post('Shop/UploadImg',{imgbase64:img,shopid:this.par.shopid}).then(x=>{
           console.log(x)
           this.par.shopimg=x.imgurl
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
        onchange(value) {
         this.par.kindid=value
       
       },
       onadd(){
         this.show=false
         
       },
       onselect(Contact){
         console.log(Contact)
         this.par.person=Contact.name
       }
   }
}
</script>

<style>

</style>