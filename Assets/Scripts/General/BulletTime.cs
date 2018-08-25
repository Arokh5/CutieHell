using UnityEngine;

public class BulletTime : MonoBehaviour {

    public static BulletTime instance;
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 0.5f;
    public float slowdownIncreaseTime = 0.5f;
    private float timeOnSlowdown = 0.0f;

    private bool inBulletTime = false;

    private float currentTimeScale
    {
        get
        {
            return TimeManager.instance.GetTimeScale();
        }
    }

	void Awake ()
    {
        if (instance == null)
            instance = this;
    }
	
	void Update ()
    {
        if (inBulletTime && !TimeManager.instance.IsTimeFrozen())
        {
            if (currentTimeScale != 1.0f)
            {
                if (timeOnSlowdown < slowdownLength)
                {
                    timeOnSlowdown += Time.unscaledDeltaTime;
                    TimeManager.instance.SetTimeScale(Mathf.Clamp(currentTimeScale, 0.0f, 1.0f));
                    Time.fixedDeltaTime = currentTimeScale * 0.02f;
                }
                else if (timeOnSlowdown < slowdownIncreaseTime + slowdownLength)
                {
                    timeOnSlowdown += Time.unscaledDeltaTime;
                    float targetTimeScale = TimeManager.instance.GetTimeScale();
                    targetTimeScale += (1f / slowdownIncreaseTime) * Time.unscaledDeltaTime;
                    targetTimeScale = Mathf.Clamp(targetTimeScale, 0.0f, 1.0f);
                    if (targetTimeScale != 1.0f)
                        TimeManager.instance.SetTimeScale(targetTimeScale);
                    else
                        TimeManager.instance.RestoreTimeScale();
                    Time.fixedDeltaTime = targetTimeScale * 0.02f;
                }
                else
                {
                    TimeManager.instance.RestoreTimeScale();
                    Time.fixedDeltaTime = currentTimeScale * 0.02f;
                    inBulletTime = false;
                }
            }
            else
            {
                TimeManager.instance.RestoreTimeScale();
                Time.fixedDeltaTime = currentTimeScale * 0.02f;
                inBulletTime = false;
            }
        }
    }

    public void DoSlowmotion(float _slowdownFactor = 0.05f, float _slowdownLength = 0.5f, float _slowdownIncreaseTime = 0.15f)
    {
        inBulletTime = true;
        TimeManager.instance.SetTimeScale(_slowdownFactor);
        Time.fixedDeltaTime = currentTimeScale * 0.02f;
        slowdownLength = _slowdownLength;
        slowdownIncreaseTime = _slowdownIncreaseTime;
        timeOnSlowdown = 0.0f;
    }
}
