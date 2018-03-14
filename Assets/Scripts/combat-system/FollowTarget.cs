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
    private float time = 0;

    #endregion

    #region Properties

    public void SetEnemy(Transform enemy)
    {
        this.enemy = enemy;
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
            transform.position = Vector3.MoveTowards(transform.position, enemy.position, attackSpeed * Time.deltaTime);
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