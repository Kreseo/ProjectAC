using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCS : MonoBehaviour
{

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x,player.position.y,-10);
    }
}
