namespace DotW.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DeleteViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public string Id { get; set; }

        public List<string> Roles { get; set; }
    }

    public class DetailsViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}