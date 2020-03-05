using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LaserSpawner : MonoBehaviour {
    public GameObject laserPrefab;
    public GameObject laserWarningPrefab;
    public TimeManager TM;
    public PostProcessVolume PV;
    private int numLasersToSpawn;
    private float warningTime;
    private bool spawning;
    private float spawnTime;
    private LensDistortion LDsettings;
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
            
            if(TM.getTimer() < 500)//have to make sure spawntime > 0 
            {
                spawnTime = 1f - .2f * (int)(TM.getTimer() / 100);
            }
            //if (TM.getTimer() > 50)
                //numLasersToSpawn = 2;//changing the number of lasers to spawn doesnt work super well with the coroutine waits, adjust for harder levels


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
                laser.GetComponent<LaserDestroyScript>().LS = this;
                //pp effects
                /*bool foundEffects = PV.profile.TryGetSettings<LensDistortion>(out LDsettings);
                if (foundEffects)
                {
                    LDsettings.intensity.value = 60;
                }*/
                //StartCoroutine(lensEffect(1.5f*spawnTime));//not feeling the lens effects rn
            }
            yield return new WaitForSeconds(spawnTime*2);
        }
    }
    IEnumerator lensEffect(float secToWait)
    {
        bool foundEffects = PV.profile.TryGetSettings<LensDistortion>(out LDsettings);
        if (foundEffects)
        {
            float maxInten = 60;
            float time = 0;
            while (time < secToWait/2)
            {
                if (time < secToWait / 3)
                {
                    LDsettings.intensity.value += 1;
                }
                else if (time < secToWait * 2 / 3)
                {
                    LDsettings.intensity.value -= 2;
                }
                else
                {
                    LDsettings.intensity.value += 1;
                }

                time += Time.deltaTime;
                //print("about to wait: " + secToWait / maxInten);
                //print("set intensity value: " + LDsettings.intensity.value);
                yield return new WaitForSeconds(secToWait/maxInten);
            }
                

        }
        
    }
    public void stopSpawning()
    {
        spawning = false;
        //CancelInvoke(); //cancels all invokes
    }
    /*public void endEffects()
    {
        bool foundEffects = PV.profile.TryGetSettings<LensDistortion>(out LDsettings);
        if (foundEffects)
        {
            LDsettings.intensity.value = 0;
        }
    }*/
}
