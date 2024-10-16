using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private AudioClip changeSound;

    private RectTransform rectTransform;
    private int currentPosition;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Base.MenuArrows.performed += OnMenuArrowChange;
        playerInputActions.Base.Interact.performed += OnInteract;
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Base.MenuArrows.performed -= OnMenuArrowChange;
        playerInputActions.Base.Interact.performed -= OnInteract;
        playerInputActions.Disable();
    }

    private void OnMenuArrowChange(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // Check if the user is moving the selection
        if (input.y > 0)
        {
            ChangePosition(-1); // Move up
        }
        else if (input.y < 0)
        {
            ChangePosition(1); // Move down
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) // Ensure the interaction happens only once
        {
            Interact();
        }
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);

        // Wrap around selection
        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition >= options.Length) // Changed from > to >=
            currentPosition = 0;

        // Update the arrow's position
        rectTransform.position = new Vector3(
            rectTransform.position.x,
            options[currentPosition].position.y,
            0
        );
    }

    private void Interact()
    {
        // Play the interaction sound
        SoundManager.instance.PlaySound(interactSound);
        // Invoke the onClick event of the current button
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
