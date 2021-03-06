﻿using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Knowzy.Domain;
using Microsoft.Azure.Documents;

namespace Microsoft.Knowzy.OrdersAPI.Data
{
    public class OrdersStore : IOrdersStore
    {
        private readonly DocumentClient _client;
        private Uri _ordersLink;
        private FeedOptions _options = new FeedOptions();

        public OrdersStore(IConfiguration config)
        {
            var EndpointUri = config["COSMOSDB_ENDPOINT"];
            var PrimaryKey = config["COSMOSDB_KEY"];

            _client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            _options.EnableCrossPartitionQuery = true;

            //Make sure the below values match your set up
            _ordersLink = UriFactory.CreateDocumentCollectionUri("knowzy", "orders");
        }

        public async Task<bool> Connected()
        {
            try
            {
                var db = await _client.GetDatabaseAccountAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Shipping> GetShippings()
        {
            return GetOrders<Shipping>("shipping");
        }

        public Shipping GetShipping(string orderId)
        {
            return GetOrder<Shipping>(orderId);
        }

        public IEnumerable<Receiving> GetReceivings()
        {
            return GetOrders<Receiving>("receiving");
        }

        public Receiving GetReceiving(string orderId)
        {
            return GetOrder<Receiving>(orderId);
        }

        public async Task UpsertAsync(Order order)
        {
            await _client.UpsertDocumentAsync(_ordersLink.ToString(), order);
        }

        public async Task DeleteOrderAsync(string orderId)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("knowzydb", "orders", orderId));
        }

        public IEnumerable<PostalCarrier> GetPostalCarriers()
        {
            return _client.CreateDocumentQuery<PostalCarrier>(
                    _ordersLink,
                    "SELECT o.postalCarrier.id, o.postalCarrier.name FROM orders o",
                    _options).ToList().GroupBy(x => x.Name).Select(x => x.First());
        }

        private IEnumerable<T> GetOrders<T>(string orderType)
        {
            return _client.CreateDocumentQuery<T>(
                _ordersLink,
                new SqlQuerySpec
                {
                    QueryText = "SELECT * FROM orders o WHERE (o.type = @ordertype)",
                    Parameters = new SqlParameterCollection()
                        {
                                     new SqlParameter("@ordertype", orderType)
                        }
                },
                _options).ToList();
        }

        private T GetOrder<T>(string orderId)
        {
            return _client.CreateDocumentQuery<T>(
                    _ordersLink,
                    new SqlQuerySpec
                    {
                        QueryText = "SELECT TOP 1 * FROM orders o WHERE (o.id = @orderid)",
                        Parameters = new SqlParameterCollection()
                        {
                                     new SqlParameter("@orderid", orderId)
                        }
                    },
                    _options).ToList().FirstOrDefault();
        }

        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }
                disposedValue = true;
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(true);
        }

    }
}