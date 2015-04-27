using System;
using System.Linq.Expressions;
using LinqKit;

namespace Bookmarky.DAL.ServiceImplementations
{
    public class SearchCriteriaBuilder<T>
    {
        private readonly bool _isAnd;
        private Expression<Func<T, bool>> _predicate;

        public SearchCriteriaBuilder(bool isAnd)
        {
            _isAnd = isAnd;
            _predicate = isAnd ? PredicateBuilder.True<T>() : PredicateBuilder.False<T>();
        }

        public SearchCriteriaBuilder<T> AppendCriteria(Expression<Func<T, bool>> predicate)
        {
            if (_isAnd)
                _predicate = _predicate.And(predicate);
            else
            {
                _predicate = _predicate.Or(predicate);
            }

            return this;
        }


        public Expression<Func<T, bool>> BuildPredicate()
        {
            return _predicate;
        }
    }
}