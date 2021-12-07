using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCameraController : MonoBehaviour
{
    
    public float rotationSpeed = 10;
    private float mInitialPositionX;
    private float mChangedPositionX;
    [SerializeField] private Transform mTargetToRotateAround;
    private Vector3 initialVector = Vector3.forward;
    public int _RotationLimit = 30;
    public bool _CameraFreeRoam = true;
    //  int? i = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //staticMovement();
        HorizontalPanningWithRotation();
    }

    /// <summary>
    /// This moves the Camera Left and Right 
    /// </summary>
    public void staticMovement()
    {
        Vector3 rotation = transform.eulerAngles;

        rotation.y += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime; 

        transform.eulerAngles = rotation;
        
    }

    private void HorizontalPanningWithRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mInitialPositionX = Input.mousePosition.x;
        }

        if (_CameraFreeRoam)
        {
            mChangedPositionX = Input.mousePosition.x;
            float rotateDegrees = 0f;
            if (mChangedPositionX < mInitialPositionX)
            {
                rotateDegrees += 50f * Time.deltaTime;
            }
            if (mChangedPositionX > mInitialPositionX)
            {
                rotateDegrees -= 50f * Time.deltaTime;
            }
            Vector3 currentVector = transform.position - mTargetToRotateAround.position;
            currentVector.y = 0;
            float angleBetween = Vector3.Angle(initialVector, currentVector) * (Vector3.Cross(initialVector, currentVector).y > 0 ? 1 : -1);
            float newAngle = Mathf.Clamp(angleBetween + rotateDegrees, -_RotationLimit, _RotationLimit);
            rotateDegrees = newAngle - angleBetween;

            transform.RotateAround(mTargetToRotateAround.position, Vector3.up, rotateDegrees);

            mInitialPositionX = mChangedPositionX;
        }
    }
}
