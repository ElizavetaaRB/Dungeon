using UnityEngine;

public class Final : MonoBehaviour
{
    private Player player;

    [SerializeField] private GameObject winScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            if (other.CompareTag(player.tag))
            {
                winScreen.SetActive(true);
            }
        }
    }
}
