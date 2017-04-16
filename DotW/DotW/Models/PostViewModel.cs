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
        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Escritor")]
        public int IdWriter { get; set; }

        [Required]
        [Display(Name = "Cuerpo")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public int IdCategory { get; set; }
    }

    public class EditPostViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Cuerpo")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public int IdCategory { get; set; }
    }

    public class DeletePostViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Categoría")]
        public string CategoryTitle { get; set; }
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