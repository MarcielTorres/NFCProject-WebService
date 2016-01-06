using System.Runtime.Serialization;

namespace NFCProject.Webservice.response._default
{
    [DataContract]
    public class ResponseStatus
    {
        [DataMember(Name = "Code", Order = 3)]
        public string Code { get; set; }

        [DataMember(Name = "Message", Order = 2)]
        public string Message { get; set; }

        [DataMember(Name = "Success", Order = 1)]
        public string Success { get; set; }
    }
}
