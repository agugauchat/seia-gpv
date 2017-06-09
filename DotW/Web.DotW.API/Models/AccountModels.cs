namespace Web.DotW.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class RegisterModel
    {
        [Required(ErrorMessage = "El campo Email es requerido.")]
        [EmailAddress(ErrorMessage = "Ingrese una dirección de correo válida.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Nombre de Usuario es requerido.")]
        [Display(Name = "Nombre de usuario")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Nombre de usuario inválido.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es requerido.")]
        [StringLength(100, ErrorMessage = "La {0} debe tener como mínimo una longitud de {2} caracteres.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y su confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class Prueba
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Prueba2
    {
        public int Id2 { get; set; }

        public Prueba objeto { get; set; }
    }
}