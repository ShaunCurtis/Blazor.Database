/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable enable
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace Blazr.UIComponents
{
    public class FormViewControl<TValue> : ComponentBase
    {
        [Parameter] public TValue? Value { get; set; }

        [Parameter] public Expression<Func<TValue>>? ValueExpression { get; set; }

        [Parameter] public string? Label { get; set; }

        [Parameter] public string? HelperText { get; set; }

        [Parameter] public string DivCssClass { get; set; } = "mb-2";

        [Parameter] public string LabelCssClass { get; set; } = "form-label";

        [Parameter] public string ControlCssClass { get; set; } = "form-control";

        [Parameter] public Type ControlType { get; set; } = typeof(InputReadOnlyText);

        [Parameter] public int ControlCols { get; set; } = 9;

        [Parameter] public int LabelCols { get; set; } = 3;

        [Parameter] public bool ShowLabel { get; set; } = true;

        [Parameter] public bool IsRow { get; set; }

        private bool IsLabel => this.ShowLabel && (!string.IsNullOrWhiteSpace(this.Label) || !string.IsNullOrWhiteSpace(this.FieldName));

        private readonly string formId = Guid.NewGuid().ToString();

        private string? DisplayLabel => this.Label ?? this.FieldName;
        private string? FieldName
        {
            get
            {
                string? fieldName = null;
                if (this.ValueExpression != null)
                    ParseAccessor(this.ValueExpression, out var model, out fieldName);
                return fieldName;
            }
        }

        private int spacerCols => 12 - (LabelCols + ControlCols);
        
        protected override void OnInitialized()
        {
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (IsRow)
                builder.AddContent(1, RowFragment);
            else
                builder.AddContent(2, BaseFragment);
        }

        private RenderFragment BaseFragment => (builder) =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(10, "class", this.DivCssClass);
            builder.AddContent(40, this.LabelFragment);
            builder.AddContent(60, this.ControlFragment);
            builder.CloseElement();
        };

        private RenderFragment RowFragment => (builder) =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(10, "class", "row form-group");
            builder.OpenElement(20, "div");
            builder.AddAttribute(30, "class", $"col-12 col-md-{this.LabelCols}");
            builder.AddContent(40, this.LabelFragment);
            builder.CloseElement();
            builder.OpenElement(40, "div");
            builder.AddAttribute(50, "class", $"col-12 col-md-{this.ControlCols}");
            builder.AddContent(60, this.ControlFragment);
            builder.CloseElement();
            if (this.spacerCols > 0)
            {
                builder.OpenElement(40, "div");
                builder.AddAttribute(50, "class", $"d-none d-md-block col-md-{this.spacerCols}");
                builder.CloseElement();
            }
            builder.CloseElement();
        };

        private RenderFragment LabelFragment => (builder) =>
        {
            if (this.IsLabel)
            {
                builder.OpenElement(110, "label");
                builder.AddAttribute(120, "for", this.formId);
                builder.AddAttribute(130, "class", this.LabelCssClass);
                builder.AddContent(140, this.DisplayLabel);
                builder.CloseElement();
            }
        };


        private RenderFragment ControlFragment => (builder) =>
        {
            if (this.IsLabel)
            {
                builder.OpenComponent(210, this.ControlType);
                builder.AddAttribute(230, "Value", this.Value);
                builder.CloseComponent();
            }
        };


        // Code lifted from FieldIdentifier.cs
        private static void ParseAccessor<T>(Expression<Func<T>> accessor, out object model, out string fieldName)
        {
            var accessorBody = accessor.Body;
            if (accessorBody is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Convert && unaryExpression.Type == typeof(object))
                accessorBody = unaryExpression.Operand;

            if (!(accessorBody is MemberExpression memberExpression))
                throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(FieldIdentifier)} only supports simple member accessors (fields, properties) of an object.");

            fieldName = memberExpression.Member.Name;
            if (memberExpression.Expression is ConstantExpression constantExpression)
            {
                if (constantExpression.Value is null)
                    throw new ArgumentException("The provided expression must evaluate to a non-null value.");
                model = constantExpression.Value;
            }
            else if (memberExpression.Expression != null)
            {
                var modelLambda = Expression.Lambda(memberExpression.Expression);
                var modelLambdaCompiled = (Func<object?>)modelLambda.Compile();
                var result = modelLambdaCompiled();
                if (result is null)
                    throw new ArgumentException("The provided expression must evaluate to a non-null value.");
                model = result;
            }
            else
                throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(FieldIdentifier)} only supports simple member accessors (fields, properties) of an object.");
        }
    }
}
#pragma warning restore CS8622
#pragma warning restore CS8602
#nullable disable
