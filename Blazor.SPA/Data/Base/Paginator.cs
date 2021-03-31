/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using System;

namespace Blazor.SPA.Data
{
    public class Paginator
    {
        /// <summary>
        /// Current Page
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// No of items to display on a page
        /// </summary>
        public int PageSize { get; set; } = 25;

        /// <summary>
        /// No of pages to show in the block
        /// </summary>
        public int BlockSize { get; set; } = 10;

        /// <summary>
        /// Records in the current list
        /// </summary>
        public int RecordCount { get; set; } = 0;

        /// <summary>
        /// Column to sort the list on
        /// </summary>
        public string SortColumn
        {
            get => (!string.IsNullOrWhiteSpace(_sortColumn)) ? _sortColumn : DefaultSortColumn;
            set => _sortColumn = value;
        }

        private string _sortColumn = string.Empty;

        /// <summary>
        /// Default sort column when list first loads
        /// </summary>
        public string DefaultSortColumn { get; set; } = "ID";

        /// <summary>
        /// sort order - default is ascending
        /// </summary>
        public bool SortDescending { get; set; }

        public Paginator(int pageSize, int blockSize)
        {
            this.BlockSize = blockSize;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Event triggered when the page changes
        /// </summary>
        public event EventHandler PageChanged;

        // Set of read only properties for calculations and control in the Paging Control
        public int LastPage => (int)((RecordCount / PageSize) + 0.5);
        public int LastBlock => (int)((LastPage / BlockSize) + 1.5);
        public int CurrentBlock => (int)((Page / BlockSize) + 1.5);
        public int StartBlockPage => ((CurrentBlock - 1) * BlockSize) + 1;
        public int EndBlockPage => StartBlockPage + BlockSize;
        public bool HasBlocks => ((RecordCount / (PageSize * BlockSize)) + 0.5) > 1;
        public bool HasPagination => (RecordCount / PageSize) > 1;

        /// <summary>
        /// Go to a specific page
        /// </summary>
        /// <param name="page"></param>
        public void ToPage(int page, bool forceUpdate = false)
        {
            if ((forceUpdate | !this.Page.Equals(page)) && page > 0)
            {
                this.Page = page;
                this.PageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Go to the next page
        /// </summary>
        public void NextPage()
            => this.ToPage(this.Page + 1);

        /// <summary>
        /// Got to the previous page
        /// </summary>
        public void PreviousPage()
                    => this.ToPage(this.Page - 1);

        /// <summary>
        /// Go to the start
        /// </summary>
        public void ToStart()
            => this.ToPage(1);

        /// <summary>
        /// Go to the Last Page
        /// </summary>
        public void ToEnd()
            => this.ToPage((int)((RecordCount / PageSize) + 0.5));

        /// <summary>
        /// Go to the next block and load the first page in the block
        /// </summary>
        public void NextBlock()
        {
            if (CurrentBlock != LastBlock)
            {
                var calcpage = (CurrentBlock * PageSize * BlockSize) + 1;
                this.Page = calcpage > LastPage ? LastPage : LastPage;
                this.PageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Go to the previous block and load the last page in the block
        /// </summary>
        public void PreviousBlock()
        {
            if (CurrentBlock != 1)
            {
                this.Page = ((CurrentBlock - 1) * PageSize * BlockSize) - 1;
                this.PageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Method to notify the paginator that some aspecy of sorting has changed
        /// </summary>
        public void NotifySortingChanged()
            => this.ToPage(1, true);
    }
}
