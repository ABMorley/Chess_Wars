using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    //References to objects in our Unity Scene
    public GameObject controller;
    public GameObject movePlate;

    //Position for this Chesspiece on the Board
    //The correct position will be set later
    private int xBoard = -1;
    private int yBoard = -1;

    //Variable for keeping track of the player it belongs to "black" or "white"
    private string player;

    //References to all the possible Sprites that this Chesspiece could be
    public Sprite black_main_castle, black_cavalry, black_archer, black_mage, black_castle, black_foot_soldier;
    public Sprite white_bottom_main_castle, white_top_main_castle, white_cavalry, white_archer, white_mage, white_castle, white_foot_soldier;

    public void Activate()
    {
        //Get the game controller
        controller = GameObject.FindGameObjectWithTag("GameController");

        //Take the instantiated location and adjust transform
        SetCoords();

        //Choose correct sprite based on piece's name
        switch (this.name)
        {
            case "black_main_castle": this.GetComponent<SpriteRenderer>().sprite = black_main_castle; player = "black"; break;
            case "black_cavalry": this.GetComponent<SpriteRenderer>().sprite = black_cavalry; player = "black"; break;
            case "black_archer": this.GetComponent<SpriteRenderer>().sprite = black_archer; player = "black"; break;
            case "black_mage": this.GetComponent<SpriteRenderer>().sprite = black_mage; player = "black"; break;
            case "black_castle": this.GetComponent<SpriteRenderer>().sprite = black_castle; player = "black"; break;
            case "black_foot_soldier": this.GetComponent<SpriteRenderer>().sprite = black_foot_soldier; player = "black"; break;
            case "white_bottom_main_castle": this.GetComponent<SpriteRenderer>().sprite = white_bottom_main_castle; player = "white"; break;
            case "white_top_main_castle": this.GetComponent<SpriteRenderer>().sprite = white_top_main_castle; player = "white"; break;
            case "white_cavalry": this.GetComponent<SpriteRenderer>().sprite = white_cavalry; player = "white"; break;
            case "white_archer": this.GetComponent<SpriteRenderer>().sprite = white_archer; player = "white"; break;
            case "white_mage": this.GetComponent<SpriteRenderer>().sprite = white_mage; player = "white"; break;
            case "white_castle": this.GetComponent<SpriteRenderer>().sprite = white_castle; player = "white"; break;
            case "white_foot_soldier": this.GetComponent<SpriteRenderer>().sprite = white_foot_soldier; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        //Get the board value in order to convert to xy coords
        float x = xBoard;
        float y = yBoard;

        //Adjust by variable offset
        x *= 0.88f;
        y *= 0.88f;

        //Add constants (pos 0,0)
        x += -6.64f;
        y += -3.1f;

        //Set actual unity values
        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            //Remove all moveplates relating to previously selected piece
            DestroyMovePlates();

            //Create new MovePlates
            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        //Destroy old MovePlates
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); //Be careful with this function "Destroy" it is asynchronous
        }
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
                // low movement
            case "black_castle":
            case "white_castle":
                LowSpeed();
                break;
                // medium movement
            case "black_foot_soldier":
            case "black_archer":
            case "black_mage":
            case "white_foot_soldier":
            case "white_archer":
            case "white_mage":
                MediumSpeed();
                break;
                // high movement
            case "black_cavalry":
            case "white_cavalry":
                HighSpeed();
                break;
        }
    }

    public void LowSpeed()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);

        PointAttackPlate(xBoard, yBoard + 1);
        PointAttackPlate(xBoard, yBoard - 1);
        PointAttackPlate(xBoard - 1, yBoard + 0);
        PointAttackPlate(xBoard - 1, yBoard - 1);
        PointAttackPlate(xBoard - 1, yBoard + 1);
        PointAttackPlate(xBoard + 1, yBoard + 0);
        PointAttackPlate(xBoard + 1, yBoard - 1);
        PointAttackPlate(xBoard + 1, yBoard + 1);
    }

    public void MediumSpeed()
    {
        PointMovePlate(xBoard, yBoard + 2);
        PointMovePlate(xBoard + 1, yBoard + 2); 
        PointMovePlate(xBoard + 2, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 2, yBoard - 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);

        PointAttackPlate(xBoard, yBoard + 1);
        PointAttackPlate(xBoard, yBoard - 1);
        PointAttackPlate(xBoard - 1, yBoard + 0);
        PointAttackPlate(xBoard - 1, yBoard - 1);
        PointAttackPlate(xBoard - 1, yBoard + 1);
        PointAttackPlate(xBoard + 1, yBoard + 0);
        PointAttackPlate(xBoard + 1, yBoard - 1);
        PointAttackPlate(xBoard + 1, yBoard + 1);
    }

    public void HighSpeed()
    {
        PointMovePlate(xBoard, yBoard + 3);
        PointMovePlate(xBoard + 1, yBoard + 3);
        PointMovePlate(xBoard + 2, yBoard + 3);
        PointMovePlate(xBoard + 3, yBoard + 3);
        PointMovePlate(xBoard + 3, yBoard + 2);
        PointMovePlate(xBoard + 3, yBoard + 1);
        PointMovePlate(xBoard + 3, yBoard);
        PointMovePlate(xBoard + 3, yBoard - 1);
        PointMovePlate(xBoard + 3, yBoard - 2);
        PointMovePlate(xBoard + 3, yBoard - 3);
        PointMovePlate(xBoard + 2, yBoard - 3);
        PointMovePlate(xBoard + 1, yBoard - 3);
        PointMovePlate(xBoard, yBoard - 3);
        PointMovePlate(xBoard - 1, yBoard - 3);
        PointMovePlate(xBoard - 2, yBoard - 3);
        PointMovePlate(xBoard - 3, yBoard - 3);
        PointMovePlate(xBoard - 3, yBoard - 2);
        PointMovePlate(xBoard - 3, yBoard - 1);
        PointMovePlate(xBoard - 3, yBoard);
        PointMovePlate(xBoard - 3, yBoard + 1);
        PointMovePlate(xBoard - 3, yBoard + 2);
        PointMovePlate(xBoard - 3, yBoard + 3);
        PointMovePlate(xBoard - 2, yBoard + 3);
        PointMovePlate(xBoard - 1, yBoard + 3);
        PointMovePlate(xBoard, yBoard + 2);
        PointMovePlate(xBoard + 1, yBoard + 2); 
        PointMovePlate(xBoard + 2, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 2, yBoard - 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);

        PointAttackPlate(xBoard, yBoard + 1);
        PointAttackPlate(xBoard, yBoard - 1);
        PointAttackPlate(xBoard - 1, yBoard + 0);
        PointAttackPlate(xBoard - 1, yBoard - 1);
        PointAttackPlate(xBoard - 1, yBoard + 1);
        PointAttackPlate(xBoard + 1, yBoard + 0);
        PointAttackPlate(xBoard + 1, yBoard - 1);
        PointAttackPlate(xBoard + 1, yBoard + 1);
    }
    public void AttackMovePlate()
    {
        
    }
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void PointAttackPlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<Chessman>().player != player )
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Adjust by variable offset
        x *= 0.88f;
        y *= 0.88f;

        //Add constants (pos 0,0)
        x += -6.64f;
        y += -3.1f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Adjust by variable offset
        x *= 0.88f;
        y *= 0.88f;

        //Add constants (pos 0,0)
        x += -6.64f;
        y += -3.1f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
