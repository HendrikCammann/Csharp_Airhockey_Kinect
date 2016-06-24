using UnityEngine; 
using System.Collections;
using Windows.Kinect;
using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

public class DetectDepth : MonoBehaviour {

    public GameObject DepthSrcManager;
    private MultiSourceManager depthManager;
    private ushort[] distances;
    private int concatStringInt;
    public bool SetupFinished = false;
    public float latency = 0.4f;
    private int lastXcoord = 0;
    private int lastYcoord = 0;
    public int thresholdBottom = 60;
    public int thresholdTop = 200;
    private bool allowMoving = true;
    private float timer;
    private bool foundHand = false;
    private int findHandX = 0;
    private int findHandY = 0;
    private int findHandIndex = 0;
    private int findHandCounter = 0;

    private Renderer rend;


    // Use this for initialization
    void Start () {
        if (DepthSrcManager == null)
        {
            Debug.Log("Assign Game Object with Depth Source Manager");
        }
        else
        {
            depthManager = DepthSrcManager.GetComponent<MultiSourceManager>();
            Debug.Log("Success");
        }
        rend = GetComponent<Renderer>();
        rend.enabled = false;
    }

	// Update is called once per frame
	void Update () {
        //Debug.Log("new");
        if (depthManager == null)
        {
            return;
        }

        distances = depthManager.GetDepthData();
        int aktDist = distances[4000];

        /*
        for(int i = 0; i < distances.Length; i++)
        {
            if(distances[i] < 800 || distances[i] > 1400)
            {
                distances[i] = 10000;
            }
        }
        */

        //int maxValue;
        //int maxIndex;

        // maxValue = (int) distances.Min();
        // maxIndex = (int) distances.ToList().IndexOf(distances.Min());

        int height = 424;
        int width = 512;

        float minValue = 10000;
        int xCoord = 0;
        int yCoord = 0;
        int index = 0;

        for (int x = 50; x < width-50; x++)
            for (int y = 50; y < height - 50; y++)
            {
                if ((minValue > distances[y * width + x]) && (distances[y * width + x] != 0)) 
                {
                    minValue = distances[y * width + x];
                    index = y * width + x;
                    xCoord = x;
                    yCoord = y;
                    timer = 0;
                }
            }

        
        if(!foundHand)
        {
            int[] test = findPlayerHand(distances, index, minValue, width);
            int aktHandX = test[0];
            int aktHandY = test[1];
            int aktIndex = test[2];

            if (aktHandX != 1 && aktHandY != 0)
            {
                int distanceToLastX = aktHandX - findHandX;
                int distanceToLastY = aktHandY - findHandY;

                if (distanceToLastX >= -10 && distanceToLastX <= 10 && distanceToLastY >= -10 && distanceToLastY <= 10)
                {
                    findHandCounter++;
                    Debug.Log(aktHandX + "," + aktHandY);
                    //Debug.Log("counter up");
                } 
            }

            if(findHandCounter >= 25)
            {
                Debug.Log("Found HAND!");
                Debug.Log(findHandX + "," + findHandY);
                findHandIndex = aktIndex;
                foundHand = true;
                rend.enabled = true;
            }

            findHandX = aktHandX;
            findHandY = aktHandY;
        } 

        /*
        if(foundHand)
        {
            int[] test2 = trackPlayerHand(distances, findHandIndex, width);

            int widthTracking = 512;
            float minValueTracking = 10000;
            int indexTracking = 0;

            for (int i = 0; i < distances.Length; i++)
            {
                if(!test2.Contains(distances[i]))
                {
                    distances[i] = 10001;
                } else
                {
                    if(minValueTracking > distances[i] && distances[i] != 0)
                    {
                        minValueTracking = distances[i];
                        indexTracking = i;
                        xCoord = indexTracking%widthTracking;
                        yCoord = indexTracking/widthTracking;
                        timer = 0;
                        findHandIndex = indexTracking;
                    }
                }
            }
        }

        if(xCoord != 0 && yCoord != 0)
        {
            allowMoving = true;
        } else
        {
            allowMoving = false;
        }

        */
        xCoord = xCoord - 256;
        yCoord = (yCoord - 212) * 2;

        //Debug.Log("(x,y)= " + xCoord + "," + yCoord);

        if (xCoord < -230)
        {
            xCoord = -230;
        }

        if(xCoord > 230)
        {
            xCoord = 230;
        }

        if(yCoord < -350)
        {
            yCoord = -350;
        }

        if(yCoord > 350)
        {
            yCoord = 350;
        }

        //y-Coord
        // int yCoord = (int)(maxIndex / width);
  
        //x-Coord
        // int xCoord = (int)(maxIndex % width);
        // Debug.Log("(x,y)= " + xCoord + "," + yCoord);
        //Debug.Log("y= " + yCoord);
        int xCoordOffset = 0;
        int yCoordOffset = 0;

        xCoordOffset = (xCoord - lastXcoord);
        yCoordOffset = (yCoord - lastYcoord);
        //Debug.Log("xOffset = " + xCoordOffset);
        //Debug.Log("yOffset = " + yCoordOffset);

        if (distances == null)
        {
            return;
        }

        /*
        bool thresholdBottomBool = false;
        bool thresholdTopBool = false;

        //Check if Offset is big enough - Rauschunterdrückung
        if (xCoordOffset > thresholdBottom || xCoordOffset < (thresholdBottom * -1) || yCoordOffset > thresholdBottom || yCoordOffset < (thresholdBottom * -1))
        {
            thresholdBottomBool = true;
        }

        //Check if Offest is not too large 
        if ((xCoordOffset < thresholdTop && (xCoordOffset > (thresholdTop * -1))) && (yCoordOffset < thresholdTop && (yCoordOffset > (thresholdTop * -1))))
        {
            thresholdTopBool = true;
        }

        //if is in between thresholds
        if (thresholdTopBool && thresholdBottomBool)
        {
            lastXcoord = xCoord;
            lastYcoord = yCoord;
            //Debug.Log(xCoordOffset + " xOffset");
            Debug.Log(yCoordOffset + " yOffset");
            timer = 0;
            //Debug.Log("Timer reset");
        }
        */


        /*
        int offset = (lastDist - aktDist) * 2;
        if(offset > 400)
        {
            offset = 500;
        }

        lastDist = aktDist;
        */
        //Debug.Log(offset);

        if (allowMoving)
        {
            Vector3 oldPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Vector3 newPos = new Vector3(xCoord, gameObject.transform.position.y, yCoord);
            //Debug.Log("newPos: x=" + newPos.x + " z=" + newPos.z);
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(oldPos, newPos, timer / latency);
        }
        /*
        //Vector3 newPos = new Vector3(gameObject.transform.position.x + xCoordOffset, 0, gameObject.transform.position.z + yCoordOffset);
        float newZCoord = gameObject.transform.position.z + (xCoordOffset * 2);
        if(newZCoord > 310 || newZCoord < -310)
        {
            if(newZCoord >= 0)
            {
                newZCoord = 310;
            }
            else
            {
                newZCoord = -310;
            }
            //Debug.Log("yOffset FIX!");
        }
        */
        /*
        if (thresholdTopBool && thresholdBottomBool)
        {
            //gameObject.transform.position = Vector3.Lerp(oldPos, newPos, latency * Time.deltaTime);
            //gameObject.transform.position = Vector3.Lerp(oldPos, newPos, 2);
            //gameObject.transform.position = Vector3.MoveTowards(oldPos, newPos, latency * Time.deltaTime);
            //gameObject.transform.position = newPos;
            timer += Time.deltaTime;
            Debug.Log("Lerping");
            //Debug.Log(Time.deltaTime + " delta");
            //Debug.Log(timer + " timer");
            transform.position = Vector3.Lerp(oldPos, newPos, timer / latency);
        }
        */
    }

