<template>
  <div>
      <van-nav-bar
      title="标题"
      left-text="返回"
      right-text="提交"
      left-arrow
      @click-left="onClickLeft"
      @click-right="onClickRight"
    />
       <van-form @submit="onSubmit">
          <van-field
            v-model="par.shopid"
            is-link
            readonly
            label="店铺"
            placeholder="请选择所在店铺"
            @click="show = true"
            />
            <van-popup v-model="show" round position="bottom">
            <van-cascader
                v-model="cascaderValue"
                title="请选择所在店铺"
                :options="columns"
                @close="show = false"
                @finish="onFinish1"
            />
            </van-popup>
            
             <van-field
            v-model="par.kindsid"
            is-link
            readonly
            label="种类"
            placeholder="请选择所在种类"
            @click="show1 = true"
            />
            <van-popup v-model="show1" round position="bottom">
            <van-cascader
                v-model="cascaderValue1"
                title="请选择所在种类"
                :options="columns1"
                @close="show1 = false"
                @finish="onFinish2"
            />
            </van-popup>

            <van-field name="uploader" label="文件上传">
              <template #input>
                <van-uploader v-model="uploader" :after-read="afterRead" max-count="1" capture="camera" />
              </template>
            </van-field>
           <van-field
              v-model="par.productName"
              name="产品名"
              label="产品名"
              placeholder="产品名"
              :rules="[{ required: true, message: '请填写产品名' }]"
              clickable
            />
               <van-field
              v-model="par.price"
              name="价格"
              label="价格"
              placeholder="价格"
              :rules="[{ required: true, message: '请填写价格' }]"
              clickable
            /> 

            <van-field
            v-model="par.unit"
            is-link
            readonly
            label="单位"
            placeholder="请选择单位"
            @click="show2 = true"
            />
            <van-popup v-model="show2" position="bottom">
              <van-picker show-toolbar
                :columns="columns2"
                @confirm="onConfirm"
                @cancel="show2 = false"
              />
            </van-popup>
              <van-field name="checkbox" label="推荐">
              <template #input>
                <van-checkbox v-model="par.hotsort" shape="square" >是否推荐</van-checkbox>
              </template>
            </van-field>
      </van-form> 
  </div>

</template>

<script>
export default {
   name:'aeproduct',
   data(){
       return{
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
           kindsid:0,
           shopid:0,
           images:'',
           productName:'',
           price:0,
           unit:'',
           isdown:0,
           hotsort:0,
           sellcount:0
         }
       }
   },
   created(){
       var b=this.$route.query.b
       this.getunits();
       if(b=="add"){
         this.getshops()  
         this.getkinds()
       }else{
        var kid=this.$route.query.rowid
        console.log(kid)
        this.$fetch('Product/QueryProductInfo',{id:kid}).then(r=>{
          if(r.code==1){
            this.par.id=r.data.id
            this.par.kindsid=r.data.kindsid
            this.par.shopid=r.data.shopid
            this.par.images=r.data.images
            this.uploader.push(r.data.images)
            this.par.productName=r.data.productName
            this.par.price=r.data.price
            this.par.isdown=r.data.isdown
            this.par.hotsort=r.data.hotsort
            this.par.sellcount=r.data.sellcount
            this.par.unit=r.data.unit
          }
        })
       }
   },
   methods:{
     getunits(){
       this.$fetch('Unit/QueryUnitList').then(x=>{
         console.log(x)
          x.forEach(r=>{
            console.log(r)
            this.columns2.push(r.unitname)
          })
       })
     },


     afterRead(file){
       console.log(file)
        file.status = 'uploading';
        file.message = '上传中...';
        this.$post('Product/UploadImg',{imgbase64:file.content,shopid:this.par.shopid}).then(x=>{
           console.log(x)
           this.par.images=x.imgurl
           file.message="上传完成"
           file.status="done"
        })
     },
     getshops(){
       this.$fetch('Shop/QueryShopListAll').then(x=>{
         console.log(x)
         x.forEach(element => {
            var kk={text:element.shopname,value:element.id}
            this.columns.push(kk)
         });
       })
     },
     getkinds(){
       this.$fetch('Kinds/QuerKindsList').then(x=>{
          console.log(x)
          x.forEach(r=>{
             var kk={text:r.kindsname,value:r.id};
             this.columns1.push(kk)
          })
       })
     },
     onClickLeft(){
           this.$router.go(-1)
     },
     onClickRight(){

       this.$post('Product/ProductAddOrEdit',this.par).then(x=>{
          this.$toast(x.msg)
       })
     },
     onFinish2({seloptin}){
       this.show1=false;
       console.log(this.cascaderValue1)
       this.par.kindsid=this.cascaderValue1
     },
      onFinish1({ selectedOptions }) {
      console.log(selectedOptions)
      this.show = false;
      this.par.shopid = selectedOptions.map((option) => option.value).join('/');
      console.log(this.par.shopid)
    },
    
    onConfirm(value) {
      this.par.unit = value;
      this.showPicker = false;
      this.show2=false
    },
    onSubmit(values) {
      console.log('submit', values);
    },
   }
}
</script>

<style>

</style>