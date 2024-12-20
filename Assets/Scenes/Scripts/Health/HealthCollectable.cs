using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip pickupSound;
    
    // Checks if a player makes contact with the health collectable
    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.tag == "Player")
        {
            SoundManager.instance.PlaySound(pickupSound);
            colision.GetComponent<Health>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}