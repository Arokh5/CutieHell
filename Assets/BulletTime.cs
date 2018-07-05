using UnityEngine;

public class BulletTime : MonoBehaviour {

    public static BulletTime instance;
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 0.5f;

	void Awake ()
    {
        if (instance == null)
            instance = this;
    }
	
	void Update ()
    {

        if (!GameManager.instance.gameIsPaused && Time.timeScale != 1.0f)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale,0.0f,1.0f);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }

    public void DoSlowmotion(float _slowdownFactor = 0.05f, float _slowdownLength = 0.5f)
    {
        Time.timeScale = _slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        slowdownLength = _slowdownLength;
    }
}
