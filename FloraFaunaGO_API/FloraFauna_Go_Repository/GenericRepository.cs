using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;

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

        public virtual async Task<TEntity?> GetById(object id)
            => await Set.FindAsync(id);
    

        public virtual Task<TEntity?> Insert(TEntity item)
        {
            if (Set.Entry(item).IsKeySet)
            {
                return Task.FromResult<TEntity?>(null);
            }
            var entry = Set.Add(item);
            return Task.FromResult<TEntity?>(entry.Entity);
        }

        public virtual void Insert(params TEntity[] entities)
        {
            foreach (TEntity entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual Task<TEntity?> Update(object id, TEntity item)
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

        public virtual async Task<bool> Delete(object id)
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
