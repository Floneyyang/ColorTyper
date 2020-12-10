using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButtonController : MonoBehaviour
{
    public Button[] colorBtns;
    public TMP_Text scoreText;
    public GameObject gameOverScreen;
    public GameObject HighestScore;

    private Color[] ballColors;
    private List<int> randomColors;
    private int r;
    private bool check = true;
    private int score = 0;
    //Subscription
    Subscription<AssignUIColorEvent> AssignUIColorEventSubscription;
    Subscription<UpdateButtonColorEvent> UpdateButtonColorEventSubscription;
    Subscription<ButtonPressedEvent> ButtonPressedEventSubscription;
    Subscription<GameOverEvent> GameOverEventSubscription;

    private void Awake()
    {
        AssignUIColorEventSubscription = _EventBus.Subscribe<AssignUIColorEvent>(_AssignColor);
        UpdateButtonColorEventSubscription = _EventBus.Subscribe<UpdateButtonColorEvent>(_UpdateColor);
        ButtonPressedEventSubscription = _EventBus.Subscribe<ButtonPressedEvent>(_CheckButton);
        GameOverEventSubscription = _EventBus.Subscribe<GameOverEvent>(_ShowScore);
    }

    void OnDestroy()
    {
        _EventBus.Unsubscribe<AssignUIColorEvent>(AssignUIColorEventSubscription);
        _EventBus.Unsubscribe<UpdateButtonColorEvent>(UpdateButtonColorEventSubscription);
        _EventBus.Unsubscribe<ButtonPressedEvent>(ButtonPressedEventSubscription);
        _EventBus.Unsubscribe<GameOverEvent>(GameOverEventSubscription);
    }

    private void Start()
    {
        StartCoroutine(ShowHighestScore());
    }

    void _AssignColor(AssignUIColorEvent e)
    {
        ballColors = e.colors;
        randomColors = new List<int>();
        for (int i = 0; i < ballColors.Length; i++)
        {
            randomColors.Add(i);
        }
        //Debug.Log(randomColors);
    }

    void _UpdateColor(UpdateButtonColorEvent e)
    {
        r = (int)Random.Range(0f, colorBtns.Length - 0.1f);
        colorBtns[r].image.color = ballColors[e.correctColor];

        //Debug.Log(e.correctColor);
        randomColors.Remove(e.correctColor);
        List<int> deletedColors = new List<int>();
        deletedColors.Add(e.correctColor);

        for (int i = 0; i < colorBtns.Length; i++)
        {
            if(i != r)
            {
                int c = randomColors[(int)Random.Range(0f, randomColors.Count - 0.1f)];
                colorBtns[i].image.color = ballColors[c];
                randomColors.Remove(c);
                deletedColors.Add(c);
            }
        }

        randomColors.AddRange(deletedColors);
        deletedColors.Clear();
        check = false;
    }

    void _CheckButton(ButtonPressedEvent e)
    {
        if (!check)
        {
            if (e.ID == r)
            {
                CorrectColorSelectedEvent c = new CorrectColorSelectedEvent();
                _EventBus.Publish<CorrectColorSelectedEvent>(c);
                check = true;
                UpdateScore();
                //Debug.Log("correct!" + Time.deltaTime);
            }
        }
    }

    void UpdateScore()
    {
        score++;
        if(score%6 == 0)
        {
            score *= 2;
        }
        scoreText.color = colorBtns[r].image.color;
        scoreText.text = score.ToString();
    }

    void _ShowScore(GameOverEvent e)
    {
        gameOverScreen.SetActive(true);
        if (GameHandler.instance.player.score < score) GameHandler.instance.player.score = score;
        gameOverScreen.transform.Find("Highest").GetComponent<Text>().text = "Highest Score: " + GameHandler.instance.player.score.ToString();
        gameOverScreen.transform.Find("Current").GetComponent<Text>().text = "Your Score: " + score.ToString();

    }

    IEnumerator ShowHighestScore()
    {
        HighestScore.GetComponent<Text>().text = "Highest Score: " + GameHandler.instance.player.score.ToString();
        yield return new WaitForSeconds(3f);
        HighestScore.SetActive(false);
    }
}
