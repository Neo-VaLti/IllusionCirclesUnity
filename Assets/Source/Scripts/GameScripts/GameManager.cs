using System;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //La variable Lifes determina cuantas vidas tiene el jugador.
    private static int Lifes = 3;
    //La variable CurrentLevel indica en que nivel va el jugador.
    private static int CurrentLevel = 0;

    //Variable que indica si el juego esta corriendo.
    private static bool GameRunning;

    void Start()
    {
        //Indica que el juego va a iniciar.
        GameRunning = true;
        //Se carga el primer nivel.
        LoadNextLevel();
    }

    //Este metodo le resta una vida al jugador.
    public static void TakeLife()
    {
        //Lifes -= 1;

        //Si las vidas son iguales a 0, entonces el juego ya no esta corriendo.
        if (Lifes == 0)
        {
            GameRunning = false;
        }
    }
    //Carga el siguiente nivel del juego.
    public static void LoadNextLevel()
    {
        //Se le añade uno al contador de niveles.
        //Ahora estamos en el siguiente nivel.
        CurrentLevel += 1;

        //Se crea el objeto y se añade al escenario.
        GameObject o = (GameObject) Instantiate(Resources.Load("_Prefabs/Stages/Level_" + (CurrentLevel < 9 ? "0" + CurrentLevel.ToString() : CurrentLevel.ToString())));
        //Se le quita "(Clone)" al nombre del objecto, para no tener que escribirlo al momento de querer
        //Acceder a sus variables.
        o.name = o.name.Replace("(Clone)", "");
    }
    public static int getNumberOfCurrentBlocksInLevel()
    {
        int n = 0;

        GameObject level = GameObject.Find("Level_" + (CurrentLevel < 9 ? "0" + CurrentLevel.ToString() : CurrentLevel.ToString()));

        foreach (Transform t in level.GetComponentsInChildren<Transform>())
        {
            n+=1;
        }

        return n-1;
    }
    //Se reinicia el juego.
    public static void RestartGame()
    {
        Lifes = 3;
        GameRunning = true;
    }
    public static bool IsGameRunning()
    {
        return GameRunning;
    }
}
