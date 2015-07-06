using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpacecraftManager : MonoBehaviour {

    public List<SpacecraftController> spacecraft;
    public Camera miniMapCam;
	// Use this for initialization
	void Start () {
        spacecraft = new List<SpacecraftController>();
	}
    public void Add(SpacecraftController sc)
    {
        spacecraft.Add(sc);
    }
    public void Remove(SpacecraftController sc)
    {
        spacecraft.Remove(sc);
    }
    const float UPDATE_CYCLE = 5;
    private float lastUpdate = 0;
	// Update is called once per frame
	void Update () {
        //if (Time.time - lastUpdate > UPDATE_CYCLE)
        //{
            //lastUpdate = Time.time;
            var minX = spacecraft.Select(s => s.transform.position.x).Min();
            var maxX = spacecraft.Select(s => s.transform.position.x).Max();
            var minY = spacecraft.Select(s => s.transform.position.y).Min();
            var maxY = spacecraft.Select(s => s.transform.position.y).Max();
            miniMapCam.transform.position = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, -10);
            miniMapCam.orthographicSize = Mathf.Max(maxY - minY, maxX - minX)/2+10;
        //}
	}
}
