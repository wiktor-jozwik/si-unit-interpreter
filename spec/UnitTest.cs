using Xunit;

namespace si_unit_compiler.spec;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        const int sum = 2 + 2;
        Assert.Equal(4, sum);
    }
}