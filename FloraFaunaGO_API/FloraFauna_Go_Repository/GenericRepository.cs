using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FloraFauna_Go_Repository
{
    public abstract class GenericRepository<TEntity> :
        IGenericRepository<TEntity> where TEntity : class
    {

        protected DbContext Context { get; private set; }
        protected DbSet<TEntity> Set { get; private set; }

        protected GenericRepository(DbContext context) 
        {
           Context = context;
           Set = Context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetById(string id)
            => await Set.FindAsync(id);

        public virtual Pagination<TEntity> GetItems(Expression<Func<TEntity, bool>>? filter = null,
                                         int index = 0, int count = 10,
                                         params string[] includeProperties)
            => GetItems(filter, null, index, count, includeProperties);

        public virtual Pagination<TEntity> GetItems(Func<IQueryable<TEntity>, IQueryable<TEntity>>? orderBy = null,
                                             int index = 0, int count = 10,
                                             params string[] includeProperties)
            => GetItems(null, orderBy, index, count, includeProperties);

        public virtual Pagination<TEntity> GetItems(int index = 0, int count = 10,
                                             params string[] includeProperties)
            => GetItems(null, null, index, count, includeProperties);

        public virtual Pagination<TEntity> GetItems(Expression<Func<TEntity, bool>>? filter = null,
                                         Func<IQueryable<TEntity>, IQueryable<TEntity>>? orderBy = null,
                                         int index = 0, int count = 10,
                                         params string[] includeProperties)
        {
            IQueryable<TEntity> query = Set;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (string includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            long totalCount = query.LongCount();
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return new Pagination<TEntity>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = query.Skip(index * count)
                        .Take(count)
                        .ToList()
            };
        }

        public virtual Task<TEntity?> Insert(TEntity item)
        {
            var entry = Set.Add(item);
            var toto = Task.FromResult<TEntity?>(entry.Entity);
            return toto;
        }

        public virtual void Insert(params TEntity[] entities)
        {
            foreach (TEntity entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual Task<TEntity?> Update(string id, TEntity item)
        {
            var originalEntity = Set.Find(id);
            if (originalEntity == null)
            {
                return Task.FromResult<TEntity?>(null);
            }

            var originalEntry = Set.Entry(originalEntity);
            var keyName = originalEntry.Metadata.FindPrimaryKey()?
                                        .Properties[0]?.Name;

            var values = typeof(TEntity).GetProperties()
                                        .Where(item => item.Name != keyName && item.CanWrite)
                                        .ToDictionary(item => item.Name, item => item.GetValue(item));
            
            originalEntry.CurrentValues.SetValues(values);
            Set.Attach(originalEntity);
            Context.Entry(originalEntity).State = EntityState.Modified;
            return Task.FromResult<TEntity?>(originalEntity);
        }

        public virtual async Task<bool> Delete(string id)
        {
            TEntity? entity = await Set.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Set.Attach(entity);
            }
            Set.Remove(entity);
            return true;
        }

        public async Task RejectChangesAsync()
        {
            foreach (var entry in Context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        await entry.ReloadAsync();
                        break;
                }
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await Context.SaveChangesAsync();
            foreach (var entity in Context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached))
            {
                entity.State = EntityState.Detached;
            }
            return result;
        }
    }
}
