using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Database.Data
{
    public class EditField
    {
        public string FieldName { get; }

        public Guid GUID { get; }

        public object Value { get; }

        public object EditedValue { get; set; }

        public object Model { get; private set; }

        public bool IsDirty
        {
            get
            {
                if (Value != null && EditedValue != null) return !Value.Equals(EditedValue);
                if (Value is null && EditedValue is null) return false;
                return true;
            }
        }

        public EditField(object model, string fieldName, object value)
        {
            this.Model = model;
            this.FieldName = fieldName;
            this.Value = value;
            this.EditedValue = value;
            this.GUID = Guid.NewGuid();
        }

        public void Reset()
            => this.EditedValue = this.Value;
    }
}
