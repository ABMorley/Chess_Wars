using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //The Chesspiece that was tapped to create this MovePlate
    GameObject reference = null;

    //Location on the board
    int matrixX;
    int matrixY;

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public GameObject canvas
    {
        get => GameObject.FindGameObjectWithTag("Canvas");
    }

    public void Start()
    {
        transform.SetParent(canvas.transform);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }

    static public void DestroyHealthBar()
    {
        //Destroy old health bars
        GameObject[] healthBars = GameObject.FindGameObjectsWithTag("HealthBar");
        for (int i = 0; i < healthBars.Length; i++)
        {
            Destroy(healthBars[i]); //Be careful with this function "Destroy" it is asynchronous
        }
    }
}
