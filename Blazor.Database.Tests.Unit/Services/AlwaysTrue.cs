using Xunit;

namespace Blazor.Database.Tests.Unit
{
    public class AlwaysTrue
    {
        [Fact]
        public void ShouldBeTrue()
            => Assert.True(condition: true);
    }
}
