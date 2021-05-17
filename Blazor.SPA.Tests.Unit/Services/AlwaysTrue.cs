using Xunit;

namespace Blazor.SPA.Tests.Unit
{
    public class AlwaysTrue
    {
        [Fact]
        public void ShouldBeTrue()
            => Assert.True(condition: true);
    }
}
