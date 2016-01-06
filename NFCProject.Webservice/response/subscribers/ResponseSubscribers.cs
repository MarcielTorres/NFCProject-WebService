using NFCProject.Webservice.models;
using NFCProject.Webservice.response._default;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NFCProject.Webservice.response.subscribers
{
    [DataContract]
    public class ResponseSubscribers
    {
        public ResponseSubscribers()
        {
            this.Subscribers = new List<Subscriber>();
            this.Status = new ResponseStatus();
        }

        [DataMember]
        public List<Subscriber> Subscribers { get; set; }

        [DataMember]
        public ResponseStatus Status { get; set; }
    }
}
