/*

The MIT License (MIT)

Copyright (c) 2015-2017 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

using UnityEngine.Playables;
using UnityStandardAssets.Characters.ThirdPerson;
using System;

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

    [Tooltip("Turns off during dialogue options")]
    public ThirdPersonUserControl playerControl;

    //Zac events
    public delegate void DialogueAction(string name);
    public event DialogueAction OnDialogueStart;
    public event DialogueAction OnDialogueEnd;

    public Text charName;
    private object text;

    void Awake()
    {
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

            //Name
            string finalLine = line.text;
            var splitline = line.text.Split(':');
            if (splitline.Length > 1)
            {
                charName.text = Capitalize(splitline[0]);
                finalLine = splitline[1].Substring(1); //Remove space
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

        playerControl.enabled = false; //Take away player control

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

        //Give player back control
        playerControl.enabled = true;

        //Reset playable
        GetComponent<PlayableDirector>().time = 0;
        GetComponent<PlayableDirector>().Pause();
    }

    /// Called by buttons to make a selection.
    public void SetOption(int selectedOption)
    {
        // Call the delegate to tell the dialogue system that we've
        // selected an option.
        SetSelectedOption(selectedOption); //null ref here sometimes, to do with my selection hackjob question mark????

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
        

        yield break;
    }

    /// Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete(string startNode)
    {
        //Debug.Log("Complete!");

        //Clear out text
        lineText.text = "";

        // Hide the dialogue interface.
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);

        //Send out event
        if (OnDialogueEnd != null)
            OnDialogueEnd(startNode);

        yield break;
    }

    string Capitalize(string s)
    {
        s = s.ToLower();
        return char.ToUpper(s[0]) + s.Substring(1);
    }

}

