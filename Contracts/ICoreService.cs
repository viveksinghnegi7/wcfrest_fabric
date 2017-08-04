using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface ICoreService
    {
        [OperationContract]
        [WebGet(UriTemplate = "", ResponseFormat = WebMessageFormat.Json)]
        string[] Home();

        [OperationContract]
        [WebGet(UriTemplate = "PresidentName?id={id}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> PresidentName(int id);

        [OperationContract]
        [WebGet(UriTemplate = "Presidents", ResponseFormat = WebMessageFormat.Json)]
        Task<string> Presidents();
    }
}
