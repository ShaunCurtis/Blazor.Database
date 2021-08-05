/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System;

namespace Blazr.SPA.Core
{
    public class RecordPager
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 25;

        public int BlockSize { get; set; } = 10;

        public int RecordCount { get; set; } = 0;

        public string SortColumn
        {
            get => (!string.IsNullOrWhiteSpace(_sortColumn)) ? _sortColumn : DefaultSortColumn;
            set => _sortColumn = value;
        }

        private string _sortColumn = string.Empty;

        public string DefaultSortColumn { get; set; } = "ID";

        public bool SortDescending { get; set; }

        public RecordPager(int pageSize, int blockSize)
        {
            this.BlockSize = blockSize;
            this.PageSize = pageSize;
        }

        public event EventHandler PageChanged;

        // Set of read only properties for calculations and control in the Paging Control
        public int LastPage => (int)Math.Ceiling((RecordCount / PageSize) + 0.5);
        public int LastBlock => (int)((LastPage / BlockSize) + 1.5);
        public int CurrentBlock => (int)((Page / BlockSize) + 1.5);
        public int StartBlockPage => ((CurrentBlock - 1) * BlockSize) + 1;
        public int EndBlockPage => StartBlockPage + BlockSize;
        public bool HasBlocks => ((RecordCount / (PageSize * BlockSize)) + 0.5) > 1;
        public bool HasPagination => (RecordCount / PageSize) > 1;

        public void ToPage(int page, bool forceUpdate = false)
        {
            if ((forceUpdate | !this.Page.Equals(page)) && page > 0)
            {
                this.Page = page;
                this.PageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void NextPage()
            => this.ToPage(this.Page + 1);

        public void PreviousPage()
                    => this.ToPage(this.Page - 1);

        public void ToStart()
            => this.ToPage(1);

        public void ToEnd()
            => this.ToPage((int)Math.Ceiling((RecordCount / PageSize) + 0.5));

        public void NextBlock()
        {
            if (CurrentBlock != LastBlock)
            {
                var calcpage = (CurrentBlock * BlockSize) + 1;
                this.Page = calcpage > LastPage ? LastPage : LastPage;
                this.PageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void PreviousBlock()
        {
            if (CurrentBlock != 1)
            {
                this.Page = ((CurrentBlock - 2) * PageSize) + 1;
                this.PageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void NotifySortingChanged()
            => this.ToPage(1, true);

        public RecordPagingData GetData => new RecordPagingData()
        {
            Page = this.Page,
            PageSize = this.PageSize,
            BlockSize = this.BlockSize,
            RecordCount = this.RecordCount,
            SortColumn = this.SortColumn,
            SortDescending = this.SortDescending
        };
    }
}
