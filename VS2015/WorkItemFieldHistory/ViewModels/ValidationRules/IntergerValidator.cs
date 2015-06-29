using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lambda3.WorkItemFieldHistory.ViewModels.ValidationRules
{
    class PositiveIntergerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int number;

            if (!int.TryParse((value ?? String.Empty).ToString(), out number))
                return new ValidationResult(false, "The value must be a number.");

            if (number < 0)
                return new ValidationResult(false, "The value must be positive");

            return new ValidationResult(true, string.Empty);
        }
    }
}
