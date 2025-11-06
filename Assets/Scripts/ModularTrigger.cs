using System;
using UnityEngine;
using UnityEngine.Events;

public class ModularTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public string targetTag = "Player"; // Only trigger on objects with this tag

    [SerializeField] private Puzzle[] puzzles;
    [SerializeField] private GameObject[] gameObjects;


    [Header("Events")]
    public UnityEvent onEnter; // Called when target enters
    public UnityEvent onExit;  // Called when target exits

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            onEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            onExit?.Invoke();
        }
    }

    public void SetInactive()
    {
        this.gameObject.SetActive(false);
    }

    public void SetPuzzlesActive(int index = -1)
    {
        if (puzzles == null || puzzles.Length == 0) return;
        if (index < 0)
        {
            foreach (var puzzle in puzzles)
            {
                if (puzzle != null)
                    puzzle.enabled = true;
            }
        } else if (index < puzzles.Length) {

            var puzzle = puzzles[index];
            if (puzzle != null)
                puzzle.enabled = true;
        }
       
    }

    public void SetPuzzlesInactive(int index = -1)
    {
        if (puzzles == null || puzzles.Length == 0) return;
        if (index < 0)
        {
            foreach (var puzzle in puzzles)
            {
                if (puzzle != null)
                    puzzle.enabled = false;
            }
        }
        else if (index < puzzles.Length)
        {

            var puzzle = puzzles[index];
            if (puzzle != null)
                puzzle.enabled = false;
        }
    }

    public void SetGameObjectsActive(int index = -1)
    {
        if (gameObjects == null || gameObjects.Length == 0) return;
        if (index < 0)
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj != null)
                    obj.gameObject.SetActive(true);
            }
        }
        else if (index < gameObjects.Length)
        {

            GameObject obj = gameObjects[index];
            if (obj != null)
                obj.gameObject.SetActive(true);
        }

    }
}
