using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace lab1.Models {
public class AddActivityViewModel {
    public List<SelectListItem> Projects { get; set; }
    public string ProjectCode { get; set; } = "";
    public string SubprojectCode { get; set; } = "";
    public int SpentTime { get; set; }
    public string Description { get; set; } = "";
}
}