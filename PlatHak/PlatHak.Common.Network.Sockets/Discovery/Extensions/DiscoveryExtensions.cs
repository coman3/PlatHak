using PlatHak.Common.Network.Sockets.Discovery.ServiceDefinition;

namespace PlatHak.Common.Network.Sockets.Discovery.Extensions
{
    public static class DiscoveryExtensions
    {
        public static ByteArrayServiceDiscoverer CreateServiceDiscoverer(this IServiceDefinition serviceDefinition)
        {
            return new ByteArrayServiceDiscoverer(serviceDefinition);
        }

        public static ServicePublisher<IServiceDefinition> CreateServicePublisher(this IServiceDefinition serviceDefinition)
        {
            return new ServicePublisher<IServiceDefinition>(serviceDefinition);
        }
    }
}