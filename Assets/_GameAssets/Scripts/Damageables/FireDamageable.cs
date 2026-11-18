using UnityEngine;

public class FireDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float _force=10f;
    public void GiveDamage(Rigidbody playerRigidbody, Transform playerVisualTransform)
    {
     Destroy(gameObject);
     HealthManager.Instance.Damage(1);
     playerRigidbody.AddForce(-playerVisualTransform.forward*_force,ForceMode.Impulse);

    }

   
}
