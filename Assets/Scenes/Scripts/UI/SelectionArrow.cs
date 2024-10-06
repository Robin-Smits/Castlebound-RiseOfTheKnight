using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private AudioClip changeSound;
    private RectTransform rectTransform;
    private int currentPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //Change the position of the arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);

        //Interaction options
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            Interact();
    }
    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);

        if (currentPosition < 0)
            currentPosition = options.Length -1;
        else if (currentPosition > options.Length -1)
            currentPosition = 0;

        rectTransform.position = new Vector3(
            rectTransform.position.x,
            options[currentPosition].position.y,
            0
        );
    }

    private void Interact() {
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
