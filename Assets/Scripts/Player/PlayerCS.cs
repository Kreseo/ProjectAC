using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool nearNPC;

    //References
    public Animator storeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerState = PlayerState.Idle;
        currentPlayerDirection = PlayerDirection.Right;
        nearNPC = true;


    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        
        if(currentPlayerState == PlayerState.Idle) 
        {
            if (nearNPC) 
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
            storeAnimator.SetBool("Open", true);
        }
        else 
        {
            currentPlayerState = PlayerState.Idle;
            storeAnimator.SetBool("Open", false);
        }
        
    }
}
