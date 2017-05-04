using Entities.PostEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DotW.Models
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "El campo Título es requerido.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        public HttpPostedFileBase File { get; set; }

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

        public CreatePostViewModel()
        {
            Tags = new List<string>();
        }
    }

    public class EditPostViewModel
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

        public EditPostViewModel()
        {
            Tags = new List<string>();
        }
    }

    public class DeletePostViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Resumen")]
        public string Summary { get; set; }

        [Display(Name = "Categoría")]
        public string CategoryTitle { get; set; }

        [Display(Name = "Estado")]
        public bool IsDraft { get; set; }

        [Display(Name = "Tags")]
        public List<string> Tags { get; set; }
    }

    public class IndexPostViewModel
    {
        public List<Post> Posts { get; set; }
    }

    public class ListPostViewModel
    {
        public List<Post> Posts { get; set; }
    }
}