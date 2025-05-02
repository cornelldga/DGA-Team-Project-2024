using UnityEngine;

[RequireComponent(typeof(CustomerTypes))]
public class CustomerSoundManager : MonoBehaviour
{
  private CustomerTypes customerTypes;

  void Start()
  {
    customerTypes = GetComponent<CustomerTypes>();
  }

  public void PlayOrderCompleteSound(Vector3 position)
  {
    int randomIndex = Random.Range(1, 3);
    string sfx_id = customerTypes.GetRandomOrderCompleteSound() + randomIndex;
    AudioManager.Instance.PlaySoundAtPoint(sfx_id, position);
  }

  public void PlayHurtSound(Vector3 position)
  {
    int randomIndex = Random.Range(1, 3);
    string sfx_id = customerTypes.GetRandomHurtSound() + randomIndex;
    AudioManager.Instance.PlaySoundAtPoint(sfx_id, position);
  }

  public void PlayTakeOrderSound(Vector3 position)
  {
    int randomIndex = Random.Range(1, 3);
    string sfx_id = customerTypes.GetRandomTakeOrderSound() + randomIndex;
    AudioManager.Instance.PlaySoundAtPoint(sfx_id, position);
  }

  public void PlayOrderFailedSound(Vector3 position)
  {
    AudioManager.Instance.PlaySoundAtPoint("sfx_failorder", position);
  }
}