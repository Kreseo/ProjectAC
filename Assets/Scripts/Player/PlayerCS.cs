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


    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 movement;


    public Text npcToolTip;


    //References
    private NPCCS nearNPC;
    public Text currentGoldText;
    public Animator storeAnimator;
    

    //Buying
    private List<Item> itemsCartBuyList;
    public List<ButtonUICS> buyingButtonUIList;
    private int cartBuyingSum;
    public Text buyingPriceText;

    //Selling
    private List<Item> itemsCartSellList;
    public List<ButtonUICS> sellingButtonUIList;
    private int cartSellingSum;
    public Text sellingPriceText;


    public Text storeTextMessage;
    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerState = PlayerState.Idle;
        currentPlayerDirection = PlayerDirection.Right;
        nearNPC = null;
        ownedItemsList = new List<Item>();

        itemsCartBuyList = new List<Item>();
        cartBuyingSum = 0;

        itemsCartSellList = new List<Item>();
        cartSellingSum = 0;

        currentGold = 5000;

        SetCurrentGoldText();

        ownedItemsList.Add(testingItem);


    }

    // Update is called once per frame
    void Update()
    {
        
        #region Movement
        
        if(currentPlayerState == PlayerState.Idle) 
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if (nearNPC != null) 
            {
                if(Input.GetKey("e"))
                {
                    storeTextMessage.text = "Press ESC to close store or click exit button";
                    PlayerOpenCloseStore(true);
                    movement = new Vector2(0, 0);
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

    #region ItemsSelectStore
    public void PlayerOpenCloseStore(bool condition)
    {
        if(condition)
        {
            currentPlayerState = PlayerState.Store;
            
            RefreshBuyingStore();
            RefreshSellingStore();
            storeAnimator.SetBool("Open", true);
            npcToolTip.text = "";
        }
        else 
        {
            currentPlayerState = PlayerState.Idle;
            storeAnimator.SetBool("Open", false);
            buyingPriceText.text = "0";
            sellingPriceText.text = "0";
            cartBuyingSum = 0;
            cartSellingSum = 0;
            itemsCartBuyList.Clear();
            itemsCartSellList.Clear();
            npcToolTip.text = "Press E to enter the " + nearNPC.transform.name;
        }
        
    }


    void SetCurrentGoldText() 
    {
        currentGoldText.text = currentGold.ToString();
    }

  

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

    public bool AddToBuyingCart(int i) 
    {
        if (nearNPC.itemsToSell[i].buyingPrice + cartBuyingSum > currentGold)
        {
            storeTextMessage.text = "Don't have enough gold to add to cart";
            return false;
        }
        else
        {
            itemsCartBuyList.Add(nearNPC.itemsToSell[i]);
            cartBuyingSum += nearNPC.itemsToSell[i].buyingPrice;
            buyingPriceText.text = cartBuyingSum.ToString();
        }
        return true;

        
    }
    public void RemoveFromBuyingCart(int i)
    {

        itemsCartBuyList.Remove(nearNPC.itemsToSell[i]);
        cartBuyingSum -= nearNPC.itemsToSell[i].buyingPrice;
        buyingPriceText.text = cartBuyingSum.ToString();
    }
    public void PurchaseFromStore() 
    {
        if(itemsCartSellList.Count == 0) 
        {
            storeTextMessage.text = "Bought clothes for " + cartBuyingSum + " gold";
            foreach (Item currentItem in itemsCartBuyList)
                ownedItemsList.Add(currentItem);
            itemsCartBuyList.Clear();
            currentGold -= cartBuyingSum;
            cartBuyingSum = 0;
            buyingPriceText.text = cartBuyingSum.ToString();
            SetCurrentGoldText();
            RefreshBuyingStore();
            RefreshSellingStore();
        }
        else
            storeTextMessage.text = "Can't buy clothes for when you are also selling";


    }

    public void RefreshSellingStore()
    {
        int ammount = ownedItemsList.Count;
        for (int i = 0; i < 16; i++)
        {

            if (i < ammount)
            {
                sellingButtonUIList[i].SetSellingItem(ownedItemsList[i]);
            }

            else
                sellingButtonUIList[i].SetSellingItem(null);
        }
    }
    public void AddToSellinggCart(int i)
    {

        itemsCartSellList.Add(ownedItemsList[i]);
        cartSellingSum += ownedItemsList[i].sellingPrice;
        sellingPriceText.text = cartSellingSum.ToString();
    }
    public void RemoveFromSellingCart(int i)
    {

        itemsCartSellList.Remove(ownedItemsList[i]);
        cartSellingSum -= ownedItemsList[i].sellingPrice;
        sellingPriceText.text = cartSellingSum.ToString();
    }
    public void SellToStore()
    {
        if (itemsCartBuyList.Count == 0)
        {
            storeTextMessage.text = "Sold clothes for " + cartSellingSum + " gold";
            foreach (Item currentItem in itemsCartSellList)
                ownedItemsList.Remove(currentItem);
            itemsCartSellList.Clear();
            currentGold += cartSellingSum;
            cartSellingSum = 0;
            sellingPriceText.text = cartSellingSum.ToString();
            SetCurrentGoldText();
            RefreshBuyingStore();
            RefreshSellingStore();
        }
        else
            storeTextMessage.text = "Can't sell clothes for when you are also buying";
        

    }
    #endregion


    #region NPC Enter and Leaves the Player Area
    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.tag == "NPC")
        {
            
            nearNPC = (NPCCS)obj.gameObject.GetComponent(typeof(NPCCS));
            npcToolTip.text = "Press E to enter the " + nearNPC.transform.name;
        }
       
    }

    void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.tag == "NPC")
        {
            npcToolTip.text = "";
            nearNPC = null;
        }

    }
    #endregion

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Land")
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
        playerAnimator.SetFloat("Horizontal", movement.x);
        playerAnimator.SetFloat("Vertical", movement.y);

        transform.rotation = Quaternion.identity;
    }

}
