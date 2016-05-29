using UnityEngine;
using System.Collections;

public class OriginSetup : MonoBehaviour {

    public GameObject DepthSrcManager;
    private MultiSourceManager depthManager;
    private ushort[] distances;

	// Use this for initialization
	void Start () {
        if(DepthSrcManager == null)
        {
            Debug.Log("Assign Game Object with Depth Source Manager");
        }
        else
        {
            depthManager = DepthSrcManager.GetComponent<MultiSourceManager>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (depthManager == null)
        {
            return;
        }

        distances = depthManager.GetDepthData();

        int center = distances.Length / 2;

        int yCoord = (int)(center / 512);
        int xCoord = (int)(center % 512);

        gameObject.transform.position = new Vector3(xCoord, gameObject.transform.position.y, yCoord);
    }
}