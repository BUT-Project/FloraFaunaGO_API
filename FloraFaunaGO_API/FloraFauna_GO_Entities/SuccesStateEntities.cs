namespace FloraFauna_GO_Entities
{
    public class SuccesStateEntities : BaseEntity
    {
        public double PercentSucces { get; set; }

        public bool IsSucces { get; set; } = false;

        public string SuccesEntitiesId { get; set; }
        public SuccesEntities SuccesEntities { get; set; }

        public string UtilisateurId { get; set; }
        public UtilisateurEntities UtilisateurEntities { get; set; }
    }
}
