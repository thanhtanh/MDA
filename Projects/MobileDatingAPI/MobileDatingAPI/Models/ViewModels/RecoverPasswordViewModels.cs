using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{
    public class RecoverPasswordViewModels : BaseApiViewModels
    {

        public string RecoveryEmail { get; set; }

    }

    public class ResetPasswordViewModels : BaseApiValidationViewModels
    {



    }

}