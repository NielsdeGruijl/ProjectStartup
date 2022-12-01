using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class PlayerInteractions : MonoBehaviour
{
    [Header("General Stuff")]
    [SerializeField] private UIManager manager;
    [SerializeField] private PauseScript pause;
    [SerializeField] private DialogueRunner dRunner;

    [Header("Fishing Game")]
    [SerializeField] private GameObject fishingGame;
    [SerializeField] private GameObject fish;
    [SerializeField] private GameObject catchArea;
    [SerializeField] private GameObject fishCamera;

    private GameObject animal;

    private RectTransform fishRect;

    private float totalFish = 0;
    private float fishSpeed = 100;

    public bool mounted = false;
    private bool hasIntroduced = false;
    private bool bobBefriended = false;
    private bool inAnimalRange = false;
    private bool inWaterRange = false;
    private bool catchingFish = false;
    private bool throwSpear = false;

    private void Start()
    {
        fishingGame.SetActive(false);
        fishRect = fishingGame.GetComponent<RectTransform>();
        dRunner.Stop();
    }

    private void Update()
    {
        if (inAnimalRange || inWaterRange || mounted)
            manager.displayText1 = true;
        else
            manager.displayText1 = false;

        if (inAnimalRange)
        {
            if (!hasIntroduced)
                manager.SetInteractionText("Press F to talk");
            if (!bobBefriended && hasIntroduced)
                manager.SetInteractionText("Press F to feed");
            if (bobBefriended)
                manager.SetInteractionText("Press F to mount");
        }

        if (inWaterRange)
            manager.SetInteractionText("Press F to fish");

        if (catchingFish)
            CatchFish();

        if (!pause.gamePaused)
        {
            PlayerInput();
        }

        if (mounted)
        {
            manager.displayText1 = true;
            manager.SetInteractionText("Press F to dismount");
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

    private void PlayerInput()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            if (inAnimalRange)
            {
                AnimalInteraction();
            }
            if (inWaterRange)
            {
                catchingFish = true;
            }
            if (mounted)
            {
                transform.SetParent(null);
                mounted = false;
                Debug.Log("You unmounted Po");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (catchingFish)
            {
                throwSpear = true;
            }
        }
    }

    private void AnimalInteraction()
    {
        if (animal != null)
        {
            if(!hasIntroduced)
            {
                dRunner.StartDialogue("Polar");
                dRunner.Stop();
                hasIntroduced = true;
            }
            if (bobBefriended && !mounted)
            {
                Debug.Log("You mounted Po");
                StartCoroutine(wait());
            }
            if (!bobBefriended && !mounted && hasIntroduced)
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
        fishCamera.SetActive(true);

        MoveFish();

        if (throwSpear)
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
            fishCamera.SetActive(false);
            throwSpear = false;
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
