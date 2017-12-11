using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds; //all backgrounds to be parallaxed
    private float[] parallaxScales; //the proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;    //how smooth the parallax will be (muss >0)

    private Transform cam;                  // reference to the main cameras transform
    private Vector3 previousCamPosition;    // the position of the camera in the previous frame

    //is called before start() 
    private void Awake(){
        //set up camera reference
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start () {
        //the previous frame had the current frame's camposition
        previousCamPosition = cam.position;

        //asigning coresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for(int i = 0; i<backgrounds.Length; i++){
            //parallaxScales[i] = backgrounds[i].position.z * -1;

            parallaxScales[i] = backgrounds[i].position.x * -1;
            Debug.Log(parallaxScales);
        }
	}
	
	// Update is called once per frame
	void Update () {
		// for each background
        for (int i = 0; i<backgrounds.Length; i++){
            //the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCamPosition.x - cam.position.x) * parallaxScales[i];

            //set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //create a target position which is the background's current position with it's target position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        //set the previousCamPos to the camera's pos at the frame
        previousCamPosition = cam.position;
	}
}
