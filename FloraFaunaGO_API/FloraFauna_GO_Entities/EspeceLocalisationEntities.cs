namespace FloraFauna_GO_Entities
{
    public class EspeceLocalisationEntities
    {
        public string EspeceId { get; set; }
        public EspeceEntities Espece { get; set; }

        public string LocalisationId { get; set; }
        public LocalisationEntities Localisation { get; set; }
    }
}
