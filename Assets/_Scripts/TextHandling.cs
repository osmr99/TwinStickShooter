using UnityEngine;
using TMPro;
using System.Collections;

public class TextHandling : MonoBehaviour
{
    TMP_Text text;
    GameManager gameManager;
    PowerupUI powerupUI;
    PlayerShootScript playerShootScript;
    int decision;
    float timeLeft;
    string[] preText = {"Speed+\n", "Large Shot\n", "Spread Shot\n", "Shot Speed+\n" };
    Color[] colorList = {Color.cyan, Color.yellow, Color.red, Color.green};
    float killTime = 2f;

    private void OnEnable()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        powerupUI = FindAnyObjectByType<PowerupUI>();
        text = GetComponent<TMP_Text>();
        playerShootScript = FindAnyObjectByType<PlayerShootScript>();
        decision = powerupUI.currentChoice;
        timeLeft = gameManager.powerupsDuration;
        StartCoroutine(StartUI());

    }

    IEnumerator StartUI()
    {
        yield return new WaitForSeconds(0.05f);
        UIHandling(decision);
    }

    public void UIHandling(int choice)
    {
        switch (choice)
        {
            case 1:
                text.color = colorList[choice - 1];
                StartCoroutine(UpgradeOne());
                break;
            case 2:
                text.color = colorList[choice - 1];
                StartCoroutine(UpgradeTwo());
                break;
            case 3:
                text.color = colorList[choice - 1];
                StartCoroutine(UpgradeThree());
                break;
            case 4:
                text.color = colorList[choice - 1];
                StartCoroutine(UpgradeFour());
                break;
            default:
                Debug.Log("Fail...");
                break;
        }
    }

    IEnumerator UpgradeOne()
    {
        if(Time.timeScale != 0)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            timeLeft -= 0.1f;
            text.text = preText[0] + "(" + timeLeft.ToString("F1") + "s)";
            if (timeLeft > 0)
            {
                StartCoroutine(UpgradeOne());
            }
            else
            {
                timeLeft = 0;
                Destroy(gameObject, killTime);
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(UpgradeOne());
        }

    }

    IEnumerator UpgradeTwo()
    {
        if(Time.timeScale != 0)
        {
            if(playerShootScript.shotID == 2)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                timeLeft -= 0.1f;
                text.text = preText[1] + "(" + timeLeft.ToString("F1") + "s)";
                if (timeLeft > 0)
                {
                    StartCoroutine(UpgradeTwo());
                }
                else
                {
                    Destroy(gameObject, killTime);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(UpgradeTwo());
        }
    }

    IEnumerator UpgradeThree()
    {
        if(Time.timeScale != 0)
        {
            if (playerShootScript.shotID == 3)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                timeLeft -= 0.1f;
                text.text = preText[2] + "(" + timeLeft.ToString("F1") + "s)";
                if (timeLeft > 0)
                {
                    StartCoroutine(UpgradeThree());
                }
                else
                {
                    Destroy(gameObject, killTime);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(UpgradeThree());
        }
    }

    IEnumerator UpgradeFour()
    {
        if(Time.timeScale != 0)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            timeLeft -= 0.1f;
            text.text = preText[3] + "(" + timeLeft.ToString("F1") + "s)";
            if (timeLeft > 0)
            {
                StartCoroutine(UpgradeFour());
            }
            else
            {
                Destroy(gameObject, killTime);
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(UpgradeFour());
        }
    }
}
