using System.Collections.Generic;

namespace lab1.Models {
public class ManagementViewModel {
    public List<SimplifiedProject> Projects { get; set; }
}

public class SimplifiedProject {
    public string Name { get; set; }
    public string Code { get; set; }
}
}