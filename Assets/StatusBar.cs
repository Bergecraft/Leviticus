using UnityEngine;
using System.Collections;

public class StatusBar : MonoBehaviour {

    Transform Health;
	// Use this for initialization
	void Start () {
        Health = transform.FindChild("HealthBar").FindChild("Health");
	}
    public void UpdateHealth(float percent)
    {
        Health.localScale = new Vector3(percent, 1, 1);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
