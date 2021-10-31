using System.Collections.Generic;

namespace lab1.Models {
public class Activity {
    public string Code { get; set; }
    public string Manager { get; set; }
    public string Name { get; set; }
    public float Budget { get; set; }
    public bool Active { get; set; }
    public List<SubActivity> SubActivities { get; set; }
}
}