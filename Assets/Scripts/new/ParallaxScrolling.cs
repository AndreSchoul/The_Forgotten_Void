// Author: André Schoul

using UnityEngine;

public class ParallaxScrolling : MonoBehaviour {

    public Transform[] backgrounds;
    public float smoothing = 15f;

    private float[] parallaxScales;
    private Vector3 previousCameraPosition;

    void Awake () {
        previousCameraPosition = transform.position;
        parallaxScales = new float[backgrounds.Length];
        for(int i = 0; i < parallaxScales.Length; i++) {
            parallaxScales[i] = i * 3 * -1;
        }
    }
	
	void Update () {
		for(int i = 0; i < backgrounds.Length; i++) {
            Vector3 parallax = (previousCameraPosition - transform.position) * (parallaxScales[i] / smoothing);
            backgrounds[i].position = new Vector3(backgrounds[i].position.x + parallax.x, backgrounds[i].position.y + parallax.y, backgrounds[i].position.z);
        }
        previousCameraPosition = transform.position;
    }
}
