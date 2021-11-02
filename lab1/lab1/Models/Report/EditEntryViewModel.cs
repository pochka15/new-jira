namespace lab1.Models {
public class EditEntryViewModel {
    public string Project { get; set; }
    public string SubCategory { get; set; }
    public int SpentTime { get; set; }
    public string Description { get; set; }
    public EntryDescription EntryDescription { get; set; }
}
}