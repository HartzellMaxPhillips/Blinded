using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimplePOVCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public bool doGravity = false;
    public float gravity = -9.8f;
    public GameObject cameraObject;

    private CharacterController characterController;
    float mousex;
    float mousey;
    private float movementx;
    private float movementz;

    private float pitch = 0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        getInput();
        moveFromInput();
        rotateFromInput();
        if(doGravity)
        {
            characterController.Move(Vector3.up * gravity * Time.deltaTime);
        }
    }

    void getInput()
    {
        movementx = Input.GetAxis("Horizontal");
        movementz = Input.GetAxis("Vertical");
        mousex = Input.GetAxis("Mouse X") * lookSpeed;
        mousey = Input.GetAxis("Mouse Y") * lookSpeed;

    }

    void moveFromInput()
    {
        Vector3 move = transform.right * movementx + transform.forward * movementz;
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    void rotateFromInput()
    {
        // Rotate the player object left and right
        transform.Rotate(Vector3.up * mousex);

        // Rotate the camera up and down
        pitch -= mousey;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        cameraObject.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

    }

    public void lockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void unlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    


}
