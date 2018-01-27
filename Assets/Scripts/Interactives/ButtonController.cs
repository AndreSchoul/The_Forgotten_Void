using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonController : MonoBehaviour {

    private bool buttonHit = false;
    private bool stay = false;
    private Vector3 startPosition;
    private Vector3 pressPosition;
    //public GameObject fahrStuhl;

    private static List<GameObject> allElevators = new List<GameObject>();

    public GameObject fahrStuhl;

    private void Awake()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        pressPosition = new Vector3(transform.position.x, transform.position.y - 0.4f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.tag == "Hero" || collision.tag == "Interactable") && buttonHit == false)
        {
            transform.position = pressPosition;
            //GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
            buttonHit = true;
            fahrStuhl.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Hero" || collision.tag == "Interactable")
        {
            buttonHit = true;
            fahrStuhl.GetComponent<FahrstuhlController>().enabled = true;
            transform.position = pressPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.tag == "Hero" || collision.tag == "Interactable") && buttonHit == true)
        {
            
            transform.position = startPosition;
            //GetComponent<SpriteRenderer>().sortingLayerName = "Interactable";
            buttonHit = false;
            fahrStuhl.GetComponent<FahrstuhlController>().enabled = false;
            stay = false;
        }
    }
}
