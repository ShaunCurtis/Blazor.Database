/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Components;
using Blazor.SPA.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.SPA.Services
{
    /// <summary>
    /// Service Class for managing Cusotm Routes and Runtime Layout Changes
    /// </summary>
    public class RouteViewService
    {
        /// <summary>
        /// List of Custom Routes
        /// </summary>
        public List<CustomRouteData> Routes { get; private set; } = new List<CustomRouteData>();

        /// <summary>
        /// Runtime Layout override
        /// </summary>
        public Type Layout { get; set; }

        /// <summary>
        /// View to load on return to Dirty Form
        /// </summary>
        public ViewData EditorView { get; set; }

        /// <summary>
        /// ID of Dirty Edit form
        /// Needed to remove it from the EditStateview if user decides to exit
        /// </summary>
        public Guid EditFormId { get; set; }

        /// <summary>
        /// Edit for to reload if there is a dirty edit state
        /// </summary>
        public Type EditForm { get; set; }

        /// <summary>
        /// User is existing a dirty Form
        /// </summary>
        public bool ConfirmDirtyExit => EditorView is not null;

        /// <summary>
        /// Method to get a Custom route match if one exists
        /// </summary>
        /// <param name="url"></param>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public bool GetRouteMatch(string url, out RouteData routeData)
        {
            var route = Routes?.FirstOrDefault(item => item.IsMatch(url)) ?? null;
            routeData = route?.RouteData ?? null;
            return route != null;
        }

        public List<EditStateData> EditStates { get; private set; } = new List<EditStateData>();

        private double garbageCollectionMinutes = -15;

        public void AddEditState(EditStateData data)
        {
            ClearEditStateGarbage();
            if (this.EditStates.Any(item => item.FormId == data.FormId && item.RecordId == data.RecordId))
            {
                var rec = this.EditStates.FirstOrDefault(item => item.FormId == data.FormId && item.RecordId == data.RecordId);
                EditStates.Remove(rec);
            }
            EditStates.Add(data);
        }

        public EditStateData GetEditState(Guid FormId)
        {
            ClearEditStateGarbage();
            return this.EditStates.FirstOrDefault(item => item.FormId == FormId);
        }

        public bool ClearEditState(Guid FormId)
        {
            ClearEditStateGarbage();
            var rec = this.EditStates.FirstOrDefault(item => item.FormId == FormId);
            var isRecord = rec != null;
            if (isRecord)
                EditStates.Remove(rec);

            return isRecord;
        }

        private void ClearEditStateGarbage()
        {
            var list = EditStates.Where(item => item.DateStamp < DateTimeOffset.Now.AddMinutes(garbageCollectionMinutes)).ToList();
            list?.ForEach(item => EditStates.Remove(item));
        }

        public void SetViewToEditForm()
            =>      this.EditorView = new ViewData(this.EditForm, null);

    }
}
