using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum PlayerTurn
{
    whiteMove,
    whiteAttack,
    blackMove,
    blackAttack
}

public enum PlayerSide
{
    white,
    black
}

public enum TurnType
{
    move,
    attack
}

public class Game : MonoBehaviour
{
    //Reference from Unity IDE
    public GameObject chesspiece;

    //Matrices needed, positions of each of the GameObjects
    //Also separate arrays for the players in order to easily keep track of them all
    //Keep in mind that the same objects are going to be in "positions" and "playerBlack"/"playerWhite"
    private GameObject[,] positions = new GameObject[16, 8];
    private GameObject[] playerBlack;
    private GameObject[] playerWhite;

    //current turn
    private PlayerTurn currentTurn = PlayerTurn.whiteMove;

    //Game Ending
    private bool gameOver = false;

    //Unity calls this right when the game starts, there are a few built in functions
    //that Unity can call for you
    public void Start()
    {
        playerWhite = new GameObject[] { Create("white_mage", 0, 0), Create("white_archer", 0, 1),
            Create("white_archer", 0, 2), Create("white_bottom_main_castle", 0, 3), Create("white_top_main_castle", 0, 4),
            Create("white_archer", 0, 5), Create("white_archer", 0, 6), Create("white_mage", 0, 7),
            Create("white_castle", 1, 0), Create("white_cavalry", 1, 1), Create("white_foot_soldier", 1, 2),
            Create("white_foot_soldier", 1, 3), Create("white_foot_soldier", 1, 4), Create("white_foot_soldier", 1, 5),
            Create("white_cavalry", 1, 6), Create("white_castle", 1, 7) };
        playerBlack = new GameObject[] { Create("black_mage", 15, 0), Create("black_archer",15,1),
            Create("black_archer",15,2), Create("black_main_castle",15,3), Create("black_archer",15,5),
            Create("black_archer",15,6), Create("black_mage",15,7), Create("black_castle", 14, 0),
            Create("black_cavalry", 14, 1), Create("black_foot_soldier", 14, 2),Create("black_foot_soldier", 14, 3),
            Create("black_foot_soldier", 14, 4), Create("black_foot_soldier", 14, 5),Create("black_cavalry", 14, 6),
            Create("black_castle", 14, 7) };

        //Set all piece positions on the positions board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
        }
        for (int i = 0; i < playerWhite.Length; i++)
        {
            SetPosition(playerWhite[i]);
        }
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script
        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); //It has everything set up so it can now Activate()
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        //Overwrites either empty space or whatever was there
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public PlayerSide GetCurrentPlayer()
    {
        switch (currentTurn)
        {
            case PlayerTurn.whiteMove:
            case PlayerTurn.whiteAttack:
                return PlayerSide.white;
            case PlayerTurn.blackMove:
            case PlayerTurn.blackAttack:
                return PlayerSide.black;
            default:
                return PlayerSide.white;
        }
    }

    public TurnType GetCurrentTurnType()
    {
        switch (currentTurn)
        {
            case PlayerTurn.whiteMove:
            case PlayerTurn.blackMove:
                return TurnType.move;
            case PlayerTurn.whiteAttack:
            case PlayerTurn.blackAttack:
                return TurnType.attack;
            default:
                return TurnType.move;
        }
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentTurn == PlayerTurn.whiteAttack)
        {
            foreach (GameObject piece in playerWhite)
            {
                if (piece != null) piece.GetComponent<Chessman>().ReduceHealCooldown();
            }
        }
        else if (currentTurn == PlayerTurn.blackAttack)
        {
            foreach (GameObject piece in playerBlack)
            {
                if (piece != null) piece.GetComponent<Chessman>().ReduceHealCooldown();
            }
        }

        switch (currentTurn)
        {
            case PlayerTurn.whiteMove:
                currentTurn = PlayerTurn.whiteAttack; break;
            case PlayerTurn.whiteAttack:
                currentTurn = PlayerTurn.blackMove; break;
            case PlayerTurn.blackMove:
                currentTurn = PlayerTurn.blackAttack; break;
            case PlayerTurn.blackAttack:
            default:
                currentTurn = PlayerTurn.whiteMove; break;
        }

        Debug.Log($"Current turn: {currentTurn}");
    }

    public static void ClearTurnElements()
    {
        MovePlate.DestroyMovePlates();
        HealthBar.DestroyHealthBar();
    }

    public void SkipCurrentTurn()
    {
        ClearTurnElements();
        NextTurn();
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            //Using UnityEngine.SceneManagement is needed here
            SceneManager.LoadScene("Game"); //Restarts the game by loading the scene over again
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearTurnElements();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipCurrentTurn();
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;

        //Using UnityEngine.UI is needed here
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}
