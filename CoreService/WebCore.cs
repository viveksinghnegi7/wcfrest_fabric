using Contracts;
using System.Fabric;
using System.Threading.Tasks;

namespace CoreService
{
    public class WebCore : ICoreService
    {
        public StatelessServiceContext Context {get;set;}

        public string[] Home()
        {
            return new string[] { this.Context.NodeContext.NodeName, "PresidentName?id=0", "Presidents" };
        }

        public Task<string> PresidentName(int id)
        {
            return Task.Run<string>(() =>
                string.Format("Node: {0} | PresidentName:{1}", this.Context.NodeContext.NodeName, "Rajeev Kumar")
            );
        }

        public Task<string> Presidents()
        {
            return Task.Run<string>(() =>
                string.Format("Node: {0} | Presidents:{1}", this.Context.NodeContext.NodeName, string.Join(", ", "Rajeev Kumar", "Rajeev Kumar")
            ));
        }
    }
}