    public int[] checkForContrast(int indexOfLargestInArray, int largestInArray, int width, ushort[]array)
    {
        bool inArrayBoundsFirst = true;
        bool inArrayBoundsSecond = true;
        bool inArrayBoundsThird = true;
        int indexForTest = indexOfLargestInArray;
        int xValueInPicUpwards = 0;
        int xValueInPicDownwards = 0;
        int xValueInPicLeft = 0;
        int[] returnArray = null;

        while (inArrayBoundsFirst)
        {
            indexForTest = indexForTest - width;

            if (indexForTest < 0)
            {
                inArrayBoundsFirst = false;
                break;
            }

            int aktValue = array[indexForTest];
            int contrast = largestInArray - aktValue;

            if(contrast > 200)
            {
                xValueInPicUpwards = indexForTest;
                break;
            }
        }

        while(inArrayBoundsSecond)
        {
            indexForTest = indexForTest + width;

            if(indexForTest > array.Length)
            {
                inArrayBoundsSecond = false;
                break;
            }

            if(indexForTest > 217088)
            {
                indexForTest = 217087;
            }

            int aktValue2 = array[indexForTest];
            int contrast2 = largestInArray - aktValue2;

            if(contrast2 > 200)
            {
                xValueInPicDownwards = indexForTest;
                break;
            }
        }

        Debug.Log(xValueInPicDownwards + "Down");
        Debug.Log(xValueInPicUpwards + "Up");

        double indexOfNumberInbetweenConstrastDropsDouble =(xValueInPicDownwards + xValueInPicUpwards) / 2;

        Debug.Log(indexOfNumberInbetweenConstrastDropsDouble + " indexDropsDouble");

        int indexOfNumerInbetweenConstrastDrops = (int) Math.Ceiling(indexOfNumberInbetweenConstrastDropsDouble);

        double helper = indexOfNumerInbetweenConstrastDrops / 2;
        int rowInPicture = (int)Math.Floor(helper);

        while(inArrayBoundsThird)
        {
            indexOfNumerInbetweenConstrastDrops = indexOfNumerInbetweenConstrastDrops - 1;
            if(indexOfNumerInbetweenConstrastDrops% width == 0)
            {
                inArrayBoundsThird = false;
                break;
            }

            if(indexOfNumerInbetweenConstrastDrops > 217088)
            {
                indexOfNumerInbetweenConstrastDrops = 217087;
            }

            int aktValue3 = array[indexOfNumerInbetweenConstrastDrops];
            var contrast3 = largestInArray - aktValue3;

            if(contrast3 > 200)
            {
                xValueInPicLeft = indexOfNumerInbetweenConstrastDrops % width;
                break;
            }

            int xValueForTrade = xValueInPicLeft;
            int yValueForTrade = rowInPicture;

            returnArray[0] = xValueForTrade;
            returnArray[1] = yValueForTrade;
        }

        return returnArray;
    } 

