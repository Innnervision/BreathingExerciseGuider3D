using UnityEngine;
using UnityEngine.UI;

public class YogaAnimationController : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component

    // Buttons for different yoga poses
    public Button pikeWalkButton;
    public Button administeringCprButton;
    public Button pushUpButton;
    public Button jumpPushUpButton;
    public Button airSquatButton;
    public Button pistolButton;
    public Button armStretchingButton;
    public Button situpsButton;

    private void Start()
    {
        // Add listeners to buttons to trigger animations
        pikeWalkButton.onClick.AddListener(() => PlayAnimation("Pike Walk"));
        administeringCprButton.onClick.AddListener(() => PlayAnimation("Talking"));
        pushUpButton.onClick.AddListener(() => PlayAnimation("Idle To Push Up"));
        jumpPushUpButton.onClick.AddListener(() => PlayAnimation("Jump Push Up"));
        airSquatButton.onClick.AddListener(() => PlayAnimation("Air Squat"));
        pistolButton.onClick.AddListener(() => PlayAnimation("Pistol"));
        armStretchingButton.onClick.AddListener(() => PlayAnimation("Arm Stretching"));
        situpsButton.onClick.AddListener(() => PlayAnimation("Neck Stretching"));
    }

    private void PlayAnimation(string animationName)
    {
        transform.Rotate(Vector3.left, 10);
        // Play the specified animation only if breathing exercises aren't active
        BreathingGuide breathingGuide = GetComponent<BreathingGuide>();
        if (!breathingGuide.IsBreathing())
        {
            animator.Play(animationName);
        }
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
