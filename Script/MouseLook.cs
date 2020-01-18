using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	[SerializeField] private RotationAxes axes = RotationAxes.MouseXAndY;
	[SerializeField] private float sensitivityX = 15F;
	[SerializeField] private float sensitivityY = 15F;

	[SerializeField] private float minimumY = -60F;
	[SerializeField] private float maximumY = 60F;
    [SerializeField] private float SpeedCamera = 10f;
	[SerializeField] private float CoefTimeScale = 1f;
	[SerializeField] private TextMeshProUGUI TxtTimeScale = null;

	[SerializeField] private bool invertY = false;
	
	float rotationY = 0F;

	void Start ()
	{
		if (GetComponent<Rigidbody>())
        {
			GetComponent<Rigidbody>().freezeRotation = true;
        }
	}

	void Update ()
	{ 
		float ySens = sensitivityY;
		if(invertY) { ySens *= -1f; }

		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + GetMouseX() * sensitivityX;
			
			rotationY += GetMouseY() * ySens;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, GetMouseX() * sensitivityX, 0);
		}
		else
		{
			rotationY += GetMouseY() * ySens;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * (!Input.GetButton("Sprint") ? SpeedCamera * (1/CoefTimeScale): SpeedCamera * 3f * (1/CoefTimeScale)));
        transform.Translate(Vector3.left * -Input.GetAxis("Horizontal") * Time.deltaTime * (!Input.GetButton("Sprint") ? SpeedCamera * (1/CoefTimeScale): SpeedCamera * 3f * (1/CoefTimeScale)));
        if(Input.GetButtonDown("Fire1"))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

		if(Input.GetKeyDown(KeyCode.P))
		{
			CoefTimeScale = (CoefTimeScale*2)/1.5f;
			Time.timeScale = CoefTimeScale;
			TxtTimeScale.text = "Temps: " + CoefTimeScale.ToString();
		}
		if(Input.GetKeyDown(KeyCode.O))
		{
			CoefTimeScale = (CoefTimeScale/2)*1.5f;
			Time.timeScale = CoefTimeScale;
			TxtTimeScale.text = "Temps: " + CoefTimeScale.ToString();
		}
	}

    float GetMouseX()
    {
        return Input.GetAxis("Mouse X");
    }

    float GetMouseY()
    {
        return Input.GetAxis("Mouse Y");
    }
}
