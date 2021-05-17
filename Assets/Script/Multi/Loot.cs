using UnityEngine;
using Photon.Pun;

public class Loot : Interaction
{
    public Vector3 tourne;
    public Itemscript item;
    private RaycastHit hit;
    [SerializeField]
    public PhotonView PV;
    public bool inter_;

    private void Awake()
    {
        inter_ = false;
    }
    public override void Interract(Joueur player)
    {
        base.Interract(player);
        inter_ = true;
        PickUp(player);
    }

    void PickUp(Joueur player)
    {
        player.inventaire.Add(item);
        inter_ = true;
    }

    [PunRPC]
    public void detruire()
    {
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void Update()    
    { 
        transform.Rotate(tourne * Time.deltaTime);;
    }
    
}
