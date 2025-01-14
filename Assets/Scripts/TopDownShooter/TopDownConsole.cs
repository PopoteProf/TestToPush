using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class TopDownConsole  : MonoBehaviour
{

    [SerializeField] private Stat _stat;
    
    [SerializeField, ColorUsage(true , true )]private Color _OffLineColor=new Color(1,0.1f,0.1f);
    [SerializeField, ColorUsage(true , true )]private Color _OnLineColor=new Color(0,2,0.5f);
    [SerializeField, ColorUsage(true , true )]private Color _ActivetedColor =new Color(0,0,2);
    [SerializeField] private MeshRenderer[] _meshRenderersForColors;
    public UnityEvent OnInteract;

    private TopDownShooterControler _player;
    public enum Stat {
        Offline, OnLine ,Activated    
    }

    private void Start() {
        UpdateColor();
    }

    private void OnTriggerEnter(Collider other) {
        if (_stat == Stat.OnLine && other.CompareTag("Player")) {
            _player =other.GetComponent<TopDownShooterControler>();
            _player.SetConsole(this);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (_stat == Stat.OnLine && other.CompareTag("Player")) {
            _player.SetConsole(this, false);
        }
    }

    public void ExitConsole()
    {
        
    }

    public void Intract() {
        _stat = Stat.Activated;
        if (_player!=null)_player.SetConsole(this, false);
        UpdateColor();
        
        OnInteract.Invoke();
    }

    private void UpdateColor() {
        Color col = _OffLineColor;
        switch (_stat) {
            case Stat.Offline:col = _OffLineColor; break;
            case Stat.OnLine:col = _OnLineColor; break;
            case Stat.Activated:col = _ActivetedColor; break;
            default: throw new ArgumentOutOfRangeException();
        }

        foreach (var meshRenderer in _meshRenderersForColors) {
            meshRenderer.material.color = col;
        }
    }

    public void ChangeStat(TopDownConsole.Stat stat) {
        _stat = stat;
        UpdateColor();
    }
}