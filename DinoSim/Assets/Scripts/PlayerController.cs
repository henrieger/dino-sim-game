using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;

    [SerializeField] PlayerInput playerInput;

    [SerializeField] float basePanThrust = 5f;
    [SerializeField] float panThrust;
    [SerializeField] float panDrag = 5f;
    [SerializeField] float panBorderThickness = 10f;
    [SerializeField] float mousePanSensibility = 0.4f;

    [SerializeField] float rotationThrust = 2f;

    Vector2 rawPanInput;
    Vector3 panInput;

    float rotationInput;

    bool canPanWithMouse = false;
    bool isPanningWithScroll = false;

    // Start is called before the first frame update
    void Start()
    { 
        playerInput = GetComponent<PlayerInput>();
        rb.drag = panDrag;
        cam.transform.LookAt(transform);
    }

    // Update is called once per frame
    void Update()
    {
        panThrust = basePanThrust * Vector3.Distance(cam.transform.position, transform.position);
        if (Mouse.current.position.ReadValue() != Vector2.zero)
            canPanWithMouse = true;

        //Cursor.visible = !Mouse.current.rightButton.isPressed;
    }

    private void FixedUpdate()
    { 
        Pan(panInput);
        Rotate(rotationInput);

        if (canPanWithMouse && !isPanningWithScroll)
        {
            if (Mouse.current.position.ReadValue().y >= Screen.height - panBorderThickness)
                Pan(Vector3.forward);
            if (Mouse.current.position.ReadValue().y <= panBorderThickness)
                Pan(Vector3.back);
            if (Mouse.current.position.ReadValue().x >= Screen.width - panBorderThickness)
                Pan(Vector3.right);
            if (Mouse.current.position.ReadValue().x <= panBorderThickness)
                Pan(Vector3.left);
        }

        if (Mouse.current.delta.ReadValue() != Vector2.zero && Mouse.current.leftButton.isPressed)
        {
            //Cursor.visible = false;
            isPanningWithScroll = true;
            Pan(new Vector3(-Mouse.current.delta.ReadValue().x * mousePanSensibility, 0, -Mouse.current.delta.ReadValue().y * mousePanSensibility));
        }
        else if(!Mouse.current.leftButton.isPressed)
        {
            //Cursor.visible = true;
            isPanningWithScroll = false;
        }
    }

    // --- Actions ---

    void Pan(Vector3 direction)
    {
        rb.AddRelativeForce(direction * panThrust, ForceMode.Acceleration);
    }

    void Rotate(float direction)
    {
        rb.AddTorque(new Vector3(0, direction * rotationThrust, 0), ForceMode.Acceleration);
    }

    // --- Input Events ---

    public void OnPan(InputAction.CallbackContext context)
    {
        rawPanInput = context.ReadValue<Vector2>();
        panInput = new Vector3(rawPanInput.x, 0, rawPanInput.y);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        rotationInput = context.ReadValue<float>();
    }
}
