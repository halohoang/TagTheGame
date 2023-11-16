using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinningZone : MonoBehaviour
{
    public static event UnityAction OnWinningZoneEntered;

    [SerializeField] private int _indexOfSceneToLoadOnWinning = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnWinningZoneEntered?.Invoke();
            Debug.Log($"<color=lime>Winning Zone was entered and accorind Event should have been fired.</color>");

            SceneManager.LoadScene(_indexOfSceneToLoadOnWinning);
        }
    }
}
