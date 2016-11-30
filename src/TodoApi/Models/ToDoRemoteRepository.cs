using System;
using System.Collections.Generic;

using DataObjects;
using DataObjects.RPCRequests;

namespace TodoApi.Models
{
    public class TodoRemoteRepository : ITodoRepository
    {
        private RpcClient _rpcClient;


        public TodoRemoteRepository()
        {
            _rpcClient = new RpcClient();
        }

        public IEnumerable<TodoItem> GetAll()
        {
            var command = new RpcRequest<GetAllDto>(new GetAllDto());
            return _rpcClient.Call<GetAllDto, IEnumerable<TodoItem>>(command);
        }

        public void Add(TodoItem item)
        {
            var command = new RpcRequest<AddItemDto>(new AddItemDto
            {
                Key = Guid.NewGuid(),
                Description = item.Description,
                IsComplete = item.IsComplete,
                TimeCreated = DateTime.UtcNow
            } );

            // We aren't interested in the result, just send the message synchronously
            _rpcClient.Call<AddItemDto, string>(command);
        }

        public TodoItem Find(Guid key)
        {
            return null;
        }

        public TodoItem Remove(Guid key)
        {
            return null;
        }

        public void Update(TodoItem item)
        {
        }
    }
}
