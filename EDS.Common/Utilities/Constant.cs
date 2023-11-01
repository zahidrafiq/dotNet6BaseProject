using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDS.Common.Utilities
{
    public static class Constant
    {
        public const string GET_API_SUCCESS_MSG = "Data Fetched Succesfully";
        public const string GET_API_ERROR_MSG = "Failed To Fetch Data";
        public const string DATA_NOT_FOUND = "Data Not Found";

    }
    public static class ErrorCodes
    {
        //This error is returned when at least one of the mandatory fields are missing.
        public const string INVALID_REQUEST_FORMAT = "INVALID_REQUEST_FORMAT";

        //When the body of the message doesn’t conform to or violates our business rules.
        public const string INVALID_INPUT_FORMAT = "INVALID_INPUT_FORMAT";
        public const string UNAUTHORIZED_ACCESS = "UNAUTHORIZED_ACCESS";
        public const string DOWNLOAD_FAILURE = "CANNOT_BE_DOWNLOADED";
        public const string INVALID_INPUT_PARAM = "INVALID_INPUT_PARAM";

        // For internal error, exceptions
        public const string SYSTEM_ERROR = "SYSTEM_ERROR";
        public const string INVALID_INPUT = "INVALID_INPUT";
        public const string NO_CONTENT = "CONTENT_NOT_AVAILABLE";


    }
}
