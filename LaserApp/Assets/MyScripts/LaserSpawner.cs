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
    public AudioSource LaserSound;
    public AudioSource WarningSound;
    public AudioSource HitSound;
    public Material touchSpotMat;
    // Use this for initialization
    void Start () {
        bool muteVal = this.gameObject.GetComponent<ScoreManager>().getMuteVal();
        LaserSound.mute = muteVal;
        WarningSound.mute = muteVal;
        HitSound.mute = muteVal;
        spawning = true;
        //InvokeRepeating("spawnLaser", 1, 2);
        numLasersToSpawn = 1;
        warningTime = .7f;
        spawnTime = 1;
        StartCoroutine(spawnLaser());
        //TM = this.gameObject.GetComponent<TimeManager>(); // assuming laser spawner and time manager scripts are attached to the same gameObject
        //TM = GameObject.Find("Controller").GetComponent<TimeManager>();
    }
	
    IEnumerator spawnLaser()
    {
        while (spawning)
        {
            
            if(TM.getTimer() < 1000)//have to make sure spawntime > 0 
            {
                spawnTime = 1f - .1f * (int)(TM.getTimer() / 100);
            }
            if (TM.getTimer() > 200)//add lasers to spawn, every too
                numLasersToSpawn = (int)(TM.getTimer() / 200) + 1;//changing the number of lasers to spawn doesnt work super well with the coroutine waits, adjust for harder levels
            //GameObject[] lasers = new GameObject[numLasersToSpawn];
            //GameObject[] warnings = new GameObject[numLasersToSpawn];
            Vector2[] spawnPosition = new Vector2[numLasersToSpawn];
            float[] rand = new float[numLasersToSpawn];
            yield return new WaitForSeconds(spawnTime);
            for (int i = 0; i < numLasersToSpawn; i++)
            {
                warningTime = .8f * spawnTime;
                float spawnY = Random.Range
                    (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
                float spawnX = Random.Range
                    (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

                spawnPosition[i] = new Vector2(spawnX, spawnY);
                rand[i] = Random.Range(0f, 180f);
                GameObject laserWarning = Instantiate(laserWarningPrefab, spawnPosition[i], Quaternion.identity);
                WarningSound.Play();
                laserWarning.transform.eulerAngles = new Vector3(laserWarning.transform.eulerAngles.x, laserWarning.transform.eulerAngles.y, rand[i]);
                laserWarning.AddComponent<LaserDestroyScript>();
                laserWarning.GetComponent<LaserDestroyScript>().secToWait = warningTime + .01f;
            }
                yield return new WaitForSeconds(warningTime);
            for (int i = 0; i < numLasersToSpawn; i++)
            {
                GameObject laser = Instantiate(laserPrefab, spawnPosition[i], Quaternion.identity);
                LaserSound.Play();
                laser.transform.eulerAngles = new Vector3(laser.transform.eulerAngles.x, laser.transform.eulerAngles.y, rand[i]);
                laser.AddComponent<LaserDestroyScript>();
                laser.GetComponent<LaserDestroyScript>().secToWait = 1.5f * spawnTime;
                laser.GetComponent<LaserDestroyScript>().LS = this;
            }
                //pp effects
                /*bool foundEffects = PV.profile.TryGetSettings<LensDistortion>(out LDsettings);
                if (foundEffects)
                {
                    LDsettings.intensity.value = 60;
                }*/
                //StartCoroutine(lensEffect(1.5f*spawnTime));//not feeling the lens effects rn
            
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

    public void playHitSound()
    {
        HitSound.Play();
        return;
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
