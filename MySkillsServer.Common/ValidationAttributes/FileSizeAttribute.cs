namespace MySkillsServer.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class FileSizeAttribute : ValidationAttribute
    {
        public FileSizeAttribute(long size)
        {
            this.Size = size;
            this.ErrorMessage = $"File should have max Size of: {size} B";
        }

        public long Size { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var hasRightSize = ((IFormFile)value).Length <= this.Size;

            if (hasRightSize is true)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
