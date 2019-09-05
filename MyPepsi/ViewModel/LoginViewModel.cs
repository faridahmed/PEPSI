using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPepsi.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User ID")]
        public int UserID { get; set; }
        //[Display(Name = "User Name")]
       // public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
    public class LoginModels
    {
        [Key]
        [Required]
        [Display(Name = "User ID")]
        public int UserID { get; set; }
        //  [Required(ErrorMessage = "Please enter the User Name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter the Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int? UserRoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class MenuModels
    {
        [Key]
        public string MainMenuName { get; set; }
        public int MainMenuId { get; set; }
        public string SubMenuName { get; set; }
        public int SubMenuId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int? RoleId { get; set; }
        public int? UserID { get; set; }
        public string RoleName { get; set; }
    }
    public class UserCreateVM
    {
        [Key]
        [Display(Name = "User ID")]
        public int UserID { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
      //  [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string MobileNo { get; set; }
       //  public string Status { get; set; }
        public Nullable<int> WorkStationID { get; set; }
        public string MachineIP { get; set; }
        public string Email { get; set; }
    }
}
