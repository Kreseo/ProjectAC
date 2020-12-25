using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemCS : MonoBehaviour
{
    public int id;
    private Item item;
    public Image buttonImage;
    public PlayerCS player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInventoryItem(Item item)
    {

        if (item != null) 
        {
            buttonImage.sprite = item.icon;
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SelectItemFromInventory()
    {
        player.SelectItemFromInventory(id);
    }
}
