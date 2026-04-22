using System.Collections;
using TMPro;
using UnityEngine;

public class HitUIFeedback : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leftText;
    [SerializeField] private TextMeshProUGUI rightText;
    [SerializeField] private TextMeshProUGUI playerStaminaText;
    [SerializeField] private TextMeshProUGUI playerMissesText;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private float displayTime = 0.5f;

    private Coroutine leftRoutine;
    private Coroutine rightRoutine;
    private int playerStamina = 100;
    private int playerMissesCount = 0;

    private void OnEnable()
    {
        playerStaminaText.text = playerStamina.ToString();
        playerMissesText.text = playerMissesCount.ToString();

        GameEventManager.OnHit += HandleHit;
    }

    private void OnDisable()
    {
        GameEventManager.OnHit -= HandleHit;
    }

    private void HandleHit(NoteType type, HitResult result)
    {
        string message = result.ToString();
        Color color = GetColor(result);

        if (type == NoteType.Left)
        {
            if (leftRoutine != null)
                StopCoroutine(leftRoutine);

            leftRoutine = StartCoroutine(ShowRoutine(leftText, message, color));
        }
        else
        {
            if (rightRoutine != null)
                StopCoroutine(rightRoutine);

            rightRoutine = StartCoroutine(ShowRoutine(rightText, message, color));
        }

        if (result == HitResult.Miss)
        {
            ApplyMissPenalty();
        }
    }

    private void ApplyMissPenalty()
    {
        playerStamina -= 10;
        playerMissesCount++;
        playerStaminaText.text = playerStamina.ToString() + "/100";
        playerMissesText.text = playerMissesCount.ToString();
        if(playerStamina <= 0)
        {
            loseScreen.SetActive(true);
            Time.timeScale = 0f;
            GameEventManager.OnGameOver?.Invoke();
        }
    }

    private IEnumerator ShowRoutine(TextMeshProUGUI textUI, string message, Color color)
    {
        textUI.text = message;
        textUI.color = color;
        textUI.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(displayTime);

        textUI.transform.localScale = Vector3.one * 1.2f;
        yield return new WaitForSecondsRealtime(0.05f);
        textUI.transform.localScale = Vector3.one;

        textUI.gameObject.SetActive(false);
    }

    private Color GetColor(HitResult result)
    {
        return result switch
        {
            HitResult.Perfect => Color.green,
            HitResult.Good => Color.yellow,
            HitResult.Bad => Color.red,
            _ => Color.gray
        };
    }

}
