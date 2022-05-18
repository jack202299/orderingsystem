using chinacity70sever.Models;

namespace chinacity70sever.BLL
{

    public class prodctinfoParam
    {
        public int userid { get; set; }
        public int proid { get; set; }

        public int shopid { get; set; }
    }

    public class productShopcartparam
    {
        public int Id { get; set; }
        public int num { get; set; }
    }
    public class productParam
    {
        public int shopid { get; set; }

        public int producttypeid { get; set; }
        public string ProductName { get; set; }

        public int page { set; get; }

        public int pageSize { set; get; }
    }
    public class ProductQueryKindsParam
    {
        public int kindsid { get; set; }

        public string productname { get; set; }
        public int page { set; get; }
        public int pagesize { set; get; }
    }

    public class uploadprama
    {
        public int shopid { get; set; }
        public string imgbase64 { get; set; }
    }
    public class uploaduserimageprama
    {
        public int userid { get; set; }
        public string imgbase64 { get; set; }
    }
    public class UserLoginParam
    {
        public string useremail { get; set;}
        public string Password { get; set;}
    }

    public class EditPwdParam
    {
        public int UserId { set; get;  }

        public string oldpwd { set; get; }

        public string newpwd { set; get; }
    }

    public class QueryUserParam
    {
        public string username { set; get; }
    }
        public class QueryShopListParam
        {
            public int kindsid { get; set; }
            public string shopname { set; get; }
            public int page { set; get; }
            public int pagesize { set; get; }
        }

        public class orderparam
        {
            public int id { set; get; }
            public string ordername { set; get; }

            public int shopid { set; get; }
            public int userid { set; get; }
            public int shipsid { set; get; }
           
            public int blfalge { set; get; }
            public string Remark { set; get; }
            public int productcount { set; get; }

            public int paymethod { set; get; }
            public int ordertype { set; get; }
            public int meney { set; get; }
            public List<tb_orderdetail> orderDeTails { set; get; }
        }
       
        public class orderAddParam
        {
              public int id { set; get; }
             public string ordername { set; get; }
              public int shopid { set; get; }
             public int productcount { set; get; }
             public decimal meney { set; get; }
             public decimal? actuallyreceived { set; get; }
             public int userid { set; get; }
             public int ordertype { set; get; }
             public List<tb_orderdetail> orderDeTails { set; get; }
         }


        public class OrderListWithShopIdParam
        {
            public string orderDate { set; get; }
            public int shopid { set; get; }

            public int blfalge { set; get; }

        }
        
        public class OrderQueryByMonthPararm
        {
            public string strd { set; get; }

            public string endd { set; get; }
            public int shopid { set; get; }
            
            public string txt { set; get; }
            public int page { set; get; }

            public int pagesize { set; get; }
          
           }
          
         public class getProductsByNameParam
         {
            public string proname { set; get; }
            public int shopid { set; get; }

            
         }
        public class OrderListWithUserIdParam
        {
            public string orderDate { set; get; }
            public int userId { set; get; }

            public int blfalge { set; get; }


        }
        public class idparam
        {
            public List<idsparam> idsparams { set; get; }
        }
        public class idsparam
        {
            public int id { set; get; }
            public int quty { set; get; }
        }

        public class queryproductParam
        {
            public string productpar { set; get; }
            public int shopid { set; get; }
            public int page { set; get; }
            public int pagesize { set; get; }

        }

        public class OrderStatusParam
        {
            public int OrderId { set; get; }
            public int blfage { set; get; }
        }
       

        public class QueryProductUpload
        {
           public int shopid { set; get; }
           public string[] querydate { set; get; }
        }

         public class UpdateAddressParam
         {
               public int id { set; get; }
               public int userid { set; get; }
               
               public bool bl { set; get; }
          }

         public class QueryMyClientageParam
         {
            public int shopid { set; get; }
            public string strname { set; get; }

          }
       
        public class QueryShopUserParam
        {
           public int shopid { set; get; }
           public int page { set; get; }
           public int pagesize { set; get; }

         }
        
        public class AddorEditPermissionParam
        {
           public int userid { set; get; }
           public List<string> vs { set; get; }
        }

        public class checkPerssionParam
        {
           public int userid { set; get; }
           public string guid { set; get; }
        }

        public class updateRowParam
        {
           public int orderid { set; get; }
           public int paystatus { set; get; }
           public int blfalge { set; get; } = 0;
        }
         
         public class queryuserParam
         {
            public string? custtxt { set; get; }
            
            public int page { set; get; }

            public int pagesize { set; get; }

         }
       
        public class queryPostionParam
        {
        public string? custtxt { set; get; }

        public int shopid { set; get; }
        public int page { set; get; }

        public int pagesize { set; get; }
         }

        public class queryProductParam
        {
           public int shopid { set; get; }
           public string barcode { set; get; } 
        }
        public class field
        {
           


        }
    
}
