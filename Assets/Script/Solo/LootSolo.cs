using UnityEngine;

public class LootSolo : InteractionSolo
{
    public Vector3 tourne;
    public Itemscript item;
    private RaycastHit hit;
    public override void Interract(JoueurSolo player)
    {
        base.Interract(player);
        PickUp(player);
    }

    void PickUp(JoueurSolo player)
    {
        bool detruit = player.inventaire.Add(item);
        if (detruit)
        {
            Destroy(this.gameObject);
        }
    }

    void Update()    
    { 
        transform.Rotate(tourne * Time.deltaTime);;
    }
    
}
