using System.ServiceModel;
using NFCProject.Webservice.response.events;
using System.ServiceModel.Web;
using NFCProject.Webservice.models;

namespace NFCProject.Webservice.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEvents" in both code and config file together.
    [ServiceContract]
    public interface IEvents
    {

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,  ResponseFormat = WebMessageFormat.Json, UriTemplate = "CreateEvent", BodyStyle = WebMessageBodyStyle.Wrapped)]
        ResponseEvent CreateEvent(Event Model);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "EditEvent/{_title}/{_description}/{_startDate}/{_endDate}")]
        ResponseEvent EditEvent(string _title, string _description, string _startDate, string _endDate);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RemoveEvent/{_id}")]
        ResponseEvent RemoveEvent(string _id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ListEvents")]
        ResponseEvents ListEvents();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ShowEvent/{_id}")]
        ResponseEvent ShowEvent(string _id);

    }
}
