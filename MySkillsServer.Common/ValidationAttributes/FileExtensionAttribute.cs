namespace MySkillsServer.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class FileExtensionAttribute : ValidationAttribute
    {
        public FileExtensionAttribute(string extension)
        {
            this.Extension = extension;
            this.ErrorMessage = $"Invalid file extension. Only {this.Extension} files are allowed.";
        }

        public string Extension { get; }

        public string ValueExtension { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var hasRightExtension = ((IFormFile)value).FileName.ToLower().EndsWith($".{this.Extension}");

            if (hasRightExtension == true)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
