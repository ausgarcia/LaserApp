using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Created by Austin Garcia
public class TimeManager : MonoBehaviour {
    private float timer;
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI multText;
    public GameObject TouchColliderPrefab;
    public GameObject deathPanel;
    public GameObject personalBestText; 
    private ScoreManager scoreMan;
    private List<GameObject> currentTouchColliders;
    private List<Touch> currentTouches;
    private int prevTouches;
    private bool stopped;
    // Use this for initialization
    void Start () {
        scoreMan = this.gameObject.GetComponent<ScoreManager>();
        timer = 0;
        //timerText.GetComponent<RectTransform>() = Screen.width;
        timerText.fontSize = (int)(Screen.width / 8.0f);
        multText.fontSize = (int)(Screen.width / 16.0f);
        multText.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, -timerText.fontSize+20, 0);
        prevTouches = 0;
        stopped = false;
    }
	
	// Update is called once per frame
	void Update () {
        int multiplier = 0;
        if (!stopped)
        {
            if (Input.touchCount > 0)
            {
                //print("touched");

                multiplier = Input.touchCount;
                timer += Time.deltaTime * multiplier;
                timerText.text = string.Format("{0:N2}", timer);
                multText.text = "x" + multiplier;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch t = Input.GetTouch(i);
                    //float tx = t.position.x;
                    //float ty = t.position.y;
                    //Current Bug, size of collider changes due to screen size/orientation
                    GameObject finger = Instantiate(TouchColliderPrefab, new Vector3(Camera.main.ScreenToWorldPoint(t.position).x, Camera.main.ScreenToWorldPoint(t.position).y, (float)-4.5), new Quaternion(90, 0, 0, 0));
                    finger.transform.eulerAngles = new Vector3(90, 0, 0);
                    finger.AddComponent<DestroyScript>();// Destroys collider after one frame
                }
            }
            else if (!multText.text.Equals("x0"))
            {
                multText.text = "x0";
            }
            prevTouches = multiplier;
        }
	}
    public void stopTime()
    {
        if (!stopped)
        {
            stopped = true;
            storeTime();    //inside for loop so any repeat calls are ignored
        }
    }
    public void displayPanel()
    {
        deathPanel.SetActive(true);
    }
    public void storeTime() //this is getting called multiple times for one death
    {
        scoreMan.addLocalScore(timer, scoreMan.getName());
        if (scoreMan.checkLocalHighScore(timer))
        {
            personalBestText.SetActive(true);
        }
    }
    public float getTimer()
    {
        return timer;
    }
    public void restartScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
