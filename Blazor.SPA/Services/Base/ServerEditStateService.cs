/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.SPA.Services
{
    public class ServerEditStateService : IEditStateService
    {
        public List<EditStateData> EditStates { get; private set; } = new List<EditStateData>();

        private double garbageCollectionMinutes = -15;

        public ValueTask AddEditState(EditStateData data)
        {
            ClearGarbage();
            if (this.EditStates.Any(item => item.FormId == data.FormId && item.RecordId == data.RecordId))
            {
                var rec = this.EditStates.FirstOrDefault(item => item.FormId == data.FormId && item.RecordId == data.RecordId);
                EditStates.Remove(rec);
            }
            EditStates.Add(data);
            return ValueTask.CompletedTask;
        }

        public ValueTask<EditStateData> GetEditState(Guid FormId)
        {
            ClearGarbage();
            var rec = this.EditStates.FirstOrDefault(item => item.FormId == FormId);
            return ValueTask.FromResult<EditStateData>(rec);
        }

        public ValueTask<bool> ClearEditState(Guid FormId)
        {
            ClearGarbage();
            var rec = this.EditStates.FirstOrDefault(item => item.FormId == FormId);
            if (rec != null)
                EditStates.Remove(rec);

            return ValueTask.FromResult<bool>(true); ;
        }

        private void ClearGarbage()
        {
            var list = EditStates.Where(item => item.DateStamp < DateTimeOffset.Now.AddMinutes(garbageCollectionMinutes)).ToList();
            list?.ForEach(item => EditStates.Remove(item));
        }

    }
}
