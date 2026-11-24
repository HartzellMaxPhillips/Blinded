using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PickupOnClick : MonoBehaviour
{
    /*
        Notes:

        - Can clip through slim planes, so make sure walls have thick colliders,
        - For smoother rigidbodies set their interpolate setting to interpolate (Forced in this code)

    */


    //if mouse is down, raycast forward. If it hits an object with a rigidbody, pick it up. If mouse is up, drop the object.
    [Header("Pickup Settings")]
    public float pickupRange = 5f;
    public float pickupSpeed = 5f;
    public Transform holdParent;
    public Camera cameraObject;

    private Rigidbody pickedUpObject;
    private float holdDistance;
    private Vector3 holdOffset;

    private bool inputMouseDown = false;

    void Update()
    {
        getInput();
        
        if(inputMouseDown)
        {
            if (!pickedUpObject)
            {
                pickupObject();    
            }
            
        }
        else
        {
            if(pickedUpObject)
            {
                dropObject();
            }
        }

    }

    private void getInput()
    {
        inputMouseDown = Input.GetMouseButton(0);
    }
    private void pickupObject()
    {
        Ray ray = cameraObject.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, pickupRange))
        {
            if(hit.rigidbody != null)
            {
                pickedUpObject = hit.rigidbody;
                pickedUpObject.interpolation = RigidbodyInterpolation.Interpolate; // This setting makes smoother movement

                holdDistance = Vector3.Distance(cameraObject.transform.position, hit.point);
                holdOffset = pickedUpObject.transform.position - hit.point;
            }
        }
    }

    private void dropObject()
    {
        pickedUpObject = null;
    }

    void FixedUpdate()
    {
        // Move Towards Target Goal (Cursor)
        if (pickedUpObject == null) return;

        Ray ray = cameraObject.ScreenPointToRay(Input.mousePosition);
        Vector3 target = ray.origin + ray.direction * holdDistance + holdOffset;

        Vector3 velocity = (target - pickedUpObject.position) / Time.fixedDeltaTime;
        pickedUpObject.linearVelocity = Vector3.ClampMagnitude(velocity, pickupSpeed);
        pickedUpObject.angularVelocity = Vector3.zero;


    }

}
