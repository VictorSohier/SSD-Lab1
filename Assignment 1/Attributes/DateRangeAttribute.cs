using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_1.Attributes
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private Nullable<DateTimeOffset> LeadingEdge { get; set; }
        private Nullable<DateTimeOffset> FallingEdge { get; set; }

        public override bool RequiresValidationContext { get; } = false;

        public DateRangeAttribute(
            string edge,
            string errorMessage,
            bool leading = true)
        {
            if (leading)
            {
                LeadingEdge = edge == "now" ? DateTimeOffset.Now : DateTimeOffset.Parse(edge);
                ErrorMessage = string.IsNullOrEmpty(errorMessage) ? "{0} must fall after {1}" : errorMessage;
            }
            else
            {
                FallingEdge = edge == "now" ? DateTimeOffset.Now : DateTimeOffset.Parse(edge);
                ErrorMessage = string.IsNullOrEmpty(errorMessage) ? "{0} must fall before {1}" : errorMessage;
            }
        }

        public DateRangeAttribute(
            string edge,
            bool leading = true)
        {
            if (leading)
            {
                LeadingEdge = edge == "now" ? DateTimeOffset.Now : DateTimeOffset.Parse(edge);
                ErrorMessage = "{0} must fall after {1}";
            }
            else
            {
                FallingEdge = edge == "now" ? DateTimeOffset.Now : DateTimeOffset.Parse(edge);
                ErrorMessage = "{0} must fall before {1}";
            }
        }

        public DateRangeAttribute(
            string leadingEdge,
            string fallingEdge,
            string errorMessage = "{0} must fall between {1} and {2}")
            : base(errorMessage)
        {
            LeadingEdge = leadingEdge == "now" ? DateTimeOffset.Now : DateTimeOffset.Parse(leadingEdge);
            FallingEdge = fallingEdge == "now" ? DateTimeOffset.Now : DateTimeOffset.Parse(fallingEdge);
        }

        public override bool IsValid(object value)
        {
            bool isValid = true;
            DateTimeOffset dtoValue;

            if (!(value is DateTimeOffset || value is DateTime))
                if (value == null)
                    return true;
                else
                    return false;
            else if (value is DateTime)
                dtoValue = new DateTimeOffset((DateTime) value);
            else
                dtoValue = (DateTimeOffset)value;

            if (LeadingEdge.HasValue)
                isValid = LeadingEdge < dtoValue;

            if (FallingEdge.HasValue)
                isValid = isValid && FallingEdge > dtoValue;

            return isValid;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isValid = true;
            DateTimeOffset dtoValue;

            if (!(value is DateTimeOffset || value is DateTime))
                if (value == null)
                    return null;
                else
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else if (value is DateTime)
                dtoValue = new DateTimeOffset((DateTime) value);
            else
                dtoValue = (DateTimeOffset)value;

            if (LeadingEdge.HasValue)
                isValid = LeadingEdge < dtoValue;

            if (FallingEdge.HasValue)
                isValid = isValid && FallingEdge > dtoValue;

            return isValid ? null : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public override string FormatErrorMessage(string name)
        {
            if (LeadingEdge == null)
                return string.Format(ErrorMessage, name, FallingEdge);
            else if (FallingEdge == null)
                return string.Format(ErrorMessage, name, LeadingEdge);
            else
                return string.Format(ErrorMessage, name, LeadingEdge, FallingEdge);
        }
    }
}
