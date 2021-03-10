/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components.Forms;

namespace Blazor.Database.Data.Validators
{
    public class DecimalValidator : Validator<decimal>
    {
        #region Public

        /// <summary>
        /// Class Contructor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <param name="model"></param>
        /// <param name="validationMessageStore"></param>
        /// <param name="message"></param>
        public DecimalValidator(decimal value, string fieldName, object model, ValidationMessageStore validationMessageStore, string message) : base(value, fieldName, model, validationMessageStore, message) { }

        /// <summary>
        /// Check of the value is greater than test
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public DecimalValidator LessThan(decimal test, string message = null)
        {
            if (!(this.Value < test))
            {
                Trip = true;
                LogMessage(message);
            }
            return this;
        }
        /// <summary>
        /// Check of the value is greater than test
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public DecimalValidator LessThanOrEqualTo(decimal test, string message = null)
        {
            if (!(this.Value <= test))
            {
                Trip = true;
                LogMessage(message);
            }
            return this;
        }

        /// <summary>
        /// Check if the value is less than
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public DecimalValidator GreaterThan(decimal test, string message = null)
        {
            if (!(this.Value > test))
            {
                Trip = true;
                LogMessage(message);
            }
            return this;
        }
        /// <summary>
        /// Check if the value is less than
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public DecimalValidator GreaterThanOrEqualTo(decimal test, string message = null)
        {
            if (!(this.Value >= test))
            {
                Trip = true;
                LogMessage(message);
            }
            return this;
        }

        #endregion
    }

    public static class DecimalValidatorExtensions
    {
        /// <summary>
        /// String Validation Extension
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <param name="model"></param>
        /// <param name="validationMessageStore"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DecimalValidator Validation(this decimal value, string fieldName, object model, ValidationMessageStore validationMessageStore, string message = null)
        {
            var validation = new DecimalValidator(value, fieldName, model, validationMessageStore, message);
            return validation;
        }
    }

}
