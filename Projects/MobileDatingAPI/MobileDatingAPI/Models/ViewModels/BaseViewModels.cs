using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{

    public class BaseApiViewModels
    {

        public bool Succeeded { get; set; }
        public string Message { get; set; }

        public string Error { get; set; }
        public int ErrorCode { get; set; }

        public BaseApiViewModels()
        {
            this.Succeeded = true;
            this.Message = "OK";
        }

        public virtual BaseApiViewModels ReturnError(string error, int errorCode)
        {
            this.Error = error;
            this.ErrorCode = errorCode;
            this.Succeeded = false;

            return this;
        }

    }

    public class BaseApiValidationViewModels: BaseApiViewModels
    {

        public List<FieldValidationFailure> ValidationFailedFields { get; private set; }

        public BaseApiValidationViewModels()
        {
            this.ValidationFailedFields = new List<FieldValidationFailure>();
        }

        public class FieldValidationFailure
        {
            public string Name { get; set; }
            public string Message { get; set; }
        }

    }

}