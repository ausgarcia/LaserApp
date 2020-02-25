using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyScript : MonoBehaviour {
    public int framesToWait;
    private int i;
	// Use this for initialization
	void Start () {
        i = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(i == framesToWait) //destroy gameobject after one frame
        {
            Destroy(this.gameObject);
        }
        i++;
	}
}
