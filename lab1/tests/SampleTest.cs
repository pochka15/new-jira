using lab1.Dtos.Others;
using lab1.Services;
using Xunit;
using Xunit.Abstractions;

namespace tests {
public class SampleTest {
    private readonly ITestOutputHelper _output;

    public SampleTest(ITestOutputHelper output) {
        _output = output;
    }

    [Fact]
    public void CorrectActivityReturned() {
        // TODO(@pochka15): take root from env
        const string dataRoot = "";
        Assert.Equal("Argus",
            new JsonProjectService(new DataPathSrc(dataRoot), null)
                .GetProjectById("ARGUS-123")
                ?.Name);
    }
}
}