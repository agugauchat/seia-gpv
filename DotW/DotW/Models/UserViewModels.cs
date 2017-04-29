namespace DotW.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DeleteViewModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required]
        public string Id { get; set; }

        [Display(Name = "Roles del usuario")]
        public List<string> Roles { get; set; }
    }

    public class DetailsViewModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Display(Name = "Roles del usuario")]
        public List<string> Roles { get; set; }

        [Display(Name = "Email confirmado")]
        public bool EmailConfirmed { get; set; }
    }

    public class EditViewModel
    {
        [Required(ErrorMessage = "El campo Email es requerido.")]
        [EmailAddress(ErrorMessage = "Ingrese una dirección de correo válida.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Nombre de Usuario es requerido.")]
        [Display(Name = "Nombre de usuario")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Nombre de usuario inválido.")]
        public string UserName { get; set; }

        public string Token { get; set; }
    }
}