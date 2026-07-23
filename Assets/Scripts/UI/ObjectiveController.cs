// Written by Joshua Newsome

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ObjectiveController : MonoBehaviour
{
    public enum ObjectiveState
    {
        learnMovement,
        learnSprint,
        learnShooting,
        clearFirstLevel,
        learnJump,
        reachSecondLevel,
        avoidObstacles,
        reachThirdLevel,
        defeatBoss,
        complete
    };

    private PlayerInput playerInput;
    private PlayerController playerController;

    [Header("Objective State")]
    public ObjectiveState currentObjective;
    private ObjectiveState lastObjective;
    public string objectiveString;
    string partString = "";

    [Header("UI References")]
    public TextMeshProUGUI objectiveText;
    public RectTransform objectivesPanel;
    public Image objectiveFlash;

    [Header("Gameplay References")]
    public GameObject player;

    [Header("Timing")]
    public float delayObjectiveUpdate = 0f;
    public float flashDuration = 0.35f;
    public float shakeDuration = 0.2f;
    public float shakeAmount = 6f;

    private Vector2 originalPanelPosition;
    private Coroutine flashRoutine;

    void Start()
    {
        if (player == null)
        {
            PlayerController foundPlayerController = FindFirstObjectByType<PlayerController>();
            if (foundPlayerController != null)
            {
                player = foundPlayerController.gameObject;
            }
        }

        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            playerController = player.GetComponent<PlayerController>();
        }

        currentObjective = ObjectiveState.learnMovement;
        lastObjective = currentObjective;

        if (objectivesPanel != null)
        {
            originalPanelPosition = objectivesPanel.anchoredPosition;
        }

        if (objectiveFlash != null)
        {
            Color c = objectiveFlash.color;
            c.a = 0f;
            objectiveFlash.color = c;
        }

        UpdateObjectiveString();
        StartCoroutine(SetObjective(objectiveString));
    }

    void Update()
    {
        CheckState();
        if (currentObjective == ObjectiveState.clearFirstLevel)
        {
            UpdateObjectiveString();
            objectiveText.text = objectiveString;
        }

        if (currentObjective != lastObjective)
        {
            lastObjective = currentObjective;
            UpdateObjectiveString();
            StartCoroutine(SetObjective(objectiveString));
        }
    }

    void CheckState()
    {
        switch (currentObjective)
        {
            case ObjectiveState.learnMovement:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || IsActionPressed("Move"))
                {
                    currentObjective = ObjectiveState.learnSprint;
                }
                break;

            case ObjectiveState.learnSprint:
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || IsActionPressed("Sprint"))
                {
                    currentObjective = ObjectiveState.learnShooting;
                }
                break;

            case ObjectiveState.learnShooting:
                if (Input.GetButton("Fire1") || IsActionPressed("Attack"))
                {
                    currentObjective = ObjectiveState.clearFirstLevel;
                }
                break;

            case ObjectiveState.clearFirstLevel:
                if (playerController != null && playerController.partsCollected >= 3)
                {
                    currentObjective = ObjectiveState.learnJump;
                }
                break;

            case ObjectiveState.learnJump:
                if (Input.GetKeyDown(KeyCode.Space) || IsActionPressed("Jump"))
                {
                    currentObjective = ObjectiveState.reachSecondLevel;
                }
                break;

            case ObjectiveState.reachSecondLevel:
                if (player != null && player.transform.position.y > 13f)
                {
                    currentObjective = ObjectiveState.avoidObstacles;
                }
                break;

            case ObjectiveState.avoidObstacles:
                if (player != null && player.transform.position.y > 38f)
                {
                    currentObjective = ObjectiveState.reachThirdLevel;
                }
                break;

            case ObjectiveState.reachThirdLevel:
                if (player != null && player.transform.position.y > 51f)
                {
                    currentObjective = ObjectiveState.defeatBoss;
                }
                break;

            case ObjectiveState.defeatBoss:
                if (GameObject.Find("Boss") == null)
                {
                    currentObjective = ObjectiveState.complete;
                }
                break;

            case ObjectiveState.complete:
                break;
        }
    }

    void UpdateObjectiveString()
    {
        switch (currentObjective)
        {
            case ObjectiveState.learnMovement:
                objectiveString = "Objective: Use mouse and WASD to move";
                break;
            case ObjectiveState.learnSprint:
                objectiveString = "Objective: Hold Shift to sprint";
                break;
            case ObjectiveState.learnShooting:
                objectiveString = "Objective: Use left click to shoot";
                break;
            case ObjectiveState.clearFirstLevel:
                int collectedParts = playerController != null ? playerController.partsCollected : 0;
                int remainingParts = 3 - collectedParts;
                if (remainingParts == 1)
                {
                    partString = "last jump part";
                } else {
                    partString = "" + remainingParts + " more jump parts"; 
                }
                objectiveString = "Objective: Destroy enemies and collect " + partString;
                break;
            case ObjectiveState.learnJump:
                objectiveString = "Objective: Use Spacebar to jump";
                break;
            case ObjectiveState.reachSecondLevel:
                objectiveString = "Objective: Reach the second level";
                break;
            case ObjectiveState.avoidObstacles:
                objectiveString = "Objective: Avoid the obstacles";
                break;
            case ObjectiveState.reachThirdLevel:
                objectiveString = "Objective: Reach the final level";
                break;
            case ObjectiveState.defeatBoss:
                objectiveString = "Objective: Defeat the boss";
                break;
            case ObjectiveState.complete:
                objectiveString = "Objective Complete: Escape successful";
                break;
        }
    }

    bool IsActionPressed(string actionName)
    {
        if (playerInput == null || playerInput.actions == null)
        {
            return false;
        }

        InputAction action = playerInput.actions[actionName];
        return action != null && action.IsPressed();
    }

    public IEnumerator SetObjective(string objective)
    {
        objectiveText.text = "Updating Objective...";
        PlayObjectiveEffect(); // flash

        yield return new WaitForSeconds(delayObjectiveUpdate);

        objectiveText.text = objective;
    }

    void PlayObjectiveEffect()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashAndShakeRoutine());
    }

    IEnumerator FlashAndShakeRoutine()
    {
        float timer = 0f;

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            float normalized = timer / flashDuration;

            if (objectiveFlash != null)
            {
                Color c = objectiveFlash.color;
                c.a = Mathf.Lerp(0.8f, 0f, normalized);
                objectiveFlash.color = c;
            }

            if (objectivesPanel != null && timer < shakeDuration)
            {
                Vector2 offset = Random.insideUnitCircle * shakeAmount * (1f - (timer / shakeDuration));
                objectivesPanel.anchoredPosition = originalPanelPosition + offset;
            }

            yield return null;
        }

        if (objectiveFlash != null)
        {
            Color c = objectiveFlash.color;
            c.a = 0f;
            objectiveFlash.color = c;
        }

        if (objectivesPanel != null)
        {
            objectivesPanel.anchoredPosition = originalPanelPosition;
        }

        flashRoutine = null;
    }
}