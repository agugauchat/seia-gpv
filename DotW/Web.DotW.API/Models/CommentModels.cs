namespace Web.DotW.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class CommentaryModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int IdWriter { get; set; }
        public string WriterUsername { get; set; }
        public string WriterUrl { get; set; }
        public List<AnswerModel> Answers { get; set; }
    }

    public class AnswerModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int IdWriter { get; set; }
        public string WriterUsername { get; set; }
        public string WriterUrl { get; set; }
    }

    public class CreateCommentaryModel
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
    }

    public class GetCommentsModel
    {
        public List<CommentaryModel> Comments { get; set; }
    }
}