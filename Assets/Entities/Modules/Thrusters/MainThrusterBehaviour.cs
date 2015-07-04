using UnityEngine;
using System.Collections;
using Assets.Serialization;
using Assets.Modules;
using Assets.Entities.Modules.Thrusters;
using Assets.Entities.Modules;

public class MainThrusterBehaviour : SpriteBehaviour<ThrusterDef>, IActivatedBehaviour
{
    private ParticleSystem[] exhausts;
	// Use this for initialization
    void Start()
    {
        base.Start();
        exhausts = transform.GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void Awake()
    {
        base.Awake();
        var exhaustPrefab = Resources.Load<GameObject>("modules/Main Thruster");
        var exhaust = Instantiate(exhaustPrefab);
        exhaust.transform.parent = transform;
        exhaust.transform.localPosition = -Vector3.up*0.1f;
    }
    private bool on;
    public void On()
    {
        if (gameObject.activeInHierarchy)
        {
            on = true;
            SetEmissionRate(200);
        }
    }
    private void SetEmissionRate(int emissionRate)
    {
        foreach (var exhaust in exhausts)
        {
            exhaust.emissionRate = emissionRate;
        }
    }
    public void Off()
    {
        on = false;
        SetEmissionRate(0);
    }
    public Vector2 ThrustVector
    {
        get{
            if (on)
            {
                return transform.up * def.thrust;
            }
            return Vector2.zero;
        }
    }
    public Vector2 position2d
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.y);
        }
    }
}
