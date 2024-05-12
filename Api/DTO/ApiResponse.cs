using System.Net;

namespace Api.DTO
{

    public class ApiResponse
    {
        public ApiResponse()
        {
            ErrorMessages = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }


        public ApiResponse BadRequestResponse(string message)
        {
            return new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = new List<string> { message }
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
                ErrorMessages = new List<string> { message }
            };
        }
        public ApiResponse UnauthorizedResponse(string message)
        {
            return new ApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                IsSuccess = false,
                ErrorMessages = new List<string> { message }
            };
        }
    }
}