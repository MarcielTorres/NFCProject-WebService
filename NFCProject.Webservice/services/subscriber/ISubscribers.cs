using NFCProject.Webservice.models;
using NFCProject.Webservice.response.subscribers;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace NFCProject.Webservice.services.subscriber
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISubscribers" in both code and config file together.
    [ServiceContract]
    public interface ISubscribers
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CreateSubscriber", BodyStyle = WebMessageBodyStyle.Wrapped)]
        ResponseSubscriber CreateSubscriber(Subscriber Model);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "EditSubscriber", BodyStyle = WebMessageBodyStyle.Wrapped)]
        ResponseSubscriber EditSubscriber(Subscriber Model);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RemoveSubscriber/{_id}")]
        ResponseSubscriber RemoveSubscriber(string _id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ListSubscribers")]
        ResponseSubscribers ListSubscribers();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ShowSubscriber/{_id}")]
        ResponseSubscriber ShowSubscriber(string _id);
    }
}
