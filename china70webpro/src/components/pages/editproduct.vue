<template>
   <div>
         <van-nav-bar fixed
      :title="$t('edp.title')"
      :left-text="$t('edp.backtl')"
     
      left-arrow
      @click-left="onClickLeft"
    
    />

     <van-form validate-first @submit="onSubmit">             
             
            <van-field :label="$t('edp.productcategory')">
                <template #input>
                       <van-dropdown-menu>
                  <van-dropdown-item v-model="par.productType" :options="option1" />
                  
                </van-dropdown-menu>
                </template>
                
            </van-field>
            <van-field name="uploader" :label="$t('edp.Photograph')">
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
             <van-field
              v-model="par.barcode"
              :name="$t('edp.barcode')"
              :label="$t('edp.barcode')"
              placeholder="条码"
              :rules="[{ required: true, message: '请填写条码' }]"
              
              clearable
              right-icon="exchange"
              @click-right-icon="ongetbarcode"
            />

           <van-field
              v-model="par.productName"
              name="产品名"
              :label="$t('edp.productname')"
              :placeholder="$t('edp.productname')"
              :rules="[{ required: true, message: '请填写产品名' }]"
              clickable
              clearable
              right-icon="exchange"
              @click-right-icon="ontraleg"
            >
             </van-field>
               <van-field
              v-model="par.alias"
              :name="$t('edp.alias')"
              :label="$t('edp.alias')"
              placeholder="别名"
              :rules="[{ required: true, message: '请填写别名' }]"
              clickable
              clearable
            />

         
               <van-field
              v-model="par.price"
              name="零售价"
              :label="$t('edp.price')"
              :placeholder="$t('edp.price')"
              :rules="[{ required: true, message: '请填写零售价' }]"
              clickable
              clearable
            /> 
                <van-field
              v-model="par.bulkprice"
              name="批发价"
              :label="$t('edp.bulkprice')"
              :placeholder="$t('edp.bulkprice')"
              :rules="[{ required: true, message: '请填写批发价' }]"
              clickable
              clearable
            /> 
            
             <van-field
              v-model="par.reserve"
              name="库存"
              :label="$t('edp.reserve')"
              :placeholder="$t('edp.reserve')"
              :rules="[{ required: true, message: '请填写库存' }]"
              clickable
              clearable
            /> 
             
               <van-field :label="$t('edp.positionid')">
                <template #input>
                       <van-dropdown-menu>
                  <van-dropdown-item v-model="par.positionid" :options="option2" />
                  
                </van-dropdown-menu>
                </template>
                
            </van-field>
             <van-field
              v-model="par.productId"
              name="货号"
              :label="$t('edp.productId')"
              placeholder="货号"
              :rules="[{ required: true, message: '请填写货号' }]"
              clickable
              clearable
            /> 


            <van-field
            v-model="par.unit"
            is-link
            readonly
            :label="$t('edp.unit')"
            :error="$t('edp.unit')"
            @click="show2 = true"
            />
            <van-popup v-model="show2" position="bottom">
              <van-picker show-toolbar
                :columns="columns2"
                @confirm="onConfirm"
                @cancel="show2 = false"
              />
            </van-popup>
           <div style="margin: 16px;">
    <van-button round block type="info" native-type="submit">{{$t('edp.subtxt')}}</van-button>
  </div>
      </van-form> 
   </div>
</template>

<script>
import Compressor from 'compressorjs';
import axios from 'axios';
export default {
    name:'editproduct',
    data(){
        return {
          option1:[],
          option2:[],
         columns:[],
         columns1:[],
         columns2:[],
         show:false,
         show1:false,
         show2:false,
         cascaderValue:'',
         cascaderValue1:'',
         cascaderValue2:'',
         fieldValue:'',
         uploader:[],
         par:{
           id:0,         
           shopid:0,
           images:'',
           productName:'',
           alias:'',
           barcode:'',
           productId:'',
           reserve:0,
           positionid:0,
           bulkprice:0,
           price:0,
           unit:'',
           isdown:0,
           hotsort:false,
           sellcount:0,
           createdate:'',
           createby:0,
           productType:0,
         }
        }
    },
 
  
    mounted(){
       if(!sessionStorage.getItem("token")) this.$router.replace({path:'/index'})
       var b=this.$route.query.b
         this.par.shopid=this.$route.query.shopid
       this.getproducttype()
       this.getunits();
       this.getpostion()
       if(b=="add"){
        // this.getshops()  
        this.par.shopid=this.$route.query.shopid
         var date=new Date()
        var y=date.getFullYear()
        var m=date.getMonth()+1
        var d=date.getDate()
        var nowdate=y+'-'+m+'-'+d
        this.par.createdate=nowdate
        this.par.createby=localStorage.getItem('userid')
       }else{
        var kid=this.$route.query.rowid
        console.log(kid)
        this.$fetch('Product/QueryProductInfo',{id:kid}).then(r=>{
          if(r.code==1){
            Object.assign(this.par,r.data)           
            this.uploader.push({url:r.data.images})
           
          }
        })
       }
    },
    methods:{
         getunits(){
           this.columns2=[]
         this.$fetch('Unit/QueryUnitList').then(x=>{
         console.log(x)
          x.forEach(r=>{
            console.log(r)
            this.columns2.push(r.unitname)
          })
       })
     },
     getpostion(){
        this.$fetch('Position/QueryPositions',{shopid:this.par.shopid}).then(x=>{
           this.option2=x
        })
     },
     ongetbarcode(){
        this.$fetch('Product/GetNumberWithDate').then(x=>{
          this.par.barcode=x.number
        })
     },
      getproducttype(){
        this.option1=[]
         this.par.shopid=this.$route.query.shopid
         console.log(this.par.shopid)
        this.$fetch('ProductType/QueryProductTypeListByShopId',{shopid: this.par.shopid}).then(x=>{
            console.log(x)
            x.data.forEach(r=>{
              var vk={text:r.protype,value:r.id}
              this.option1.push(vk)
            })
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
             this.$post('Product/UploadImg',{imgbase64:img,shopid:this.par.shopid}).then(x=>{
           console.log(x)
           this.par.images=x.imgurl
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
      if (file.url==undefined) vk=this.par.images
       this.$fetch('Product/DelImgByUrl',{url:vk}).then(x=>{
         console.log(x)
         this.$toast(x.msg)
       })
      return true
     },
      onSubmit(values) {
      console.log('submit', values);
       this.$post('Product/ProductAddOrEdit',this.par).then(x=>{
            this.par.images=''
            this.uploader=[]
            this.par.productName=''
            this.par.price=''
          this.$toast(x.msg)
       })
     },
     onConfirm(value) {
      this.par.unit = value;
      this.showPicker = false;
      this.show2=false
    },
    ontraleg(){
      if(this.par.productName==''){
        this.$toast('没有内容可翻译')
        return
      }
     
      this.$fetch('Product/CNtoHR',{txt:this.par.productName}).then(x=>{
         if(x.code==0) this.par.alias=x.result
      })
    }
    }
}
</script>

<style>

</style>