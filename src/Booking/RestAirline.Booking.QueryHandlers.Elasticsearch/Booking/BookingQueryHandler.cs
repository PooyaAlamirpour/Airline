using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Elasticsearch.ReadStores;
using EventFlow.Queries;
using Nest;
using RestAirline.Booking.Queries.Elasticsearch.Booking;
using RestAirline.Booking.ReadModel.Elasticsearch;

namespace RestAirline.Booking.QueryHandlers.Elasticsearch.Booking
{
    public class BookingQueryHandler : IQueryHandler<BookingIdQuery, BookingReadModel>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IReadModelDescriptionProvider _modelDescriptionProvider;

        public BookingQueryHandler(IElasticClient elasticClient, IReadModelDescriptionProvider modelDescriptionProvider)
        {
            _elasticClient = elasticClient;
            _modelDescriptionProvider = modelDescriptionProvider;
        }

        public async Task<BookingReadModel> ExecuteQueryAsync(BookingIdQuery query, CancellationToken cancellationToken)
        {
            var index = _modelDescriptionProvider.GetReadModelDescription<BookingReadModel>().IndexName;


            var getResponse = await _elasticClient.GetAsync<BookingReadModel>(
                    query.BookingId,
                    d => d
                        .RequestConfiguration(c => c
                            .AllowedStatusCodes((int) HttpStatusCode.NotFound))
                        .Index(index.Value),
                    cancellationToken)
                .ConfigureAwait(false);


            var searchResponse = await _elasticClient.SearchAsync<BookingReadModel>(d => d
                        .RequestConfiguration(c => c
                            .AllowedStatusCodes((int) HttpStatusCode.NotFound))
                        .Index(index.Value)
                        .Query(q => q.Match(m => m.Field(f => f.Id).Query(query.BookingId))),
                    cancellationToken)
                .ConfigureAwait(false);

//            return searchResponse.Documents
//                .First();

            return getResponse.Source;
        }
    }
}