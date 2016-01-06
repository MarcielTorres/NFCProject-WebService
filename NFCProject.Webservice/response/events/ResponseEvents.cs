using NFCProject.Webservice.models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NFCProject.Webservice.response.events
{
    [DataContract]
    public class ResponseEvents
    {
        public ResponseEvents()
        {
            this.Events = new List<Event>();
        }
        
        [DataMember]
        public List<Event> Events { get; set; }
    }
}
