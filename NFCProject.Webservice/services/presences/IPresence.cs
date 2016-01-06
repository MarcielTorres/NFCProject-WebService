using NFCProject.Webservice.models;
using NFCProject.Webservice.response.presences;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace NFCProject.Webservice.services.presences
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPresence" in both code and config file together.
    [ServiceContract]
    public interface IPresence
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ListPresences")]
        ResponsePresences ListPresences();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ConfirmPresence", BodyStyle = WebMessageBodyStyle.Wrapped)]
        ResponsePresences ConfirmPresence(PresenceControl Model);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "VerifySubscriber", BodyStyle = WebMessageBodyStyle.Wrapped)]
        ResponsePresences VerifySubscriber(PresenceControl Model);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RemovePresence/{id}")]
        ResponsePresences RemovePresence(string id);

    }
}
