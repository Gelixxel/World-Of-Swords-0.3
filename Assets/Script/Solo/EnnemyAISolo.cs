using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyAISolo : InteractionSolo
{
    public GameObject[] ListPlayer;
    public int numplayer;
    public float[] Listdist;
    public float distmin = -1;
    public SpawnerSolo spawner;


    public string[] QuestName; //Nom des quêtes où le monstre doit mourir, peut être vide si il n'y en a pas
    public int Level;
    public int HealthPoint;
    public int HealthMax; //Pour la barre de vie
    public int Defence;
    public int Damage;
    public int Manapoint; //Pour les mages
    public int Manamax; //Pour les mages
    public int Sagacity; //Pour les mages
    public GameObject lootdrop;
    public GameObject loot_a;

    //Variable de patrouille
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public float WaitTime;
    private float WaitTimeCount;
    //Distance entre le joueur et l'ennemi
    private float Distance;

    // Cible de l'ennemi
    public GameObject Target;
    public JoueurSolo PlayerTarget;

    //Distance de poursuite
    public float chaseRange = 10;

    // Portée des attaques
    public float attackRange = 2.2f;

    // Cooldown des attaques
    public float attackRepeatTime = 1;
    private float attackTime;

    // Agent de navigation
    private NavMeshAgent agent;
    
    // Animations de l'ennemi
    //private Animation animations;

    // Vie de l'ennemi
    private bool isDead = false;

    // Animations de l'ennemi
    private Animator animations;
    private bool IsWalking = false;
    private bool IsRunning = false;


    void Start()
    {
        /*for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            Listdist[i] = -1;
        }*/
        agent = gameObject.GetComponent<NavMeshAgent>();
        animations = gameObject.GetComponent<Animator>();
        attackTime = Time.time;
        WaitTimeCount = WaitTime;
    }



    void Update()
    {
        if (!isDead)
        {
            ListPlayer = GameObject.FindGameObjectsWithTag("Player");
            numplayer = ListPlayer.Length;
            Target = ListPlayer[0];
            Listdist = new float[numplayer];
            distmin = Vector3.Distance(ListPlayer[0].transform.position, transform.position);

            animations.SetBool("IsWalking", IsWalking);
            animations.SetBool("IsRunning", IsRunning);

            // On cherche le joueur en permanence et on calcule la distance entre le joueur et l'ennemi, en fonction de cette distance on effectue diverses actions
            for (int i = 0; i < numplayer; i++)
            {
                Listdist[i] = Vector3.Distance(ListPlayer[i].transform.position, transform.position);

                if (distmin > Listdist[i])
                {
                    distmin = Listdist[i];
                    Target = ListPlayer[i];
                }
            }

            // Quand l'ennemi est loin = idle
            if (distmin > chaseRange)
            {
                Patrole();
            }

            // Quand l'ennemi est proche mais pas assez pour attaquer
            if (distmin < chaseRange && distmin > attackRange)
            {
                chase();
            }

            // Quand l'ennemi est assez proche pour attaquer
            if (distmin < attackRange)
            {
                attack(Target.GetComponent<JoueurSolo>());
            }

            if (HealthPoint <= 0)
            {
                Dead(Target.GetComponent<JoueurSolo>());
            }
        }
        else
        {
            Dead(PlayerTarget);
        }

        // poursuite
        void chase()
        {
            IsRunning = true;
            IsWalking = false;
            //animations.Play("IsRunning");
            agent.SetDestination(Target.transform.position);

        }

        void Patrole()
        {
            if (!walkPointSet)
            {
                SearchWalkPoint();
            }
            else
            {
                agent.SetDestination(walkPoint);
            }
            Vector3 distancetopoint = transform.position - walkPoint;
            if (distancetopoint.magnitude < 2f)
            {
                if (WaitTime <= 0)
                {
                    WaitTime = WaitTimeCount;
                    walkPointSet = false;
                }
                else
                {
                    WaitTime -= Time.deltaTime;
                }

            }
        }
        void SearchWalkPoint()
        {
            float randomz = Random.Range(-walkPointRange, walkPointRange);
            float randomx = Random.Range(-walkPointRange, walkPointRange);
            walkPoint = new Vector3(transform.position.x + randomx, transform.position.y, transform.position.z + randomz);
            walkPointSet = true;
            IsWalking = true;
            IsRunning = false;
        }

        // Combat
        void attack(JoueurSolo player)
        {
            // empeche l'ennemi de traverser le joueur
            agent.destination = transform.position;

            //Si pas de cooldown
            if (Time.time > attackTime)
            {
                //animations.Play("hit");
                player.HealthPoint -= ((2 * Level / 5) + 2) * (Damage / player.Defence);
                attackTime = Time.time + attackRepeatTime;
            }
        }

        // idle
        /*void idle()
        {
            // animations.Play("idle");
        }*/
    }

    public override void Interract(JoueurSolo playera)
    {
        base.Interract(playera);

        if (!isDead && playera.Touche)
        {
            int dmg = ((2 * playera.Level / 5) + 2) * (playera.Damage / Defence);
            HealthPoint -= dmg;
            Debug.Log("On a infligé " + dmg + " point de dégats");
            Debug.Log(HealthPoint);
            playera.Touche = false;
            if (HealthPoint <= 0)
            {
                isDead = true;
                PlayerTarget = playera;
            }
        }
    }

    public void Dead(JoueurSolo playere)
    {
        //animations.Play("die");
        playere.Money += Random.Range(1, 100);
        playere.Experience += 10;
        if (lootdrop != null)
        {
            Instantiate(lootdrop, loot_a.transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        if (spawner != null)
        {
            spawner.spawn--;
        }
        if (playere.Questnotdoneyet == new List<QuestSolo>())
        {
            return;
        }
        foreach (var quete in QuestName)
        {
            foreach (var quest in playere.listdesqêtes)
            {
                if (quest.Titre == quete && quest.Active)
                {
                    quest.Avancer();
                }
            }
        }
    }
}
