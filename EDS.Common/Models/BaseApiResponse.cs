
using EDS.Common.Utilities;

namespace HMIS.IM.Common.Models
{
    public class BaseApiResponse
    {
        public bool Error { get; set; } // Indicates if there is an error. 0 = No Error, 1 = Some Error

        public string Message { get; set; } // In case of success, this contains success message

        public List<Error> Errors { get; set; } // In case of error, list of error would be shown

        public BaseApiResponse() { }

        //public BaseApiResponse(ModelStateDictionary modelState)
        //{
        //    Message = "";
        //    Errors = modelState.Keys
        //            .SelectMany(key => modelState[key].Errors.Select(x => new Error(ErrorCodes.INVALID_REQUEST_FORMAT, key + "-" + x.ErrorMessage)))
        //            .ToList();
        //    if (Errors.Count > 0) Error = true;
        //}

        public BaseApiResponse(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                Error = true;
                if (Errors == null) Errors = new List<Error>();
                Errors.Add(new Error(ErrorCodes.INVALID_INPUT_FORMAT, error));
            }
        }
        public BaseApiResponse(string ErrorType, string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                Error = true;
                if (Errors == null) Errors = new List<Error>();
                Errors.Add(new Error(ErrorType, error));

            }
        }

        public static implicit operator Stream(BaseApiResponse v)
        {
            throw new NotImplementedException();
        }
    }

    public class Error
    {
        public string ErrorCode { get; set; } // one of the standard error codes defined below

        public string ErrorDescription { get; set; } // description of the error

        public Error(string code, string description)
        {
            ErrorCode = code;
            ErrorDescription = description;
        }
        public Error()
        {
        }
    }
    }
