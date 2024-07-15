namespace kafkaclient.web.Core.Interfaces;

public interface IGenericRepository<T> : 
    IReadOnlyRepository<T>,
    ICreateOnlyRepository<T>,
    IUpdateOnlyRepository<T>,
    IDeleteOnlyRepository<T> where T : class
{
    
}