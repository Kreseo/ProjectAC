using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyingButtonUICS : MonoBehaviour
{

    public int id;
    private Item item;
    public Image buttonImage;
    public Text priceText;
    public Button button;
    private bool selected;
    public PlayerCS player;

    // Start is called before the first frame update
    void Start()
    {

        selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBuyingItem(Item newItem, bool hasItem) 
    {
        if(newItem != null) 
        {
            gameObject.SetActive(true);
            item = newItem;
            buttonImage.sprite = item.icon;
            priceText.text = item.buyingPrice.ToString();
            selected = false;
            buttonImage.color = Color.white;
            if(hasItem)
                button.interactable = false;
        }
        else 
        {
            gameObject.SetActive(false);
            
        }
       

    }


    public void SelectItem()
    {
        if (selected)
        {
            buttonImage.color = Color.white;
            player.RemoveFromBuyingCart(id);
        }
        else
        {
            buttonImage.color = Color.grey;
            player.AddToBuyingCart(id);
        }

        selected = !selected;
        
    }
}
