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
        Inventory
    }


    private PlayerState currentPlayerState;
    public List<Item> ownedItemsList;
    private int currentGold;

    public Item testingItem;


    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 movement;


    public Text npcToolTip;
    public Text inventoryToolTip;


    //References
    private NPCCS nearNPC;
    public Text currentGoldText;
    public Animator storeAnimator;
    public Animator inventoryAnimator;


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

    //Inventory

    public List<InventoryItemCS> inventoryItemUIList;
    public SelectedItemCS selectedItemUI;
    private Item selectedItemInventory;

    //Equipment
    private Item equippedHelmet;
    private Item equippedChest;
    private Item equippedPants;
    private Item equippedBoots;

    //EquipmentUI
    public Image equippedUIHelmet;
    public Image equippedUIChest;
    public Image equippedUIPants;
    public Image equippedUIBoots;

    

    public Text storeTextMessage;
    public Text inventoryTextMessage;
    
    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerState = PlayerState.Idle;
        nearNPC = null;
        ownedItemsList = new List<Item>();

        itemsCartBuyList = new List<Item>();
        cartBuyingSum = 0;

        itemsCartSellList = new List<Item>();
        cartSellingSum = 0;

        currentGold = 5000;

        SetCurrentGoldText();

        ownedItemsList.Add(testingItem);

        equippedHelmet = null;
        equippedChest = null;
        equippedPants = null;
        equippedBoots = null;

        playerAnimator.SetInteger("FacingDirection", 1);


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
                    inventoryToolTip.text = "";
                    PlayerOpenCloseStore(true);
                    movement = new Vector2(0, 0);
                }
                
            }
            if (Input.GetKey("i"))
            {
                inventoryTextMessage.text = "Press ESC to close inventory or click exit button";
                inventoryToolTip.text = "";
                npcToolTip.text = "";
                PlayerOpenCloseInvetory(true);
                movement = new Vector2(0, 0);
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

        #region Inventory
        if (currentPlayerState == PlayerState.Inventory)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                PlayerOpenCloseInvetory(false);

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
            inventoryToolTip.text = "Inventory can be open with 'i'";

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

    #region Inventory/Equipment
    public void PlayerOpenCloseInvetory(bool condition) 
    {
        if (condition)
        {
            currentPlayerState = PlayerState.Inventory;
            inventoryAnimator.SetBool("Open", true);
            selectedItemInventory = null;
            selectedItemUI.TurnOffItem();
            if(equippedHelmet==null)
                equippedUIHelmet.gameObject.SetActive(false);
            if (equippedChest == null)
                equippedUIChest.gameObject.SetActive(false);
            if (equippedPants == null)
                equippedUIPants.gameObject.SetActive(false);
            if (equippedBoots == null)
                equippedUIBoots.gameObject.SetActive(false);

            int ammount = ownedItemsList.Count;
            for (int i=0; i<16;i++)
            {
                if(i< ammount)
                {
                    inventoryItemUIList[i].SetInventoryItem(ownedItemsList[i]);
                }
                else 
                {
                    inventoryItemUIList[i].SetInventoryItem(null);
                }
                
            }
        }
        else 
        {
            currentPlayerState = PlayerState.Idle;
            inventoryAnimator.SetBool("Open", false);
            inventoryToolTip.text = "Inventory can be open with 'i'";
            if (nearNPC != null) 
            {
                npcToolTip.text = "Press E to enter the " + nearNPC.transform.name;
            }
        }
    }

    public void SelectItemFromInventory(int id)
    {
        bool equipped = CompareEquipped(ownedItemsList[id]);
        if(selectedItemInventory==null)
            selectedItemUI.SelectItem(ownedItemsList[id],true, equipped);
        else
            selectedItemUI.SelectItem(ownedItemsList[id], false, equipped);
        selectedItemInventory = ownedItemsList[id];
    }

    public void EquipUnequipItem()
    {
        if (CompareEquipped(selectedItemInventory))
        {
            switch (selectedItemInventory.type)
            {
                case Item.ItemType.Helmet:
                    equippedHelmet = null;
                    equippedUIHelmet.gameObject.SetActive(false);
                    ((SpriteRenderer)gameObject.transform.GetChild(0).GetComponent(typeof(SpriteRenderer))).sprite = null;
                    break;
                case Item.ItemType.Chest:
                    equippedChest = null;
                    equippedUIChest.gameObject.SetActive(false);
                    ((SpriteRenderer)gameObject.transform.GetChild(1).GetComponent(typeof(SpriteRenderer))).sprite = null;
                    break;
                case Item.ItemType.Pants:
                    equippedPants = null;
                    equippedUIPants.gameObject.SetActive(false);
                    ((SpriteRenderer)gameObject.transform.GetChild(2).GetComponent(typeof(SpriteRenderer))).sprite = null;
                    break;
                case Item.ItemType.Boots:
                    equippedBoots = null;
                    equippedUIBoots.gameObject.SetActive(false);
                    ((SpriteRenderer)gameObject.transform.GetChild(3).GetComponent(typeof(SpriteRenderer))).sprite = null;
                    break;
            }
            selectedItemUI.ResetItem(false);
            inventoryTextMessage.text = "Successfully unequipped " + selectedItemInventory.name;
        }
        else
        {
            switch(selectedItemInventory.type)
            {
                case Item.ItemType.Helmet:
                    equippedHelmet = selectedItemInventory;
                    equippedUIHelmet.sprite = equippedHelmet.icon;
                    equippedUIHelmet.gameObject.SetActive(true);
                    ((SpriteRenderer)gameObject.transform.GetChild(0).GetComponent(typeof(SpriteRenderer))).sprite = equippedHelmet.icon;
                    break;
                case Item.ItemType.Chest:
                    equippedChest = selectedItemInventory;
                    equippedUIChest.sprite = equippedChest.icon;
                    equippedUIChest.gameObject.SetActive(true);
                    ((SpriteRenderer)gameObject.transform.GetChild(1).GetComponent(typeof(SpriteRenderer))).sprite = equippedChest.icon;
                    break;
                case Item.ItemType.Pants:
                    equippedPants = selectedItemInventory;
                    equippedUIPants.sprite = equippedPants.icon;
                    equippedUIPants.gameObject.SetActive(true);
                    ((SpriteRenderer)gameObject.transform.GetChild(2).GetComponent(typeof(SpriteRenderer))).sprite = equippedPants.icon;
                    break;
                case Item.ItemType.Boots:
                    equippedBoots = selectedItemInventory;
                    equippedUIBoots.sprite = equippedBoots.icon;
                    equippedUIBoots.gameObject.SetActive(true);
                    ((SpriteRenderer)gameObject.transform.GetChild(3).GetComponent(typeof(SpriteRenderer))).sprite = equippedBoots.icon;
                    break;
            }

            selectedItemUI.ResetItem(true);
            inventoryTextMessage.text = "Successfully equipped " + selectedItemInventory.name;
        }
    }

    private bool CompareEquipped(Item item)
    {

        switch (item.type) 
        {
            case Item.ItemType.Helmet:
                if(item == equippedHelmet)
                {
                    return true;
                }
                break;

            case Item.ItemType.Chest:
                if (item == equippedChest)
                {
                    return true;
                }
                break;
            case Item.ItemType.Pants:
                if (item == equippedPants)
                {
                    return true;
                }
                break;
            case Item.ItemType.Boots:
                if (item == equippedBoots)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    #endregion



    #region Player Movement Physics
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

        //Doing this so the animations only have 4 directions so the items on top of the char dont mess up on the bleding tree, also to prioritize 1 direction over another
        if(movement.x > 0)
            playerAnimator.SetInteger("FacingDirection", 1);
        else
            if(movement.x < 0)
                playerAnimator.SetInteger("FacingDirection", 3);
            else
                if(movement.y > 0)
                    playerAnimator.SetInteger("FacingDirection", 0);
                else
                    if(movement.y<0)
                        playerAnimator.SetInteger("FacingDirection", 2);

        transform.rotation = Quaternion.identity;
    }

    #endregion


}
