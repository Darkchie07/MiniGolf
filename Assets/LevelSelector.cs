using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private const string saveLevel = "SAVELEVEL";

    public List<Button> levelButton = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(saveLevel))
        {
            var lastLevel = PlayerPrefs.GetInt(saveLevel);
            for (int i = 0; i < lastLevel; i++)
            {
                levelButton[i].interactable = true;
            }
        }
        else
        {
            PlayerPrefs.SetInt(saveLevel, 1);
            var lastLevel = PlayerPrefs.GetInt(saveLevel);
            for (int i = 0; i < lastLevel; i++)
            {
                levelButton[i].interactable = true;
            }
        }
    }

    public void SceneLoader(string name)
    {
        SceneManager.LoadScene(name);
    }
}
