using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    //References to objects in our Unity Scene
    public GameObject movePlate;
    public GameObject healthBar;

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
    private int baseDamage;
    // Variable representing heal distance, or -1 to disable
    private int healDistance = -1;
    // Variable representing heal cooldown in turns
    private int healCooldown = 0;
    private int maxHealCooldown = 2;

    //Variable for keeping track of the player it belongs to "black" or "white"
    private PlayerSide player;

    //References to all the possible Sprites that this Chesspiece could be
    public Sprite black_main_castle, black_cavalry, black_archer, black_mage, black_castle, black_foot_soldier;
    public Sprite white_bottom_main_castle, white_top_main_castle, white_cavalry, white_archer, white_mage, white_castle, white_foot_soldier;

    public void Activate()
    {
        //Get the game controller
        GameObject controller = Singleton.GameController;

        //Take the instantiated location and adjust transform
        SetPositionFromCoords();

        //Choose correct sprite based on piece's name
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (this.name)
        {
            case "black_main_castle": spriteRenderer.sprite = black_main_castle; player = PlayerSide.black; break;
            case "black_cavalry": spriteRenderer.sprite = black_cavalry; player = PlayerSide.black; break;
            case "black_archer": spriteRenderer.sprite = black_archer; player = PlayerSide.black; break;
            case "black_mage": spriteRenderer.sprite = black_mage; player = PlayerSide.black; break;
            case "black_castle": spriteRenderer.sprite = black_castle; player = PlayerSide.black; break;
            case "black_foot_soldier": spriteRenderer.sprite = black_foot_soldier; player = PlayerSide.black; break;
            case "white_bottom_main_castle": spriteRenderer.sprite = white_bottom_main_castle; player = PlayerSide.white; break;
            case "white_top_main_castle": spriteRenderer.sprite = white_top_main_castle; player = PlayerSide.white; break;
            case "white_cavalry": spriteRenderer.sprite = white_cavalry; player = PlayerSide.white; break;
            case "white_archer": spriteRenderer.sprite = white_archer; player = PlayerSide.white; break;
            case "white_mage": spriteRenderer.sprite = white_mage; player = PlayerSide.white; break;
            case "white_castle": spriteRenderer.sprite = white_castle; player = PlayerSide.white; break;
            case "white_foot_soldier": spriteRenderer.sprite = white_foot_soldier; player = PlayerSide.white; break;
        }
        switch (this.name)
        {
            case "black_castle":
            case "white_castle":
                speed = 1;
                maxHealth = health = 100;
                baseDamage = 10;
                break;
            case "black_foot_soldier":
            case "white_foot_soldier":
                speed = 2;
                maxHealth = health = 60;
                baseDamage = 10;
                break;
            case "black_archer":
            case "white_archer":
                speed = 2;
                maxHealth = health = 30;
                baseDamage = 30;
                minKillDistance = 1;
                maxKillDistance = 4;
                break;
            case "black_mage":
            case "white_mage":
                speed = 2;
                maxHealth = health = 30;
                baseDamage = 20;
                lineOfSight = true;
                healDistance = 1;
                maxHealCooldown = 2;
                break;
            case "black_cavalry":
            case "white_cavalry":
                speed = 3;
                maxHealth = health = 60;
                baseDamage = 20;
                break;
        }
    }

    public void SetCoords(int x, int y)
    {
        SetXBoard(x);
        SetYBoard(y);
        SetPositionFromCoords();
    }

    public void SetPositionFromCoords()
    {
        //Get the board value in order to convert to xy coords
        float x = xBoard;
        float y = yBoard;

        //Adjust by variable offset
        x *= BOX_WIDTH;
        y *= BOX_HEIGHT;

        //Add constants (pos 0,0)
        x += BOX_OFFSET_X;
        y += BOX_OFFSET_Y;

        //Set actual unity values
        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard() => xBoard;
    public int GetYBoard() => yBoard;

    public void SetXBoard(int x) => xBoard = x;
    public void SetYBoard(int y) => yBoard = y;

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    const float DamageVariableMin = 0.75f;
    const float DamageVariableMax = 1.25f;

    public int GetDamage()
    {
        return (int)(baseDamage * Random.Range(DamageVariableMin, DamageVariableMax));
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

    public void ReduceHealCooldown()
    {
        if (healCooldown > 0) healCooldown--;
    }

    public void SetHealCooldown()
    {
        healCooldown = maxHealCooldown;
    }

    public bool CheckHealCooldown()
    {
        return healCooldown > 0;
    }

    private void OnMouseUp()
    {
        if (!Singleton.Game.IsGameOver() && Singleton.Game.GetCurrentPlayer() == player)
        {
            //Remove all moveplates relating to previously selected piece
            Game.ClearTurnElements();

            //Create new MovePlates
            InitiateMovePlates();

            // Show health bar
            InitiateHealthBar();
        }
    }

    public void InitiateMovePlates()
    {
        TurnType turnType = Singleton.Game.GetCurrentTurnType();

        if (turnType == TurnType.move)
        {
            InitiateMovePlates(PointMovePlate, speed, excludeSelf: true);
        }
        else if (turnType == TurnType.attack)
        {
            InitiateMovePlates(PointAttackPlate, minKillDistance, maxKillDistance);

            if (healDistance >= 0)
            {
                InitiateMovePlates(PointHealPlate, healDistance, excludeSelf: false);
            }
        }
    }

    public void InitiateMovePlates(PointAnyPlate plateType, int maxRange, bool excludeSelf = false)
    {
        for (int x = xBoard - maxRange; x <= xBoard + maxRange; x++)
        {
            for (int y = yBoard - maxRange; y <= yBoard + maxRange; y++)
            {
                if (excludeSelf && (x == xBoard) && (y == yBoard)) continue;  // skip checking own position
                plateType(x, y);
            }
        }
    }

    /// <param name="minRange">The exclusive minimum distance, so 1 means it must be 2 or more squares away</param>
    /// <param name="maxRange">The inclusive maximum distance, so 4 means it must be 4 or fewer squares away</param>
    public void InitiateMovePlates(PointAnyPlate plateType, int minRange, int maxRange)
    {
        for (int x = xBoard - maxRange; x <= xBoard + maxRange; x++)
        {
            for (int y = yBoard - maxRange; y <= yBoard + maxRange; y++)
            {
                if ((xBoard - minRange <= x) && (x <= xBoard + minRange)
                    && (yBoard - minRange <= y) && (y <= yBoard + minRange))
                {
                    continue;  // skip checking below min distance
                }
                plateType(x, y);
            }
        }
    }

    public delegate void PointAnyPlate(int x, int y);

    public void PointMovePlate(int x, int y)
    {
        Game game = Singleton.Game;
        if (game.PositionOnBoard(x, y))
        {
            GameObject cp = game.GetPosition(x, y);

            // Debug.Log($"{x}, {y}, {cp}");

            if (cp == null)
            {
                MovePlateSpawn(x, y, MovePlateType.move);
            }
        }
    }

    public void PointAttackPlate(int x, int y)
    {
        Game game = Singleton.Game;
        if (game.PositionOnBoard(x, y))
        {
            if (game.GetPosition(x, y) != null && game.GetPosition(x, y).GetComponent<Chessman>().player != player)
            {
                MovePlateSpawn(x, y, MovePlateType.attack);
            }
        }
    }

    public void PointHealPlate(int x, int y)
    {
        if (CheckHealCooldown()) return;

        Game game = Singleton.Game;
        if (game.PositionOnBoard(x, y))
        {
            if (game.GetPosition(x, y) == null) return;
            Chessman other = game.GetPosition(x, y).GetComponent<Chessman>();
            // If piece is on our side and has below full health
            if (other.player == player && other.GetHealth() < other.GetMaxHealth())
            {
                MovePlateSpawn(x, y, MovePlateType.heal);
            }
        }
    }

    const int BOX_COUNT_WIDTH = 16;
    const int BOX_COUNT_HEIGHT = 8;
    const float BOX_WIDTH = 0.88f;
    const float BOX_HEIGHT = 0.88f;
    const float BOX_OFFSET_X = -BOX_WIDTH * (BOX_COUNT_WIDTH - 1f) / 2;
    const float BOX_OFFSET_Y = -BOX_HEIGHT * (BOX_COUNT_HEIGHT - 1f) / 2;

    public void MovePlateSpawn(int matrixX, int matrixY, MovePlateType type = MovePlateType.move)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Adjust by variable offset
        x *= BOX_WIDTH;
        y *= BOX_HEIGHT;

        //Add constants (pos 0,0)
        x += BOX_OFFSET_X;
        y += BOX_OFFSET_Y;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.type = type;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void InitiateHealthBar()
    {
        //Get the board value in order to convert to xy coords
        float x = xBoard;
        float y = yBoard;

        //Adjust by variable offset
        x *= BOX_WIDTH;
        y *= BOX_HEIGHT;

        //Add constants (pos 0,0)
        x += BOX_OFFSET_X;
        y += BOX_OFFSET_Y;

        y += -BOX_HEIGHT / 2 + 0.16f;

        //Set actual unity values
        GameObject hb = Instantiate(healthBar, new Vector3(x, y, -3.0f), Quaternion.identity);

        HealthBar hbScript = hb.GetComponent<HealthBar>();
        hbScript.SetReference(gameObject);
        hbScript.SetCoords(xBoard, yBoard);
        hbScript.SetMaxHealth(maxHealth);
        hbScript.SetHealth(health);
    }
}
