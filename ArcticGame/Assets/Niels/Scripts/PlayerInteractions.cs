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
    [SerializeField] private LineView lineView;

    [Header("Fishing Game")]
    [SerializeField] private GameObject fishingGame;
    [SerializeField] private GameObject fish;
    [SerializeField] private GameObject catchArea;
    private GameObject fishCamera;

    private GameObject animal;

    private RectTransform fishRect;

    private float totalFish = 0;
    private float fishSpeed = 100;

    public bool mounted = false;
    private bool nearPo = false;
    private bool nearWater = false;
    private bool nearBridge = false;
    private bool catchingFish = false;
    private bool throwSpear = false;

    private bool polar1 = false;
    private bool polar2 = false;
    private bool bridge1 = false;

    private void Start()
    {
        pause.gamePaused = true;

        if (mounted)
            pause.gamePaused = false;

        fishingGame.SetActive(false);
        fishRect = fishingGame.GetComponent<RectTransform>();
        //dRunner.Stop();
    }

    private void Update()
    {
        if (nearPo || nearWater || mounted || nearBridge)
            manager.displayText1 = true;
        else
            manager.displayText1 = false;

        if (nearPo)
        {
            if (!polar1 || !polar2)
                manager.SetInteractionText("Press F to talk");
/*            if (!befriended && polar1)
                manager.SetInteractionText("Press F to feed");*/
            if (polar2)
                manager.SetInteractionText("Press F to mount");
        }
        if (nearWater)
            manager.SetInteractionText("Press F to fish");
        if (nearBridge)
            manager.SetInteractionText("Press F to interact");

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
            animal = other.gameObject;
            nearPo = true;
        }
        if (other.CompareTag("fish"))
        {
            fishCamera = other.transform.Find("FishCam").gameObject;
            nearWater = true;
        }
        if (other.CompareTag("Bridge"))
        {
            nearBridge = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PolarBear"))
        {
            animal = null;
            nearPo = false;
        }
        if (other.CompareTag("fish"))
        {
            nearWater = false;
        }
        if (other.CompareTag("Bridge"))
        {
            nearBridge = false;
        }
    }

    private void PlayerInput()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            if (nearPo)
            {
                AnimalInteraction();
            }
            if (nearWater)
            {
                catchingFish = true;
            }
            if (nearBridge)
            {
                BridgeInteraction();
            }
            if (mounted)
            {
                transform.SetParent(null);
                mounted = false;
                //Debug.Log("You unmounted Po");
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
            if(!polar1 && bridge1)
            {
                pause.gamePaused = true;
                dRunner.StartDialogue("Polar");
                //dRunner.Stop();
                polar1 = true;
            }
            if (polar2 && !mounted)
            {
                //Debug.Log("You mounted Po");
                StartCoroutine(wait());
            }
            if (!polar2 && !mounted && polar1)
            {
                if(totalFish > 0)
                {
                    pause.gamePaused = true;
                    dRunner.StartDialogue("Polar2");
                    //dRunner.Stop();
/*                    manager.SetFeedbackText("Po likes you");
                    manager.displayFeedback1 = true;*/
                    totalFish--;
                    manager.AddFish(totalFish);
                    //Debug.Log("You befriended Po");
                    polar2 = true;
                }
                else
                {
                    manager.SetFeedbackText("Po wants some fish");
                    manager.displayFeedback1 = true;
                }

            }
        }
    }

    private void BridgeInteraction()
    {
        if (!bridge1)
        {
            pause.gamePaused = true;
            dRunner.StartDialogue("Bridge1");
            //dRunner.Stop();
            bridge1 = true;
        }
        if (bridge1 && polar2 && mounted)
        {

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

    [YarnCommand("ContinueGame")]
    public void ContinueGame()
    {
        pause.gamePaused = false;
        //Debug.Log("Game continued");
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
        //Debug.Log("mounted set to True");
        mounted = true;
    }
}
