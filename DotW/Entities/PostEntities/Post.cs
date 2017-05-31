namespace Entities.PostEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Post
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Resumen")]
        public string Summary { get; set; }

        [Display(Name = "Escritor")]
        public int IdWriter { get; set; }

        [Display(Name = "Escritor")]
        public string WriterUserName { get; set; }

        [Display(Name = "Cuerpo")]
        public string Body { get; set; }

        [Display(Name = "Fecha de alta")]
        public DateTime EffectDate { get; set; }

        [Display(Name = "Categoría")]
        public int IdCategory { get; set; }

        [Display(Name = "Categoría")]
        public string CategoryTitle { get; set; }

        [Display(Name = "Es borrador")]
        public bool IsDraft { get; set; }

        [Display(Name = "Imagen Principal")]
        public string PrincipalImageName { get; set; }

        public List<string> Tags { get; set; }

        public DateTime? NullDate { get; set; }
    }
}
