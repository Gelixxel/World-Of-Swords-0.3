using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController pc;
    //Variable pour le déplacement
    public int walkspeed;
    public int runspeed;
    public int jumpforce;
    public int gravity;
    //Variable pour la direction
    private Vector3 _moveDir;
    
    //Attack
    public int damage;

    public Animator anim;
    private bool IsRunning = false;
    private bool IsWalking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        pc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Calcul de la direction suivant course/marche
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _moveDir = new Vector3(Input.GetAxis("Horizontal") * runspeed, _moveDir.y,
                Input.GetAxis("Vertical") * runspeed);
            IsRunning = true;
            IsWalking = false;
        }
        else
        {
            _moveDir = new Vector3(Input.GetAxis("Horizontal") * walkspeed, _moveDir.y, Input.GetAxis("Vertical") * walkspeed);
            IsRunning = false;
            IsWalking = true;
        }
        
        //Check de la touche e
        if (Input.GetButtonDown("Jump") && pc.isGrounded)
                 {
                     //On saute
                     _moveDir.y = jumpforce;
                 }
        //Si on se déplace
        if (_moveDir.x != 0 || _moveDir.z != 0)
        {
            //On tourne le perso dans la bonne direction
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(new Vector3(_moveDir.x, 0, _moveDir.z )), 0.15f);
        }
        else
        {
            IsRunning = false;
            IsWalking = false;
        }
        
        //On applique la gravité
        _moveDir.y -= gravity * Time.deltaTime;
        
        anim.SetBool("IsRunning", IsRunning);
        anim.SetBool("IsWalking", IsWalking); 
        
        //On applique le déplacement
        pc.Move(_moveDir * Time.deltaTime);
    }   
}
