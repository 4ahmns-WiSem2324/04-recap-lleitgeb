using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public Button button1;
    public Button button6;
    int currentNumber;
    int playerScore;
    int button1ClickCount;
    int button6ClickCount;
    int difficultyLevel;
    float reactionTime;
    bool gameStarted;
    bool textSet;

    void Start()
    {
        playerScore = 0;
        reactionTime = 1.5f;
        gameStarted = false;
        button1ClickCount = 0;
        button6ClickCount = 0;
        textSet = false;

        EnableButton(button1);
        DisableButton(button6);
    }

    void Update()
    {
        if (gameStarted)
        {
            reactionTime -= Time.deltaTime;

            if (reactionTime <= 0)
            {
                GameOver();
            }
        }
    }

    public void OnButtonClick(int buttonNumber)
    {
        if (gameStarted && buttonNumber == currentNumber)
        {
            playerScore++;
            NextNumber();
        }
    }

    public void OnButton1Click()
    {
        if (!gameStarted)
        {
            button1ClickCount++;

            if (button1ClickCount == 2 && !textSet)
            {
                SetNumberText("8888");
                textSet = true;
            }

            if (button1ClickCount == 2)
            {
                EnableButton(button6);
            }
        }
    }

    public void OnButton6Click()
    {
        if (!gameStarted)
        {
            button6ClickCount++;

            if (button6ClickCount == 1)
            {
                SetNumberText("D1");
                StartCoroutine(StartGameWithDelay(2.0f, 1));
            }
            else if (button6ClickCount == 2)
            {
                SetNumberText("D2");
                StartCoroutine(StartGameWithDelay(2.0f, 2));
            }
        }
    }

    IEnumerator StartGameWithDelay(float delay, int level)
    {
        yield return new WaitForSeconds(delay);
        StartGame(level);
    }

    void StartGame(int level)
    {
        gameStarted = true;
        EnableButton(button1);
        difficultyLevel = level;
        NextNumber();
    }

    void NextNumber()
    {
        if (gameStarted)
        {
            if (difficultyLevel == 1)
            {
                currentNumber = Random.Range(1, 7);
                reactionTime = Mathf.Lerp(1.5f, 1, playerScore * 0.1f);
            }
            else if (difficultyLevel == 2)
            {
                do
                {
                    currentNumber = Random.Range(10, 100);
                }

                while (currentNumber.ToString().Contains("7") || currentNumber.ToString().Contains("8")
                || currentNumber.ToString().Contains("9") || currentNumber.ToString().Contains("0"));

                reactionTime = Mathf.Lerp(2f, 1, playerScore * 0.1f);
            }

            numberText.text = currentNumber.ToString();
        }
    }

    void DisableButton(Button button)
    {
        button.interactable = false;
    }

    void EnableButton(Button button)
    {
        button.interactable = true;
    }

    void SetNumberText(string text)
    {
        numberText.text = text;
    }

    void GameOver()
    {
        gameStarted = false;
        numberText.text = "Score: " + playerScore;
        button1ClickCount = 0;
        button6ClickCount = 0;
        EnableButton(button1);
        textSet = false;
        playerScore = 0;
    }
}