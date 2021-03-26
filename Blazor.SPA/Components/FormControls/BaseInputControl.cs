/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// 60% of Code from Microsoft's InputBase control
/// ==================================

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Blazor.SPA.Components
{
#nullable enable
    public class BaseInputControl<TValue> : ComponentBase, IDisposable
    {

        [Parameter] public TValue? Value { get; set; }

        [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

        [Parameter] public Expression<Func<TValue>>? ValueExpression { get; set; }

        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

        [CascadingParameter] public EditContext CascadedEditContext { get; set; } = default!;

        protected FieldIdentifier FieldIdentifier { get; set; }

        protected EditContext EditContext { get; set; } = default!;

        protected virtual TValue? CurrentValue
        {
            get => Value;
            set => UpdateValue(value);
        }

        /// <summary>
        /// Method to Update the Value
        /// Can be overridden
        /// </summary>
        /// <param name="value"></param>
        protected virtual void UpdateValue(TValue? value)
        {
            var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, this.Value);
            if (hasChanged)
            {
                SetValue(value);
                if (ValueChanged.HasDelegate)
                    _ = ValueChanged.InvokeAsync(value);
                if (!FieldIdentifier.Equals(default(FieldIdentifier)))
                    EditContext.NotifyFieldChanged(FieldIdentifier);
            }
        }

        /// <summary>
        /// Method to Set the Value
        /// Can be overridden
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetValue(TValue? value)
                => Value = value;

        /// <summary>
        /// Overrides the base SetParametersAsync to check if we have an EditContext and set up the FieldIdentfier
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            if (EditContext == null)
            {
                if (CascadedEditContext == null)
                    throw new InvalidOperationException($"{GetType()} requires a cascading parameter. For example, you can use {GetType().FullName} inside an {nameof(EditForm)}.");
                EditContext = CascadedEditContext;
                if (ValueExpression != null)
                    FieldIdentifier = FieldIdentifier.Create(ValueExpression);
                EditContext.OnValidationStateChanged += this.OnValidateStateChanged;
            }
            else if (CascadedEditContext != EditContext)
                throw new InvalidOperationException($"{GetType()} does not support changing the EditContext dynamically.");

            return base.SetParametersAsync(ParameterView.Empty);
        }

        /// <summary>
        /// Gets the class for the FieldIdentifier
        /// Gets set by the validation process
        /// </summary>
        private string FieldClass
            => EditContext.FieldCssClass(FieldIdentifier);

        /// <summary>
        /// Builds the composite class from user entered string and the FieldIdentified set value
        /// </summary>
        protected string CssClass
        {
            get
            {
                if (AdditionalAttributes != null &&
                    AdditionalAttributes.TryGetValue("class", out var @class) &&
                    !string.IsNullOrEmpty(Convert.ToString(@class, CultureInfo.InvariantCulture)))
                {
                    return $"{@class} {FieldClass}";
                }
                return FieldClass; // Never null or empty
            }
        }

        private void OnValidateStateChanged(object? sender, ValidationStateChangedEventArgs eventArgs)
        {
            UpdateAdditionalValidationAttributes();
            StateHasChanged();
        }

        private void UpdateAdditionalValidationAttributes()
        {
            var hasAriaInvalidAttribute = AdditionalAttributes != null && AdditionalAttributes.ContainsKey("aria-invalid");
            if (EditContext.GetValidationMessages(FieldIdentifier).Any())
            {
                if (hasAriaInvalidAttribute)
                    return;

                if (ConvertToDictionary(AdditionalAttributes, out var additionalAttributes))
                    AdditionalAttributes = additionalAttributes;

                additionalAttributes["aria-invalid"] = true;
            }
            else if (hasAriaInvalidAttribute)
            {
                if (AdditionalAttributes!.Count == 1)
                    AdditionalAttributes = null;
                else
                {
                    if (ConvertToDictionary(AdditionalAttributes, out var additionalAttributes))
                        AdditionalAttributes = additionalAttributes;
                    additionalAttributes.Remove("aria-invalid");
                }
            }
        }

        /// <summary>
        /// Returns a dictionary with the same values as the specified <paramref name="source"/>.
        /// </summary>
        /// <returns>true, if a new dictrionary with copied values was created. false - otherwise.</returns>
        private bool ConvertToDictionary(IReadOnlyDictionary<string, object>? source, out Dictionary<string, object> result)
        {
            var newDictionaryCreated = true;
            if (source == null)
                result = new Dictionary<string, object>();
            else if (source is Dictionary<string, object> currentDictionary)
            {
                result = currentDictionary;
                newDictionaryCreated = false;
            }
            else
            {
                result = new Dictionary<string, object>();
                foreach (var item in source)
                {
                    result.Add(item.Key, item.Value);
                }
            }
            return newDictionaryCreated;
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
        }

        void IDisposable.Dispose()
        {
            EditContext.OnValidationStateChanged -= this.OnValidateStateChanged;
            Dispose(disposing: true);
        }

    }
}
#nullable disable
