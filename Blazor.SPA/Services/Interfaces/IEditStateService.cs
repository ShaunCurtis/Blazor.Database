/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Data;
using System;
using System.Threading.Tasks;

namespace Blazor.SPA.Services
{
    public interface IEditStateService
    {
        public ValueTask AddEditState(EditStateData data);

        public ValueTask<EditStateData> GetEditState(Guid FormId);

        public ValueTask<bool> ClearEditState(Guid FormId);
    }
}
