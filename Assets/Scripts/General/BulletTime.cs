using UnityEngine;

public class BulletTime : MonoBehaviour {

    public static BulletTime instance;
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 0.5f;
    public float slowdownIncreaseTime = 0.5f;
    private float timeOnSlowdown = 0.0f;
    private bool inBulletTime = false;

	void Awake ()
    {
        if (instance == null)
            instance = this;
    }
	
	void Update ()
    {
        if (inBulletTime)
        {
            if (!GameManager.instance.gameIsPaused && Time.timeScale != 1.0f)
            {
                if (timeOnSlowdown < slowdownLength)
                {
                    timeOnSlowdown += Time.unscaledDeltaTime;
                    Time.timeScale = Mathf.Clamp(Time.timeScale, 0.0f, 1.0f);
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                }
                else if (timeOnSlowdown < slowdownIncreaseTime + slowdownLength)
                {
                    timeOnSlowdown += Time.unscaledDeltaTime;
                    Time.timeScale += (1f / slowdownIncreaseTime) * Time.unscaledDeltaTime;
                    Time.timeScale = Mathf.Clamp(Time.timeScale, 0.0f, 1.0f);
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                }
                else
                {
                    Time.timeScale = 1.0f;
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                    inBulletTime = false;
                }
            }
        }
    }

    public void DoSlowmotion(float _slowdownFactor = 0.05f, float _slowdownLength = 0.5f, float _slowdownIncreaseTime = 0.15f)
    {
        inBulletTime = true;
        Time.timeScale = _slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        slowdownLength = _slowdownLength;
        slowdownIncreaseTime = _slowdownIncreaseTime;
        timeOnSlowdown = 0.0f;
    }
}
