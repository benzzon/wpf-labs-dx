//using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LabsUI.Models
{
    [POCOViewModel]
    public partial class PersonModel : IDataErrorInfo
    {
        [BindableProperty]
        [Required(ErrorMessage = "You must provide a name.")]
        [StringLength(200, MinimumLength = 3,
        ErrorMessage = "The name must be at least 3 characters long")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The name must only contain letters (a-z, A-Z).")] 
        public virtual string PersonName { get; set; }

        [BindableProperty]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public virtual string Email { get; set; }

        [BindableProperty]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public virtual string PhoneNumber { get; set; }

        [BindableProperty]
        public virtual string Gender { get; set; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                var context = new ValidationContext(this, null, null) { MemberName = columnName };
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateProperty(GetType().GetProperty(columnName).GetValue(this, null), context, results))
                {
                    return results[0].ErrorMessage;
                }
                return null;
            }
        }
    }
}
