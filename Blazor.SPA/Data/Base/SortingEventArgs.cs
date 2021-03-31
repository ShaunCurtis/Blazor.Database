using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.SPA.Data
{
    public class SortingEventArgs
    {
        public string SortColumn { get; set; }
        
        public bool Descending { get; set; }

        public static SortingEventArgs Get(string sortColumn, bool descending)
            => new SortingEventArgs() { SortColumn = sortColumn, Descending = descending };
    }
}
