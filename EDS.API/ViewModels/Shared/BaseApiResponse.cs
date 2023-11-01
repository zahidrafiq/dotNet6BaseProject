
using EDS.Common.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HMIS.IM.API.ViewModels.Shared
{
    public class BaseResponseModel
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = "";
        public int code { get; set; }
        public string? errorCode { get; set; } = null;
    }

    public class ApiGridResponse<VM>: BaseResponseModel
    {
        public List<VM>? itemList { get; set; }
        public int totalCount { get; set; } = 0;
        public ApiGridResponse<VM> GetResponseObject(List<VM>? itemList, bool isSuccess, string message = "Action done Successfully", int code = StatusCodes.Status200OK)
        {
            return new ApiGridResponse<VM>()
            {
                itemList = itemList,
                isSuccess = isSuccess,
                code = code,
                message = message,
                totalCount = itemList is not null ? itemList.Count : 0

            };
        }
        public ApiGridResponse<VM> GetSuccessResponseObject(List<VM>? itemList, string message = "Action done Successfully")
        {
            return new ApiGridResponse<VM>()
            {
                isSuccess = true,
                code = StatusCodes.Status200OK,
                message = message,
                itemList = itemList,
                totalCount = itemList is not null? itemList.Count : 0
               
            };
        }       
        public ApiGridResponse<VM> GetErrorResponseObject(int code = 500, string? errorCode = "", string message = "Some Internal Server Error Occured.")
        {
            return new ApiGridResponse<VM>()
            {
                isSuccess = false,
                code = code,
                errorCode = errorCode,
                message = message,
                itemList = null
            };
        }
        public ApiGridResponse<VM> GetNullResponseObject(string message = Constant.DATA_NOT_FOUND, int code = StatusCodes.Status200OK)
        {
            return new ApiGridResponse<VM>()
            {
                isSuccess = false,               
                errorCode = null,
                itemList = new List<VM>(),
                message = message,
                code = code
            };
        }
    }

    public class ApiResponse<VM>: BaseResponseModel
    {
        public VM? data {get; set; }
        public ApiResponse<VM> GetResponseObject(VM? data, bool isSuccess, string message, int code)
        {
            return new ApiResponse<VM>()
            {
                data = data,
                isSuccess = isSuccess,
                code = code,
                message = message
            };
        }
        public ApiResponse<VM> GetSuccessResponseObject(VM? data, string message = "Action done Successfully")
        {
            return new ApiResponse<VM>()
            {
                isSuccess = true,
                code = StatusCodes.Status200OK,
                message = message,
                data = data
            };
        }
        public ApiResponse<VM> GetErrorResponseObject(int code = 500, string? errorCode = "", string message = "Some Internal Server Error Occured.")
        {
            return new ApiResponse<VM>()
            {
                isSuccess = false,
                code = code,
                errorCode = errorCode,
                message = message,
                data = default(VM)
            };
        }
        public ApiResponse<VM> GetNullResponseObject(string message = Constant.DATA_NOT_FOUND, int code = StatusCodes.Status200OK)
        {
            return new ApiResponse<VM>()
            {
                isSuccess = false,
                errorCode = null,
                data = default(VM),
                message = message,
                code = code
            };
        }

    }

}