namespace TaxiDigital.Application.Common.Storage;

public interface IStorageService
{
    Task<bool> AddMessage(string Queue, string req);
}
