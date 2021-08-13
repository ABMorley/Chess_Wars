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
        if (type == MovePlateType.attack)
        {
            //Set to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        else if (type == MovePlateType.heal)
        {
            //Set to green
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (type == MovePlateType.move)
        {
            //Set the Chesspiece's original location to be empty
            controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(), 
                reference.GetComponent<Chessman>().GetYBoard());

            //Move reference chess piece to this position
            reference.GetComponent<Chessman>().SetXBoard(matrixX);
            reference.GetComponent<Chessman>().SetYBoard(matrixY);
            reference.GetComponent<Chessman>().SetCoords();

            //Update the matrix
            controller.GetComponent<Game>().SetPosition(reference);

            //Switch Current Player
            controller.GetComponent<Game>().NextTurn();

            //Destroy the move plates including self
            reference.GetComponent<Chessman>().DestroyMovePlates();
        }
        else if (type == MovePlateType.attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            Chessman cm = cp.GetComponent<Chessman>();

            if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
            if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

            Destroy(cp);
        }
        else if (type == MovePlateType.heal)
        {
            // TODO
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            Chessman cm = cp.GetComponent<Chessman>();

            if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
            if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

        }
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
}
