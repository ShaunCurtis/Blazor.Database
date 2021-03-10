/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

namespace Blazor.Database.Data.Validators
{
    public class ValidatorValue
    {
        public string Field { get; init; }

        public bool IsValid { get; set; }

        public ValidatorValue(string field, bool valid)
        {
            this.Field = field;
            this.IsValid = valid;
        }

    }
}
