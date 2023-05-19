using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    private Vector2 angle = new Vector2(90 * Mathf.Deg2Rad, 0);

    private Camera cam;

    public float limitFieldOfViewCameraY1=45;
    public float limitFieldOfViewCameraY2 = 75;

    public float limitOffsetY1 = -50;
    public float limitOffsetY2 = -10;



    public Transform follow;
    public float distance;

    [SerializeField]
    private Vector2 sensitivity;

    private float Y = 0; // set this to 0.2
    private float B = 0; // set this to -0.2

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //angle.y += 1 * Mathf.Deg2Rad * sensitivity.y;
        //angle.y = Mathf.Clamp(angle.y, -50 * Mathf.Deg2Rad, -10 * Mathf.Deg2Rad);

        angle = new Vector2(45, Mathf.Lerp(limitOffsetY1, limitOffsetY2, 0.5f)) * Mathf.Deg2Rad;
        cam = GetComponent<Camera>();

        sensitivity = GameManager.Instance.GetCameraInfo();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            float hor = Input.GetAxis("Mouse X");
            
            if (hor != 0)
            {
                angle.x += hor * Mathf.Deg2Rad * sensitivity.x;
            }

            float ver = Input.GetAxis("Mouse Y");

            if (ver != 0)
            {
                angle.y += ver * Mathf.Deg2Rad * sensitivity.y;
                angle.y = Mathf.Clamp(angle.y, limitOffsetY1*Mathf.Deg2Rad,  limitOffsetY2*Mathf.Deg2Rad);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && cam.fieldOfView>limitFieldOfViewCameraY1)
            cam.fieldOfView--;
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && cam.fieldOfView < limitFieldOfViewCameraY2)
            cam.fieldOfView++;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 orbit = new Vector3(
            Mathf.Cos(angle.x) * Mathf.Cos(angle.y),
            -Mathf.Sin(angle.y),
            -Mathf.Sin(angle.x) * Mathf.Cos(angle.y)
            );

        transform.position = follow.position + orbit * distance;
        transform.rotation = Quaternion.LookRotation(follow.position - transform.position);
    }
}
