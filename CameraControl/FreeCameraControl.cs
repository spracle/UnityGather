using UnityEngine;
using System.Collections;

public class FreeCameraControl : MonoBehaviour {

    public float lookSpeed = 15.0f;
    public float moveSpeed = 15.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        OVRManager.display.RecenterPose();
        Cursor.visible = true;
    }
	
	// Update is called once per frame
	void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * lookSpeed;
        rotationY += Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        transform.position += transform.forward * moveSpeed * Input.GetAxis("Vertical");
        transform.position += transform.right * moveSpeed * Input.GetAxis("Horizontal");

        if (Input.GetKeyDown("left ctrl"))
        {
            //OVRDevice.ResetOrientation(0);
            OVRManager.display.RecenterPose();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += new Vector3(0, 0.1f, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += new Vector3(0, -0.1f, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
