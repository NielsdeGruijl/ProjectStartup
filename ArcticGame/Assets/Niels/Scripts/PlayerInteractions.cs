using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private UIManager manager;

    [Header("fishing game")]
    [SerializeField] private GameObject fishingGame;
    [SerializeField] private GameObject fish;
    [SerializeField] private GameObject catchArea;

    private GameObject animal;

    private RectTransform fishRect;

    private float totalFish = 0;
    private float fishSpeed = 100;

    public bool mounted = false;
    private bool bobBefriended = false;
    private bool inAnimalRange = false;
    private bool inWaterRange = false;
    private bool catchingFish = false;

    private void Start()
    {
        fishingGame.SetActive(false);
        fishRect = fishingGame.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (inAnimalRange || inWaterRange || mounted)
            manager.displayText1 = true;
        else
            manager.displayText1 = false;

        if (inAnimalRange)
            AnimalInteraction();
        if (inWaterRange)
        {
            manager.SetInteractionText("Press F to fish");
            if (Input.GetKeyUp(KeyCode.F))
            {
                catchingFish = true;
            }
        }

        if (catchingFish)
        {
            CatchFish();
        }
            

        if (mounted)
        {
            manager.displayText1 = true;
            manager.SetInteractionText("Press F to dismount");
        }

        if (Input.GetKeyUp(KeyCode.F) && mounted)
        {
            transform.SetParent(null);
            mounted = false;
            Debug.Log("You unmounted Po");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PolarBear"))
        {
            Debug.Log("Press F to befriend Po");
            animal = other.gameObject;
            inAnimalRange = true;
        }
        if (other.CompareTag("fish"))
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

        if (other.CompareTag("fish"))
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
                Debug.Log("You mounted Po");
                StartCoroutine(wait());
            }
            if (Input.GetKeyUp(KeyCode.F) && !bobBefriended && !mounted)
            {
                if(totalFish > 0)
                {
                    manager.SetFeedbackText("Po likes you");
                    manager.displayFeedback1 = true;
                    totalFish--;
                    manager.AddFish(totalFish);
                    Debug.Log("You befriended Po");
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
        manager.SetInteractionText("Press SpaceBar to catch");
        fishingGame.SetActive(true);

        MoveFish();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (fish.transform.localPosition.x >= catchArea.transform.localPosition.x)
            {
                manager.SetFeedbackText("You caught a fish!");
                manager.displayFeedback1 = true;
                totalFish++;
                manager.AddFish(totalFish);
            }
            else
            {
                manager.SetFeedbackText("You missed!");
                manager.displayFeedback1 = true;
            }

            fish.transform.localPosition = Vector3.zero;
            fishingGame.SetActive(false);
            catchingFish = false;
        }
    }

    private void MoveFish()
    {
        float multiplier = 1 + ((fish.transform.localPosition.x / fishRect.rect.width) * 5);
        fish.transform.Translate((Vector3.right * fishSpeed * multiplier) * Time.deltaTime);

        if (fish.transform.localPosition.x >= fishRect.rect.width || fish.transform.localPosition.x <= 0)
        {
            fishSpeed *= -1;
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("mounted set to True");
        mounted = true;
    }
}
