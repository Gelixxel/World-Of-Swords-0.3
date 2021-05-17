using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestGiverSolo : InteractionSolo
{
    //La Qûete à donner :
    public List<QuestSolo> LesQuêtes;

    public QuestSolo LaQûete;
    //Pour l'item, il faudra faire plus ou moins comme l'inventaire. 
    //J'attend d'avoir l'inventaire pour copier coller la méthode et ne pas avoir de truc différent.

    //Les Quêtes à remplir
    public string[] QuestName; //Nom des quêtes où il faut parler au PNJ, peut être vide si il n'y en a pas
    public List<AudioClip> PhrasesPNG;
    public List<AudioClip> Aurevoirs;
    public GameObject PointDeExclamation;  //Pour dire si il peut donner une quête ou non
    

    public override void Interract(JoueurSolo player)
    {
        base.Interract(player);
        int random = Random.Range(0, PhrasesPNG.Count);
        AudioClip PhrasePNG = PhrasesPNG[random];
        Sound.sound.AudioSource.PlayOneShot(PhrasePNG);

        (int i, int nbQuest) = (0, LesQuêtes.Count);
        bool Condition = false;
        foreach (var quête in QuestName)
        {
            foreach (var quest in player.listdesqêtes)
            {
                if (quest.Titre == quête)
                {
                    Condition = !quest.Active;
                    quest.Avancer();
                    PointDeExclamation.SetActive(false);
                }
            }
        }

        if (QuestName.Length == 0)
        {
            Condition = true;
        }
        bool afficheounon = Condition;
        while (i < nbQuest && Condition && afficheounon)
        {
            Condition = false;
            foreach (var quest in player.listdesqêtes)
            {
                if (i < nbQuest && quest.Titre == LesQuêtes[i].Titre)
                {
                    Debug.Log("Quête suivante");
                    i++;
                    Condition = true;
                    afficheounon = !quest.Active;
                }
            }
        }

        afficheounon = afficheounon & i < nbQuest;
        if (afficheounon)
        {
            LaQûete = LesQuêtes[i];

            player.Fenêtre_de_quête.SetActive(true);
            player.Titre.text = LaQûete.Titre;
            player.Description.text = LaQûete.Description;
            player.Résumé.text = LaQûete.RésuméRapide;
            player.Gold.text = LaQûete.RecompenseGold.ToString() + " PO";
            player.XP.text = LaQûete.RecompenseExperience.ToString() + " EXP";
            player.bouton.QuestGiver = this;
            LaQûete.Avancement = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            player.Cam.GetComponent<ThirdPersonCameraControlSolo >().IsPaused = true;
        }
        else
        {
            Debug.Log("Je suis déjà entrain de faire cette quête ou j'ai fini toutes ses quêtes.");
        }
    }

    public void AcceptQuest()
    {
        LaQûete.Active = true;
        player.listdesqêtes.Add(LaQûete);
        player.Questnotdoneyet.Add(LaQûete);
        LaQûete.player = player;
        if (!(player.Onsuitunequête))
        {
            player.LaQestSuivieMaGueule = LaQûete;
            player.QuêteSuivie.text = player.LaQestSuivieMaGueule.RésuméRapide + '\n' +
                                      player.LaQestSuivieMaGueule.Avancement + " / " + player.LaQestSuivieMaGueule.Finalité;
            player.Suivie.SetActive(true);
            player.Onsuitunequête = true;
        }
        Debug.Log("J'ai accepeté");
        Resume();
        PointDeExclamation.SetActive(false);
    }

    public void RefusetQuest()
    {
        Debug.Log("J'ai Refusé");
        Resume();
    }

    public void Resume() //Remise en marche du jeu
    {
        player.Fenêtre_de_quête.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        player.Cam.GetComponent<ThirdPersonCameraControlSolo>().IsPaused = false;
        if (Aurevoirs.Count != 0)
        {
            int random = Random.Range(0,Aurevoirs.Count);
            Sound.sound.AudioSource.PlayOneShot(Aurevoirs[random]);
        }
        
    }

    public void Update()
    {
        Transform Unjoueur = GameObject.FindWithTag("Player").transform;

        foreach (var quête in QuestName)
        {
            foreach (var quest in Unjoueur.GetComponent<JoueurSolo>().Questnotdoneyet)
            {
                if (quest.Titre == quête)
                {
                    PointDeExclamation.SetActive(true);
                    return;
                }
            }
        }

        (int i, int nbQuest) = (0, LesQuêtes.Count);
        bool Condition = true;
        bool afficheounon = true;
        while (i < nbQuest && Condition && afficheounon)
        {
            Condition = false;
            foreach (var quest in Unjoueur.GetComponent<JoueurSolo>().listdesqêtes)
            {
                if (i < nbQuest && quest.Titre == LesQuêtes[i].Titre)
                {
                    i++;
                    Condition = true;
                    afficheounon = !quest.Active;
                }
            }
        }

        afficheounon = afficheounon & i < nbQuest;
        if (afficheounon)
        {
            PointDeExclamation.SetActive(true);
        }
        
    }

    public void Delay()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay() //  <-  C'est pour attendre :)
    {
        yield return new WaitForSeconds(6);
    }
}
