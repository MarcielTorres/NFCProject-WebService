using NFCProject.Webservice.models;
using NFCProject.Webservice.response._default;
using System.Collections.Generic;

namespace NFCProject.Webservice.response.presences
{
    public class ResponsePresences
    {
        public ResponsePresences()
        {
            this.Status = new ResponseStatus();
            this.Presences = new List<PresenceControl>();
        }

        public ResponseStatus Status { get; set; }

        public List<PresenceControl> Presences { get; set; }
    }
}
