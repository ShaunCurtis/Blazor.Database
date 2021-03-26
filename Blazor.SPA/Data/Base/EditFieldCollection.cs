﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.SPA.Data
{
    public class EditFieldCollection : IEnumerable
    {
        private List<EditField> _items = new List<EditField>();

        public int Count => _items.Count;

        public Action<bool> FieldValueChanged;

        public bool IsDirty => _items.Any(item => item.IsDirty);

        public void Clear()
            => _items.Clear();

        public void ResetValues()
            => _items.ForEach(item => item.Reset());

        public IEnumerator GetEnumerator()
            => new EditFieldCollectionEnumerator(_items);

        public T Get<T>(string FieldName)
        {
            var x = _items.FirstOrDefault(item => item.FieldName.Equals(FieldName, StringComparison.CurrentCultureIgnoreCase));
            if (x != null && x.Value is T t) return t;
            return default;
        }


        public T GetEditValue<T>(string FieldName)
        {
            var x = _items.FirstOrDefault(item => item.FieldName.Equals(FieldName, StringComparison.CurrentCultureIgnoreCase));
            if (x != null && x.EditedValue is T t) return t;
            return default;
        }

        public bool TryGet<T>(string FieldName, out T value)
        {
            value = default;
            var x = _items.FirstOrDefault(item => item.FieldName.Equals(FieldName, StringComparison.CurrentCultureIgnoreCase));
            if (x != null && x.Value is T t) value = t;
            return x.Value != default;
        }

        public bool TryGetEditValue<T>(string FieldName, out T value)
        {
            value = default;
            var x = _items.FirstOrDefault(item => item.FieldName.Equals(FieldName, StringComparison.CurrentCultureIgnoreCase));
            if (x != null && x.EditedValue is T t) value = t;
            return x.EditedValue != default;
        }

        public bool HasField(EditField field)
            => this.HasField(field.FieldName);

        public bool HasField(string FieldName)
        {
            var x = _items.FirstOrDefault(item => item.FieldName.Equals(FieldName, StringComparison.CurrentCultureIgnoreCase));
            if (x is null | x == default) return false;
            return true;
        }

        public bool SetField(string FieldName, object value)
        {
            var x = _items.FirstOrDefault(item => item.FieldName.Equals(FieldName, StringComparison.CurrentCultureIgnoreCase));
            if (x != null && x != default)
            {
                x.EditedValue = value;
                this.FieldValueChanged?.Invoke(this.IsDirty);
                return true;
            }
            return false;
        }

        public bool AddField(object model, string fieldName, object value)
        {
            this._items.Add(new EditField(model, fieldName, value));
            return true;
        }

        public class EditFieldCollectionEnumerator : IEnumerator
        {
            private List<EditField> _items = new List<EditField>();
            private int _cursor;

            object IEnumerator.Current
            {
                get
                {
                    if ((_cursor < 0) || (_cursor == _items.Count))
                        throw new InvalidOperationException();
                    return _items[_cursor];
                }
            }
            public EditFieldCollectionEnumerator(List<EditField> items)
            {
                this._items = items;
                _cursor = -1;
            }
            void IEnumerator.Reset()
                => _cursor = -1;

            bool IEnumerator.MoveNext()
            {
                if (_cursor < _items.Count)
                    _cursor++;

                return (!(_cursor == _items.Count));
            }
        }
    }
}
