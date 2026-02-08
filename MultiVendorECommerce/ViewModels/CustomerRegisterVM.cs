using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuz.ViewModels
{
    public class CustomerRegisterVM : RegisterVM
    {
        [Required,MaxLength(50)]
        public string FirstName { get; set; } = null!;


        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;
    }

}
