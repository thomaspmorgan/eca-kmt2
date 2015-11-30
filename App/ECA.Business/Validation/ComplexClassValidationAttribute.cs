using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

public class ComplexClassValidation
{
    public static List<ValidationResult> GetValidationResult(object value)
    {
        var results = new List<ValidationResult>();
        
        if (value == null) {
            results.Add(new ValidationResult("Validation object is null."));
        }
        else
        {
            var nestedValidationProperties = value.GetType().GetProperties();
            //.Where(p => ValidationAttribute.IsDefined(p, typeof(ValidationAttribute)))
            //.OrderBy(p => p.Name);

            foreach (var property in nestedValidationProperties)
            {
                var validators = ValidationAttribute.GetCustomAttributes(property, typeof(ValidationAttribute)) as ValidationAttribute[];

                if (validators == null || validators.Length == 0) continue;

                foreach (var validator in validators)
                {
                    //var propertyValue = property.GetValue(value, null);
                    //var valResult = validator.GetValidationResult(property, new ValidationContext(property, null, null));
                    var valResults = new List<ValidationResult>();

                    var actual = Validator.TryValidateObject(property, new ValidationContext(property, null, null), valResults.ToList(), true);

                    //if (valResults != null && valResults.Count > 0)
                    //{
                    //    results.AddRange(valResults);
                    //}
                    // get nested properties
                    //var nestedValidators = GetNestedPropertyValue(property, property.Name);
                }
            }
        }
        
        return results;
    }
    
    public static object GetNestedPropertyValue(object customObject, string fullyQualifiedPropertyName)
    {
        if (!String.IsNullOrEmpty(fullyQualifiedPropertyName))
        { 
            foreach (string propertyName in fullyQualifiedPropertyName.Split('.'))
            {
                PropertyInfo propertyInfo = customObject.GetType().GetProperty(propertyName);
                customObject = propertyInfo.GetValue(customObject, null);
            }
        }

        if (customObject == null)
        { 
            throw new Exception("Property value could not be determined");
        }

        return customObject;
    }
}

//public class ValidationResult
//{
//    public bool IsValid { get; set; }
//    public IList<string> Errors { get; set; }
//    public string errorMessage { get; set; }
//    public ValidationResult Success;

//    public ValidationResult()
//    {
//        Errors = new List<string>();
//    }
//}

