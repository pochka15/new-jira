using lab1.Models;

namespace lab1.Dtos.Project {
public class SubprojectDto {
    public string Id { get; set; }

    public Subproject ToModel() {
        return new Subproject {
            Id = Id
        };
    }
}
}