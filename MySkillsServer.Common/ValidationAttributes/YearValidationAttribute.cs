namespace MySkillsServer.Common.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class YearValidationAttribute : ValidationAttribute
    {
        public YearValidationAttribute(int minYear)
        {
            this.MinYear = minYear;
            this.ErrorMessage = $"Value should be between {this.MinYear} and {DateTime.UtcNow.Year}";
        }

        public int MinYear { get; }

        public override bool IsValid(object value)
        {
            if (value is int intValue)
            {
                if (intValue <= DateTime.UtcNow.Year
                    && intValue >= this.MinYear)
                {
                    return true;
                }
            }

            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue.Year <= DateTime.UtcNow.Year
                    && dateTimeValue.Year >= this.MinYear)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
