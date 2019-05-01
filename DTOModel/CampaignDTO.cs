#region Auto generated information. Please do not modify

// DunnhumbyHomeWork DunnhumbyHomeWork CampaignDTO.cs
// BILA007 Bilangi, Vivek-Vardhan, IT Collection International
// 2019-04-28 22:25
// 2019-04-28 21:30

#endregion

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
