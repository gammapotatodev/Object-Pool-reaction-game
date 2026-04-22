using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class NoteSpawner : MonoBehaviour
{
    [Header("Note prefab")]
    [SerializeField] private GameObject notePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private int poolSize = 15;
    [SerializeField] private float minSpawnInterval = 0.4f;
    [SerializeField] private float maxSpawnInterval = 1.2f;

    [Header("Spawn Positions")]
    [SerializeField] private Vector3 leftSpawnPosition = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 rightSpawnPosition = new Vector3(0, 0, 0);

    private IObjectPool<Note> notePool;
    private bool isGameOver = false;

    private void Awake()
    {
        Time.timeScale = 1f;
        
        notePool = new ObjectPool<Note>(
            createFunc: CreateNote,
            actionOnGet: note => note.gameObject.SetActive(true),
            actionOnRelease: note => note.gameObject.SetActive(false),
            actionOnDestroy: note => Destroy(note.gameObject),
            collectionCheck: true,
            defaultCapacity: poolSize,
            maxSize: poolSize * 2
        );
    }

    private void OnEnable()
    {
        GameEventManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEventManager.OnGameOver -= OnGameOver;
    }

    private void Start()
    {
        StartCoroutine(SpawnNotes());
    }

    private IEnumerator SpawnNotes()
    {
        while(!isGameOver)
        {
            SpawnRandomNote();
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);
        }
    }

    private void SpawnRandomNote()
    {
        NoteType type = Random.value > 0.5f ? NoteType.Left : NoteType.Right;
        Note note = notePool.Get();

        Vector3 pos = (type == NoteType.Left) ? leftSpawnPosition : rightSpawnPosition;
        note.Initialize(this, pos, type);
    }

    private Note CreateNote()
    {
        GameObject obj = Instantiate(notePrefab, transform);
        obj.SetActive(false);
        return obj.GetComponent<Note>();
    }

    public void ReturnToPool(Note note)
    {
        notePool.Release(note);
    }

    private void OnGameOver()
    {
        isGameOver = true;
    }
}
