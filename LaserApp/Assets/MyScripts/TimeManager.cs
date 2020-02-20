using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Created by Austin Garcia
public class TimeManager : MonoBehaviour {
    private double timer;
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI multText;
    public GameObject TouchColliderPrefab;
    private List<GameObject> currentTouchColliders;
    private List<Touch> currentTouches;
    private int prevTouches;
    // Use this for initialization
    void Start () {
        timer = 0;
        //timerText.GetComponent<RectTransform>() = Screen.width;
        timerText.fontSize = (int)(Screen.width / 8.0f);
        multText.fontSize = (int)(Screen.width / 16.0f);
        multText.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, -timerText.fontSize+20, 0);
        prevTouches = 0;
    }
	
	// Update is called once per frame
	void Update () {
        int multiplier = 0;
        if (Input.touchCount > 0)
        {
            //print("touched");
            
            multiplier = Input.touchCount;
            timer += Time.deltaTime*multiplier;
            //timer = System.Math.Truncate(1000 * timer) / 1000;
            timerText.text = string.Format("{0:N2}",timer);
            multText.text = "x" + multiplier;
            //if (multiplier > prevTouches)
            //{
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch t = Input.GetTouch(i);
                    //float tx = t.position.x;
                    //float ty = t.position.y;
                    //Current Bug, size of collider changes due to screen size/orientation
                    GameObject finger = Instantiate(TouchColliderPrefab, new Vector3(Camera.main.ScreenToWorldPoint(t.position).x, Camera.main.ScreenToWorldPoint(t.position).y, (float)-4.5), new Quaternion(90, 0, 0, 0));
                    finger.transform.eulerAngles = new Vector3(90, 0, 0);
                finger.AddComponent<DestroyScript>();// Destroys collider after one frame
                    //Destroy(finger);
                    //currentTouchColliders.Add(finger);//One of these only allows one finger to appear at a time
                    //currentTouches.Add(t);
                }
            //}
            /*else if (multiplier < prevTouches)
            {
                GameObject colliderToDelete = currentTouchColliders[currentTouchColliders.Count - 1];
                currentTouchColliders.Remove(colliderToDelete);
                Destroy(colliderToDelete);

                Touch touchToDelete = currentTouches[currentTouches.Count - 1];
                currentTouches.Remove(touchToDelete);
            }*/
            /*for(int i = 0; i < currentTouchColliders.Count; i++)
            {
                currentTouchColliders[i].transform.position = new Vector3(Camera.main.ScreenToWorldPoint(currentTouches[i].position).x, Camera.main.ScreenToWorldPoint(currentTouches[i].position).y, (float)-4.5);

            }*/
        }
        else if(!multText.text.Equals("x0"))
        {
            multText.text = "x0";
        }
        prevTouches = multiplier;


        //GameObject g = Instantiate(TouchColliderPrefab, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, (float)-4.5), Quaternion.identity);
        //g.transform.eulerAngles = new Vector3(90, 0, 0);
	}
}
