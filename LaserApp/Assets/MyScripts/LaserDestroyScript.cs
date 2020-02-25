using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyScript : MonoBehaviour {
    public float secToWait;
    private int i;
    private bool destroy;
	// Use this for initialization
	void Start () {
        destroy = true;
        StartCoroutine(timedDestroy());
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    IEnumerator timedDestroy()
    {
        yield return new WaitForSeconds(secToWait);
        if (destroy)
        {
            Destroy(this.gameObject);
        }
    }
    public void dontDestroy()
    {
        destroy = false;
    }
}
