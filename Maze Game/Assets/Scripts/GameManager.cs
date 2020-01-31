using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles game logic (start, finish, collectables, communicates with UI, and handles sfx
/// </summary>
public class GameManager : Singleton<GameManager>
{
    #region Public Variables
    public int startingScore = 200;                 //Score to start with
    public int coinValue = 3;                       //How much is a coin worth?
    public float secondsBetweenReduction;           //How many seconds between point reduction?
    public int scoreReductionValue;                 //How many points to reduce?
    public int wallBonusTime = 10;                  //How long does the wall bonus last?
    public int mapBonusTime = 20;                   //How long does the minimap bonus last?
    public Material material1;                      //Materials to make clear with bonus
    public Material material2;
    public Text scoreText;                          //Text displaying the score
    public Camera miniMap;                          //Camera to wake up for minimap bonus
    public Text mapCountdownText;                   //Text that displays minimap bonus time remaining
    public Text wallCountdownText;                  //Text that displays wall bonus time remaining
    public CanvasGroup popupGroup;                  //Displays the initial and final instructions
    public Text popupText;
    public PlayerController controller;             //The player controller
    #endregion
    #region Private Variables
    private bool isGameOver = false;
    private int score;
    private IEnumerator scoreCoroutine;
    private IEnumerator invisibleCoroutine;
    private IEnumerator minimapCoroutine;
    private IEnumerator popupCoroutine;
    private Color invisibleColor = Color.white;
    #endregion


    /// <summary>
    /// Start is called before the first frame update. Set all the UI and coroutines
    /// </summary>
    void Start()
    {
        score = startingScore;
        scoreCoroutine = ScoreReducer();
        StartCoroutine(scoreCoroutine);


        material1.SetColor("_Color", Color.white);
        material2.SetColor("_Color", Color.white);
        invisibleColor.a = 0;
        wallCountdownText.enabled = false;
        mapCountdownText.enabled = false;
        miniMap.enabled = false;
        popupCoroutine = InstructionFade(0, 5, 2);
        StartCoroutine(popupCoroutine);
    }

    /// <summary>
    /// Reset the materials at the end
    /// </summary>
    private void OnApplicationQuit()
    {
        material1.SetColor("_Color", Color.white);
        material2.SetColor("_Color", Color.white);
    }

    /// <summary>
    /// Update is called once per frame. Look for quitting or restarting input. Update the score
    /// </summary>
    void Update()
    {
        UpdateScore();
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Handles collectable logic and feeds coroutines if a powerup.
    /// </summary>
    /// <param name="collectionType"></param> What type of collectable?
    /// <param name="clip"></param> What audio clip to play?
    public void CollectObject(Collectable.CollectableType collectionType, AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip, 0.7f);
        switch(collectionType)
        {
            case Collectable.CollectableType.Coin:
                score += coinValue;
                break;
            case Collectable.CollectableType.Camera:
                if (minimapCoroutine != null)
                {
                    StopCoroutine(minimapCoroutine);
                }
                minimapCoroutine = MiniMap();
                StartCoroutine(minimapCoroutine); 
                break;
            case Collectable.CollectableType.Invisible:
                if(invisibleCoroutine != null)
                {
                    StopCoroutine(invisibleCoroutine);
                }
                invisibleCoroutine = InvisibleWalls();
                StartCoroutine(invisibleCoroutine);
                break;
            case Collectable.CollectableType.Objective:
                FinishGame();
                break;
        }
    }

    /// <summary>
    /// Every frame update the score
    /// </summary>
    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// Finish game logic - apply message, stop player controller, stop changing the score.
    /// </summary>
    private void FinishGame()
    {
        popupText.text = "YOU WIN! \n PRESS 'R' TO RESTART";
        popupCoroutine = InstructionFade(1, 0, 2);
        StartCoroutine(popupCoroutine);
        isGameOver = true;
        controller.SetGameOver();
    }

    /// <summary>
    /// Everytime the amount of time passes that decreases the score, decrease it a certain amount. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScoreReducer()
    {
        while(!isGameOver)
        {
            yield return new WaitForSeconds(secondsBetweenReduction);
            score -= scoreReductionValue;
        }
    }

    /// <summary>
    /// Invisible wall powerup
    /// </summary>
    /// <returns></returns>
    private IEnumerator InvisibleWalls()
    {
        //Set the wall materials to invisible
        material1.SetColor("_Color", invisibleColor);
        material2.SetColor("_Color", invisibleColor);

        //Setup the timer initially
        int timeRemaining = wallBonusTime;
        wallCountdownText.text = timeRemaining.ToString();
        wallCountdownText.enabled = true;
        //While the time hasn't run out, reduce the time and update the text
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1);
            timeRemaining--;
            wallCountdownText.text = timeRemaining.ToString();
        }

        yield return new WaitForSeconds(0.1f);

        //Return the original material and remove the text
        wallCountdownText.enabled = false;
        material1.SetColor("_Color", Color.white);
        material2.SetColor("_Color", Color.white);
        yield return null;
    }

    /// <summary>
    /// Minimap powerup
    /// </summary>
    /// <returns></returns>
    private IEnumerator MiniMap()
    {
        //Turn on the minimap and set the text counter initially
        miniMap.enabled = true;
        int timeRemaining = mapBonusTime;
        mapCountdownText.text = timeRemaining.ToString();
        mapCountdownText.enabled = true;
        //While time still remains, decrement the time
        while(timeRemaining > 0)
        {
            yield return new WaitForSeconds(1);
            timeRemaining--;
            mapCountdownText.text = timeRemaining.ToString();
        }

        //Return things to normal
        yield return new WaitForSeconds(0.1f);
        miniMap.enabled = false;
        mapCountdownText.enabled = false;
        yield return null;
    }

    /// <summary>
    /// Fade the popup canvas
    /// </summary>
    /// <param name="finalAlpha"></param>Either 1 or 0 (on or off)
    /// <param name="initialWait"></param> How long before fading?
    /// <param name="timeToFade"></param> How long does it take to fade?
    /// <returns></returns>
    private IEnumerator InstructionFade(float finalAlpha, float initialWait, float timeToFade)
    {
        yield return new WaitForSeconds(initialWait);
        float startingAlpha = popupGroup.alpha;
        float t = 0;
        while(popupGroup.alpha != finalAlpha)
        {
            t += Time.deltaTime;
            popupGroup.alpha = Mathf.Lerp(startingAlpha, finalAlpha, t / timeToFade);
            yield return new WaitForEndOfFrame();
        }
    }

}
