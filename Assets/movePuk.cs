using UnityEngine;
using System.Collections;

public class movePuk : MonoBehaviour {

	GameObject focusObj;
	Vector3 lastPos;
	Vector3 lastVelocity;

	void Update() {
		if (Input.GetMouseButtonDown (0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay
			(Input.mousePosition);
			if (!Physics.Raycast (ray, out hit, 10000))
				return;

			if (hit.transform.gameObject.tag == "stick") {
				focusObj = hit.transform.gameObject;
			}	
		} else if (focusObj && (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))) {
			focusObj.GetComponent<Rigidbody> ().AddForce (lastVelocity * 1000);
			focusObj = null;
		} else if (focusObj && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))) {
			Vector3 mPos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
			focusObj.transform.position = new Vector3 (mPos.x, focusObj.transform.position.y, mPos.z);

			lastVelocity = (focusObj.transform.position - lastPos);
			lastPos = focusObj.transform.position;
		}	
			
	}	
}
