using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour {
    int i = 0;
    private GameObject manager;
    private TimeManager TM;
    private LaserSpawner LS;


	// Use this for initialization
	void Start () {

        manager = GameObject.Find("Controller");
        TM = manager.GetComponent<TimeManager>();
        LS = manager.GetComponent<LaserSpawner>();

    }
	private void OnTriggerEnter(Collider other)
    {
        print("COLLISION");
        if (other.gameObject.tag == "laser")
        {
            print("LASER");
            TM.stopTime();
            LS.stopSpawning();
        }
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
