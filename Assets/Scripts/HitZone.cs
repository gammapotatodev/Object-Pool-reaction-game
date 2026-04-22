using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    [SerializeField] private NoteType zoneType;

    private List<Note> notesInZone = new List<Note>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        Note note = other.GetComponent<Note>();
        if (note != null && note.Type == zoneType)
        {
            notesInZone.Add(note);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Note note = other.GetComponent<Note>();
        if (note != null)
        {
            notesInZone.Remove(note);
        }
    }

    public Note GetClosestNote()
    {
        if (notesInZone.Count == 0) return null;

        Note closest = null;
        float minDistance = float.MaxValue;

        foreach (var note in notesInZone)
        {
            float dist = Mathf.Abs(note.transform.position.y - transform.position.y);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = note;
            }
        }

        return closest;
    }
    
    public NoteType ZoneType => zoneType;
}

