using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GiftDepo.Validators
{
    class NotEmptyValidationRule : ValidationRule
    {
       
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strReady = (value ?? "").ToString();
            var strOk = string.IsNullOrWhiteSpace(strReady);
            if (strReady == "0")
            {
                return new ValidationResult(false, "Value must be greate than 0.");
            }
            
            return strOk ? new ValidationResult(false, "Field is required.") : ValidationResult.ValidResult;
        }
    }
}
