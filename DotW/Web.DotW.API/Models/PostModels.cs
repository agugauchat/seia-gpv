namespace Web.DotW.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditPostModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Título es requerido.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        public string base64File { get; set; }

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

        public string base64File { get; set; }

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

    public class GetPostModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Body { get; set; }

        public string ImageUrl { get; set; }

        public CategoryModel Category { get; set; }

        public string CategoryUrl { get; set; }

        public string WriterUrl { get; set; }

        public int WritterId { get; set; }

        public List<string> Tags { get; set; }

        public DateTime EffectDate { get; set; }

        public int GoodVotes { get; set; }

        public int BadVotes { get; set; }

        public GetCommentsModel Comments { get; set; }
    }

    public class VotePostModel
    {
        public bool Good { get; set; }

        public bool Bad { get; set; }
    }

    public class PostComplaintModel
    {
        [Display(Name = "Comentario")]
        public string Commentary { get; set; }
    }
}