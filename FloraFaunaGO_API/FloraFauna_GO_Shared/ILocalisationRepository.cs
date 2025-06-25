namespace FloraFauna_GO_Shared
{
    public interface ILocalisationRepository<Tinput, Toutput> : IGenericRepository<Tinput, Toutput>
        where Tinput : class
        where Toutput : class
    {
        Task<Pagination<Toutput>> GetAllLocalisation(int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetLocalisationByCaptureDetail(string idCaptureDetail, int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetLocalisationByEspece(string isEspece, int index = 0, int count = 15);
    }

    public interface ILocalisationRepository<T> : ILocalisationRepository<T, T>
        where T : class
    {

    }
}
