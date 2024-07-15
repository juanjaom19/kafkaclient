using kafkaclient.web.Core.Entities;
using kafkaclient.web.Core.Interfaces;
using kafkaclient.web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace kafkaclient.web.Infrastructure.Respository;

public class ClusterRepository : 
    GenericRepository<Cluster>, IClusterRepository
{
    private readonly DbSet<Cluster> _clusterContext;

    public ClusterRepository(ApplicationDbContext db): base(db)
    {
        _clusterContext = db.Set<Cluster>();
    }
}