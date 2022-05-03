using System.Linq.Expressions;

namespace AuctionBackend.Application.Helper
{
    public static class Extensions
    {
        public static Expression<Func<T, bool>> Inverse<T>(this Expression<Func<T, bool>> e)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(e.Body),
                e.Parameters[0]);
        }
    }
}
