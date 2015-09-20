using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECA.Core.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class TestDbSet<TEntity> : DbSet<TEntity>, IQueryable, IEnumerable<TEntity>, IDbAsyncEnumerable<TEntity>
        where TEntity : class
    {
        ObservableCollection<TEntity> _data;
        IQueryable _query;

        /// <summary>
        /// 
        /// </summary>
        public TestDbSet()
        {
            _data = new ObservableCollection<TEntity>();
            _query = _data.AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override TEntity Add(TEntity item)
        {
            _data.Add(item);
            return item;
        }
        /// <summary>
        /// Method to remove a range from an IEnumberable of Entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public override IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                var index = _data.IndexOf(entity);
                _data.RemoveAt(index);
            }
            return _data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override TEntity Remove(TEntity item)
        {
            _data.Remove(item);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override TEntity Attach(TEntity item)
        {
            _data.Add(item);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDerivedEntity"></typeparam>
        /// <returns></returns>
        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        public override ObservableCollection<TEntity> Local
        {
            get { return _data; }
        }

        /// <summary>
        /// 
        /// </summary>
        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        /// <summary>
        /// 
        /// </summary>
        Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        /// <summary>
        /// 
        /// </summary>
        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<TEntity>(_query.Provider); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<TEntity>(_data.GetEnumerator());
        }
    }

    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    } 
}
