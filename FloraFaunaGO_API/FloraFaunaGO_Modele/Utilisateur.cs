using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FloraFaunaGO_Modele;

public class Utilisateur
{
    private uint id;

    public uint Id
    {
        get { return id; }
        set
        {
            if (value < 0) id = 0;
            else id = value;
        }
    }

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
            else mail = this.Pseudo + "@mail.com";
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

    private List<uint> lidCapture;
    public List<uint> Lidcapture
    {
        get { return lidCapture; }
        set
        {
            if (value == null) return;
            if (lidCapture == null) lidCapture = new List<uint>();
            lidCapture = value;
        }
    }

    public Utilisateur(uint id, string pseudo, string mail, string hash_mdp, DateTime date_inscription, List<uint> lidcapture)
    {
        Id = id;
        Pseudo = pseudo;
        Mail = mail;
        Hash_mdp = hash_mdp;
        DateInscription = date_inscription;
        Lidcapture = lidcapture;
    }
}
