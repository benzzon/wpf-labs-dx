//using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.DataAnnotations;
using System;

namespace LabsUI.Models
{
    [POCOViewModel]
    public partial class PersonModel
    {
        [BindableProperty]
        public virtual string PersonName { get; set; }
        [BindableProperty]
        public virtual string Email { get; set; }
        [BindableProperty]
        public virtual string PhoneNumber { get; set; }
        [BindableProperty]
        public virtual string Gender { get; set; }
    }
}
