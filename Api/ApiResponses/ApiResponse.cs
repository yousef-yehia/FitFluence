using System.Net;

namespace Api.ApiResponses
{

    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? ErrorMessage { get; set; }
        public object Result { get; set; }


        public ApiResponse BadRequestResponse(string message)
        {
            return new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessage =  message 
            };
        }
        public ApiResponse OkResponse(object result)
        {
            return new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            };
        }
        public ApiResponse NotFoundResponse(string message)
        {
            return new ApiResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccess = false,
                ErrorMessage =  message 
            };
        }
        public ApiResponse UnauthorizedResponse(string message)
        {
            return new ApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                IsSuccess = false,
                ErrorMessage = message 
            };
        }
    }
}