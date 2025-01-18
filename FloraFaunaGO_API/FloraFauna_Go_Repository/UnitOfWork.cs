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
    public class UnitOfWork : IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities, SuccesEntities, SuccesStateEntities>
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

        public ICaptureDetailRepository<CaptureDetailsEntities> CaptureDetailRepository
        {
            get {
                if (captureDetailRepository == null)
                {
                    captureDetailRepository = new CaptureDetailRepository(Context);
                }
                return captureDetailRepository;
            }
        }

        private ICaptureDetailRepository<CaptureDetailsEntities> captureDetailRepository;

        public IEspeceRepository<EspeceEntities> EspeceRepository
        {
            get
            {
                if (especeRepository == null)
                {
                    especeRepository = new EspeceRepository(Context);
                }
                return especeRepository;
            }
        }

        private IEspeceRepository<EspeceEntities> especeRepository;

        public ISuccessRepository<SuccesEntities> SuccessRepository
        {
            get
            {
                if (successRepository == null)
                {
                    successRepository = new SuccessRepository(Context);
                }
                return successRepository;
            }
        }

        private ISuccessRepository<SuccesEntities> successRepository;

        public ISuccessStateRepository<SuccesStateEntities> SuccessStateRepository
        {
            get
            {
                if (successStateRepository == null)
                {
                    successStateRepository = new SuccessStateRepository(Context);
                }
                return successStateRepository;
            }
        }

        private ISuccessStateRepository<SuccesStateEntities> successStateRepository;

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
