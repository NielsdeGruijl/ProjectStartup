using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text interactionText;
    [SerializeField] private Text feedbackText;
    [SerializeField] private Text fishText;

    private float alpha;

    public bool displayText1 = false;
    public bool displayFeedback1 = false;

    private void Update()
    {
        if (!interactionText.enabled && displayText1)
            interactionText.enabled = true;
        if (interactionText.enabled && !displayText1)
            interactionText.enabled = false;

        if (!feedbackText.enabled && displayFeedback1)
            StartCoroutine(ShowFeedback());
    }

    public void SetInteractionText(string text)
    {
        interactionText.text = text;
        //Debug.Log("Interaction prompt changed");
    }

    public void SetFeedbackText(string text)
    {
        feedbackText.text = text;
    }

    public void AddFish(float totalFish)
    {
        fishText.text = "Fish: " + totalFish;
    }

    private IEnumerator ShowFeedback()
    {
        feedbackText.enabled = true;
        yield return new WaitForSeconds(1);
        displayFeedback1 = false;
        feedbackText.enabled = false;
    }
}
