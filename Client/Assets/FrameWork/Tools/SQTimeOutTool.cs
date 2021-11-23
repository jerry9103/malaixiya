using UnityEngine;
using System.Collections;


public class SQTimeOutTool : MonoBehaviour {

	public static SQTimeOutTool It;

	void Awake(){
		It = this;
	}

	public void AddDelay(System.Action call, float timeOut){
		StartCoroutine(TimeDown (call, timeOut));
	}


	IEnumerator TimeDown(System.Action call, float time){
		yield return new WaitForSeconds (time);
		call ();
	}
}
