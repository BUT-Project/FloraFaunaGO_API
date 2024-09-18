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

    private DateTime date_inscription;
    public DateTime DateInscription
    {
        get { return date_inscription; }
        set
        {
            if (value < DateTime.Now) date_inscription = DateTime.Now;
            else date_inscription = value;
        }
    }

    private List<CaptureEntities> captures;
    public List<CaptureEntities> Captures => new List<CaptureEntities>();

    public UtilisateurEntities(string pseudo, string mail, string hash_mdp, DateTime date_inscription)
    {
        Pseudo = pseudo;
        Mail = mail;
        Hash_mdp = hash_mdp;
        DateInscription = date_inscription;
        captures = new List<CaptureEntities>();
    }
}
