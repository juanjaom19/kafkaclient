using kafkaclient.web.Core.Entities;
using kafkaclient.web.Core.Interfaces;

namespace kafkaclient.web.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IClusterRepository clusterRepository)
    {
        Clusters = clusterRepository;
    }

    public IClusterRepository Clusters { get; }
}