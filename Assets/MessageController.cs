using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class MessageController : MonoBehaviour {
    float SCROLL_SPEED = 0.2f;
    //List<string> messages = new List<string>();
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //if (messages.Count > 0)
        //{
        //    GetComponentInChildren<Text>().text = messages.Aggregate((s, sum) => sum + "\n" + s);
        //}
        //GetComponentInChildren<Text>().transform.position = Vector3.MoveTowards(GetComponentInChildren<Text>().transform.position, startPos, SCROLL_SPEED*Time.deltaTime);
	}
    //Color lastColor = Color.white;
    public void AddMessage(string message, Color? color = null)
    {

        var go = new GameObject(message);
        //var col = color ?? Color.white;
        if (color != null)
        {
            message = "<color=#" + ColorToString(color.Value) + ">"+message+"</color>";
            //lastColor = col;7
        }
        var cont = transform.Find("Text Container");
        go.AddComponent<Text>();
        go.GetComponent<Text>().text = message;
        go.GetComponent<Text>().fontSize = 12;
        go.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        go.AddComponent<CanvasGroup>();
        DOTween.To(() => go.GetComponent<CanvasGroup>().alpha, (a) => go.GetComponent<CanvasGroup>().alpha = a, 0.2f, 10);
        
        //go.AddComponent<LayoutElement>();
        //go.GetComponent<LayoutElement>().minHeight = 12;
        //go.GetComponent<LayoutElement>().preferredHeight = 12;
        GameObject.Destroy(go, 10);
        go.transform.parent = cont;
        //GetComponentInChildren<Text>().transform.position += Vector3.up * 10;
        //GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
        //GetComponent<ScrollRect>().verticalNormalizedPosition += 10;
        //transform.position -= Vector3.up * 10;
    }
    public string ColorToString(Color c)
    {
        return ((int)(c.r * 255)).ToString("X2") +
               ((int)(c.g * 255)).ToString("X2") +
               ((int)(c.b * 255)).ToString("X2") +
               ((int)(c.a * 255)).ToString("X2");
    }
}
