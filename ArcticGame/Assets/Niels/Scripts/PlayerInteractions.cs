using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private UIManager manager;
    private GameObject animal;

    private float totalFish = 0;

    public bool mounted = false;
    private bool bobBefriended = false;
    private bool inAnimalRange = false;
    private bool inWaterRange = false;

    private void Update()
    {
        if (inAnimalRange || inWaterRange || mounted)
            manager.displayText1 = true;
        else
            manager.displayText1 = false;

        if (inAnimalRange)
            AnimalInteraction();
        if (inWaterRange)
            CatchFish();

        if (mounted)
        {
            manager.displayText1 = true;
            manager.SetInteractionText("Press F to dismount");
        }

        if (Input.GetKeyUp(KeyCode.F) && mounted)
        {
            transform.SetParent(null);
            mounted = false;
            Debug.Log("You unmounted Bob");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PolarBear"))
        {
            Debug.Log("Press F to befriend Bob");
            animal = other.gameObject;
            inAnimalRange = true;
        }
        if (other.CompareTag("Water"))
        {
            inWaterRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PolarBear"))
        {
            animal = null;
            inAnimalRange = false;
        }

        if (other.CompareTag("Water"))
        {
            inWaterRange = false;
        }
    }

    private void AnimalInteraction()
    {
        if (animal != null)
        {
            if (!bobBefriended)
                manager.SetInteractionText("Press F to feed");
            if (bobBefriended)
                manager.SetInteractionText("Press F to mount");

            if (Input.GetKeyUp(KeyCode.F) && bobBefriended && !mounted)
            {
                Debug.Log("You mounted Bob");
                StartCoroutine(wait());
            }
            if (Input.GetKeyUp(KeyCode.F) && !bobBefriended && !mounted)
            {
                if(totalFish > 0)
                {
                    manager.SetFeedbackText("Po likes you");
                    manager.displayFeedback1 = true;
                    totalFish--;
                    Debug.Log("You befriended Bob");
                    bobBefriended = true;
                }
                else
                {
                    manager.SetFeedbackText("Po wants some fish");
                    manager.displayFeedback1 = true;
                }

            }
        }
    }

    private void CatchFish()
    {
        manager.SetInteractionText("Press F to fish");

        if (Input.GetKeyUp(KeyCode.F))
        {
            totalFish++;
            manager.AddFish(totalFish);
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("mounted set to True");
        mounted = true;
    }
}
