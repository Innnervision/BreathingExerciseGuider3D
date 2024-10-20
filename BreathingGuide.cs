using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BreathingGuide : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public Text guideText; // Reference to the UI Text component
    public Image breathIcon; // Reference to the breathing icon
    public Animator guideAnimator; // Reference to the Animator component
    public float followSpeed = 2f; // Speed at which the guide follows the player
    public float followDistance = 15f; // Distance to maintain in front of the player
    public float adviceFrequency = 30f; // How often to give upgrade advice (in seconds)
    public float humanBehaviorFrequency = 10f; // Time interval for human-like behaviors

    private float idleTimer = 0f; // Timer to track idle time
    private float adviceTimer = 0f; // Timer to track when to give advice
    private float behaviorTimer = 0f; // Timer to track when to do human-like behavior
    private bool isBreathing = false; // Flag to indicate if breathing exercises are active
    private bool isWalking = false; // Flag to track if the character is walking

    private void Start()
    {
        breathIcon.gameObject.SetActive(false);
        adviceTimer = adviceFrequency; // Initialize advice timer
        behaviorTimer = humanBehaviorFrequency; // Initialize behavior timer
    }

    private void Update()
    {
        FollowPlayer();

        // Reset the idle timer if mouse is clicked
        if (Input.GetMouseButtonDown(0))
        {
            breathIcon.gameObject.SetActive(false);
            idleTimer = 0f; // Reset the timer on mouse click
        }
        else
        {
            idleTimer += Time.deltaTime;

            // Start breathing exercises if idle time exceeds limit and not already breathing
            if (idleTimer >= 10f && !isBreathing)
            {
                breathIcon.gameObject.SetActive(true);
                StartCoroutine(StartBreathingExercises());
                idleTimer = float.MaxValue; // Prevent multiple starts
            }

            // Give upgrade advice after a set time
            adviceTimer += Time.deltaTime;
            if (adviceTimer >= adviceFrequency)
            {
                StartCoroutine(GiveAdvice());
                adviceTimer = 0f; // Reset the advice timer
            }

            // Trigger random human-like behaviors after a set time
            behaviorTimer += Time.deltaTime;
            if (behaviorTimer >= humanBehaviorFrequency)
            {
                PerformHumanLikeBehavior();
                behaviorTimer = 0f; // Reset behavior timer
            }
        }
    }

    private void FollowPlayer()
    {
        // Calculate target position in front of the player
        Vector3 targetPosition = player.transform.position + player.transform.forward * followDistance;
        targetPosition.y = transform.position.y; // Maintain Y position

        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > followDistance)
        {
            // Move towards the player if too far
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            guideAnimator.Play("isWalking"); // Trigger walk animation
            isWalking = true;
            // Rotate to face the player (only Y-axis)
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0; // Ignore Y-axis for rotation
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            // Stop walking when near the player
            if (isWalking)
            {
                guideAnimator.SetBool("isWalking", false); // Stop walk animation
                isWalking = false;
            }
        }

        // Rotate to face the player (only Y-axis)
       /* Vector3 direction = player.transform.position - transform.position;
        direction.y = 0; // Ignore Y-axis for rotation
        transform.rotation = Quaternion.LookRotation(direction);*/
    }

    private void PerformHumanLikeBehavior()
    {
        // Randomly trigger human-like behaviors (e.g., idle, talk, look around)
        int randomBehavior = Random.Range(0, 3); // Random number to choose behavior

        switch (randomBehavior)
        {
            case 0:
                guideAnimator.Play("Idle"); // Trigger idle animation
                break;
            case 1:
                guideAnimator.Play("No"); // Trigger look around animation
                break;
            case 2:
                guideAnimator.Play("Talking"); // Trigger talking animation
                StartCoroutine(ShowText("Just checking in! Stay focused and keep breathing."));
                break;
        }
    }

    private IEnumerator StartBreathingExercises()
    {
        isBreathing = true; // Set breathing flag

        yield return ShowText("Welcome to your breathing guide.");
        yield return ShowText("Let�s take a moment to focus on your breath.");
        yield return ShowText("Follow my instructions for a calming breathing exercise.");

        // Breathing exercise loop
        while (true)
        {
            yield return StartCoroutine(BreathIn());
            yield return StartCoroutine(BreathOut());
        }
    }

    private IEnumerator GiveAdvice()
    {
        guideAnimator.SetTrigger("Talk"); // Trigger talking animation
        guideText.text = "Remember to upgrade yourself: stay positive, set goals, and keep learning!";
        yield return new WaitForSeconds(5); // Display advice for 5 seconds
    }

    private IEnumerator ShowText(string message)
    {
        guideText.text = message; // Set the text
        yield return new WaitForSeconds(3); // Display the message for 3 seconds
    }

    private IEnumerator BreathIn()
    {
        guideText.text = "Inhale deeply through your nose... 1... 2... 3... 4...";
        breathIcon.color = Color.green; // Change icon color for inhale
        breathIcon.gameObject.SetActive(true); // Show icon during inhale
        yield return new WaitForSeconds(4); // Wait for inhalation duration
        guideText.text = "Hold that breath gently... 1... 2... 3...";
        yield return new WaitForSeconds(3); // Hold duration
    }

    private IEnumerator BreathOut()
    {
        guideText.text = "Now, slowly exhale through your mouth... 1... 2... 3... 4...";
        breathIcon.color = Color.red; // Change icon color for exhale
        yield return new WaitForSeconds(4); // Wait for exhalation duration
        guideText.text = "Release any tension as you exhale.";
        yield return new WaitForSeconds(3); // Pause for emphasis
        guideText.text = "Great job! Let's continue together.";
        yield return new WaitForSeconds(3); // Pause before next cycle

        breathIcon.gameObject.SetActive(false); // Hide icon after cycle
    }

    public bool IsBreathing()
    {
        return isBreathing; // Public method to check if breathing exercise is active
    }
}
