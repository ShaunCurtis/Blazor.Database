
namespace Blazor.Database.Components
{
    public class UIButtonColumn : UIColumn
    {
        protected override string PrimaryClass => this.Cols > 0 ? $"col-{this.Cols} text-right" : $"col text-right";

    }
}
