using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    protected float _rotationSpeed = 500.0f;

    protected Coroutine _orbitUpdateRoutine = null;

    protected Vector3 _defaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        _defaultPosition = transform.position;
        _orbitUpdateRoutine = StartCoroutine(Orbit());
    }

    private IEnumerator Orbit()
    {
        while(true)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                CameraOrbit();
            }
            yield return null;
        }
    }

    private void CameraOrbit()
    {
        if(Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.right, -verticalInput);
            transform.Rotate(Vector3.up, horizontalInput, Space.World);
        }
    }

    public void ChangeFocusTarget(Transform target)
    {
        gameObject.transform.SetPositionAndRotation(target.position, transform.rotation);
    }

    public void SetToDefaultPosition()
    {
        transform.SetPositionAndRotation(_defaultPosition, transform.rotation);
    }
}
