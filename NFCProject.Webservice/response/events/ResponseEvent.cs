using NFCProject.Webservice.models;
using NFCProject.Webservice.response._default;


namespace NFCProject.Webservice.response.events
{
    public class ResponseEvent
    {
        public ResponseEvent()
        {
            this.Status = new ResponseStatus();
        }
        

        public ResponseStatus Status { get; set; }
        public Event Event { get; set; }

    }
}
