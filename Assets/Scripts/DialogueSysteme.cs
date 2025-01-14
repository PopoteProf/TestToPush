using System;
using Cinemachine;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class DialogueSysteme : MonoBehaviour
{
    [SerializeField] private DialogueElement[] _dialogueElements;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Transform _playerPos;
    [SerializeField] private float _setUpTime =1;
    [SerializeField] private DialoguePanel _npcDialoguePanel;
    [SerializeField] private DialoguePanel _playerDialoguePanel;
    [SerializeField] private Transform _playerCamTarget;
    [SerializeField] private Transform _npcCamTarget;
    [SerializeField] private Animator _npcAnimator;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;

    public bool IsInDialogue;
    private int _dialogueIndex = 0;
    private PlayerControler _player;
    private bool _doSetup;


    public void Update() {
        if (!IsInDialogue) return;
        if (Input.GetButtonDown("Fire1")) {
            _dialogueIndex++;
            PlayerNewDialogue();
        }
    }

    private void PlayerNewDialogue() {
        if (_dialogueElements.Length <= _dialogueIndex) {
            EndDialogue();
            return;
        }

        DialogueElement dialogueElement = _dialogueElements[_dialogueIndex];

        if (dialogueElement.NPCTalking) {
            _playerDialoguePanel.ClosePanel();
            _npcDialoguePanel.DisplayText(dialogueElement.Dialogue);
            _cameraTarget.position = _npcCamTarget.position;
            _npcAnimator.SetBool("IsTalking", true);
            _player.SetPlayerIsTalking(false);

            
        }
        else {
            _npcDialoguePanel.ClosePanel();
            _playerDialoguePanel.DisplayText(dialogueElement.Dialogue);
            _cameraTarget.position = _playerCamTarget.position;
            _npcAnimator.SetBool("IsTalking",false);
            _player.SetPlayerIsTalking(true);
        }
        if (!dialogueElement._AudioElement.IsNull)
        {
            PlayDialogueSound(dialogueElement._AudioElement);
        }
    }

    private void PlayDialogueSound(AudioElement ae)
    {
        AudioSource audioSource = transform.AddComponent<AudioSource>();
        audioSource.clip = ae.GetSound();
        audioSource.pitch = ae.GetPitch();
        audioSource.volume = ae.Volume;
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length+1);
    }

    private void EndDialogue() {
        _npcAnimator.SetBool("IsTalking",false);
        _player.SetPlayerIsTalking(false);
        IsInDialogue = false;
        _playerDialoguePanel.ClosePanel();
        _npcDialoguePanel.ClosePanel();
        _virtualCamera.enabled = false;
        _player.EndDialogue();
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")&&!_doSetup) {
            StartSetupDialogue(other.GetComponent<PlayerControler>());
        }
    }

    private void StartSetupDialogue(PlayerControler player) {
        player.SetUpForDialogue(_playerPos.position, _setUpTime, _cameraTarget.position);
        _virtualCamera.enabled = true;
        _playerCamTarget = player.DialoguePlayerCamTransform;
        _player = player;
        Invoke("StartDialogue", _setUpTime);
        _doSetup = true;
        Debug.Log("Do Dialogue Setup");
    }

    private void StartDialogue() {
        IsInDialogue = true;
        PlayerNewDialogue();
        _doSetup = false;
    }
}

[Serializable]
public struct DialogueElement {
    [TextArea] public string Dialogue;
    //public Transform CamTarget;
    public bool NPCTalking;
    public AudioElement _AudioElement;
}