using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelItem : MonoBehaviour {

    public float addedFuel = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var tankController = collision.gameObject.GetComponent<TankController>();
            tankController.AddFuel(addedFuel);
            Destroy(this.gameObject);
        }
    }
}
