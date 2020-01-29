using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int startingScore = 500;
    public int coinValue = 10;
    public float secondsBetweenReduction;
    public int scoreReductionValue;
    public Material material1;
    public Material material2;

    private bool isGameOver = false;
    private int score;
    private IEnumerator scoreCoroutine;
    private IEnumerator invisibleCoroutine;
    private Color invisibleColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        score = startingScore;
        scoreCoroutine = ScoreReducer();
        StartCoroutine(scoreCoroutine);


        material1.SetColor("_Color", Color.white);
        material2.SetColor("_Color", Color.white);
        invisibleColor.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    public void CollectObject(Collectable.CollectableType collectionType)
    {
        switch(collectionType)
        {
            case Collectable.CollectableType.Coin:
                score += coinValue;
                print("Collected a coin");
                break;
            case Collectable.CollectableType.Camera:
                print("Collected a camera bonus");
                break;
            case Collectable.CollectableType.Invisible:
                if(invisibleCoroutine != null)
                {
                    StopCoroutine(invisibleCoroutine);
                }
                invisibleCoroutine = InvisibleWalls();
                StartCoroutine(invisibleCoroutine);
                print("Collected an invisible bonus");
                break;
            case Collectable.CollectableType.Objective:
                print("Collected the objective");
                FinishGame();
                break;
        }
    }


    private void UpdateScore()
    {
        //Update GUI

    }

    private void FinishGame()
    {
        //Update GUI
        isGameOver = true;
    }


    private IEnumerator ScoreReducer()
    {
        while(!isGameOver)
        {
            yield return new WaitForSeconds(secondsBetweenReduction);
            score -= scoreReductionValue;
        }
    }


    private IEnumerator InvisibleWalls()
    {
        material1.SetColor("_Color", invisibleColor);
        material2.SetColor("Color", invisibleColor);
        yield return new WaitForSeconds(5);
        material1.SetColor("_Color", Color.white);
        material2.SetColor("_Color", Color.white);
        yield return null;
    }
}
