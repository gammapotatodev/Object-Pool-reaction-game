using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private HitZone leftZone;
    [SerializeField] private HitZone rightZone;
    [SerializeField] private HitUIFeedback feedbackUI;

    [Header("Accuracy properties")]
    [SerializeField] private float perfectWindow = 0.1f;
    [SerializeField] private float goodWindow = 0.25f;
    [SerializeField] private float badWindow = 0.4f;

    private bool isGameOver = false;

    private void Update()
    {
        if (isGameOver) return;

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            TryHit(leftZone);
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            TryHit(rightZone);
        }
    }

    private void OnEnable()
    {
        GameEventManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEventManager.OnGameOver -= OnGameOver;
    }


    private void TryHit(HitZone zone)
    {
        Note note = zone.GetClosestNote();

        if (note == null)
        {
            GameEventManager.OnHit?.Invoke(zone.ZoneType, HitResult.Miss);
            return;
        }

        float distance = Mathf.Abs(note.transform.position.y - zone.transform.position.y);

        if (distance <= perfectWindow)
        {
            GameEventManager.OnHit?.Invoke(zone.ZoneType, HitResult.Perfect);
            note.OnHit();
        }
        else if (distance <= goodWindow)
        {
            GameEventManager.OnHit?.Invoke(zone.ZoneType, HitResult.Good);
            note.OnHit();
        }
        else if (distance <= badWindow)
        {
            GameEventManager.OnHit?.Invoke(zone.ZoneType, HitResult.Bad);
            note.OnHit();
        }
        else
        {
            GameEventManager.OnHit?.Invoke(zone.ZoneType, HitResult.Miss);
        }
    }

    private void OnGameOver()
    {
        isGameOver = true;
    }
}
