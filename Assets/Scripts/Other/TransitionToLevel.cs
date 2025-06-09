using UnityEngine;
using UnityEngine.Video;
using EasyTransition;
public class TransitionToLevel : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public TransitionSettings transition;
    public string SceneToTransition;
    public long framesBeforeEnd; // X frames before the end to trigger the function
    private bool hasTriggered = false;

    void Update()
    {
        if (videoPlayer.isPlaying && !hasTriggered)
        {
            long totalFrames = (long)videoPlayer.frameCount;
            long currentFrame = videoPlayer.frame;

            if (totalFrames > 0 && currentFrame >= totalFrames - framesBeforeEnd)
            {
                hasTriggered = true;
                OnFramesRemainingReached();
            }
        }
    }

    public void OnFramesRemainingReached()
    {
        TransitionManager.Instance().Transition(SceneToTransition, transition, 0f);
    }
}
