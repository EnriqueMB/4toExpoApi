

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace _4toExpoApi.DataAccess.Entities
{
    public class RedPatrocinador
    {
        [Key]
        public int IdRedPatrocinador { get; set; }
        public int IdPatrocinador { get; set; }
        public int IdRedSocial { get; set; }
        public string UrlRedSocial { get; set; }
        public int IdBanner {  get; set; }


        [ForeignKey("IdRedSocial")]
        public RedSocial? Red {  get; set; }
        [ForeignKey(nameof(IdBanner))]
        public Banner? Banner { get; set; }

        
    }
}
