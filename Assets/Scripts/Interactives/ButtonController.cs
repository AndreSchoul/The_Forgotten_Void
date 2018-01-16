using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonController : MonoBehaviour {

    private bool buttonHit = false;
    private bool stay = false;
    //public GameObject fahrStuhl;

    private static List<GameObject> allElevators = new List<GameObject>();

    public GameObject fahrStuhl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.tag == "Hero" || collision.tag == "Interactable") && buttonHit == false && stay == false)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f, 0);
            GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
            buttonHit = true;
            fahrStuhl.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Hero" || collision.tag == "Interactable")
        {
            stay = true;
            buttonHit = true;
            fahrStuhl.GetComponent<FahrstuhlController>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.tag == "Hero" || collision.tag == "Interactable") && buttonHit == true)
        {
            
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, 0);
            GetComponent<SpriteRenderer>().sortingLayerName = "Interactable";
            buttonHit = false;
            fahrStuhl.GetComponent<FahrstuhlController>().enabled = false;
            stay = false;
        }
    }
}
