using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCS : MonoBehaviour
{
    enum PlayerState
    {
        Idle,
        Store,
        Inventory,
        Status
    }

    enum PlayerDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    private PlayerState currentPlayerState;
    private PlayerDirection currentPlayerDirection;
    
    public List<Item> ownedItemsList;
    private int currentGold;

    public Item testingItem;
    


    //References
    private NPCCS nearNPC;
    public Text currentGoldText;
    public Animator storeAnimator;
    

    //Buying
    private List<Item> itemsCartBuyList;
    public List<BuyingButtonUICS> buyingButtonUIList;
    private int cartSum;
    public Text buyingPriceText;


    // Start is called before the first frame update
    void Start()
    {
        currentPlayerState = PlayerState.Idle;
        currentPlayerDirection = PlayerDirection.Right;
        nearNPC = null;
        ownedItemsList = new List<Item>();
        itemsCartBuyList = new List<Item>();
        cartSum = 0;
        currentGold = 100000;

        SetCurrentGoldText();

        ownedItemsList.Add(testingItem);


    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        
        if(currentPlayerState == PlayerState.Idle) 
        {
            if (nearNPC != null) 
            {
                if(Input.GetKey("e"))
                {
                    PlayerOpenCloseStore(true);
                }
            }
        }
        #endregion

        #region Store  

        if (currentPlayerState == PlayerState.Store)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                PlayerOpenCloseStore(false);
            }
        }
        #endregion

    }

    public void PlayerOpenCloseStore(bool condition)
    {
        if(condition)
        {
            currentPlayerState = PlayerState.Store;
            
            RefreshBuyingStore();
            storeAnimator.SetBool("Open", true);
        }
        else 
        {
            currentPlayerState = PlayerState.Idle;
            storeAnimator.SetBool("Open", false);
        }
        
    }


    void SetCurrentGoldText() 
    {
        currentGoldText.text = currentGold.ToString();
    }

    #region ItemsSelectStore

    public void RefreshBuyingStore()
    {
        int ammount = nearNPC.itemsToSell.Count;
        for (int i = 0; i < 16; i++)
        {
            
            if (i < ammount)
            {
                bool hasItem = ownedItemsList.Contains(nearNPC.itemsToSell[i]);
                buyingButtonUIList[i].SetBuyingItem(nearNPC.itemsToSell[i], hasItem);
            }

            else
                buyingButtonUIList[i].SetBuyingItem(null, false);
        }
    }
    public void AddToBuyingCart(int i) 
    {
        
        itemsCartBuyList.Add(nearNPC.itemsToSell[i]);
        cartSum += nearNPC.itemsToSell[i].buyingPrice;
        buyingPriceText.text = cartSum.ToString();
    }
    public void RemoveFromBuyingCart(int i)
    {

        itemsCartBuyList.Remove(nearNPC.itemsToSell[i]);
        cartSum -= nearNPC.itemsToSell[i].buyingPrice;
        buyingPriceText.text = cartSum.ToString();
    }

    public void PurchaseFromStore() 
    {
        foreach(Item currentItem in itemsCartBuyList) 
            ownedItemsList.Add(currentItem);
        itemsCartBuyList.Clear();
        currentGold -= cartSum;
        cartSum = 0;
        buyingPriceText.text = cartSum.ToString();
        SetCurrentGoldText();
        RefreshBuyingStore();
        
    }
    #endregion


    #region NPC Enter and Leaves the Player Area
    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.tag == "NPC")
        {
            Debug.Log("Entra");
            nearNPC = (NPCCS)obj.gameObject.GetComponent(typeof(NPCCS));
        }
       
    }

    void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.tag == "NPC")
        {
            Debug.Log("Sale");
            nearNPC = null;
        }

    }
    #endregion

}
