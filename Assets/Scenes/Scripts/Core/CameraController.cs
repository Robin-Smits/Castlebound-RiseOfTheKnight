using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float cameraHeight;

    // Updates the camera relative to the player (is called once per frame)
    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y + cameraHeight, transform.position.z);
    }
}

