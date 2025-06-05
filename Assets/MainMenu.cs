using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject lvlselect;
    public GameObject credits;
    // Start is called before the first frame update
    void Start()
    {
        lvlselect?.SetActive(false);
        credits?.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LevelSelect()
    {
        lvlselect.SetActive(true);
    }

    public void Credits()
    {
        credits?.SetActive(true);
    }

    public void back()
    {
        lvlselect?.SetActive(false);
        credits?.SetActive(false);
    }

    public void lvl1()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void lvl2()
    {
        SceneManager.LoadScene("Level1");
    }

    public void lvl3()
    {
        SceneManager.LoadScene("Level2");
    }

    public void lvl4()
    {
        SceneManager.LoadScene("CustomLvl");
    }
}
