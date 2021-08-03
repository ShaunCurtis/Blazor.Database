/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components.Forms;

namespace Blazr.SPA.Data
{
    public class IntValidator : Validator<int>
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
        public IntValidator(int value, string fieldName, object model, ValidationMessageStore validationMessageStore, string message) : base(value, fieldName, model, validationMessageStore, message) { }

        /// <summary>
        /// Check of the value is greater than test
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public IntValidator LessThan(int test, string message = null)
        {
            if (!(this.Value < test))
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
        public IntValidator GreaterThan(int test, string message = null)
        {
            if (!(this.Value > test))
            {
                Trip = true;
                LogMessage(message);
            }
            return this;
        }

        #endregion
    }
    
    public static class IntValidatorExtensions
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
        public static IntValidator Validation(this int value, string fieldName, object model, ValidationMessageStore validationMessageStore, string message = null)
        {
            var validation = new IntValidator(value, fieldName, model, validationMessageStore, message);
            return validation;
        }
    }

}
