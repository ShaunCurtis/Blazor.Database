using System;
using System.Text.Json;

namespace Blazor.SPA.Data
{
    public class FiltorItem
    {
        public string FieldName { get; set; }

        public object Value
        {
            get
            {
                if (this._Value is JsonElement)
                    this.SetValue();
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        private void SetValue()
        {
            if (this._Value is JsonElement)
            {
                var element = (JsonElement)_Value;
                switch (this.ObjectType)
                {
                    case "System.Int32":
                        if (element.TryGetInt32(out int ivalue)) _Value = ivalue;
                        break;
                    case "System.Int64":
                        if (element.TryGetInt64(out long lvalue)) _Value = lvalue;
                        break;
                    case "System.Decimal":
                        if (element.TryGetDecimal(out decimal dvalue)) _Value = dvalue;
                        break;
                    case "System.DateTime":
                        if (element.TryGetDateTime(out DateTime dtvalue)) _Value = dtvalue;
                        break;
                    default:
                        _Value = element.GetString();
                        break;
                };
            }
        }

        private object _Value = null;

        public string ObjectType { get; set; }

    }
}
