using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;

namespace FloraFauna_Go_Repository
{
    class UnitOfWork: IUnitOfWork<EspeceEntities, CaptureEntities, UtilisateurEntities>
    {
        private FloraFaunaGoDB Context { get; set; }

        public UnitOfWork(FloraFaunaGoDB context)
        {
            Context = context;
            Context.Database.EnsureCreated();
        }

        public IUserRepository<UtilisateurEntities> UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(Context);
                }
                return userRepository;
            }
        }

        private IUserRepository<UtilisateurEntities> userRepository;

        public ICaptureRepository<CaptureEntities> CaptureRepository
        {
            get
            {
                if (captureRepository == null)
                {
                    captureRepository = new CaptureRepository(Context);
                }
                return captureRepository;
            }
        }

        private ICaptureRepository<CaptureEntities> captureRepository;

        public IEspeceRepository<EspeceEntities> EspeceRepository 
        {
            get
            {
                if(especeRepository == null)
                {
                    especeRepository = new EspeceRepository(Context);
                }
                return especeRepository;
            }
        }

        private IEspeceRepository<EspeceEntities> especeRepository;



        public Task<bool> AddNewCapture(IEnumerable<CaptureEntities> capture, IEnumerable<UtilisateurEntities> user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddNewEspece(IEnumerable<EspeceEntities> espece)
        {
            throw new NotImplementedException();
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

        public async Task<IEnumerable<object?>?> SaveChangesAsync()
        {
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                await RejectChangesAsync();
                return null;
            }
            var entries = Context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached).ToList();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
            return entries.Select(entry => entry.Entity).Where(e => e is EspeceEntities || e is CaptureEntities || e is UtilisateurEntities);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                Context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
