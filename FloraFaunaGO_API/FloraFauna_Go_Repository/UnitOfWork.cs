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
    public class UnitOfWork : IUnitOfWork<EspeceEntities, EspeceEntities, CaptureEntities, CaptureEntities, CaptureDetailsEntities, CaptureDetailsEntities, UtilisateurEntities, UtilisateurEntities, SuccesEntities, SuccesEntities, SuccesStateEntities, SuccesStateEntities>
    {
        private FloraFaunaGoDB Context { get; set; }

        public UnitOfWork(FloraFaunaGoDB context)
        {
            Context = context;
            Context.Database.EnsureCreated();
        }

        public IUserRepository<UtilisateurEntities, UtilisateurEntities> UserRepository
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

        private IUserRepository<UtilisateurEntities, UtilisateurEntities> userRepository;

        public ICaptureRepository<CaptureEntities, CaptureEntities> CaptureRepository
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

        private ICaptureRepository<CaptureEntities, CaptureEntities> captureRepository;

        public ICaptureDetailRepository<CaptureDetailsEntities, CaptureDetailsEntities> CaptureDetailRepository
        {
            get
            {
                if (captureDetailRepository == null)
                {
                    captureDetailRepository = new CaptureDetailRepository(Context);
                }
                return captureDetailRepository;
            }
        }

        private ICaptureDetailRepository<CaptureDetailsEntities, CaptureDetailsEntities> captureDetailRepository;

        public IEspeceRepository<EspeceEntities, EspeceEntities> EspeceRepository
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

        private IEspeceRepository<EspeceEntities, EspeceEntities> especeRepository;

        public ISuccessRepository<SuccesEntities, SuccesEntities> SuccessRepository
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

        private ISuccessRepository<SuccesEntities, SuccesEntities> successRepository;

        public ISuccessStateRepository<SuccesStateEntities, SuccesStateEntities> SuccessStateRepository
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

        private ISuccessStateRepository<SuccesStateEntities, SuccesStateEntities> successStateRepository;

        public async Task<bool> AddNewCaptureAsync(CaptureEntities capture, UtilisateurEntities user)
        {
            try
            {
                if (await CaptureRepository.Insert(capture) == null)
                {
                    Context.Captures.Attach(capture);
                    await Context.Entry(capture).ReloadAsync();
                }
                
                if (user == null || user.Captures == null)
                    throw new ArgumentNullException(nameof(user.Captures));

                user.Captures.Add(capture);

                if (await UserRepository.Update(user.Id, user) == null)
                {
                    Context.Utilisateur.Attach(user);
                    await Context.Entry(user).ReloadAsync();
                }

                await SaveChangesAsync();
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> AddNewCaptureDetailAsync(CaptureDetailsEntities captureDetail)
        {
            try
            {
                await CaptureDetailRepository.Insert(captureDetail);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> DeleteEspeceAsync(EspeceEntities espece)
        {
            try
            {
                await EspeceRepository.Delete(espece.Id);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> DeleteSuccessAsync(SuccesEntities success)
        {
            try
            {
                await SuccessRepository.Delete(success.Id);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> AddSuccesStateAsync(SuccesStateEntities successState)
        {
            try
            {
                await SuccessStateRepository.Insert(successState);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> DeleteSuccessStateAsync(SuccesStateEntities successState)
        {
            try
            {
                await SuccessStateRepository.Delete(successState.Id);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> AddUserAsync(UtilisateurEntities user)
        {
            try
            {
                await UserRepository.Insert(user);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(UtilisateurEntities user)
        {
            try
            {
                await UserRepository.Delete(user.Id);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> LoginUserAsync(UtilisateurEntities user)
        {
            // Implémentation de la logique de connexion utilisateur
            throw new NotImplementedException();
        }

        public async Task<bool> LogoutUserAsync(UtilisateurEntities user)
        {
            // Implémentation de la logique de déconnexion utilisateur
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterUserAsync(UtilisateurEntities user)
        {
            try
            {
                await UserRepository.Insert(user);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
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
