namespace kafkaclient.web.Core.Interfaces;

public interface IUnitOfWork
{
    IClusterRepository Clusters { get; } 
}