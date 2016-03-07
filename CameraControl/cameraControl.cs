using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {

    private Transform camTransform;

    public float sensitivity = 4.0f; // Sensitivity set by player

    private float rotX = 0.0f;
    private float rotY = 0.0f;

    private float SPEED = 1.0f;

    private float minimumX = -360f;
    private float maximumX = 360f;

    private float minimumY = -85f;
    private float maximumY = 85f;

    private Quaternion originalRotation;
    public float smoothSpeed = 0.35f;

    [HideInInspector]
    public float sensitivityAmt = 4.0f; // Actual sensitivity

	// Use this for initialization
	void Start () {
        //attach camera to this object
        Camera.main.transform.parent = transform;

        camTransform = transform;  // storing the transform of the gameObject
        Cursor.visible = false; // hiding the cursor
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true; // Restricting the rotation due to external physics if any RigidBody is attached
        }
        //Setting the initial sensitivity to that set by the player
        sensitivityAmt = sensitivity;
        //TODO : syncing rotation
        originalRotation = camTransform.localRotation;
	
	}
	
	// Update is called once per frame
	void Update () {
        //Locking the cursor to the middle of the screen
       // Screen.lockCursor = true;
       // Cursor.visible = true; // hiding the cursor

        //Start reading mouse input if 1 second has crossed and the game is not paused
        if (Time.timeSinceLevelLoad > 1 && Time.timeScale > 0)
        {
            //calculating the rotation w.r.t the X and Y axis from the input
            rotX += Input.GetAxisRaw("Mouse X") * sensitivityAmt * Time.timeScale;
            rotY += Input.GetAxisRaw("Mouse Y") * sensitivityAmt * Time.timeScale;
            //Limiting the value of the rotation so that it doesn't cross a certain angle
            rotX = limitAngle(rotX, minimumX, maximumX);
            rotY = limitAngle(rotY, minimumY, maximumY);
            //Applying the rotation
            Quaternion xQuaternion = Quaternion.AngleAxis(rotX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotY, -Vector3.right);
            camTransform.rotation = Quaternion.Slerp(camTransform.rotation, originalRotation * xQuaternion * yQuaternion, smoothSpeed * Time.smoothDeltaTime * 60 / Time.timeScale);
        }

        //WASD movement
        camTransform.Translate(Vector3.right * Time.deltaTime * SPEED * Input.GetAxis("Horizontal")); 
        camTransform.Translate(Vector3.forward * Time.deltaTime * SPEED * Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

	}

    public static float limitAngle(float angle, float min, float max)   // To limit the angle moved by the player
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}
