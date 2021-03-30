using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.SPA.Data
{
    /// <summary>
    /// Enum for the State of the FilterListCollection
    /// NotSet is important during Component Initialization.
    /// </summary>
    public enum FilterListCollectionState
    {
        NotSet = 0,
        Show = 1,
        Hide = 2
    }

    public class Filtor : ICollection<FiltorItem>
    {
        /// <summary>
        /// 
        /// </summary>
        public FilterListCollectionState ShowState { get; set; } = FilterListCollectionState.NotSet;

        public bool OnlyLoadIfFilters { get; set; } = false;

        /// <summary>
        /// Boolean to determine if the filter should be shown in the UI
        /// </summary>
        public bool Show => this.ShowState == FilterListCollectionState.Show;

        /// <summary>
        /// Boolean to tell the list loader if it need to load
        /// </summary>
        public bool Load => this.filterList.Count > 0 || !this.OnlyLoadIfFilters;

        /// <summary>
        /// Count Property for the list
        /// </summary>
        public int Count => this.filterList.Count;

        /// <summary>
        /// IEnumerable Implementation
        /// </summary>
        /// <returns></returns>
        public IEnumerator<FiltorItem> GetEnumerator()
        {
            foreach (var item in filterList)
                yield return item;
        }

        /// <summary>
        /// IEnumerable Implementation
        /// </summary>
        ///// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// private list of the filters
        /// </summary>
        private List<FiltorItem> filterList = new List<FiltorItem>();

        public Filtor()
        {
        }

        public void Add(FiltorItem item)
            => filterList.Add(item);

        public void Clear()
            => filterList.Clear();

        public bool Contains(FiltorItem item)
            => filterList.Contains(item);

        public void CopyTo(FiltorItem[] items, int index)
            => filterList.CopyTo(items, index);

        public bool IsReadOnly => false;

        public bool Remove(FiltorItem item)
            => filterList.Remove(item);


        /// <summary>
        /// Try Getter for a filter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetFilter(string name, out object value)
        {
            value = null;
            var filter = filterList.FirstOrDefault(FilterListItem => FilterListItem.FieldName.Equals(name));
            if (filter != default)
            {
                value = filter.Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Getter for a filter
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetFilter(string name)
        {
            if (this.TryGetFilter(name, out object value))
                return value;
            return null;
        }

        /// <summary>
        /// Setter for a filter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="overwite"></param>
        /// <returns></returns>
        public bool SetFilter(string name, object value, bool overwite = true)
        {
            var filter = filterList.FirstOrDefault(FilterListItem => FilterListItem.FieldName.Equals(name));
            if (filter == default)
            {
                filterList.Add(new FiltorItem() { FieldName = name, Value = value, ObjectType = value.GetType().ToString() });
                return true;
            }
            else
            {
                if (overwite)
                {
                    filter.Value = value;
                    filter.ObjectType = value.GetType().ToString();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Method to delete a filter from the filter list
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteFilter(string name)
        {
            var filter = filterList.FirstOrDefault(FilterListItem => FilterListItem.FieldName.Equals(name));
            if (filter != default)
            {
                filterList.Remove(filter);
                return true;
            }
            return false;
        }
    }
}
