using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using UnityEngine.Rendering.PostProcessing;

public class HeadsUpDisplay : MonoBehaviour
{
    public float BarFillRate = 0.05f;
    public string InteractionText = "InteractionPrompt";
    public string WarningText = "WarningText";
    public string TutorialText = "TutorialText";

    public PostProcessProfile PlayerProfile;
    public PostProcessProfile GhostProfile;

    private float warningResetTime;

    private Dictionary<string, Text> texts; // Dictionary of text contents on HUD
    private Image sunBar, waterBar, wiltBar;
    private Text wiltText;
    private Stack<string> promptTextStack;
    private Color wiltBarColor, wiltTextColor;

    PlayerState PS;
    GhostManager GM;
    PostProcessVolume PPV;

    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponent<PlayerState>();
        GM = GetComponent<GhostManager>();
        PPV = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessVolume>();
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
        wiltText = texts["WiltText"];
        wiltBarColor = wiltBar.color;
        wiltTextColor = wiltText.color;

        HideWiltBar();
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
            wiltBar.fillAmount = 1f - ((Time.time - GM.startTime) / GM.duration);
            wiltText.text = ((int)(GM.duration - Time.time + GM.startTime)).ToString();

            if ((GM.duration - Time.time + GM.startTime) < 5f)
            {
                if ((int)((GM.duration - Time.time + GM.startTime) * 2) % 2 == 0)
                {
                    wiltBar.color = Color.red;
                    wiltText.color = Color.red;
                }
                else
                {
                    wiltBar.color = wiltBarColor;
                    wiltText.color = wiltTextColor;
                }
            }
        }

        if (Time.time - warningResetTime > 1.5f)
        {
            texts[WarningText].text = "";
        }
    }

    public void SetTutorial(string text)
    {
        texts[TutorialText].text = text;
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
        wiltBar.transform.parent.gameObject.SetActive(true);
        wiltText.color = wiltTextColor;
        wiltBar.color = wiltBarColor;
        wiltText.text = "";
        wiltBar.fillAmount = 1f;
    }

    public void HideWiltBar()
    {
        wiltBar.transform.parent.gameObject.SetActive(false);
    }

    public void PlayerView()
    {
        PPV.profile = PlayerProfile;
        HideWiltBar();
        sunBar.transform.parent.gameObject.SetActive(true);
        waterBar.transform.parent.gameObject.SetActive(true);
    }

    public void GhostView()
    {
        PPV.profile = GhostProfile;
        ShowWiltBar();
        sunBar.transform.parent.gameObject.SetActive(false);
        waterBar.transform.parent.gameObject.SetActive(false);
    }
}
