using UnityEngine;
using UnityEngine.UI;

public class TopDownUIHpBar : MonoBehaviour
{
    [SerializeField] private Image _impFill;

    private void Start() {
        GameStaticManager.OnPlayerHpChange+= GameStaticManagerOnOnPlayerHpChange;
    }

    private void GameStaticManagerOnOnPlayerHpChange(object sender, float e) {
        _impFill.fillAmount = e;
    }
}

