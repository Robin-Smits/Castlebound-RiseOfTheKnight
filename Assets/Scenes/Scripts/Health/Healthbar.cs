using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;
    // Start is called before the first frame update
    void Start()
    {
        currenthealthBar.fillAmount = health.GetCurrentHealth() / 10;
    }

    // Updates the healthbar (is called once per frame)
    void Update()
    {
        currenthealthBar.fillAmount = health.GetCurrentHealth() / 10;
    }
}