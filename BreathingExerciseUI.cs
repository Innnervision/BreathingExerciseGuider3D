using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BreathingExerciseUI : MonoBehaviour
{
    public Text instructionText;
    public Text motivationalText;
    public Slider progressBar;
    public Button startButton;
    public Button stopButton;
    public Button changeEnvironmentButton;
    public Image breathingImage;
    public Material[] skyboxMaterials;
    public float skyboxTransitionDuration = 2f; // Duration of the skybox change

    private bool isBreathing;
    private float cycleProgress;
    private string[] motivationalMessages =
    {
        "Relax, you're doing great!",
        "Keep breathing to reduce stress!",
        "Feel the calmness take over your body."
    };

    private enum ExerciseType
    {
        RegularBreathing,
        FourSevenEightBreathing,
        BoxBreathing
    }

    private ExerciseType currentExercise = ExerciseType.RegularBreathing;

    // Instruction sets for different breathing exercises
    private string[] regularBreathingInstructions =
    {
        "Inhale deeply, hold for a few seconds, then exhale.",
        "Hold your breath for a few seconds.",
        "Exhale and relax."
    };

    private string[] fourSevenEightInstructions =
    {
        "Inhale for 4 seconds...",
        "Hold your breath for 7 seconds...",
        "Exhale slowly for 8 seconds..."
    };

    private string[] boxBreathingInstructions =
    {
        "Inhale for 4 seconds...",
        "Hold your breath for 4 seconds...",
        "Exhale for 4 seconds...",
        "Hold exhalation for 4 seconds..."
    };

    private void Start()
    {
        startButton.onClick.AddListener(StartBreathing);
        stopButton.onClick.AddListener(StopBreathing);
        changeEnvironmentButton.onClick.AddListener(ChangeEnvironment);

        ResetUI();
    }

    private void Update()
    {
        if (isBreathing)
        {
            UpdateBreathingCycle();
        }
    }

    private void ResetUI()
    {
        progressBar.value = 0;
        instructionText.text = "";
        motivationalText.text = "Get ready to start your breathing exercise!";
        breathingImage.transform.localScale = Vector3.one;
    }

    private void StartBreathing()
    {
        isBreathing = true;
        cycleProgress = 0;
        ShowMotivationalMessage();
        StartCoroutine(BreathingExerciseRoutine());
    }

    private void StopBreathing()
    {
        isBreathing = false;
        StopAllCoroutines();
        ResetUI();
    }

    private void UpdateBreathingCycle()
    {
        cycleProgress += Time.deltaTime;

        float progressRatio = cycleProgress / 8f;
        progressBar.value = progressRatio;

        float scale = Mathf.Lerp(1f, 1.2f, Mathf.PingPong(cycleProgress * 2, 1));
        breathingImage.transform.localScale = new Vector3(scale, scale, scale);
    }

    private IEnumerator BreathingExerciseRoutine()
    {
        switch (currentExercise)
        {
            case ExerciseType.RegularBreathing:
                yield return StartCoroutine(RegularBreathingExercise());
                break;
            case ExerciseType.FourSevenEightBreathing:
                yield return StartCoroutine(FourSevenEightBreathingExercise());
                break;
            case ExerciseType.BoxBreathing:
                yield return StartCoroutine(BoxBreathingExercise());
                break;
        }

        ShowMotivationalMessage();
        ResetUI();
    }

    private IEnumerator RegularBreathingExercise()
    {
        for (int i = 0; i < 4; i++)
        {
            ShowInstruction(regularBreathingInstructions[0]);
            yield return new WaitForSeconds(4f);

            ShowInstruction(regularBreathingInstructions[1]);
            yield return new WaitForSeconds(4f);

            ShowInstruction(regularBreathingInstructions[2]);
            yield return new WaitForSeconds(4f);
        }
    }

    private IEnumerator FourSevenEightBreathingExercise()
    {
        for (int i = 0; i < 4; i++)
        {
            ShowInstruction(fourSevenEightInstructions[0]);
            yield return new WaitForSeconds(4f);

            ShowInstruction(fourSevenEightInstructions[1]);
            yield return new WaitForSeconds(7f);

            ShowInstruction(fourSevenEightInstructions[2]);
            yield return new WaitForSeconds(8f);
        }
    }

    private IEnumerator BoxBreathingExercise()
    {
        for (int i = 0; i < 4; i++)
        {
            ShowInstruction(boxBreathingInstructions[0]);
            yield return new WaitForSeconds(4f);

            ShowInstruction(boxBreathingInstructions[1]);
            yield return new WaitForSeconds(4f);

            ShowInstruction(boxBreathingInstructions[2]);
            yield return new WaitForSeconds(4f);

            ShowInstruction(boxBreathingInstructions[3]);
            yield return new WaitForSeconds(4f);
        }
    }

    private void ShowInstruction(string message)
    {
        instructionText.text = message;
        StartCoroutine(AnimateText(instructionText)); // Animate the text to make it more attractive
    }

    private void ShowMotivationalMessage()
    {
        int randomMessageIndex = Random.Range(0, motivationalMessages.Length);
        motivationalText.text = motivationalMessages[randomMessageIndex];
        StartCoroutine(AnimateText(motivationalText)); // Animate motivational text
    }

    // Coroutine to smoothly animate text size and color
    private IEnumerator AnimateText(Text textComponent)
    {
        float duration = 1f;
        Color originalColor = textComponent.color;
        Vector3 originalScale = textComponent.transform.localScale;
        textComponent.color = Color.yellow; // Eye-catching color

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float scale = Mathf.Lerp(1f, 1.2f, t / duration);
            textComponent.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        yield return new WaitForSeconds(1f); // Hold the animation for 1 second

        // Reset to original color and scale
        textComponent.color = originalColor;
        textComponent.transform.localScale = originalScale;
    }

    private void ChangeEnvironment()
    {
        if (skyboxMaterials.Length > 0)
        {
            changeEnvironmentButton.interactable = false; // Disable button during skybox change
            StartCoroutine(SmoothSkyboxTransition());
        }
    }

    // Coroutine to smoothly transition between skyboxes
    private IEnumerator SmoothSkyboxTransition()
    {
        Material currentSkybox = RenderSettings.skybox;
        int randomIndex = Random.Range(0, skyboxMaterials.Length);
        Material nextSkybox = skyboxMaterials[randomIndex];

        float duration = skyboxTransitionDuration;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            RenderSettings.skybox.Lerp(currentSkybox, nextSkybox, t / duration);
            yield return null;
        }

        RenderSettings.skybox = nextSkybox; // Ensure the transition completes
        changeEnvironmentButton.interactable = true; // Re-enable button
        ShowMotivationalMessage(); // Show motivational message after changing environment
    }
}
