using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Quest
{
    public bool Active;     //La quête est active ?
    public Joueur player; //Le joueur chargé de la quête

    //La qûete :
    public string Titre;    //Nom de la quête
    public string Description;    //Description de la quête
    public int Avancement;     //L'avancement actuelle de la quête
    public int Finalité;       //L'objectif de la quête 
    public string RésuméRapide; //Pour l'affichage de la liste des quêtes.
    
    //Les récompenses :
    public int RecompenseExperience; //L'XP (peut être 0 mais là c'est chelou)
    public int RecompenseGold; //L'or (peut être 0)
    public Itemscript RecompenseItem; //Les loots (peut être None)
    

    public void Avancer()
    {
        Avancement++;
        if (player.LaQestSuivieMaGueule == this)
        {
            player.QuêteSuivie.text = player.LaQestSuivieMaGueule.RésuméRapide + '\n' +
                                      player.LaQestSuivieMaGueule.Avancement + " / " + player.LaQestSuivieMaGueule.Finalité;
        }
        if (Avancement == Finalité)
        {
            player.Money += RecompenseGold; //On donne l'or
            player.Experience += RecompenseExperience; //On donne l'exp
            Active = false; //On a fini la quête
            if (player.LaQestSuivieMaGueule == this)
            {
                player.Suivie.SetActive(false);
                player.Onsuitunequête = false;
            }
            player.AfficheFinQuest(this);
            player.Questnotdoneyet.Remove(this);
        }
    }
    
}
