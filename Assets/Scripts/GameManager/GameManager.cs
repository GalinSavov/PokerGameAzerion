using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    //quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
    //resets the scene so the player can play again

    public void DealAgain()
    {
        SceneManager.LoadScene(0);
    }
}
