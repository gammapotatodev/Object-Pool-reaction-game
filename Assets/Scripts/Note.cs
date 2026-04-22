using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Speed settings")]
    [SerializeField] private float speed = 5f;

    private NoteSpawner noteSpawner;
    private NoteType noteType;
    private Vector3 startPosition;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(NoteSpawner spawner, Vector3 spawnPos, NoteType type)
    {
        noteSpawner = spawner;
        noteType = type;
        startPosition = spawnPos;
        
        transform.position = spawnPos;

        if(spriteRenderer != null)
        {
            spriteRenderer.color = type == NoteType.Left ? Color.orange : Color.blue;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -2.5f)
        {
            GameEventManager.OnHit?.Invoke(noteType, HitResult.Miss);
            ReturnToPool();
        }
    } 

    public void OnHit()
    {
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        transform.position = startPosition;
        noteSpawner.ReturnToPool(this);
    }

    public NoteType Type => noteType;
}
