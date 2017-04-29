namespace DotW.Models
{
    using Entities.CategoryEntities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage ="El campo Título es requerido.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El campo Descripción es requerido.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo Resumen es requerido.")]
        [MaxLength(500, ErrorMessage = "El Resumen debe tener como máximo una longitud de 500 caracteres.")]
        [Display(Name = "Resumen")]
        public string Summary { get; set; }

        [Display(Name = "Categoría Superior")]
        public int? IdUpperCategory { get; set; }
    }

    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Título es requerido.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El campo Descripción es requerido.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo Resumen es requerido.")]
        [MaxLength(500, ErrorMessage = "El Resumen debe tener como máximo una longitud de 500 caracteres.")]
        [Display(Name = "Resumen")]
        public string Summary { get; set; }

        [Display(Name = "Categoría Superior")]
        public int? IdUpperCategory { get; set; }
    }

    public class DeleteCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Título es requerido.")]
        [Display(Name = "Título")]
        public string Title { get; set; }
    }

    public class IndexCategoryViewModel
    {
        public List<Category> Categories { get; set; }
    }

    public class ListCategoriesViewModel
    {
        public List<Category> Categories { get; set; }
    }

    public class DetailsCategoryViewModel
    {
        [Required(ErrorMessage = "El campo Título es requerido.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El campo Descripción es requerido.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo Resumen es requerido.")]
        [MaxLength(500, ErrorMessage = "El Resumen debe tener como máximo una longitud de 500 caracteres.")]
        [Display(Name = "Resumen")]
        public string Summary { get; set; }

        [Display(Name = "Categoría Superior")]
        public int? IdUpperCategory { get; set; }

        [Display(Name = "Categoría Superior")]
        public string TitleUpperCategory { get; set; }
    }
}