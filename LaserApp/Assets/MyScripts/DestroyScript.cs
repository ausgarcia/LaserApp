using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour {
    int i = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(i == 1) //destroy gameobject after one frame
        {
            Destroy(this.gameObject);
        }
        i++;
	}
}
