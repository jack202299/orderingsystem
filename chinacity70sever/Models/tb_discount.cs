using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_discount")]
    public class tb_discount
    {
        public int id { get; set; }
        public string Discount { get; set; }

        public string DiscountName { get; set; } 
            
      }
}
