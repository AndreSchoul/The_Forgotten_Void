using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUp_Controller : MonoBehaviour
{
    private static DamagePopUp popUpText;
    private static GameObject canvas;

    public static void Initialize() {
        canvas = GameObject.Find("BattleUI");
        if(!popUpText) {
            popUpText = Resources.Load<DamagePopUp>("Logics/PopUp_Text_Parent");
        }      
    }

    public static void CreateDamagePopUp_Text(string text, Transform location) {
        Initialize();
        DamagePopUp instance = Instantiate(popUpText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector3(location.position.x, location.position.y + 3, location.position.z));
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.SetText(text);
    }
}
