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

    private GameObject aimArrow;
    private bool mouseDown;

	// Use this for initialization
	void Start () {
        mouseDown = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        CreateOrDestroyArrow();
        AdjustCrosshair();
        AdjustArrow();
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
            mouseDown = false;
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
}
