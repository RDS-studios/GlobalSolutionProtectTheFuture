using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class aids : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public int nextSceneIndexOffset = 1;

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("VideoPlayer not assigned and not found on this GameObject.");
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + nextSceneIndexOffset;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene in Build Settings.");
        }
    }
}
