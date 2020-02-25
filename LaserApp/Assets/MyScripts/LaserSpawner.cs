using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour {
    public GameObject laserPrefab;
    private int numLasersToSpawn;
	// Use this for initialization
	void Start () {
        InvokeRepeating("spawnLaser", 1, 2);
        numLasersToSpawn = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void spawnLaser()
    {
        for (int i = 0; i < numLasersToSpawn; i++)
        {
            float spawnY = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float spawnX = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            Vector2 spawnPosition = new Vector2(spawnX, spawnY);
            float rand = Random.Range(0f, 180f);
            //Quaternion laserQuat = new Quaternion(Quaternion.identity.x, Quaternion.identity.y, rand, Quaternion.identity.w);
            print(rand);
            GameObject laser = Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
            laser.transform.eulerAngles  = new Vector3(laser.transform.eulerAngles.x, laser.transform.eulerAngles.y, rand);
            laser.AddComponent<LaserDestroyScript>();
            laser.GetComponent<LaserDestroyScript>().framesToWait = 100;
        }
    }
    public void stopSpawning()
    {
        CancelInvoke(); //cancels all invokes
    }
}
