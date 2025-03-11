using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class SceneChangeOnTrigger : MonoBehaviour
{
    // The scene index or scene name to load when the player triggers the collider
    public int sceneIndex; // You can set this in the Inspector
    // OR
    // public string sceneName; // Alternatively, use this if you want to load by name

    // Called when another collider enters this trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger area
        if (other.CompareTag("Player"))
        {
            // Load the scene either by index or by name
            SceneManager.LoadScene(sceneIndex); // If using sceneIndex
            // SceneManager.LoadScene(sceneName); // If using sceneName
        }
    }
}
