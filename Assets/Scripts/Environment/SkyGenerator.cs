using UnityEngine;
using System.Collections;
using System.Linq;

public class SkyGenerator : MonoBehaviour {
    public Transform STAR_PREFAB;
	// Use this for initialization
	void Start () {
        var background = new GameObject("Background");
        background.transform.parent = this.transform;
	    foreach(var i in Enumerable.Range(0,50000)){
            var star = Instantiate(STAR_PREFAB);
            var pos = Random.insideUnitCircle*1000;
            star.position = new Vector3(pos.x,pos.y,10);
            star.parent = background.transform;
            star.GetComponent<SpriteRenderer>().color = GetRandomStarColor();
        }
	}

    private Color GetRandomStarColor()
    {
        var type = Random.value;
        if (type > 0.9f)//blue dwarf
        {
            var white = Random.value;
            var blue = Random.value;
            return new Color(white, white, white+blue*(1-white));
        }
        else if(type>.3f)//yellow
        {
            var red = Random.value;
            return new Color(1,0.5f+red*0.5f,0.5f);
        }
        else //red dwarf
        {
            var red = Random.value;
            return new Color(0.8f+red*0.2f, .8f, .8f);
        }
    }
	
	// Update is called once per frame
	void Update () {
	}
}
