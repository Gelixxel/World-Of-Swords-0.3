using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControlSolo : MonoBehaviour
{
    float rotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;

    public Transform Obstruction;
    float zoomSpeed = 2f;

    public bool IsPaused = false;

    void Start()
    {
        Obstruction = Target;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (!IsPaused)
        {
            CamControl();
            ViewObstructed();
        }
    }
    

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);
        if (Player.GetComponent<JoueurSolo>().IsClimbing() == false)
        {
            if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.D)))
            {
                Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
                Player.rotation = Quaternion.Euler(0, mouseX, 0);
            }
            else
            {
                Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            }

        }
        else
        {
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        }
        transform.LookAt(Target);
        
    }
    

    void ViewObstructed()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Target.position - transform.position, out hit, 11f))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                Obstruction = hit.transform;
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                if (Vector3.Distance(Obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, Target.position) >= 1.5f)
                {
                    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
                }
            }
            else
            {
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if (Vector3.Distance(transform.position, Target.position) < 11f)
                {
                    transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                }
            }
        }
    }
}