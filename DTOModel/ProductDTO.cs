using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DunnhumbyHomeWork.DTOModel
{
    public class ProductDTO
    {
        [StringLength(36)]
        [Required]
        public string Id { get; set; }

        [StringLength(256)]
        [Required]
        public string Name { get; set; }

        [StringLength(32)]
        [Required]
        public string Category { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
