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
            get
            {
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

        public async Task<bool> AddSuccesStateAsync(SuccesStateEntities successState, UtilisateurEntities user, SuccesEntities success)
        {
            try
            {
                successState.UtilisateurEntities = user;
                successState.SuccesEntities = success;

                if (await SuccessStateRepository.Insert(successState) == null)
                {
                    Context.SuccesState.Attach(successState);
                    await Context.Entry(successState).ReloadAsync();
                }

                if (await UserRepository.Insert(user) == null)
                {
                    Context.Utilisateur.Attach(user);
                    await Context.Entry(user).ReloadAsync();
                }

                if (await SuccessRepository.Insert(success) == null)
                {
                    Context.Succes.Attach(success);
                    await Context.Entry(success).ReloadAsync();
                }

                if (user.SuccesState == null)
                {
                    user.SuccesState = new List<SuccesStateEntities>();
                }
                user.SuccesState.Add(successState);

                if (await UserRepository.Update(user.Id, user) == null)
                {
                    Context.Utilisateur.Attach(user);
                    await Context.Entry(user).ReloadAsync();
                }

                if (success.SuccesStates == null)
                {
                    success.SuccesStates = new List<SuccesStateEntities>();
                }
                success.SuccesStates.Add(successState);

                if (await SuccessRepository.Update(success.Id, success) == null)
                {
                    Context.Succes.Attach(success);
                    await Context.Entry(success).ReloadAsync();
                }

                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> DeleteSuccesStateAsync(SuccesStateEntities successState, UtilisateurEntities user, SuccesEntities success)
        {
            try
            {
                await SuccessStateRepository.Delete(successState.Id);

                // Remove successState from the user's SuccesState collection
                if (user.SuccesState != null)
                {
                    user.SuccesState.Remove(successState);
                }

                // Remove successState from the success's State collection
                if (success.SuccesStates != null)
                {
                    success.SuccesStates.Remove(successState);
                }

                if (await UserRepository.Update(user.Id, user) == null)
                {
                    Context.Utilisateur.Attach(user);
                    await Context.Entry(user).ReloadAsync();
                }

                if (await SuccessRepository.Update(success.Id, success) == null)
                {
                    Context.Succes.Attach(success);
                    await Context.Entry(success).ReloadAsync();
                }

                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> AddCaptureAsync(CaptureEntities capture, UtilisateurEntities user)
        {
            try
            {
                if (await CaptureRepository.Insert(capture) == null)
                {
                    Context.Captures.Attach(capture);
                    await Context.Entry(capture).ReloadAsync();
                }

                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                if (user.Captures == null)
                    user.Captures = new List<CaptureEntities>();

                user.Captures.Add(capture);

                if (await UserRepository.Update(user.Id, user) == null)
                {
                    Context.Utilisateur.Attach(user);
                    await Context.Entry(user).ReloadAsync();
                }

                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> DeleteCaptureAsync(CaptureEntities capture, UtilisateurEntities user, IEnumerable<CaptureDetailsEntities> captureDetails)
        {
            try
            {
                foreach (var detail in captureDetails)
                {
                    await CaptureDetailRepository.Delete(detail.Id);
                }

                await CaptureRepository.Delete(capture.Id);

                if (await UserRepository.Update(user.Id, user) == null)
                {
                    Context.Utilisateur.Attach(user);
                    await Context.Entry(user).ReloadAsync();
                }

                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> AddCaptureDetailAsync(CaptureDetailsEntities captureDetail, CaptureEntities capture)
        {
            try
            {
                if (await CaptureDetailRepository.Insert(captureDetail) == null)
                {
                    Context.CaptureDetails.Attach(captureDetail);
                    await Context.Entry(captureDetail).ReloadAsync();
                }

                if (capture.CaptureDetails == null)
                {
                    capture.CaptureDetails = new List<CaptureDetailsEntities>();
                }
                capture.CaptureDetails.Add(captureDetail);

                if (await CaptureRepository.Update(capture.Id, capture) == null)
                {
                    Context.Captures.Attach(capture);
                    await Context.Entry(capture).ReloadAsync();
                }


                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> DeleteCaptureDetailAsync(CaptureDetailsEntities captureDetail, CaptureEntities capture)
        {
            try
            {
                await CaptureDetailRepository.Delete(captureDetail.Id);
                capture.CaptureDetails.Remove(captureDetail);

                if (await CaptureRepository.Update(capture.Id, capture) == null)
                {
                    Context.Captures.Attach(capture);
                    await Context.Entry(capture).ReloadAsync();
                }

                
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> DeleteUser(UtilisateurEntities user, IEnumerable<CaptureEntities> captures, IEnumerable<SuccesStateEntities> successStates)
        {
            try
            {
                foreach (var capture in captures)
                {
                    foreach (var captureDetail in capture.CaptureDetails)
                    {
                        await CaptureDetailRepository.Delete(captureDetail.Id);
                    }
                    await CaptureRepository.Delete(capture.Id);
                }

                foreach (var successState in successStates)
                {
                    await SuccessStateRepository.Delete(successState.Id);
                }

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
