using System.ComponentModel.DataAnnotations;

namespace MultiVendorECommerce.Attribute
{
    public class MinFilesCountAttribute : ValidationAttribute
    {
        private readonly int _min;

        public MinFilesCountAttribute(int min)
        {
            _min = min;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var files = value as IList<IFormFile>;
            if (files == null || files.Count < _min)
                return new ValidationResult($"At least {_min} image is required");

            return ValidationResult.Success;
        }
    }

}
