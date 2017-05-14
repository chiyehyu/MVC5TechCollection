

using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MVC5Course.Models.Validation;

namespace MVC5Course.Models.ViewModels
{
    public class ProductListSearchVM : IValidatableObject
    {
        public string q { get; set; }
        public int? s1 { get; set; }
        public int? s2 { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (s2.HasValue && s1.HasValue && s2.Value < s1.Value)
            {
                yield return new ValidationResult("Stock range invalid");
            }
            yield break;
            //throw new NotImplementedException();
        }
    }
}