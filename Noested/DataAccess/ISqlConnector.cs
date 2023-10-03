using Noested.Entities;
using System.Data;

namespace Noested.DataAccess
{
    public interface ISqlConnector
    {
        IDbConnection GetDbConnection();
    }
}