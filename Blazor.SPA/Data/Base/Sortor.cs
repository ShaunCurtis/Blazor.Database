using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.SPA.Data
{
    public class Sortor
    {
        public string SortColumn
        {
            get => (!string.IsNullOrWhiteSpace(_sortColumn)) ? _sortColumn : DefaultSortColumn;
            set => _sortColumn = value;
        }

        private string _sortColumn = string.Empty;

        public string DefaultSortColumn { get; set; } = "ID";

        public bool Descending { get; set; }

        //public PropertyInfo GetProperty<TRecord>(string propertyname) 
        //    => default(TRecord).GetType().GetProperty(propertyname);

    }
}
