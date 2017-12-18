using UnityEngine;

[ExecuteInEditMode]
public class ScaleWidthCamera : MonoBehaviour {

    public int targetWidth = 640;
    public float pixelsToUnits = 100f;
	
	// Update is called once per frame
	void Update () {
        int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);
        GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize﻿ = height / pixelsToUnits / 2;
    }
}
