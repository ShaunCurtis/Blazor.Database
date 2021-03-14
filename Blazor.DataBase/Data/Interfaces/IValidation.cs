using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Database.Data
{
    public interface IValidation
    {

        public bool Validate(ValidationMessageStore validationMessageStore, string fieldname, object model = null);

    }
}
