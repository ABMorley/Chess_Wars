using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MovePlateType
{
    move,
    attack,
    heal
}

public class MovePlate : MonoBehaviour
{
    //Some functions will need reference to the controller
    public GameObject controller;

    //The Chesspiece that was tapped to create this MovePlate
    GameObject reference = null;

    //Location on the board
    int matrixX;
    int matrixY;

    public MovePlateType type = MovePlateType.move;

    public void Start()
    {
        if (type == MovePlateType.move)
        {
            // Set to black
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }
        else if (type == MovePlateType.attack)
        {
            // Set to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        else if (type == MovePlateType.heal)
        {
            // Set to purple
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.2f, 1.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Chessman me = reference.GetComponent<Chessman>();

        if (type == MovePlateType.move)
        {
            //Set the Chesspiece's original location to be empty
            controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(), 
                reference.GetComponent<Chessman>().GetYBoard());

            //Move reference chess piece to this position
            me.SetCoords(matrixX, matrixY);

            //Update the matrix
            controller.GetComponent<Game>().SetPosition(reference);
        }
        else if (type == MovePlateType.attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            Chessman cm = cp.GetComponent<Chessman>();

            cm.DealDamage(me.GetDamage());
            if (cm.GetHealth() <= 0) Destroy(cp);
        }
        else if (type == MovePlateType.heal)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            Chessman cm = cp.GetComponent<Chessman>();

            cm.HealHealth();
            me.SetHealCooldown();
        }

        //Switch Current Player
        controller.GetComponent<Game>().NextTurn();

        //Destroy the move plates including self
        MovePlate.DestroyMovePlates();
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

    static public void DestroyMovePlates()
    {
        //Destroy old MovePlates
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); //Be careful with this function "Destroy" it is asynchronous
        }
    }
}
