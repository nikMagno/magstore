using System.Data;

namespace MagStore.Connection
{
    public interface IConnection
    {
        IDbConnection OpenConnection();
    }
}