    public int[] getElementsAroundHighValue(int indexOfLargestInArray, int width, ushort[] array)
    {
        int numSelectedRows = 50;
        int numSelectedCols = 50;
        int[] neededIndexes = null;

        int startingXValue = indexOfLargestInArray - 1 - width;

        for(int i=0; i < numSelectedRows; i++)
        {
            if(i==0)
            {
                for(int j = 0; j < numSelectedCols; j++)
                {
                    int index = startingXValue + j;
                    int length = neededIndexes.Length;
                    neededIndexes[length - 1] = index;
                }
            }
            else
            {
                startingXValue += width;
                for(int k = 0; k < numSelectedCols; k++)
                {
                    int index2 = startingXValue + k;
                    int length = neededIndexes.Length;
                    neededIndexes[length - 1] = index2;
                }
            }
        }

        return neededIndexes;
    }

    //not perfecty but idea is right - little bit of jumping - mabye because of holding kinect in one hand
    public int[] findPlayerHand(ushort[] depthData, int indexOfLargest, float largest, int width)
    {
        int[] returnArray = { 0, 0, 0 };
        int index = indexOfLargest;
        int leftXvalue = 0;
        int rightXvalue = 0;
        int topYvalue = 0;
        int contrast = -200;
        bool allowedRunning = true;

        while (allowedRunning)
        {
            index -= 1;
            if ((index % width < 50) || (index % width > width - 50))
            {
                leftXvalue = index;
                allowedRunning = false;
                break;
            }
            else
            {
                float currentDepthValue = depthData[index];
                int currentContrast = (int)(largest - currentDepthValue);
                if (currentContrast < contrast)
                {
                    leftXvalue = index;
                    allowedRunning = false;
                    break;
                }
            }
        }

        index = indexOfLargest;
        allowedRunning = true;

        while (allowedRunning)
        {
            index += 1;
            if (index % width < 50 || (index % width > width - 50))
            {
                rightXvalue = index;
                allowedRunning = false;
                break;
            }
            else
            {
                float currentDepthValue = depthData[index];
                int currentContrast = (int)(largest - currentDepthValue);
                if (currentContrast < contrast)
                {
                    rightXvalue = index;
                    allowedRunning = false;
                    break;
                }
            }
        }

        int averageRightandLeft = (rightXvalue + leftXvalue) / 2;
        index = averageRightandLeft;
        allowedRunning = true;

        while (allowedRunning)
        {
            index += 1;
            if (index + width < 49 * width)
            {
                topYvalue = index;
                allowedRunning = false;
                break;
            }
            else
            {
                float currentDepthValue = depthData[index];
                int currentContrast = (int)(largest - currentDepthValue);
                if (currentContrast < contrast)
                {
                    topYvalue = index;
                    allowedRunning = false;
                    break;
                }
            }
        }

        //y-Coord
        int yValue = (int)(topYvalue / width);

        //x-Coord
        int xValue = (int)(topYvalue % width);


        returnArray[0] = xValue;
        returnArray[1] = yValue;
        returnArray[2] = topYvalue; //index in ARRAY

        return returnArray;
    }

    //needs testing
    public int[] trackPlayerHand(ushort[] depthData, int indexOfLargest, int width)
    {
        int[] returnArray = { 0 };
        int index = indexOfLargest;
        int numRowToCheck = 25;

        int startingIndex = indexOfLargest - ((numRowToCheck / 2) * width);
        int startingIndexCountUp = startingIndex;

        for (int j = 0; j < numRowToCheck; j++)
        {
            if (j > 0)
            {
                startingIndex = indexOfLargest - ((numRowToCheck / 2) * width) - (j * width);
                startingIndexCountUp = startingIndex;
            }

            for (int i = (startingIndex % width); i >= 50; i--)
            {
                int arrayLength = returnArray.Length;
                if (arrayLength == 1)
                {
                    returnArray[0] = startingIndexCountUp;
                }
                else
                {
                    returnArray[arrayLength - 1] = startingIndexCountUp;
                }

                startingIndexCountUp = startingIndexCountUp - 1;
            }

            startingIndexCountUp = startingIndex + 1;

            for (int k = ((startingIndex % width) + 1); k <= width - 50; k++)
            {
                int arrayLength = returnArray.Length;
                if (arrayLength == 1)
                {
                    returnArray[0] = startingIndexCountUp;
                }
                else
                {
                    returnArray[arrayLength - 1] = startingIndexCountUp;
                }

                startingIndexCountUp = startingIndexCountUp + 1;
            }
        }

        return returnArray;
    }
}

