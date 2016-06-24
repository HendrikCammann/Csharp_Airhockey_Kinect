using UnityEngine;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;


public class OriginSetup : MonoBehaviour {

    public GameObject DepthSrcManager;
    private MultiSourceManager depthManager;
    private ushort[] distances;
    private Texture2D image; 


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
    void Update()
    {
        if (depthManager == null)
        {
            return;
        }

        distances = depthManager.GetDepthData();
        image = depthManager.GetColorTexture();

        //Debug.Log(image.format);
        /*
        Image<Gray, byte> depthImage = new Image<Gray, byte>(640, 480);
        byte[] depthPixelData = new byte[512 * 424];
        for(int i = 0; i < distances.Length; i++)
        {
            depthPixelData[i] = (byte) (distances[i] / 31.37);
        }
        depthImage.Bytes = depthPixelData;

        Debug.Log(depthImage);
        */

    }

}