using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Contracts;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Globalization;
using System.ServiceModel.Description;

namespace CoreService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class CoreService : StatelessService
    {
        public CoreService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            const int bufferSize = 512000; //500KB
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                SendTimeout = TimeSpan.FromSeconds(30),
                ReceiveTimeout = TimeSpan.FromSeconds(30),
                CloseTimeout = TimeSpan.FromSeconds(30),
                MaxReceivedMessageSize = bufferSize,
                MaxBufferSize = bufferSize,
                MaxBufferPoolSize = bufferSize * Environment.ProcessorCount,
            };

            var webBinding = new WebHttpBinding(WebHttpSecurityMode.None);
            var serviceUri = string.Empty;

            yield return new ServiceInstanceListener(context =>
            {
                string host = context.NodeContext.IPAddressOrFQDN;
                serviceUri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/Core/", "http", host, 8001);
                var address = new EndpointAddress(serviceUri); //

                var webCore = new WebCore { Context = context };

                var listener =  new WcfCommunicationListener<ICoreService>(context, webCore, webBinding, address);

                var eb = listener.ServiceHost.Description.Endpoints.Last();
                eb.EndpointBehaviors.Add(new WebHttpBehavior());
                return listener;
            });
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }
}
