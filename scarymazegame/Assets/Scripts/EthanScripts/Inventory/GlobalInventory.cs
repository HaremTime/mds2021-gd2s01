//
// Bachelor of Software Engineering
// Media Design School
// Auckland
// New Zealand
//
// (c) 2020 Media Design School
//
// File Name : GlobalInventory.cs
// Description : Controls everything that requires an inventory, and renders their items in physically.
// Author : Ethan Velasco Uy
// Mail : ethan.uy@mediadesignschool.com
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInventory : MonoBehaviour
{
    [Header("Backpack")]
    public GameObject backpackSlotContainer;
    public List<GameObject> backpack;
    public List<int> backpackItemCount;

    [Header("Backpack values")]
    public int maxSpace;

    public bool AddItem(GameObject item)
    {
        // Check if item already exists in backpack and can be stacked.
        for (int i = 0; i < maxSpace; i++)
        {
            // If item of same type exists and currently isn't at a max stack.
            if (backpack[i] == item && backpackItemCount[i] != item.GetComponent<ItemScript>().maxStack)
            {
                backpackItemCount[i] += 1;
                return true;
            }
        }

        // Find a spot to put new item in backpack.
        for (int i = 0; i < maxSpace; i++)
        {
            if (backpack[i] == null)
            {
                backpack[i] = item;
                backpackItemCount[i] += 1;
                return true;
            }
        }

        print("Item failed to add to inventory.");
        return false;
    }

    public void DropItem(string slotName)
    {
        // Get slot from name
        int slot = (int)char.GetNumericValue(slotName[slotName.Length-1]);

        if (backpackItemCount[slot] > 0)        // If the slot contains at least one item.
        {
            backpackItemCount[slot] -= 1;       // Reduce item by 1.
            
            if (backpackItemCount[slot] == 0)   // If the deletion caused the slot to be empty...
            {
                backpack[slot] = null;          // Slot is now empty, then proceed to delete item in slot.
                Destroy(backpackSlotContainer.transform.Find("Slot " + slot).GetChild(0).gameObject);
            }
        }
        LoadBackpack();                         // Update changes to backpack
    }

    public void DropItemGameObject(GameObject item)
    {
        for (int i = 0; i < maxSpace; i++)
        {
            if (backpack[i] == item)
            {
                backpackItemCount[i]--;
                if (backpackItemCount[i] == 0) backpack[i] = null;
            }
        }
    }
    
    public void LoadInSlot(GameObject item, Transform slot)      // Add a child inside slot
    {
        var newItem = Instantiate(item);                // Create item
        newItem.name = item.name;

        // If gravity induced, reset that.
        if (newItem.GetComponent<Rigidbody>() != null) newItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        
        newItem.transform.SetParent(slot);                      // Set parent
        newItem.transform.localPosition = Vector3.zero;         // Set position
        newItem.transform.localEulerAngles = Vector3.zero;      // Local rotation
    }

    public void DestroyInSlot(Transform slot)   // Destroy all children in a slot.
    {
        if (slot.GetComponentsInChildren<Transform>().Length > 0)   // If slot isn't empty.
        {
            foreach (Transform child in slot)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void LoadBackpack()
    {
        for (int i = 0; i < maxSpace; i++)
        {
            if (backpack[i] != null)
            {
                DestroyInSlot(backpackSlotContainer.transform.Find("Slot " + i));                   // Destroy item inside
                LoadInSlot(backpack[i], backpackSlotContainer.transform.Find("Slot " + i));         // Load item in slot
            }
            else
            {
                DestroyInSlot(backpackSlotContainer.transform.Find("Slot " + i));  // Destroy item inside
            }
        }
    }

    public bool SearchForItem(GameObject _item)
    {
        for (int i = 0; i < backpack.Count; i++)
        {
            if (backpack[i].gameObject.GetComponent<ItemScript>().itemName == _item.GetComponent<ItemScript>().itemName)
            {
                return true;
            }
        }
        return false; // No item.
    }

    private void Start()
    {
        // Allocate max space.
        maxSpace = backpackSlotContainer.transform.childCount;

        for (int i = 0; i < maxSpace; i++)
        {
            backpack.Add(null);
            backpackItemCount.Add(0);
        }
        
        LoadBackpack();
    }
}
