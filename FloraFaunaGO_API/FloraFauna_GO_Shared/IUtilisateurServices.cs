using FloraFaunaGO_Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Shared
{
    internal interface IUtilisateurServices
    {
        Task<UtilisateurEntities> GetUtilisateurEntities(Guid id);

        Task<bool> DeleteUtilisateurEntities(Guid id);
    }
}
