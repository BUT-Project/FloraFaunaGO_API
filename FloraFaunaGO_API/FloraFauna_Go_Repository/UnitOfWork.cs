﻿using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;

namespace FloraFauna_Go_Repository
{
    public class UnitOfWork : IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities, SuccesEntities, SuccesStateEntities, LocalisationEntities>
    {
        private FloraFaunaGoDB Context { get; set; }

        public UnitOfWork(FloraFaunaGoDB context)
        {
            try
            {
                Context = context;
                Context.Database.EnsureCreated();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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

        public ILocalisationRepository<LocalisationEntities> LocalisationRepository
        {
            get
            {
                if (localisationRepository == null)
                {
                    localisationRepository = new LocalisationRepository(Context);
                }
                return localisationRepository;
            }
        }
        private ILocalisationRepository<LocalisationEntities> localisationRepository;

        public async Task<bool> AddSuccesStateAsync(SuccesStateEntities successState, UtilisateurEntities user, SuccesEntities success)
        {
            try
            {
                successState.SuccesEntitiesId = success.Id;
                successState.UtilisateurId = user.Id;

                if (await SuccessStateRepository.Insert(successState) == null)
                {
                    Context.SuccesState.Attach(successState);
                    await Context.Entry(successState).ReloadAsync();
                }

                if (user.SuccesState == null)
                {
                    user.SuccesState = new List<SuccesStateEntities>();
                }
                user.SuccesState.Add(successState);

                if (success.SuccesStates == null)
                {
                    success.SuccesStates = new List<SuccesStateEntities>();
                }
                success.SuccesStates.Add(successState);

                //await SaveChangesAsync();
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

                successState.UtilisateurId = user.Id;
                successState.SuccesEntitiesId = success.Id;
                successState.UtilisateurEntities = user;
                successState.SuccesEntities = success;

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
                capture.UtilisateurId = user.Id;

                if (await CaptureRepository.Insert(capture) == null)
                {
                    Context.Captures.Attach(capture);
                    await Context.Entry(capture).ReloadAsync();
                }

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

                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> AddCaptureDetailAsync(CaptureDetailsEntities captureDetail, CaptureEntities capture, LocalisationEntities localisation)
        {
            try
            {
                if (await LocalisationRepository.Insert(localisation) == null)
                {
                    Context.Localisation.Attach(localisation);
                    await Context.Entry(localisation).ReloadAsync();
                }

                captureDetail.LocalisationId = localisation.Id;
                captureDetail.CaptureId = capture.Id;


                if (await CaptureDetailRepository.Insert(captureDetail) == null)
                {
                    Context.CaptureDetails.Attach(captureDetail);
                    await Context.Entry(captureDetail).ReloadAsync();
                }

                if (capture.CaptureDetails == null)
                    capture.CaptureDetails = new List<CaptureDetailsEntities>();

                capture.CaptureDetails.Add(captureDetail);

                //if (await CaptureRepository.Update(capture.Id, capture) == null)
                //{
                //    Context.Captures.Attach(capture);
                //    await Context.Entry(capture).ReloadAsync();
                //}


                //await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> DeleteCaptureDetailAsync(CaptureDetailsEntities captureDetail, CaptureEntities capture,
                                                          LocalisationEntities localisation)
        {
            try
            {

                captureDetail.LocalisationId = localisation.Id;
                captureDetail.CaptureId = capture.Id;
                captureDetail.Capture = capture;
                captureDetail.Localisation = localisation;

                // Vérifier si la localisation est référencée par d'autres détails de capture
                var isLocalisationReferenced = await Context.CaptureDetails.AnyAsync(cd => cd.LocalisationId == localisation.Id);

                if (!isLocalisationReferenced)
                {
                    await LocalisationRepository.Delete(localisation.Id);
                }

                if (await CaptureRepository.Update(capture.Id, capture) == null)
                {
                    Context.Captures.Attach(capture);
                    await Context.Entry(capture).ReloadAsync();
                }

                await CaptureDetailRepository.Delete(captureDetail.Id);
                capture.CaptureDetails.Remove(captureDetail);


                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> AddEspeceAsync(EspeceEntities espece, IEnumerable<LocalisationEntities> localisation)
        {
            try
            {
                foreach (var localisationEntity in localisation)
                {
                    if (await LocalisationRepository.Insert(localisationEntity) == null)
                    {
                        Context.Localisation.Attach(localisationEntity);
                        await Context.Entry(localisation).ReloadAsync();
                    }
                }

                if (await EspeceRepository.Insert(espece) == null)
                {
                    Context.Espece.Attach(espece);
                    await Context.Entry(espece).ReloadAsync();
                }

                foreach (var loc in localisation)
                {
                    var especeLocalisation = new EspeceLocalisationEntities
                    {
                        EspeceId = espece.Id,
                        LocalisationId = loc.Id
                    };
                    Context.EspeceLocalisation.Add(especeLocalisation);
                }

                //await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }

        public async Task<bool> DeleteEspeceAsync(EspeceEntities espece, IEnumerable<LocalisationEntities> localisations)
        {
            try
            {
                /*
                 Delete Order:
                1. Delete EspeceLocalisation (idEspece & idLocalisation)
                2. Delete Capture (CaptureDetail with it -> use DeleteCaptureAsync)
                3. Delete Localisation associated with
                4. Delete Espece
                 */
                foreach (var localisation in localisations)
                {
                    var especeLocalisations = await Context.EspeceLocalisation
                        .Where(el => el.EspeceId == espece.Id && el.LocalisationId == localisation.Id)
                        .ToListAsync();

                    foreach (var especeLocalisation in especeLocalisations)
                    {
                        Context.EspeceLocalisation.Remove(especeLocalisation);
                    }

                    var localisationEntity = await Context.Localisation
                        .Include(l => l.EspeceLocalisation)
                        .FirstOrDefaultAsync(l => l.Id == localisation.Id);

                    if (localisationEntity != null)
                    {
                        var especeLocalisationToRemove = localisationEntity.EspeceLocalisation
                            .Where(el => el.EspeceId == espece.Id)
                            .ToList();

                        foreach (var el in especeLocalisationToRemove)
                        {
                            localisationEntity.EspeceLocalisation.Remove(el);
                        }

                        if (!localisationEntity.EspeceLocalisation.Any())
                        {
                            Context.Localisation.Remove(localisationEntity);
                        }
                    }
                }

                var captures = await Context.Captures
                    .Include(c => c.CaptureDetails)
                    .Where(c => c.EspeceId == espece.Id)
                    .ToListAsync();

                foreach (var capturing in captures)
                {
                    var CaptureDetails = (await CaptureDetailRepository.GetCaptureDetailByCapture(capturing.Id)).Items.ToList();
                    var Utilisateur = (await UserRepository.GetUserByCapture(capturing.Id)).Items.FirstOrDefault();
                    await DeleteCaptureAsync(capturing, Utilisateur, CaptureDetails);
                }

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

        public async Task<bool> DeleteUser(UtilisateurEntities user, IEnumerable<CaptureEntities> captures, IEnumerable<SuccesStateEntities> successStates)
        {
            try
            {
                foreach (var capture in captures)
                {
                    await DeleteCaptureAsync(capture, user, (await CaptureDetailRepository.GetCaptureDetailByCapture(capture.Id)).Items);
                }

                foreach (var successState in successStates)
                {
                    await DeleteSuccesStateAsync(successState, user, (await SuccessRepository.GetSuccessBySuccessState(successState.Id)).Items.FirstOrDefault());
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
            return entries.Select(entry => entry.Entity).Where(e => e is EspeceEntities || e is CaptureEntities || e is UtilisateurEntities || e is SuccesEntities || e is SuccesStateEntities || e is LocalisationEntities || e is CaptureDetailsEntities || e is EspeceLocalisationEntities);
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

        public async Task<bool> AddSuccess(SuccesEntities success)
        {
            try
            {
                if (await SuccessRepository.Insert(success) == null)
                {
                    Context.Succes.Attach(success);
                    await Context.Entry(success).ReloadAsync();
                }

                var user = Context.Users.ToList();
                foreach (var u in user)
                {
                    var successState = new SuccesStateEntities
                    {
                        PercentSucces = 0,
                        SuccesEntitiesId = success.Id,
                        IsSucces = false,
                        SuccesEntities = success
                    };
                    successState.UtilisateurId = u.Id;
                    successState.UtilisateurEntities = u;
                    if (await SuccessStateRepository.Insert(successState) == null)
                    {
                        Context.SuccesState.Attach(successState);
                        await Context.Entry(successState).ReloadAsync();
                    }
                }

                //await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                await RejectChangesAsync();
                return false;
            }
        }
    }
}
