﻿using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Tests.Clients
{
    [Path("api")]
    public interface IReturnClientWithMetadata : IReturnClient
    {
        [GetMethod]
        new Task<BasicEntity> GetAsync(int id);

        [GetMethod]
        new BasicEntity Get(int id);

        [PostMethod]
        new Task PostAsync(BasicEntity entity);

        [PostMethod]
        new void Post(BasicEntity entity);
    }
}
