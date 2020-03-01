using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class HeadsUpDisplay : MonoBehaviour
{
    public float BarFillRate = 0.05f;
    public string InteractionText = "InteractionPrompt";
    public string WarningText = "WarningText";

    private float warningResetTime;

    private Dictionary<string, Text> texts; // Dictionary of text contents on HUD
    private Image sunBar, waterBar, wiltBar;
    private Stack<string> promptTextStack;

    PlayerState PS;
    GhostManager GM;

    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponent<PlayerState>();
        GM = GetComponent<GhostManager>();
        texts = new Dictionary<string, Text>();
        List<Text> children = new List<Text>(GetComponentsInChildren<Text>());
        promptTextStack = new Stack<string>();
        promptTextStack.Push(" ");
        foreach (Text t in children)
        {
            texts.Add(t.name, t);
            Debug.Log("HUD::Added: " + t.name);
        }
        sunBar = GameObject.Find("SunBarForeground").GetComponent<Image>();
        waterBar = GameObject.Find("WaterBarForeground").GetComponent<Image>();
        wiltBar = GameObject.Find("WiltBarForeground").GetComponent<Image>();
        warningResetTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (PS.waterMeter - waterBar.fillAmount > BarFillRate)
            waterBar.fillAmount += BarFillRate;
        else if (PS.waterMeter - waterBar.fillAmount < -BarFillRate)
            waterBar.fillAmount -= BarFillRate;
        else
            waterBar.fillAmount = PS.waterMeter;

        if (PS.sunMeter - sunBar.fillAmount > BarFillRate)
            sunBar.fillAmount += BarFillRate;
        else if (PS.sunMeter - sunBar.fillAmount < -BarFillRate)
            sunBar.fillAmount -= BarFillRate;
        else
            sunBar.fillAmount = PS.sunMeter;

        if (GM.isRecording)
        {
            if (!wiltBar.gameObject.activeSelf)
            {
                wiltBar.gameObject.SetActive(true);
            }
            wiltBar.fillAmount = 1f - ((Time.time - GM.startTime) / GM.duration);
        }
        else
        {
            if (wiltBar.gameObject.activeSelf)
            {
                wiltBar.gameObject.SetActive(false);
            }
        }

        if (Time.time - warningResetTime > 1.5f)
        {
            texts[WarningText].text = "";
        }
    }

    public void SetWarning(string text)
    {
        texts[WarningText].text = text;
        warningResetTime = Time.time;
    }

    public string GetText(string key)
    {
        return texts[key].text;
    }

    public void SetText(string key, string text)
    {
        texts[key].text = text;
    }

    public void PushPrompt(string text)
    {
        promptTextStack.Push(text);
        texts[InteractionText].text = text;
    }

    public void PopPrompt()
    {
        promptTextStack.Pop();
        texts[InteractionText].text = promptTextStack.Peek();
    }

    public void PopPromptOnMatch(string text)
    {
        if (text.Equals(promptTextStack.Peek()))
        {
            PopPrompt();
        }
    }

    public void ClearPrompt()
    {
        promptTextStack.Clear();
        PushPrompt(" ");
    }

    public void ShowText(string key)
    {
        texts[key].enabled = true;
    }

    public void HideText(string key)
    {
        texts[key].enabled = false;
    }

    public void ShowWiltBar()
    {
        wiltBar.enabled = true;
    }

    public void HideWiltBar()
    {
        wiltBar.enabled = false;
    }
}
