using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoController : MonoBehaviour
{
    public delegate void VoidCallback();

    #region Fields
    [SerializeField]
    private RenderTexture renderTexture;

    private VideoPlayer videoPlayer;
    private RawImage rawImage;
    private bool hasStarted = false;
    private bool hasFinished = false;
    private VoidCallback endCallback = null;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(renderTexture, "ERROR: Render Texture (TexnderTexture) not assigned for VideoController script in GameObject '" + gameObject.name + "'!");
        videoPlayer = GetComponent<VideoPlayer>();
        UnityEngine.Assertions.Assert.IsNotNull(videoPlayer, "ERROR: A VideoPlayer Component could not be found by VideoController in GameObject " + gameObject.name);
        rawImage = GetComponent<RawImage>();
        UnityEngine.Assertions.Assert.IsNotNull(rawImage, "ERROR: A RawImage Component could not be found by VideoController in GameObject " + gameObject.name);

        rawImage.enabled = false;
    }

    private void Update()
    {
        if (videoPlayer.isPrepared)
        {
            if (hasStarted && !rawImage.enabled && !hasFinished)
            {
                rawImage.enabled = true;
            }

            if (!hasStarted)
            {
                hasStarted = true;
                videoPlayer.Play();
            }

            if (Application.isFocused && hasStarted && !videoPlayer.isPlaying && !hasFinished)
            {
                Debug.Log("FINISHED");
                hasFinished = true;
                rawImage.enabled = false;
                if (endCallback != null)
                {
                    VoidCallback callback = endCallback;
                    endCallback = null;
                    callback();
                }
            }
        }
    }
    #endregion

    #region Public Methods
    public void StartVideoPlayback(VoidCallback onEndCallback = null)
    {
        endCallback = onEndCallback;
        videoPlayer.Prepare();
    }
    #endregion
}
