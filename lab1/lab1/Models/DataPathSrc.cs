namespace lab1.Models {
/// <summary>
/// A source that provides with a path to the json data root folder
/// </summary>
public class DataPathSrc {
    public readonly string Path;

    public DataPathSrc(string dataPath) {
        Path = dataPath;
    }
}
}