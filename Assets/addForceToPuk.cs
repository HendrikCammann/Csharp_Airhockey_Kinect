using UnityEngine;
using System.Collections;

public class addForceToPuk : MonoBehaviour {

    public float forceAPP = 5000;
    public GameObject puk;
    public GameObject stickObject;
    public Rigidbody stick;
    Vector3 lastVelocity;



    void OnCollisionEnter(Collision collison)
    {
        if(collison.gameObject.tag == "stick")
        {
            stickObject = GameObject.FindGameObjectWithTag("stick");
            stick = stickObject.GetComponent<Rigidbody>();
            puk = GameObject.FindGameObjectWithTag("puk");
            lastVelocity = stick.velocity;
            Debug.Log("Puk was Hit");
            //puk.GetComponent<Rigidbody>().AddForce(lastVelocity * 500);

            puk.GetComponent<Rigidbody>().AddForce(-transform.forward * 500, ForceMode.Acceleration);
            //puk.GetComponent<Rigidbody>().AddForce(lastVelocity * 1000);
            puk = null;
            
        }
    }
}
