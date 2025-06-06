using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip videoClipSecreto;
    public int nextSceneIndexOffset = 1;
    [SerializeField] bool specialCene = false;
     
    void Start()
    {
        if(PlayerPrefs.GetInt("savedTu", 0) == 1 && specialCene)
        {
            if (videoClipSecreto != null)
            {
                videoPlayer.clip = videoClipSecreto;
            }
            else
            {
                Debug.LogError("VideoClipSecreto is not assigned.");
            }
        }

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

        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCamera = Camera.main;
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
