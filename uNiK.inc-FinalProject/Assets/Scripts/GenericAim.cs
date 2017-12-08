using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAim : MonoBehaviour {
    
    [SerializeField] private Transform crosshairTransform;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject aimArrowPrefab;
    [SerializeField] private float xScaleMod;
    [SerializeField] private float yScaleMod;
    [SerializeField] private float yPosMod;
    [SerializeField] private float yScaleMax;

    [SerializeField] private Transform firingPosition;
    [SerializeField] private Rigidbody2D projectile;
    [SerializeField] private float minPower = 10f;
    [SerializeField] private float maxPower = 50f;
    [SerializeField] private float chargeTime = 0.75f;
    [SerializeField] private float delay = 1.0f;
    [SerializeField] private float firingPosOffset = 0.5f;      // Testing firing position

    private GameObject aimArrow;
    private bool mouseDown;

    private string fireButton = "Fire1";
    private float chargeSpeed;
    private float currentPower;
    private float lastShot;

    // Use this for initialization
    void Start () {
        mouseDown = false;
        chargeSpeed = (maxPower - minPower) / chargeTime;
        lastShot = 0f;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        CreateOrDestroyArrow();
        AdjustCrosshair();
        AdjustArrow();
        ChargeAndFireShot();
    }

    private void ChargeAndFireShot()
    {
        if (!Input.GetButton(fireButton) && mouseDown && (Time.time - lastShot) > delay) //checks to see if the button is released, but we have not fired yet
        {
            Fire();
            currentPower = minPower;
            lastShot = Time.time;
            mouseDown = false;
        }
        else if (Input.GetButton(fireButton)) //checks if the firing button is held down, but we haven't fired yet
        {
            currentPower += chargeSpeed * Time.deltaTime;
            Mathf.Clamp(currentPower, minPower, maxPower);
        }
    }

    private void AdjustCrosshair()
    {
        Vector3 mousePos = cam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        Vector2 newPos = Vector2.MoveTowards(gameObject.GetComponent<Transform>().position,
            mousePos, 2f);
        crosshairTransform.position = newPos;
    }

    /*
     * Depending on mouse input, this creates or destroys an aim arrow object 
     * at the tank this script is attached to.
     * ~natechica
     */
    private void CreateOrDestroyArrow()
    {
        if (Input.GetMouseButton(0) && !mouseDown)
        {
            mouseDown = true;
            CreateArrow();
        }
        if (!Input.GetMouseButton(0))
        {
            DestroyArrow();
        }
    }

    /*
     * Creates aim arrow and parents it to the tank and adjusts its initial position
     * ~natechica
     */
    private void CreateArrow()
    {
        aimArrow = Instantiate(aimArrowPrefab, gameObject.transform.position, Quaternion.identity);
        aimArrow.transform.parent = gameObject.transform;
        aimArrow.transform.position = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + 0.05f,
            gameObject.transform.position.z);

        StartCoroutine(ContinuouslyAdjustAndScale());
    }

    /*
    * Rotate arrow towards crosshair (credits: https://www.youtube.com/watch?v=mKLp-2iseDc)
    * ~natechica
    */
    private void AdjustArrow()
    {
        if (aimArrow != null)
        {
            Vector3 direction = crosshairTransform.position - aimArrow.GetComponent<Transform>().position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            aimArrow.transform.rotation = rotation;
        }
    }

    /*
     * Destroys aim arrow if it exists and resets mouseDown to false
     * ~natechica
     */
    public void DestroyArrow()
    {
        if (aimArrow != null)
        {
            Destroy(aimArrow);
            //mouseDown = false;
        }
    }

    /*
     * Makes the arrow grow while mouse1 is being held down. Stops at a certain size.
     * This is probably doable without a coroutine
     * ~natechica
     */
    private IEnumerator ContinuouslyAdjustAndScale()
    {
        while (aimArrow != null && aimArrow.transform.localScale.y < yScaleMax)
        {
            ScaleAndMoveArrow();
            yield return new WaitForFixedUpdate();
        }
    }

    /*
     * Coroutine calls this function which scales and moves the arrow
     * ~natechica
     */
    private void ScaleAndMoveArrow()
    {
        if (aimArrow != null)
        {
            float xScale = aimArrow.transform.localScale.x + xScaleMod;
            float yScale = aimArrow.transform.localScale.y + yScaleMod;
            float zScale = aimArrow.transform.localScale.z;
            aimArrow.transform.localScale = new Vector3(xScale, yScale, zScale);

            float xPos = aimArrow.transform.position.x;
            float yPos = aimArrow.transform.position.y + yPosMod;
            float zPos = aimArrow.transform.position.z;
            aimArrow.transform.position = new Vector3(xPos, yPos, zPos);
        }
    }

    private void Fire()
    {
        Vector2 direction = crosshairTransform.position - transform.position;
        // Part of testing firing position
        Vector2 tempFiringPos = Vector2.MoveTowards(transform.position, crosshairTransform.position, firingPosOffset);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Rigidbody2D formProjectile = Instantiate(projectile, tempFiringPos, Quaternion.Euler(0f, 0f, angle)) as Rigidbody2D;
        formProjectile.velocity = currentPower * CalculateAngle(direction);

        if (TurnSystem.Instance != null)
        {
            transform.parent.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
            if (projectile.CompareTag("PassThrough"))
            {
                TurnSystem.Instance.Event_ShotFiredNoTracking();
            }
            else
            {
                TurnSystem.Instance.Event_ShotFired(formProjectile.gameObject);
            }
        }
    }

    private Vector2 CalculateAngle(Vector2 direction)
    {
        Vector2 temp = direction;
        int xNeg = 1;
        int yNeg = 1;
        if (temp.x < 0)
        {
            xNeg = -1;
        }
        if (temp.y < 0)
        {
            yNeg = -1;
        }
        temp.x = temp.x * temp.x;
        temp.y = temp.y * temp.y;
        temp = temp / (direction.magnitude * direction.magnitude);
        temp.x = Mathf.Sqrt(temp.x) * xNeg;
        temp.y = Mathf.Sqrt(temp.y) * yNeg;
        return temp;
    }

    public void SetProjectile(Rigidbody2D newProjectile)
    {
        projectile = newProjectile;
    }

    public void ResetShot()
    {
        DestroyArrow();
        currentPower = minPower;
        lastShot = Time.time;
        mouseDown = false;
    }
}
