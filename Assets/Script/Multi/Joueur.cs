using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Joueur : MonoBehaviour
{
    //Variable Multi
    public static Joueur Instance;
    PhotonView PV;
    public int ID;

    //Le focus pour une interaction
    public Interaction focus;
    
    //UI Vie/Endurance
    [SerializeField] public Image Healthbar;
    [SerializeField] public Image Staminabar;
    [SerializeField] GameObject ui;
    [SerializeField] Camera minimapCam;

    //L'ennemie
    public EnnemyAI ennemy;

    //public Weapon weapon;
    public int Money;
    public float HealthPoint;
    public float HealthMax;
    public float Stamina;
    public float StaminaMax;
    public bool CanUseStam;
    public int Tenacity;
    public int Defence;
    public int Damage;
    public int Manapoint;
    public int Manamax;
    public int Sagacity;
    // public string[] Skill;
    public int MaxInventory;
    public int Level;
    public float Experience;
    public float ExperienceMax;
    public Interaction[] Targets;
    public GameObject Animlvlup;
    public List<Quest> listdesqêtes;
    //L'affichage de la quête
    public Text Titre;
    public Text Description;
    public Text Résumé;
    public Text Gold;
    public Text XP;
    public GameObject Fenêtre_de_quête; // Ce qui affiche au moment d'accepter / refuser une quête
    public ButtonQuest CLaHess; //J'ai pas trouvé d'autre technique pour accepter/reffuser une quêtes à partir d'un bouton.
    
    public GameObject Cam; //Pour la pause
    public GameObject FenêtreFinDeQuête;    //Pour l'affichage fin de quête
    public Text FenêtreTitre;              //Pour savoir quoi écrire

    // Variables concernant l'attaque / les interactions
    public float attackCooldown;
    public bool isAttacking = false;
    private float currentCooldown;
    public float attackRange;
    public GameObject rayHit;
     public float interractRange;
    public string inputInterract = "F";
    public bool Touche;
    
    // Variables concernant l'affichage des quêtes (la liste) :)
    public int Questactuel;
    public List<Quest> Questnotdoneyet;
    public string inputJournal = "J";
    public GameObject Journal;
    public bool JournalIsActive;
    public Text JournalTitre;
    public Text JournalDescription;
    public Text JournalRésumé;
    public Text JournalAvancement;
    public GameObject YaPasDeQuête;
    public GameObject Suivie;
    public Text QuêteSuivie;   //Pour le suivi de quête (le texte affiché)
    public Quest LaQestSuivieMaGueule; //Pour enlever le suivi une fois la quest fini
    public bool Onsuitunequête;
    public Text TextSuivie;
    
    //Variables concernant l'affichage de l'inventaire
    public string InputInventory;
    public GameObject Inventaire;
    public bool InventoryIsActive;
    public Inventory inventaire;
    public InventorySlot[] MainInventory;
    public InventorySlot[] EquippedInventory;
    
    // Le personnage est-il mort ?
    public bool isDead = false;

    private CharacterController cc;
    public float speed = 6f;
    public float runspeed = 10f;
    public float jumpSpeed = 8f;
    public float flyspeed = 10f;
    public float gravity = 4f;
    public float flying_gravity = 5f;
    private Vector3 moveDirection = Vector3.zero;
    float toground = 0.8f;
    float towall = 1f;
    float climbspeed = 2f;
    public float flycountdown = 0.2f;
    public float changeflycountdown;
    public bool IsFlying = false;
    public bool IsGrounded;
    public bool isClimbing;
    public bool isalmostclimbing;


    public Animator anim;
    private bool IsRunning = false;
    private bool IsWalking = false;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            ID = PV.ViewID;
        }
        rayHit = GameObject.Find("Rayhit");
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(minimapCam);
            Destroy(ui);
            //Destroy(cc);
        }
    }

    public int GetMaxInv()
    {
        return MaxInventory;
    }

    //On verifie si le joueur est au sol
    public bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, toground);
    }

    //On verifie si on peut grimper au mur devant sois
    public bool IsClimbing()
    {
        return (!isGrounded() && Physics.Raycast(transform.position, transform.forward, towall) && CanUseStamina());
    }

    //On verifie si on est pas au sol ni entrain de grimper
    public bool CanFly()
    {
        return (!isGrounded() && !IsClimbing() && flycountdown <= 0f && CanUseStamina());
    }

    public void UpdateStatus(int level)
    {
        HealthMax += 20;
        HealthPoint = HealthMax;
        Tenacity += 3;
        Defence += 3;
        Damage += 3;
        Manamax += 10;
        Manapoint = Manamax;
        Sagacity += 3;
        StaminaMax += 10;
    }
    void SetFocus(Interaction newFocus)
    {
        if (focus != null && focus != newFocus)
        {
            focus.PlusFocus();
        }
        focus = newFocus;
        newFocus.Focus(this);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.PlusFocus();
            focus = null;
        }
    }

    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }


        //UI ACTUALISATION
        Healthbar.fillAmount = HealthPoint / HealthMax;
        Staminabar.fillAmount = Stamina / StaminaMax;
        Staminabar.enabled = !(Stamina >= StaminaMax);
        if (!CanUseStamina())
        {
            Staminabar.GetComponent<Image>().color = Color.red;
        }
        else
        {
            Staminabar.GetComponent<Image>().color = new Color(0,190,0);
        }
        //Level-UP

        if (Experience >= ExperienceMax)
        {
            Level += 1;
            Experience -= ExperienceMax;
            ExperienceMax *= 1.5f;
            UpdateStatus(Level);
            GameObject LVLUP = Instantiate(Animlvlup, transform.position, Quaternion.identity.normalized);
            Sound.sound.AudioSource.PlayOneShot(Sound.sound.LVLUP);
            Destroy(LVLUP, 5);
        }


        //On verifie si le joueur est au dessus du vide
        if (CanFly())
        {
            IsGrounded = false;
            IsWalking = false;
            IsRunning = false;
            //On implemente le fait de sortir sa "paravoile" si elle n'est pas sortie et la ranger si elle est deja sortie
            if (!IsFlying)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    //On rajoute un Timer histoire de ne pas faire bugger le tout
                    if (changeflycountdown <= 0f)
                    {
                        IsFlying = true;
                        changeflycountdown = 0.6f;
                    }
                    
                }
                changeflycountdown -= Time.deltaTime;
            }
            else
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    if (changeflycountdown <= 0f)
                    {
                        IsFlying = false;
                        changeflycountdown = 0.6f;
                    }
                }
                changeflycountdown -= Time.deltaTime;
            }
        }

        // On fait voler le joueur avec ces caracteristiques de deplacements
        if (IsFlying)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection.y = -flying_gravity;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= flyspeed;
            moveDirection.z *= flyspeed;
            Stamina = Stamina - Time.fixedDeltaTime * 2f;
            Staminabar.enabled = true;
        }

        // On fait grimper le joueur avec ces caracteristiques de deplacements
        if (IsClimbing())
        {
            IsRunning = false;
            IsWalking = false;
            isClimbing = true;
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            moveDirection = transform.TransformDirection(moveDirection);
            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
                isalmostclimbing = false;
                Stamina = Stamina - Time.fixedDeltaTime * 2f;
                Staminabar.enabled = true;
            }
            else if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                moveDirection *= climbspeed;
                isalmostclimbing = false;
                Stamina = Stamina - Time.fixedDeltaTime * 2f;
                Staminabar.enabled = true;
            }
            else
            {
                isClimbing = false;
                isalmostclimbing = true;
            }

        }
        else
        {
            isClimbing = false;
        }
        
        
        // On fait bouger le joueur au sol avec ces caracteristiques ci-dessous
        if (isGrounded())
        {
            changeflycountdown = 0f;
            IsGrounded = true;
            IsFlying = false;
            flycountdown = 0.6f;
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            // si il sprint
            if (Input.GetKey(KeyCode.LeftShift) && CanUseStamina())
            {
                Stamina = Stamina - Time.fixedDeltaTime * 6f;
                moveDirection.x *= runspeed;
                moveDirection.z *= runspeed;
                moveDirection.y -= gravity * Time.deltaTime;
                IsRunning = true;
                IsWalking = false;
                Staminabar.enabled = true;

            }
            else
            {
                if (Stamina < StaminaMax)
                {
                    Stamina = Stamina + Time.fixedDeltaTime * 3f;
                    
                }
                moveDirection.x *= speed;
                moveDirection.z *= speed;
                moveDirection.y -= gravity * Time.deltaTime;
                IsRunning = false;
                IsWalking = true;
            }

            // Si il saute
            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
            }

            if (moveDirection.x == 0 || moveDirection.z == 0)
            {
                IsWalking = false;
                IsRunning = false;
            }
        }
        else
        {
            IsGrounded = false;
            flycountdown -= Time.deltaTime;
        }
        //On initialise la gravité qui n'est pas utilisée si on grimpe ou vole
        if (!IsClimbing())
        {
            if (!IsFlying)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
           
        }
        anim.SetBool("IsRunning", IsRunning);
        anim.SetBool("IsWalking", IsWalking);
        anim.SetBool("IsClimbing", isClimbing);
        anim.SetBool("IsAlmostClimbing", isalmostclimbing);
        anim.SetBool("IsFlying", IsFlying);
        anim.SetBool("IsGrounded", IsGrounded);
        cc.Move(moveDirection * Time.deltaTime);

        //Fonction pour ouvrir l'inventaire
        if (Input.GetKeyDown(InputInventory))
        {
            if (InventoryIsActive)
            {
                Inventaire.SetActive(false);
                InventoryIsActive = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cam.GetComponent<ThirdPersonCameraControl>().IsPaused = false;
            }
            else
            {
                Inventaire.SetActive(true);
                InventoryIsActive = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Cam.GetComponent<ThirdPersonCameraControl>().IsPaused = true;
            }

        }
        for (int i = 0; i < MainInventory.Length; i++)
        {
            if (inventaire.Items[i] != null)
            {
                MainInventory[i].AddItem(inventaire.Items[i]);
            }
            else
            {
                MainInventory[i].ClearSlot();
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (inventaire.EquippedItems[i] != null)
            {
                EquippedInventory[i].AddItem(inventaire.EquippedItems[i]);
            }
            else
            {
                EquippedInventory[i].ClearSlot();
            }
        }
        
        //Fonction d'ouvrir le journal de quêtes
        if (Input.GetKeyDown(inputJournal))
        {
            Questactuel = 0;
            if (JournalIsActive)
            {
                JournalIsActive = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cam.GetComponent<ThirdPersonCameraControl>().IsPaused = false;
                if (Questnotdoneyet.Count == 0)
                {
                    YaPasDeQuête.SetActive(false);
                }
                else
                {
                    Journal.SetActive(false);
                }
            }
            else
            {
                JournalIsActive = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Cam.GetComponent<ThirdPersonCameraControl>().IsPaused = true;
                Questactuel = 0;
                if (Questnotdoneyet.Count == 0)
                {
                    YaPasDeQuête.SetActive(true);
                }
                else
                {
                    Journal.SetActive(true);
                    JournalAvancement.text = Questnotdoneyet[Questactuel].Avancement + " / " + Questnotdoneyet[Questactuel].Finalité;
                    JournalDescription.text = Questnotdoneyet[Questactuel].Description;
                    JournalRésumé.text = Questnotdoneyet[Questactuel].RésuméRapide;
                    JournalTitre.text = Questnotdoneyet[Questactuel].Titre;
                    if (Onsuitunequête && Questnotdoneyet[Questactuel] == LaQestSuivieMaGueule)
                    {
                        TextSuivie.text = "Ne plus suivre la quête";
                    }
                    else
                    {
                        TextSuivie.text = "Suivre la quête";
                    }
                }
                
            }
        }
        
        // Fonction d'attaque
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && !JournalIsActive)
        {
            Attack();
        }
        else
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0)
            {
                currentCooldown = attackCooldown;
                isAttacking = false;
            }
        }
        
        //Les interractions
        if (Input.GetKeyDown(inputInterract)&& !JournalIsActive)  
        {
            Targets = FindObjectsOfType<Interaction>();
            foreach (var Target in Targets)
            {
                if (Vector3.Distance(Target.transform.position, transform.position) <= interractRange)
                {
                    Interaction interraction = Target.GetComponent<Interaction>();
                    if (interraction) //On vérifie qu'on a une interraction
                    {
                        Debug.Log("On tient un truc là !");
                        SetFocus(interraction);
                        interraction.Interract(this);
                        return;
                    }
                    else
                    {
                        Debug.Log("Trop loin");
                    }
                }
            }
        }

        //Lacher le focus
        if (Input.GetKeyDown(KeyCode.F4))
        {
            RemoveFocus();
        }  
    }
    
    public bool CanUseStamina()
    {
        if (Stamina <= 0)
        {
            CanUseStam = false;
        }

        if (Stamina >= StaminaMax)
        {
            CanUseStam = true;
        }
        while (Stamina > 0 && CanUseStam)
        {
            return CanUseStam;
        }

        return CanUseStam;
    }


    // Fonction d'attaque
    public void Attack()
    {
        if (!isAttacking)
        {
            //animations.Play("attack");

            RaycastHit hit;
            isAttacking = true;

            if (Physics.Raycast(rayHit.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
            {

                Debug.DrawLine(rayHit.transform.position, hit.point, Color.red);

                if (hit.transform.tag == "test")
                {
                    print(hit.transform.name + " detected");
                    ennemy = hit.transform.GetComponent<EnnemyAI>();
                    Touche = true;
                    ennemy.Interract(this);
                }

            }
            
        }

    }
    


    public void Dead()
    {
        isDead = true;
        //animations.Play("die");
        Destroy(transform.gameObject,5);
    }

    public void AfficheFinQuest(Quest quest)
    {
        FenêtreTitre.text = "Vous venez de terminer :"+'\n'+'"' +quest.Titre+'"';
        StartCoroutine(delay());
        
    }
    IEnumerator delay()  // Pour faire attendre 
    {
        FenêtreFinDeQuête.SetActive(true);
        yield return new WaitForSeconds(6);
        FenêtreFinDeQuête.SetActive(false);
    }

    public void Previous()
    {
        if (Questactuel != 0)
        {
            Questactuel -= 1;
            JournalAvancement.text = Questnotdoneyet[Questactuel].Avancement + " / " + Questnotdoneyet[Questactuel].Finalité;
            JournalDescription.text = Questnotdoneyet[Questactuel].Description;
            JournalRésumé.text = Questnotdoneyet[Questactuel].RésuméRapide;
            JournalTitre.text = Questnotdoneyet[Questactuel].Titre;
            if (Onsuitunequête && Questnotdoneyet[Questactuel] == LaQestSuivieMaGueule)
            {
                TextSuivie.text = "Ne plus suivre la quête";
            }
            else
            {
                TextSuivie.text = "Suivre la quête";
            }
        }
    }
    public void Next()
    {
        if (Questactuel != Questnotdoneyet.Count -1)
        {
            Questactuel += 1;
            JournalAvancement.text = Questnotdoneyet[Questactuel].Avancement + " / " + Questnotdoneyet[Questactuel].Finalité;
            JournalDescription.text = Questnotdoneyet[Questactuel].Description;
            JournalRésumé.text = Questnotdoneyet[Questactuel].RésuméRapide;
            JournalTitre.text = Questnotdoneyet[Questactuel].Titre;
            if (Onsuitunequête && Questnotdoneyet[Questactuel] == LaQestSuivieMaGueule)
            {
                TextSuivie.text = "Ne plus suivre la quête";
            }
            else
            {
                TextSuivie.text = "Suivre la quête";
            }
        }
    }

    public void SuivreQuest()
    {
        if (Onsuitunequête && LaQestSuivieMaGueule == Questnotdoneyet[Questactuel])
        {
            Suivie.SetActive(false);
            Onsuitunequête = false;
            TextSuivie.text = "Suivre la quête";
        }
        else
        {
            LaQestSuivieMaGueule = Questnotdoneyet[Questactuel];
            QuêteSuivie.text = LaQestSuivieMaGueule.RésuméRapide + '\n' +
                               LaQestSuivieMaGueule.Avancement + " / " + LaQestSuivieMaGueule.Finalité;
            Suivie.SetActive(true);
            Onsuitunequête = true;
            TextSuivie.text = "Ne plus suivre la quête";
        }
    }
}
