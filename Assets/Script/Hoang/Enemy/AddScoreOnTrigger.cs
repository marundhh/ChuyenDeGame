using UnityEngine;
using Fusion;

public class AddScoreOnTrigger : NetworkBehaviour
{
    public PlayerProperties playerProps;

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.CompareTag("Collectible"))
        {
            playerProps.score += 10; // Cộng 10 điểm
            Debug.Log("Score increased! Current score: " + playerProps.score);
            Destroy(other.gameObject); // Nếu là vật thể bình thường
            
        }

    }
}
