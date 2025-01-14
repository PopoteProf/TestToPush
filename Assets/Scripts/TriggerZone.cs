using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour {
   [SerializeField] private string tag = "Player";
   public UnityEvent OnTagTriggerEnter;
   private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Player")) {
         OnTagTriggerEnter.Invoke();
      }
   }
}