using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
  
    [Table("tb_unit")]
    public class tb_unit
    {
      
        public int id { get; set; }
        public string unitname { get; set; }
    }
}
