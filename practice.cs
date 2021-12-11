using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class practice : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI textObject;
    [SerializeField] GameObject nextExperiment;//Forward to the next experiment
    [SerializeField] AudioSource a0;
    [SerializeField] Material mA, mB;

    int playCount = 0;//[0, 3)
    int state = -2;//initial state of the system
    float startTime, durationTime;
    int currentIndex = 0;//Begin experiment at zeroth element, audio sources are being played currently
    // Start is called before the first frame update
    void Start()
    {
        //Somewhat silly as this class will be instanced and is not static, but won't ruin performance or afflict control
        //Documentation:
        //  https://docs.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/architecture/controllers-pointers-and-focus?view=mrtkunity-2021-05
        //  In usual fashion, have to find the namespace of this stuff in its own page because why not.
        Microsoft.MixedReality.Toolkit.Input.PointerUtils.SetHandRayPointerBehavior(Microsoft.MixedReality.Toolkit.Input.PointerBehavior.AlwaysOff);

        this.GetComponent<MeshRenderer>().material = mA;
    }

    private void resetWait(float duration)
    {
        startTime = 0.0f;
        durationTime = duration;
    }
    private bool waitFor()
    {
        startTime += Time.deltaTime;
        if (startTime >= durationTime)
            return true;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            //Countdown to ready participant
            case -2:
                {
                    //Welcome screen and wait for participant to be ready to start
                    textObject.text = "Welcome, a practice sound will be played, then the experiment will begin after a button press.";
                    state += 1;
                    break;
                }
            case -1:
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        state += 1;
                    }
                    break;
                }
            case 0:
                {
                    textObject.text = "3";
                    resetWait(2.0f);
                    state += 1;
                    break;
                }
            case 1:
                {
                    if (waitFor()) state += 1;
                    break;
                }
            case 2:
                {
                    textObject.text = "2";
                    resetWait(2.0f);
                    state += 1;
                    break;
                }
            case 3:
                {
                    if (waitFor()) state += 1;
                    break;
                }
            case 4:
                {
                    textObject.text = "1";
                    resetWait(2.0f);
                    state += 1;
                    break;
                }
            case 5:
                {
                    if (waitFor()) state += 1;
                    break;
                }
            //Perform a trial in the experiment
            case 6:
                {
                    if (playCount == 3)
                    {
                        textObject.text = "What location:\nFront, Back, Left, Right, Top or Bottom";
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            //The sound has played 3 times, wait for the participant response
                            //Once recorded, press the button to move to the next sound
                            state = 0;
                            playCount = 0;
                            currentIndex += 1;
                        }
                        break;
                    }
                    else
                    {
                        textObject.text = "";
                        a0.Play();
                        state += 1;
                    }
                    break;
                }
            case 7:
                {
                    if (!a0.isPlaying) state += 1;
                    else this.GetComponent<MeshRenderer>().material = mB;
                    break;
                }
            case 8:
                {
                    this.GetComponent<MeshRenderer>().material = mA;
                    resetWait(1.5f);
                    state += 1;
                    break;
                }
            case 9:
                {
                    if (waitFor())
                    {
                        playCount += 1;
                        state = 6;
                    }
                    break;
                }
        }
        if (currentIndex >= 1)
        {
            textObject.text = "";
            nextExperiment.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
