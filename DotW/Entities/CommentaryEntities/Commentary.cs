namespace Entities.CommentaryEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Commentary
    {
        public int Id { get; set; }

        public int IdPost { get; set; }

        public int IdUser { get; set; }

        [Display(Name = "Autor")]
        public string WriterUserName { get; set; }

        [Display(Name = "Comentario")]
        [MaxLength(250)]
        public string CommentaryText { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime EffectDate { get; set; }

        [Display(Name = "Fecha de baja")]
        public DateTime? NullDate { get; set; }
    }
}
