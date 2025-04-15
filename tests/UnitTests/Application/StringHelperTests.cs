using Application.Common;

namespace UnitTests.Application;

public class StringHelperTests
{
    [Theory]
    [InlineData("test", "Test")]
    [InlineData("TEST", "Test")]
    [InlineData("tEST", "Test")]
    [InlineData("t", "T")]
    [InlineData(" Test", " test")]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData(" ", "")]
    public void CapitalizeFirstLetter_MustReturnCorrectly(string input, string expected)
    {
        var result = StringHelper.CapitalizeFirstLetter(input);
        Assert.Equal(expected, result);
    }
}
