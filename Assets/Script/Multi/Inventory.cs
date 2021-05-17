using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region L'instance
    
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Wsh pokito c'est pas normal , c'est peut-être du à un multi mais c'est grave chelou");
        }
        instance = this;
    }
    
    #endregion

    public delegate void OnItemChange();

    public Joueur player;

    public OnItemChange onItemChangeCallback;
    public  Itemscript[] Items = new Itemscript[25];
    public Itemscript[] EquippedItems = new Itemscript[4];

    public bool Add(Itemscript item)
    {
        bool condition = false;
        int i = 0;
        while (i < 25 && !condition)
        {
            if (Items[i] == null)
            {
                condition = true;
            }
            else
            {
                i++;
            }
        }

        if (!condition)
        {
            Debug.Log("Inventaire plein");
            return false;
        }

        Items[i] = item;
        if (onItemChangeCallback != null)
        {
            onItemChangeCallback.Invoke();
        }
        return true;
    }
    
    public void Equiped(Itemscript item, int emplacement)
    {
        AddStats(item);
        switch (item.Itis)
        {
            case "Helmet":
                if(EquippedItems[0] == null)
                {
                    EquippedItems[0] = item;
                    Items[emplacement] = null;
                }
                else
                {
                    DelStats(EquippedItems[0]);
                    //EquippedItems[0].IsEquipped = false;
                    (Items[emplacement], EquippedItems[0]) = (EquippedItems[0], Items[emplacement]);
                    /*Items.Add(EquippedItems[0]);
                    EquippedItems[0] = item;
                    Items[emplacement] = EquippedItems[0];
                    item.IsEquipped = true; */
                } 
                break;
            case "Chestplate":
                if (EquippedItems[1] == null)
                {
                    EquippedItems[1] = item;
                    Items[emplacement] = null;
                }
                else
                {
                    DelStats(EquippedItems[1]);
                    (Items[emplacement], EquippedItems[1]) = (EquippedItems[1], Items[emplacement]);
                    /*EquippedItems[1].IsEquipped = false;
                    Items.Add(EquippedItems[1]);
                    EquippedItems[1] = item;
                    Items.Remove(item);
                    item.IsEquipped = true;*/
                }
                break;
            case "Leggings":
                if (EquippedItems[2] == null)
                {
                    EquippedItems[2] = item;
                    Items[emplacement] = null;
                }
                else
                {
                    DelStats(EquippedItems[2]);
                    (Items[emplacement], EquippedItems[2]) = (EquippedItems[2], Items[emplacement]);
                    /*EquippedItems[2].IsEquipped = false;
                    Items.Add(EquippedItems[2]);
                    EquippedItems[2] = item;
                    Items.Remove(item);
                    item.IsEquipped = true;*/
                    
                }
                break;
            case "Weapon":
                if (EquippedItems[3] == null)
                {
                    EquippedItems[3] = item;
                    Items[emplacement] = null;
                }
                else
                {
                    DelStats(EquippedItems[3]);
                    (Items[emplacement], EquippedItems[3]) = (EquippedItems[3], Items[emplacement]);
                    /*EquippedItems[3].IsEquipped = false;
                    Items.Add(EquippedItems[3]);
                    EquippedItems[3] = item;
                    Items.Remove(item);
                    item.IsEquipped = true;*/
                }
                break;
            case "Consommable":
                Items[emplacement] = null;
                break;
            default:
                break;
        }
    }

    public void Remove(Itemscript item,int emplacement)
    {
        Items[emplacement] = null;
    }

    public void RemoveEqquiped(Itemscript item)
    {
        if (Add(item))
        {
            DelStats(item);
            switch (item.Itis)
            {
                case "Helmet":
                    EquippedItems[0] = null;
                    //EquippedItems.SetValue(null, 0);
                    //EquippedItems[0].icon = null;
                    break;
                case "Chestplate":
                    EquippedItems[1] = null;
                    //EquippedItems.SetValue(null, 1);
                    //EquippedItems[1].icon = null;
                    break;
                case "Leggings":
                    EquippedItems[2] = null;
                    //EquippedItems.SetValue(null, 2);
                    //EquippedItems[2].icon = null;
                    break;
                default:
                    EquippedItems[3] = null;
                    //EquippedItems.SetValue(null, 3);
                    //EquippedItems[3].icon = null;
                    break;
            }
        }
        else
        {
            Debug.Log("Inventaire plein, je ne peux pas me déséquiper de ça.");
        }
    }

    public void AddStats(Itemscript item)
    {
        player.Damage += item.Damage;
        player.Defence += item.Defence;
        player.HealthMax += item.HealthMax;
        player.HealthPoint += item.Health;
        player.Manamax += item.Mana;
        player.Tenacity += item.Tenacity;
        player.Sagacity += item.Sagacity;
    }

    public void DelStats(Itemscript item)
    {
        player.Damage -= item.Damage;
        player.Defence -= item.Defence;
        player.HealthMax -= item.HealthMax;
        player.HealthPoint -= item.Health;
        player.Manamax -= item.Mana;
        player.Tenacity -= item.Tenacity;
        player.Sagacity -= item.Sagacity;
    }
    
}
