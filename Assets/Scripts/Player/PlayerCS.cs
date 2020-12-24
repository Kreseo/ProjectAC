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


    //References
    private NPCCS nearNPC;
    public Animator storeAnimator;
    public List<BuyingButtonUICS> buyingButtonUIList;
    

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerState = PlayerState.Idle;
        currentPlayerDirection = PlayerDirection.Right;
        nearNPC = null;
        ownedItemsList = new List<Item>();


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
            int ammount = nearNPC.itemsToSell.Count;
            for(int i =0; i< 4; i++) 
            {
                if (i < ammount)
                    buyingButtonUIList[i].SetNewItem(nearNPC.itemsToSell[i]);
                else
                    buyingButtonUIList[i].SetNewItem(null);
            }
            storeAnimator.SetBool("Open", true);
        }
        else 
        {
            currentPlayerState = PlayerState.Idle;
            storeAnimator.SetBool("Open", false);
        }
        
    }


    #region ItemsSelectStore
    public void AddToBuyingCart(int i) 
    {

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
