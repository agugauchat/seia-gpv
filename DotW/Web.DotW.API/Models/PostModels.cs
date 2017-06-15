namespace Web.DotW.API.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class EditPostModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Título es requerido.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        public HttpPostedFileBase File { get; set; }

        [Display(Name = "Imagen principal")]
        public string ImageName { get; set; }

        [Display(Name = "Imagen cargada")]
        public string PrincipalImageName { get; set; }

        [Required(ErrorMessage = "El campo Resumen es requerido.")]
        [MaxLength(500, ErrorMessage = "El Resumen debe tener como máximo una longitud de 500 caracteres.")]
        [Display(Name = "Resumen")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "El campo Cuerpo es requerido.")]
        [Display(Name = "Cuerpo")]
        public string Body { get; set; }

        [Required(ErrorMessage = "La Categoría es requerida.")]
        [Display(Name = "Categoría")]
        public int IdCategory { get; set; }

        [Display(Name = "Es borrador")]
        public bool IsDraft { get; set; }

        [Display(Name = "Tags")]
        public List<string> Tags { get; set; }

        [Display(Name = "Publicación sin imagen")]
        public bool DeleteImage { get; set; }

        public EditPostModel()
        {
            Tags = new List<string>();
        }
    }

    public class CreatePostModel
    {
        [Required(ErrorMessage = "El campo Título es requerido.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        public HttpPostedFileBase File { get; set; }

        public string base64 { get; set; }

        [Display(Name = "Imagen principal")]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "El campo Escritor es requerido.")]
        [Display(Name = "Escritor")]
        public int IdWriter { get; set; }

        [Required(ErrorMessage = "El campo Resumen es requerido.")]
        [MaxLength(500, ErrorMessage = "El Resumen debe tener como máximo una longitud de 500 caracteres.")]
        [Display(Name = "Resumen")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "El campo Cuerpo es requerido.")]
        [Display(Name = "Cuerpo")]
        public string Body { get; set; }

        [Required(ErrorMessage = "La Categoría es requerida.")]
        [Display(Name = "Categoría")]
        public int IdCategory { get; set; }

        [Display(Name = "Es borrador")]
        public bool IsDraft { get; set; }

        [Display(Name = "Tags")]
        public List<string> Tags { get; set; }

        public CreatePostModel()
        {
            Tags = new List<string>();
        }
    }
}