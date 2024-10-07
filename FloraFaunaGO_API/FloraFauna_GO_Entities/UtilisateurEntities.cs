using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FloraFaunaGO_Modele;

public class UtilisateurEntities
{
    private Guid id;

    public Guid Id { get; }

    private string pseudo;
    public string Pseudo
    {
        get { return pseudo; }
        set
        {
            if (string.IsNullOrEmpty(value)) pseudo = "Inconnu";
            else pseudo = value;
        }
    }

    private string mail;
    public string Mail
    {
        get { return mail; }
        set
        {
            if (value.Contains("@") && value.Contains(".")) mail = value;
            else mail = Pseudo + "@mail.com";
        }
    }

    private string hash_mdp;
    public string Hash_mdp
    {
        get { return hash_mdp; }
        set
        {
            if (string.IsNullOrEmpty(value) && hash_mdp == null) hash_mdp = "m0ts_2_pa$$e";
            else if (hash_mdp != null) return;
            else hash_mdp = value;
        }
    }

    private DateTime dateInscription;
    public DateTime DateInscription
    {
        get { return dateInscription; }
        set
        {
            if (value < DateTime.Now) dateInscription = DateTime.Now;
            else dateInscription = value;
        }
    }

    private List<CaptureEntities> captures;
    public List<CaptureEntities> Captures => captures;

    public UtilisateurEntities() { }
}
