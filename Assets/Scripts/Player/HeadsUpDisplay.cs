using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour
{
    private Dictionary<string, Text> texts; // Dictionary of text contents on HUD
    
    
    // Start is called before the first frame update
    void Start()
    {
        texts = new Dictionary<string, Text>();
        List<Text> children = new List<Text>(GetComponentsInChildren<Text>());
        foreach (Text t in children)
        {
            texts.Add(t.name, t);
        }
    }

    public void SetText(string key, string text)
    {
        texts[key].text = text;
    }

    public void EnableText(string key)
    {
        texts[key].enabled = true;
    }

    public void DisableText(string key)
    {
        texts[key].enabled = false;
    }
}
