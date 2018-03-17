using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private int damage;

    private Transform enemy = null;
    private Vector3 hitPoint;
    private float time = 0;

    #endregion

    #region Properties

    public void SetEnemy(Transform enemy)
    {
        this.enemy = enemy;
    }

    public void SetHitPoint(Vector3 hitPoint)
    {
        this.hitPoint = hitPoint;
    }

    #endregion

    #region MonoBehaviour Methods

    private void Update()
    {
        SetOrbDirection();
        DestroyOrb();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == enemy)
        {
            other.GetComponent<AIEnemy>().TakeDamage(damage, AttackType.WEAK);
            Destroy(gameObject);
        }
    }

	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Private Methods
	
    private void SetOrbDirection()
    {
        if (enemy != null)
        {
            float yOffset = Mathf.Abs(enemy.position.y - hitPoint.y);
            Vector3 hitPos = new Vector3(enemy.position.x, enemy.position.y + yOffset, enemy.position.z);

            transform.position = Vector3.MoveTowards(transform.position, hitPos, attackSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.forward * attackSpeed * Time.deltaTime);
        }
    }

    private void DestroyOrb()
    {
        time += Time.deltaTime;

        if (time >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

	#endregion
}