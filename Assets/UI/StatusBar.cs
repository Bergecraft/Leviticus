using UnityEngine;
using System.Collections;

public class StatusBar : MonoBehaviour {

    Transform Health;
    Transform Shield;
    Transform Energy;
	// Use this for initialization
	void Start () {
        Health = transform.FindChild("HealthBar").FindChild("Health");
        Shield = transform.FindChild("ShieldBar").FindChild("Shield");
        Energy = transform.FindChild("EnergyBar").FindChild("Energy");
	}
    public void UpdateHealth(float percent)
    {
        Health.localScale = new Vector3(percent, 1, 1);
    }
    public void UpdateShield(float percent)
    {
        Shield.localScale = new Vector3(percent, 1, 1);
    }
    public void UpdateEnergy(float percent)
    {
        Energy.localScale = new Vector3(percent, 1, 1);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
