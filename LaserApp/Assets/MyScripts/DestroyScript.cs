using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour {
    int i = 0;
    private GameObject manager;
    private TimeManager TM;
    private LaserSpawner LS;
    private bool startCircleAnim;
    private bool destroy;
    private float circleSize;
    private GameObject CircleEffect;

    // Use this for initialization
    void Start () {

        manager = GameObject.Find("Controller");
        TM = manager.GetComponent<TimeManager>();
        LS = manager.GetComponent<LaserSpawner>();
        startCircleAnim = false;
        destroy = true;
        circleSize = 0.01f;

    }
	private void OnTriggerEnter(Collider other)
    {
        //print("COLLISION");
        if (other.gameObject.tag == "laser")
        {
            //print("LASER");
            TM.stopTime();
            TM.displayPanel();
            LS.stopSpawning();
            other.gameObject.GetComponent<LaserDestroyScript>().dontDestroy();
            CircleEffect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            CircleEffect.transform.localScale= new Vector3(circleSize, circleSize, 1.0f);
            CircleEffect.transform.position = this.transform.position;
            startCircleAnim = true;
            destroy = false;
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }
	// Update is called once per frame
	void Update () {
        if (startCircleAnim && circleSize < 150)
        {
            circleSize += .4f;
            //CircleEffect
            CircleEffect.transform.localScale = new Vector3(circleSize, circleSize, 1.0f);
            CircleEffect.layer = 8;
        }
		if(i == 1 && destroy) //destroy gameobject after one frame
        {
            Destroy(this.gameObject);
        }
        i++;
	}
}
