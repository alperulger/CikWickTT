using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{

private PlayerController _playerController;
    private void Awake()
    {
       _playerController=GetComponent<PlayerController>(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<ICollectible>( out var collectible))
        {
            collectible.Collect();
        }
      
        
    }
    void OnCollisionEnter(Collision collision)
    {
           Debug.Log("Collision en");
        if(collision.gameObject.TryGetComponent<IBoostable>(out var boostable))
        {
            boostable.Boost(_playerController);
        }
    }
   


}
