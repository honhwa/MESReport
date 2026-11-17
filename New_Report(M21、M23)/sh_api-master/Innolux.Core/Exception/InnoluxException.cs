using Innolux.Core.Extensions.ApiModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;

namespace Innolux.Core.Exception
{
    [Serializable]
    public class InnoluxException : System.Exception
    {



        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        public InnoluxException() : base()
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        public InnoluxException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public InnoluxException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public InnoluxException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }

        public InnoluxException(ErrResult errResult) : base(JsonConvert.SerializeObject(errResult, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }))
        {
        }
    }
}
