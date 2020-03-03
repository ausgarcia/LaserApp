using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour {
    public GameObject laserPrefab;
    public GameObject laserWarningPrefab;
    public TimeManager TM;
    private int numLasersToSpawn;
    private float warningTime;
    private bool spawning;
    private float spawnTime;
	// Use this for initialization
	void Start () {
        spawning = true;
        //InvokeRepeating("spawnLaser", 1, 2);
        numLasersToSpawn = 1;
        warningTime = .7f;
        spawnTime = 1;
        StartCoroutine(spawnLaser());
        //TM = this.gameObject.GetComponent<TimeManager>(); // assuming laser spawner and time manager scripts are attached to the same gameObject
        //TM = GameObject.Find("Controller").GetComponent<TimeManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator spawnLaser()
    {
        while (spawning)
        {
            if(TM == null)//WHY IS THE GAMEOBJECT FOUND
            {
                print("whaaaaat");
            }
            if(TM.getTimer() < 500)//have to make sure spawntime > 0 //BUT THE COMPONENT ISNT
            {
                spawnTime = 1f - .2f * (int)(TM.getTimer() / 100);
            }
            


            yield return new WaitForSeconds(spawnTime);
            for (int i = 0; i < numLasersToSpawn; i++)
            {
                warningTime = .7f * spawnTime;
                float spawnY = Random.Range
                    (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
                float spawnX = Random.Range
                    (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

                Vector2 spawnPosition = new Vector2(spawnX, spawnY);
                float rand = Random.Range(0f, 180f);
                GameObject laserWarning = Instantiate(laserWarningPrefab, spawnPosition, Quaternion.identity);
                laserWarning.transform.eulerAngles = new Vector3(laserWarning.transform.eulerAngles.x, laserWarning.transform.eulerAngles.y, rand);
                laserWarning.AddComponent<LaserDestroyScript>();
                laserWarning.GetComponent<LaserDestroyScript>().secToWait = warningTime+.01f;
                yield return new WaitForSeconds(warningTime);
                GameObject laser = Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
                laser.transform.eulerAngles = new Vector3(laser.transform.eulerAngles.x, laser.transform.eulerAngles.y, rand);
                laser.AddComponent<LaserDestroyScript>();
                laser.GetComponent<LaserDestroyScript>().secToWait = 1.5f * spawnTime;
            }
            yield return new WaitForSeconds(spawnTime*2);
        }
    }
    public void stopSpawning()
    {
        spawning = false;
        //CancelInvoke(); //cancels all invokes
    }
}
