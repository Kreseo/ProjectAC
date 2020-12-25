using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemCS : MonoBehaviour
{

    public Image selectedItemIcon;
    public Text selectedItemText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void  SelectItem(Item item,bool first,bool equipped)
    {
        if (first)
        {
            gameObject.SetActive(true);
        }
        selectedItemIcon.sprite = item.icon;
        if(equipped)
            selectedItemText.text = "Unequip";
        else
            selectedItemText.text = "Equip";



    }

    public void ResetItem()
    {
        gameObject.SetActive(false);
    }
}
