using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

using UnityEngine.Playables;
using UnityStandardAssets.Characters.ThirdPerson;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

/// Displays dialogue lines to the player, and sends
/// user choices back to the dialogue system.
/// 
public class DialogueUI : Yarn.Unity.DialogueUIBehaviour
{

    /// The object that contains the dialogue and the options.
    /** This object will be enabled when conversation starts, and 
     * disabled when it ends.
     */
    public GameObject dialogueContainer;

    /// The UI element that displays lines
    public Text lineText;

    /// A UI element that appears after lines have finished appearing
    public GameObject continuePrompt;

    /// A delegate (ie a function-stored-in-a-variable) that
    /// we call to tell the dialogue system about what option
    /// the user selected
    private Yarn.OptionChooser SetSelectedOption;

    /// How quickly to show the text, in seconds per character
    [Tooltip("How quickly to show the text, in seconds per character")]
    public float textSpeed = 0.025f;

    /// The buttons that let the user choose an option
    public List<Button> optionButtons;

    [Tooltip("How long to wait for the options UI transition")]
    public float WaitForUITransition = 0.4f;
    
    [HideInInspector]
    public ThirdPersonUserControl playerControl;

    //Zac events
    public delegate void DialogueAction(string name);
    public event DialogueAction OnDialogueStart;
    public event DialogueAction OnDialogueEnd;
    public event DialogueAction OnTargetChanged;

    public Text charName;
    public string TalkingTo { get; set; }


    void Start()
    {
        //Debug.Log(UppercaseFirst("big Butts i cannot lie") + ".");

        // Start by hiding the container, line and option buttons
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);

        lineText.gameObject.SetActive(false);

        for(int i = 0; i < optionButtons.Count; i++)
        {
            var button = optionButtons[i];
            //yarn spinner no likey guess I'll die
            //button.onClick.AddListener(delegate { SetOption(i); }); //hook up button events automagically
            button.gameObject.SetActive(false);
        }

        // Hide the continue prompt if it exists
        if (continuePrompt != null)
            continuePrompt.SetActive(false);

        //Debug.Assert(GameObject.FindWithTag("Player"), "You silly goose! I couldn't find the player! Make sure they're tagged as 'Player' " +
        //                                               "and are in the scene! Winkey face! 0w0");

        //Fallback to combat player depending on which scene we're in
        GameObject playerObj;
        playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null) playerObj = CombatManager.instance.Party[0];
        
        playerControl = playerObj.GetComponent<ThirdPersonUserControl>();        
    }

    /// Show a line of dialogue, gradually
    public override IEnumerator RunLine(Yarn.Line line)
    {
        // Show the text
        lineText.gameObject.SetActive(true);

        if (textSpeed > 0.0f)
        {
            // Display the line one character at a time
            var stringBuilder = new StringBuilder();
            yield return null;

            //Extract name
            string finalLine = line.text;
            var splitline = line.text.Split(':');

            Debug.Assert(splitline.Length <= 2, "There's two colons in the node " + line.text);

            if (splitline.Length > 1)
            {
                var name = UppercaseFirst(splitline[0]);

                //Fire off event
                if (TalkingTo != name && OnTargetChanged != null)
                {
                    TalkingTo = name;
                    OnTargetChanged(TalkingTo);
                }

                charName.text = name;
                finalLine = splitline[1].Substring(1);  //Remove space on dialogue

                if (finalLine.Length >= 142)
                    Debug.Log("Line might be off screen! Line:" + finalLine);
            }

            foreach (char c in finalLine) 
            {
                stringBuilder.Append(c);
                lineText.text = stringBuilder.ToString();

                //Early exit
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    lineText.text = finalLine;
                    break;
                }
                else
                    yield return new WaitForSeconds(textSpeed);
            }
        }
        else
        {
            // Display the line immediately if textSpeed == 0
            lineText.text = line.text;
        }

        yield return null; //(HACKY HACK HACK) Skip a frame to stop early exit triggering continue

        // Show the 'press any key' prompt when done, if we have one
        if (continuePrompt != null)
            continuePrompt.SetActive(true);

        // Wait for any user input
        while (Input.GetKeyDown(KeyCode.Space) == false)
        {
            yield return null;
        }

        // Hide the text and prompt
        lineText.gameObject.SetActive(false);

        if (continuePrompt != null)
            continuePrompt.SetActive(false);

    }

    /// Show a list of options, and wait for the player to make a selection.
    public override IEnumerator RunOptions(Yarn.Options optionsCollection,
                                            Yarn.OptionChooser optionChooser)
    {
        //Make sure the text is visible
        lineText.gameObject.SetActive(true);

        // Do a little bit of safety checking
        if (optionsCollection.options.Count > optionButtons.Count)
        {
            Debug.LogWarning("There are more options to present than there are" +
                             "buttons to present them in. This will cause problems.");
        }

        // Display each option in a button, and make it visible
        int i = 0;
        foreach (var optionString in optionsCollection.options)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<Text>().text = optionString;
            i++;
        }

        // Record that we're using it
        SetSelectedOption = optionChooser;

        //Start UI animation
        GetComponent<PlayableDirector>().Play();

        yield return new WaitForSeconds(WaitForUITransition); //Wait for it to play
        GetComponent<PlayableDirector>().Pause(); //Stop on idle

        // Wait until the chooser has been used and then removed (see SetOption below)
        while (SetSelectedOption != null)
        {
            yield return null;
        }

        //Play the anim out
        GetComponent<PlayableDirector>().Play();

        //wait again
        yield return new WaitForSeconds(WaitForUITransition * 2);

        // Hide all the buttons
        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        //Reset playable
        GetComponent<PlayableDirector>().time = 0;
        GetComponent<PlayableDirector>().Pause();
    }

    /// Called by buttons to make a selection.
    public void SetOption(int selectedOption)
    {
        // Call the delegate to tell the dialogue system that we've
        // selected an option.
        SetSelectedOption(selectedOption); //tell zac if there was a null ref here

        // Now remove the delegate so that the loop in RunOptions will exit
        SetSelectedOption = null;
    }

    /// Run an internal command.
    public override IEnumerator RunCommand(Yarn.Command command)
    {
        // "Perform" the command
        Debug.Log("Command: " + command.text);

        yield break;
    }

    /// Called when the dialogue system has started running.
    public override IEnumerator DialogueStarted(string startNode)
    {
        //Debug.Log("Dialogue starting!");

        // Enable the dialogue controls.
        if (dialogueContainer != null)
            dialogueContainer.SetActive(true);
        
        //Send out event
        if (OnDialogueStart != null)
            OnDialogueStart(startNode);

        if(playerControl)
            playerControl.CanMove = false;

        yield break;
    }

    /// Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete(string startNode)
    {
        //Clear out text
        lineText.text = "";

        // Hide the dialogue interface.
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);

        //Send out event + reset talkingTo so ChangeTarget gets fired at the start of the next convo
        if (OnDialogueEnd != null)
        {
            TalkingTo = "";
            OnDialogueEnd(startNode);
            Debug.Log("Dialogue ended");
        }

        //Give player back control
        if(playerControl) playerControl.CanMove = true;

        yield break;
    }

    static string UppercaseFirst(string s)
    {
        s = s.ToLower(); //Sanitize input

        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }

        var words = s.Split(' ');
        string r = ""; 
        foreach(string w in words)
        {
            r += char.ToUpper(w[0]) + w.Substring(1) + " ";
        }

        r = r.Remove(r.Length - 1); //Remove trailing space
        return r;
    }

}

