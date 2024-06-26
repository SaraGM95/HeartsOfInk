﻿using Assets.Scripts.Data.TutorialModels;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    private TutorialController tutorialController;
    private bool closeOnAccept;
    public Text txtTitle;
    public Text txtContent;
    public Button cancel;

    public void DisplayMessage(TutorialController tutorialController, string title, string content, bool closeOnAccept)
    {
        this.tutorialController = tutorialController;
        this.closeOnAccept = closeOnAccept;
        DisplayMsg(title, content);
    }

    public void DisplayDecision(TutorialController tutorialController, string title, string content, bool closeOnAccept)
    {
        this.tutorialController = tutorialController;
        this.closeOnAccept = closeOnAccept;
        cancel.gameObject.SetActive(true);
        DisplayMsg(title, content);
    }

    public void DisplayMessage(string title, string content, bool closeOnAccept)
    {
        this.closeOnAccept = closeOnAccept;
        DisplayMsg(title, content);
    }

    public void DisplayMessage(string title, string content)
    {
        closeOnAccept = true;
        DisplayMsg(title, content);
    }

    private void DisplayMsg(string title, string content)
    {
        txtTitle.text = title;
        txtContent.text = content;
        this.gameObject.SetActive(true);
        Debug.Log("Displaying message for title: " + title);
    }

    public void Accept()
    {
        cancel.gameObject.SetActive(false);

        if (closeOnAccept)
        {
            this.gameObject.SetActive(false);
        }
        else if (tutorialController != null)
        {
            TutorialStep tutorialStep = tutorialController.NextMessage();
            if (tutorialStep != null)
            {
                DisplayMessage(tutorialStep.Title[0].Value, tutorialStep.Content[0].Value, false);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("tutorialController is null on Accept");
        }
    }

    public void Cancel()
    {
        cancel.gameObject.SetActive(false);
        this.gameObject.SetActive(false);

        if (tutorialController != null)
        {
            tutorialController.DiscardTutorial();
        }
    }
}
