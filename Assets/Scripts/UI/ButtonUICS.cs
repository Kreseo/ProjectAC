using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUICS : MonoBehaviour
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
            if (hasItem)
                button.interactable = false;
            else
                button.interactable = true;
        }
        else 
        {
            gameObject.SetActive(false);
            
        }
       

    }

    public void SetSellingItem(Item newItem)
    {
        if (newItem != null)
        {
            gameObject.SetActive(true);
            item = newItem;
            buttonImage.sprite = item.icon;
            priceText.text = item.sellingPrice.ToString();
            selected = false;
            buttonImage.color = Color.white;
        }
        else
        {
            gameObject.SetActive(false);

        }


    }


    public void SelectItem(bool buying)
    {
        if (selected)
        {
            buttonImage.color = Color.white;
            if(buying)
                player.RemoveFromBuyingCart(id);
            else
                player.RemoveFromSellingCart(id);
            selected = !selected;
        }
        else
        {
           
            if (buying)
            {
                if(player.AddToBuyingCart(id))
                { 
                    buttonImage.color = Color.grey;
                    selected = !selected;
                }
            }
            else
            {
                buttonImage.color = Color.grey;
                player.AddToSellinggCart(id);
                selected = !selected;
            }
                
        }

        
        
    }
}
