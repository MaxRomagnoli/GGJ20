using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FascettaManager : MonoBehaviour
{
    [Header("Oggetti")]
    [SerializeField] private ItemToFix[] items;
    [SerializeField] private ColorZipPalette palette;
    [SerializeField] private GameObject prefabFascetta;

    [Header("Variabili")]
    //[SerializeField] private string songToPlay;
    [SerializeField] private int fascette3Stelle = 4;
    [SerializeField] private int fascette2Stelle = 3;
    [SerializeField] private float distanzaMinimaGenerazione = 1f;
    [SerializeField] private int fascetteGenerabili = 5;
    [SerializeField] private bool isGameOver = false;
    [SerializeField] private Color loseStarColor;

    [Header("UI")]
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private Text fascetteText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private Text mainText;
    [SerializeField] private Image stella3;
    [SerializeField] private Image stella2;
    [SerializeField] private Image stellaMenu1;
    [SerializeField] private Image stellaMenu2;
    [SerializeField] private Image stellaMenu3;

    private Vector2 startingDragPosition;
    private Fascetta fascettaAttuale = null;
    private int colorPicked;

    // Start is called before the first frame update
    void Start()
    {
        //if(!string.IsNullOrEmpty( songToPlay ))
        //{
        //    AudioManager.instance.StopAll();
        //    AudioManager.instance.Play(songToPlay);
        //}
        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        if( Input.GetMouseButtonDown(0) )
        {
            startingDragPosition = mousePosition;
        }
        else if( Input.GetMouseButton(0) && Vector2.SqrMagnitude( mousePosition - startingDragPosition) > distanzaMinimaGenerazione)
        {
            if(fascettaAttuale != null)
            {
                fascettaAttuale.LookAtMouse(mousePosition);
            }
            else if(fascetteGenerabili > 0)
            {
                //instanzia la fascetta
                fascetteGenerabili--;
                if(fascetteText != null )
                {
                    fascetteText.text = "Zip: x" + fascetteGenerabili.ToString();
                }
                if(stella3 != null && stellaMenu3 != null && fascetteGenerabili < fascette3Stelle)
                {
                    stella3.color = loseStarColor;
                    stellaMenu3.color = loseStarColor;
                }
                if (stella2 != null && stellaMenu2 != null && fascetteGenerabili < fascette2Stelle)
                {
                    stella2.color = loseStarColor;
                    stellaMenu2.color = loseStarColor;
                }
                fascettaAttuale = Instantiate(prefabFascetta, Vector3.zero, Quaternion.identity).GetComponentInChildren<Fascetta>();
                fascettaAttuale.transform.position = startingDragPosition;
                fascettaAttuale.LookAtMouse(mousePosition);
                fascettaAttuale.SetColor( palette.colors[ colorPicked % palette.colors.Length ] );
                colorPicked++;
            }
            else
            {
                GameOver(false);
            }
        }
        else if( Input.GetMouseButtonUp(0) )
        {
            if(fascettaAttuale != null)
            {
                fascettaAttuale.StartZip();
                fascettaAttuale = null;
            }
        }

        if (isGameOver) { return; }

        //IsWinning
        bool canWin = true;
        foreach (ItemToFix item in items) {
            if(!item.Win())
            {
                canWin = false;
                break;
            }
        }
        if(canWin)
        {
            GameOver(true);
        }
    }

    void GameOver(bool haveWin)
    {
        isGameOver = true;
        if(haveWin)
        {
            AudioManager.instance.Play("win");
            nextLevelButton.SetActive(true);
            mainText.text = "Victory!";
        }
        else if(stellaMenu1 != null && stellaMenu2 != null && stellaMenu3 != null)
        {
            AudioManager.instance.Play("lose");
            stellaMenu1.color = loseStarColor;
            stellaMenu2.color = loseStarColor;
            stellaMenu3.color = loseStarColor;
        }
        playerPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void NextLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextLevel);
        /*Debug.Log(" current: " + currentIndex);
        Debug.Log(" all: " + (SceneManager.sceneCountInBuildSettings));
        if (currentIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentIndex);
        }
        else
        {
            LoadLevel("MainMenu");
        }*/
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
