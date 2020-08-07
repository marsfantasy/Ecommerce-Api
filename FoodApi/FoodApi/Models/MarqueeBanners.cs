using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace FoodApi.Models
{
    public class MarqueeBanner
    {   
        public int Id { get; set; }
        public string Title { get; set; }
        public string BannerContent { get; set; }
        public string Image { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsEnable { get; set; }
        public int Seq { get; set; }
        
        [NotMapped]
        public byte[] ImageArray { get; set; }
    }
}
