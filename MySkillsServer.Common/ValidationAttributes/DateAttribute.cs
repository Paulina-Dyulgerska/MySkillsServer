namespace MySkillsServer.Common.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    public class DateAttribute : ValidationAttribute
    {
        private const string DateTimeFormat = "dd/MM/yyyy";

        public DateAttribute(string minDate)
        {
            this.MinDate = DateTime.ParseExact(minDate, DateTimeFormat, CultureInfo.InvariantCulture);
            this.ErrorMessage = $"Date should be between {this.MinDate.Date.ToShortDateString()} " +
                $"and {DateTime.UtcNow.Date.ToShortDateString()}";
        }

        public DateTime MinDate { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue.ToUniversalTime() <= this.MinDate.Date
                    || dateTimeValue.ToUniversalTime() > DateTime.UtcNow.Date)
                {
                    return new ValidationResult(this.ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
