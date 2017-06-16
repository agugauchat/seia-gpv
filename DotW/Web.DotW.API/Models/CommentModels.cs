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
        [Required(ErrorMessage = "El campo IdPost es requerido.")]
        public int IdPost { get; set; }

        [Required(ErrorMessage = "El campo Resumen es requerido.")]
        [MinLength(1, ErrorMessage = "El Resumen debe tener como mínimo una longitud de 1 carácter.")]
        [MaxLength(250, ErrorMessage = "El Resumen debe tener como máximo una longitud de 250 caracteres.")]
        public string TextComment { get; set; }

        public int? IdUpperComment { get; set; }
    }

    public class GetCommentsModel
    {
        public List<CommentaryModel> Comments { get; set; }
    }
}