using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
    [SerializeField] private Transform crosshairTransform;

    public SpriteRenderer cannon;

	// Update is called once per frame
	void FixedUpdate () {
        adjustCannon();
	}

    private void adjustCannon()
    {
        Vector3 direction = crosshairTransform.position - cannon.GetComponent<Transform>().position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle > 0f && angle < 90f)
        {
            Quaternion rot = Quaternion.Euler(-angle, 90f, 0f);
            cannon.transform.rotation = rot;
        }
        else if (angle > 90f && angle < 180f)
        {
            Quaternion rot = Quaternion.Euler(-angle, 90f, 180f);
            cannon.transform.rotation = rot;
        }
        else if (angle < 0f && angle > -45f)
        {
            Quaternion rot = Quaternion.Euler(-angle, 90f, 0f);
            cannon.transform.rotation = rot;
        }
        else if (angle < -135f && angle > -180f)
        {
            Quaternion rot = Quaternion.Euler(-angle, 90f, 180f);
            cannon.transform.rotation = rot;
        }
    }
}
