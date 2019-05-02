using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DunnhumbyHomeWork.DTOModel
{
    public class CampaignDTO
    {
        [StringLength(36)]
        [Required]
        public string Id { get; set; }

        [StringLength(256)]
        [Required]
        public string Name { get; set; }

        [Required]
        public ProductDTO Product { get; set; }

        [Required]
        public string StartDate { get; set; }

        [Required]
        public string EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
