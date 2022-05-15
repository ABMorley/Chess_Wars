using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Singleton
{
    public static GameObject GameController { get => GameObject.FindGameObjectWithTag("GameController"); }
    public static Game Game { get => GameController.GetComponent<Game>(); }
}
