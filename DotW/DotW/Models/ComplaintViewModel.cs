namespace DotW.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class ComplaintPostViewModel
    {
        [Required(ErrorMessage = "El campo Comentario es obligatorio")]
        [Display(Name = "Comentario")]
        public string Commentary { get; set; }

        [Required]
        public int PostId { get; set; }
    }
}