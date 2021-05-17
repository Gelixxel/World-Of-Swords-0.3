using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Inventory Inventory;
    public Image icon;
    public Image Remove;
    public Button Button;

    Itemscript item;

    public void AddItem(Itemscript newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        Button.interactable = true;
        Remove.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        Button.interactable = false;
        Remove.enabled = false;
    }

    public void Equipped()
    {
        int i = Int32.Parse(this.name);
        if (i > 24)
        {
            return;
        }
        item = Inventory.Items[i];
        Inventory.Equiped(item,i);
        //item.IsEquipped = true;
    }

    public void Removed()
    {
        int i = Int32.Parse(this.name);
        if (i < 25)
        {
            item = Inventory.Items[i];
            Inventory.Remove(item,i);
        }
        else
        {
            item = Inventory.EquippedItems[i - 25];
            Inventory.RemoveEqquiped(item);
        }
    }
}
