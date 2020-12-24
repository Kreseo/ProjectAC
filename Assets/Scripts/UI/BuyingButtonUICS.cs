using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyingButtonUICS : MonoBehaviour
{
    private Item item;
    public Image buttonImage;
    public Text priceText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewItem(Item newItem) 
    {
        if(newItem != null) 
        {
            gameObject.SetActive(true);
            item = newItem;
            buttonImage.sprite = item.icon;
            priceText.text = item.buyingPrice.ToString();
        }
        else 
        {
            gameObject.SetActive(false);
        }
       

    }
}
