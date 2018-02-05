using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlendenController : MonoBehaviour {
    public float zoomfaktor = 1f;                   // wie schnell wird rausgezoomt
    public float blacktimer = 2f;                   // Parameter wie lange Schwarzgeblendet bleibt
    public float blendenspeed = 0.010f;             // Wie Schnell die blende an sich ist
    public float rotationsGeschwindigkeit = 30f;    // Wie schnell wird rotiert 0 = keine rotation
    public AudioSource kampfBlendenSound;
    public Canvas canvas;
    public static bool isBlenden = false;

    private bool wirdDunkel = false;
    private bool wirdHell = false;

    private SpriteRenderer blende;
    private GameObject cam;
    private GameObject jnrChar;

    void Start()
    {
        blende = GetComponent<SpriteRenderer>();
        kampfBlendenSound = GetComponent<AudioSource>();
        cam = GameObject.Find("Main Camera");
        jnrChar = GameObject.FindGameObjectWithTag("Hero");

        canvas.GetComponent<CanvasGroup>().alpha = 0;
    }

    void FixedUpdate() {
        if (isBlenden)
        {
            // Aktuelle Farbe der Blende
            Color current = new Color(blende.color.r, blende.color.g, blende.color.b, blende.color.a);
 
            // Schwarzblende
            if (wirdDunkel)
            {
                // Alphakanal und Rotation VLT noch reinzoomen?
                blende.color = new Color(current.r, current.g, current.b, current.a + blendenspeed);
                cam.transform.Rotate(0,0, rotationsGeschwindigkeit);
    
                // Wenn Schwarz Kamera auf Kampfposition bringen
                if (blende.color.a >= 1)
                {
                    // Wenn im Kampf
                    if (PlayerController.isInFight) {                        
                        cam.transform.position = new Vector3(-200, 0, -25);
                        cam.transform.Rotate(0, 0, cam.transform.eulerAngles.z * -1f);                       
                    }
                    else
                    {
                        Vector3 jnrPosition = new Vector3(jnrChar.transform.position.x, jnrChar.transform.position.y, -20);
                        cam.transform.position = jnrPosition;
                        cam.transform.Rotate(0, 0, cam.transform.eulerAngles.z * -1f);
                    }                                        
                }
                
                // Startet Rückblende auf Weiß
                if (blende.color.a >= 1f * blacktimer)
                {
                    wirdDunkel = false;
                    wirdHell = true;


                    //canvas.GetComponent<CanvasGroup>().alpha = blende.color.a;
                }

                if (blende.color.a >= 0.5f * blacktimer) canvas.GetComponent<CanvasGroup>().alpha += 0.025f;
            }

            // Auf Schicht blenden
            if(wirdHell)
            {
                blende.color = new Color(current.r, current.g, current.b, current.a - blendenspeed);
                if (cam.transform.position.z - zoomfaktor >= -20)
                {
                    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z - zoomfaktor);
                }
                                
                // Blenden beenden
                if (blende.color.a <= 0)
                {
                    wirdHell = false;
                    isBlenden = false;
                }
            }
        }
    }
    
    // Öffentliche Funktion zum Blenden kann von überall Aufgerufen werden
    public void blenden()
    {
        kampfBlendenSound.Play();
        isBlenden = true;
        wirdDunkel = true;
    }
}