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
        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Categoría Superior")]
        public int? IdUpperCategory { get; set; }
    }

    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Categoría Superior")]
        public int? IdUpperCategory { get; set; }
    }

    public class DeleteCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
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
        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Categoría Superior")]
        public int? IdUpperCategory { get; set; }

        [Display(Name = "Categoría Superior")]
        public string TitleUpperCategory { get; set; }
    }
}