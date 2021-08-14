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

    // Variable representing the speed of a unit
    private int speed = 0;
    // Variable representing the kill range of a unit
    private int minKillDistance = 0;  // exclusive minimum
    private int maxKillDistance = 1;  // inclusive maximum
    private bool lineOfSight = false;
    // Variable representing health
    private int health;
    private int maxHealth;
    // Variable representing attack damage
    private int damage;
    // Variable representing heal distance, or -1 to disable
    private int healDistance = -1;

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
        switch (this.name)
        {
            case "black_castle":
            case "white_castle":
                speed = 1;
                maxHealth = health = 10;
                damage = 1;
                break;
            case "black_foot_soldier":
            case "white_foot_soldier":
                speed = 1;
                maxHealth = health = 6;
                damage = 1;
                break;
            case "black_archer":
            case "white_archer":
                speed = 2;
                maxHealth = health = 3;
                damage = 2;
                minKillDistance = 1;
                maxKillDistance = 4;
                break;
            case "black_mage":
            case "white_mage":
                speed = 2;
                maxHealth = health = 3;
                damage = 3;
                lineOfSight = true;
                healDistance = 1;
                break;
            case "black_cavalry":
            case "white_cavalry":
                speed = 3;
                maxHealth = health = 6;
                damage = 2;
                break;
        }
    }

    public void SetCoords(int x, int y)
    {
        SetXBoard(x);
        SetYBoard(y);
        SetCoords();
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

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void DealDamage(int dealtDamage)
    {
        health = health - dealtDamage;
        if (health < 0) health = 0;
    }

    public void HealHealth()
    {
        health = maxHealth;
    }

    public void HealHealth(int healing)
    {
        health = health + healing;
        if (health > maxHealth) health = maxHealth;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            //Remove all moveplates relating to previously selected piece
            MovePlate.DestroyMovePlates();

            //Create new MovePlates
            InitiateMovePlates();
        }
    }

    public void InitiateMovePlates()
    {
        for (int x = xBoard - speed; x <= xBoard + speed; x++)
        {
            for (int y = yBoard - speed; y <= yBoard + speed; y++)
            {
                if (x == xBoard && y == yBoard) continue;  // skip checking own position
                PointMovePlate(x, y);
            }
        }

        for (int x = xBoard - maxKillDistance; x <= xBoard + maxKillDistance; x++)
        {
            for (int y = yBoard - maxKillDistance; y <= yBoard + maxKillDistance; y++)
            {
                if ((xBoard - minKillDistance <= x) && (x <= xBoard + minKillDistance)
                    && (yBoard - minKillDistance <= y) && (y <= yBoard + minKillDistance))
                {
                    continue;  // skip checking own position or below min kill distance
                }
                PointAttackPlate(x, y);
            }
        }

        if (healDistance >= 0)
        {
            for (int x = xBoard - healDistance; x <= xBoard + healDistance; x++)
            {
                for (int y = yBoard - healDistance; y <= yBoard + healDistance; y++)
                {
                    // if (x == xBoard && y == yBoard) continue;  // skip checking own position
                    PointHealPlate(x, y);
                }
            }
        }
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            // Debug.Log($"{x}, {y}, {cp}");

            if (cp == null)
            {
                MovePlateSpawn(x, y, MovePlateType.move);
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
                MovePlateSpawn(x, y, MovePlateType.attack);
            }
        }
    }

    public void PointHealPlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<Chessman>().player == player )
            {
                MovePlateSpawn(x, y, MovePlateType.heal);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY, MovePlateType type = MovePlateType.move)
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
        mpScript.type = type;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
