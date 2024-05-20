using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{

    [SerializeField]
    private InputActionReference aimControl;
    [SerializeField]
    private int priorityBoostAmount = 10;
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private Canvas thirdPersonCanvas, aimCanvas;

    private void OnEnable()
    {
        aimControl.action.Enable();
    }

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        aimControl.action.performed += ctx => StartAim();
        aimControl.action.canceled += ctx => CancelAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        aimCanvas.gameObject.SetActive(true);
        thirdPersonCanvas.enabled = false;
        
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        thirdPersonCanvas.enabled = true;
        aimCanvas.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        aimControl.action.Disable();
    }
}
