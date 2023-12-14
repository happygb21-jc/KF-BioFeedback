using SqlSugar;

namespace Kingfar.BioFeedback.DataAccess
{
    public interface ISqlSugarRepository<TEntity> : ISimpleClient<TEntity> where TEntity : class, new()
    {
    }
}