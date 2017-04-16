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

        [Display(Name = "Escritor")]
        public int IdWriter { get; set; }

        [Display(Name = "Cuerpo")]
        public string Body { get; set; }

        [Display(Name = "Fecha de alta")]
        public DateTime EffectDate { get; set; }

        [Display(Name = "Categoría")]
        public int IdCategory { get; set; }

        [Display(Name = "Categoría")]
        public string CategoryTitle { get; set; }
    }
}
