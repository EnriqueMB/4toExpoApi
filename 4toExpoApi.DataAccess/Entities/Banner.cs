
using System.ComponentModel.DataAnnotations.Schema;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Banner
    {
        public int Id { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? Descripcion { get; set; }
        public string? UrlVideo { get; set; }
        public int IdPatrocinador {  get; set; }
      
        public virtual ICollection<RedPatrocinador> RedPatrocinador { get; set; }

    }
}
