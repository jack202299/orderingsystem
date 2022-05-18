export default{
    getUrlKey: function (name) {
    return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.href) || [, ""])[1].replace(/\+/g, '%20')) || null
    },
    is_arry:function(ary,val){
        var bl=ary.includes(val);
        if(bl) return false;
        return true;
    },
    compressImage:function (file, config) {
        // eslint-disable-next-line no-unused-vars
        let src
        // eslint-disable-next-line no-unused-vars
        let files
        // let fileSize = parseFloat(parseInt(file['size']) / 1024 / 1024).toFixed(2)
        const read = new FileReader()
        read.readAsDataURL(file)
        return new Promise(function (resolve, reject) {
          read.onload = function (e) {
            let img = new Image()
            img.src = e.target.result
            img.onload = function () {
              // 默认按比例压缩
              let w = config.width
              let h = config.height
              // 生成canvas
              let canvas = document.createElement('canvas')
              let ctx = canvas.getContext('2d')
              let base64
              // 创建属性节点
              canvas.setAttribute('width', w)
              canvas.setAttribute('height', h)
              ctx.drawImage(this, 0, 0, w, h)
      
              base64 = canvas.toDataURL(file['type'], config.quality)
      
              // 回调函数返回file的值（将base64编码转成file）
              // files = dataURLtoFile(base64) // 如果后台接收类型为base64的话这一步可以省略
              // 回调函数返回file的值（将base64转为二进制）
              // let fileBinary = dataURLtoBlob(base64)
      
              resolve(base64)
            }
          }
        })
      }
}
