using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneDemoScript : MonoBehaviour {

	public List<GameObject> vikings=new List<GameObject>();

	private bool showBarsVikings=true;
	private bool showTextVikings=true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * 10);
	}

	void OnGUI () {
		if (GUI.Button (new Rect (5, 5, 180, 30), "Show/Hide Vikings bars")) {
			if(showBarsVikings) {
				for(int i=0; i<vikings.Count; i++)
					vikings[i].GetComponent<LiveBar>().hideLiveBar();
				showBarsVikings=false;
			}
			else {
				for(int i=0; i<vikings.Count; i++)
					vikings[i].GetComponent<LiveBar>().showLiveBar();
				showBarsVikings=true;
			}
		}


		if (GUI.Button (new Rect (5, 50, 180, 30), "Show/Hide Vikings text")) {
			if(showTextVikings) {
				for(int i=0; i<vikings.Count; i++)
					vikings[i].GetComponent<LiveBar>().hideTextBar();
				showTextVikings=false;
			}
			else {
				for(int i=0; i<vikings.Count; i++)
					vikings[i].GetComponent<LiveBar>().showTextBar();
				showTextVikings=true;
			}
		}

		if (GUI.Button (new Rect (5, 100, 180, 30), "Random damage to Vikings")) {
			for(int i=0; i<vikings.Count; i++) {
				if(vikings[i].GetComponent<LiveBar>().getCurrentLive()>0) {
					vikings[i].GetComponent<LiveBar>().decreaseLive((int)Random.Range(0,25));
					if(vikings[i].GetComponent<LiveBar>().getCurrentLive()==0) {
						Destroy(vikings[i]);
						vikings.RemoveAt(i);
					}
				}
			}
		}
	}
}
