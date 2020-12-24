using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerCS : MonoBehaviour
{

    public GameObject tileHolder;
    public List<GameObject> tileList;

    private byte[][] tileMatrix;
    // Start is called before the first frame update
    void Start()
    {

        tileMatrix = new byte[5][];
        InitializeMatrix();
        RenderMatrix();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeMatrix()
    {
        tileMatrix[0] = new byte[] { 0, 0, 1, 1, 1};
        tileMatrix[1] = new byte[] { 0, 0, 0, 0, 0};
        tileMatrix[2] = new byte[] { 0, 0, 0, 0, 0};
        tileMatrix[3] = new byte[] { 0, 0, 0, 0, 0};
        tileMatrix[4] = new byte[] { 0, 0, 0, 0, 0};
    }

    void RenderMatrix()
    {
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                GameObject newTile = (GameObject)GameObject.Instantiate(tileList[tileMatrix[row][col]], new Vector2(col , (row * -1)), tileList[tileMatrix[row][col]].transform.rotation);
                newTile.transform.SetParent(tileHolder.transform);
            }
        }
    }
}
