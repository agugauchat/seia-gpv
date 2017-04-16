namespace Entities.CategoryEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Fecha de baja")]
        public DateTime? NullDate { get; set; }

        [Display(Name = "Categoría Padre")]
        public int? IdUpperCategory { get; set; }
    }
}
