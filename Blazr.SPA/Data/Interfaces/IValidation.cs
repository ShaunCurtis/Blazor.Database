﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components.Forms;

namespace Blazr.SPA.Core
{
    public interface IValidation
    {
        public bool Validate(ValidationMessageStore validationMessageStore, string fieldname, object model = null);
    }
}
