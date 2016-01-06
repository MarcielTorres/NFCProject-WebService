using NFCProject.Webservice.models;
using NFCProject.Webservice.response._default;

namespace NFCProject.Webservice.response.subscribers
{
    public class ResponseSubscriber
    {
        public ResponseSubscriber()
        {
            this.Status = new ResponseStatus();
        }

        public ResponseStatus Status { get; set; }

        public Subscriber Subscriber { get; set; }
    }
}
