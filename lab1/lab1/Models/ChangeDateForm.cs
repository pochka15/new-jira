using System;
using System.ComponentModel.DataAnnotations;

namespace lab1.Models {
/// <summary>
/// Form data which is primarily used for the feature: change date on the report page
/// </summary>
public class ChangeDateForm {
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddT00:00}")]
    public DateTime Date { get; set; }

    public string UserName { get; set; }
}
}