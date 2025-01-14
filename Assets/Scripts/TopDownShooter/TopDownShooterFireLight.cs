using UnityEngine;

public class TopDownShooterFireLight: MonoBehaviour {
    [SerializeField] private float _lifeTime = 0.1f;
    [SerializeField] private Light _light;

    private float _timer;

    public void Fire()
    {
        _light.enabled = true;
        _timer = 0;
    }

    private void Update() {
        if (_light.enabled)
        {
            _timer += Time.deltaTime;
            if (_timer >= _lifeTime)
            {
                _light.enabled = false;
            }
        }
    }
}