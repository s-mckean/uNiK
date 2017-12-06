using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {

    [SerializeField] private Animator m_CamAnim;
    
	// Use this for initialization
	IEnumerator Start () {
        m_CamAnim.speed = 0;
        yield return new WaitForSeconds(7.0f);
        m_CamAnim.speed = 1;
        Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
