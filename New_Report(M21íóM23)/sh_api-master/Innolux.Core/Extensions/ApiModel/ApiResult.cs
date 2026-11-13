namespace Innolux.Core.Extensions.ApiModel
{

    public class ApiResult
    {

        private string _message;


        /// <summary>
        /// 状态码
        /// </summary>
        public ApiCodeEnum Code { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success => Code == ApiCodeEnum.Ok;
        /// <summary>
        /// 信息
        /// </summary>
        public string Message
        {
            get { return _message ?? Code.GetDisplayName(); }
            set { _message = value; }
        }

        /// <summary>
        /// 返回结果，默认成功
        /// </summary>
        public ApiResult()
        {
            Code = ApiCodeEnum.Ok;
        }
        public ApiResult(ApiCodeEnum code, string message = null)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 返回指定 Code
        /// </summary>
        public static ApiResult FromCode(ApiCodeEnum code, string message = null)
        {
            return new ApiResult(code, message);
        }

        /// <summary>
        /// 返回默认操作失败信息
        /// </summary>
        public static ApiResult Fail(ApiCodeEnum code = ApiCodeEnum.Fail)
        {
            return new ApiResult(code);
        }

        /// <summary>
        /// 返回指定操作失败信息
        /// </summary>
        public static ApiResult Fail(string message, ApiCodeEnum code = ApiCodeEnum.Fail)
        {
            return new ApiResult(code, message);
        }

        /// <summary>
        /// 返回默认服务器内部错误信息
        /// </summary>
        public static ApiResult Error(ApiCodeEnum code = ApiCodeEnum.Error)
        {
            return new ApiResult(code);
        }

        /// <summary>
        /// 返回指定服务器内部错误的信息
        /// </summary>
        public static ApiResult Error(string message, ApiCodeEnum code = ApiCodeEnum.Error)
        {
            return new ApiResult(code, message);
        }

        /// <summary>
        /// 返回默认404信息
        /// </summary>
        public static ApiResult NotFound(ApiCodeEnum code = ApiCodeEnum.NotFound)
        {
            return new ApiResult(code);
        }

        /// <summary>
        /// 返回指定404信息
        /// </summary>
        public static ApiResult NotFound(string message, ApiCodeEnum code = ApiCodeEnum.NotFound)
        {
            return new ApiResult(code, message);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        public static ApiResult Ok(string message = null)
        {
            return FromCode(ApiCodeEnum.Ok, message);
        }

        /// <summary>
        /// 返回指定 Code
        /// </summary>
        public static ApiResult<T> FromCode<T>(ApiCodeEnum code, string message = null)
        {
            return new ApiResult<T>(code, message);
        }

        /// <summary>
        /// 返回指定 Code和提示信息
        /// </summary>
        public static ApiResult<T> FromCode<T>(ApiCodeEnum code, T data, string message = null)
        {
            return new ApiResult<T>(code, message, data);
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        public static ApiResult<T> FromData<T>(T data)
        {
            return new ApiResult<T>(data);
        }

        /// <summary>
        /// 返回数据和提示信息
        /// </summary>
        public static ApiResult<T> FromData<T>(T data, string message)
        {
            return new ApiResult<T>(ApiCodeEnum.Ok, message, data);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        public static ApiResult<T> Ok<T>(T data)
        {
            return FromData(data);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        public static ApiResult<T> Ok<T>(T data, string message)
        {
            return FromData(data, message);
        }

    }

    /// <summary>
    /// API 返回JSON字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ApiResult
    {


        public ApiResult(T data)
            : base(ApiCodeEnum.Ok)
        {
            Data = data;
        }

        public ApiResult(ApiCodeEnum code, string message = null)
            : base(ApiCodeEnum.Ok, message)
        {

        }

        public ApiResult(ApiCodeEnum code, string message = null, T data = default(T))
            : base(ApiCodeEnum.Ok, message)
        {
            Data = data;
        }

        public ApiResult()
        {

        }

        /// <summary>
        /// 数据集
        /// </summary>
        public T Data { get; set; }




    }

    public class ErrResult : ApiResult
    {


        public ErrResult(string message)
            : base(ApiCodeEnum.Error, message)
        {

        }

        public ErrResult(ApiCodeEnum code)
            : base(code)
        {

        }

        public ErrResult(ApiCodeEnum code, string message)
            : base(code, message)
        {

        }

    }
}
