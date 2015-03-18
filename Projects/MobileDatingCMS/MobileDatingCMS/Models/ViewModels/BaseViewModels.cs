using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileDatingCMS.Models.ViewModels
{
    public abstract class BaseViewModels
    {

        public int ID { get; set; }
        public bool Active { get; set; }
        public string ErrorMessage { get; set; }

        public bool ValidateModel(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                foreach (var state in modelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        this.ErrorMessage = error.ErrorMessage;
                    }

                    if (this.ErrorMessage != null)
                    {
                        break;
                    }
                }

                return false;
            }

            return true;
            
        }

    }
}