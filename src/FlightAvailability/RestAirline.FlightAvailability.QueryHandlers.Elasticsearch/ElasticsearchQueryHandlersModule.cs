using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;

namespace RestAirline.FlightAvailability.QueryHandlers.Elasticsearch
{
    public class ElasticsearchQueryHandlersModule : IModule
    {
        public void Register(IEventFlowOptions eventFlowOptions)
        {
            eventFlowOptions.AddQueryHandlers(typeof(ElasticsearchQueryHandlersModule).Assembly);
        }
    }
}