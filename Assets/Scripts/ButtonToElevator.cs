using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonToElevator : MonoBehaviour {

    private static List<GameObject> buttons;
    private static List<GameObject> elevators;

	private void Start () {
        buttons = GameObject.FindGameObjectsWithTag("Button").ToList();
        elevators = GameObject.FindGameObjectsWithTag("Elevator").ToList();
        ElevatorToButton();
    }

    private void ElevatorToButton() {
        if(buttons.Count != elevators.Count) throw new Exception("There must be the same number of elevators and buttons");
        //for(int i = 0; i < buttons.Count; i++) {
        for (int i = buttons.Count - 1; i >= 0; i--) {
            float shortestDistance = Mathf.Infinity;
            GameObject closestElevator = null;
            //for (int j = 0; j < elevators.Count; j++) {
            for (int j = elevators.Count - 1; j >= 0; j--) {
                float distance = Vector2.Distance(buttons[i].transform.position, elevators[j].transform.position);
                if (distance < shortestDistance) {
                    shortestDistance = distance;
                    closestElevator = elevators[j];
                }             
            }
            buttons[i].GetComponent<ButtonController>().fahrStuhl = closestElevator;
            buttons.Remove(buttons[i]);
            elevators.Remove(closestElevator);         
        }
    }
}
