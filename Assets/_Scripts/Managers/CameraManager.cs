using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    private Coroutine activeSequence;
    private bool skipRequested;
    public bool isShifting;
    public static CameraManager Instance { get; private set; }
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private Camera mainCamera;
    [System.Serializable] public struct LabeledCamera{
        public string label;
        public CinemachineVirtualCamera virtualCamera;
    }
    [SerializeField] private List<LabeledCamera> _allCameras = new List<LabeledCamera>();

    [SerializeField] List<string> introSequnce = new();


    public LabeledCamera GetCameraByLabel(string label){
        return _allCameras.Find(cam => cam.label == label);
    }
    public void SwitchToVirtualCamera(string label){
        foreach (var cam in _allCameras){
            cam.virtualCamera.Priority = 0;
        }
        GetCameraByLabel(label).virtualCamera.Priority=100;
    }
    
    //fja prima sekvencu kamera koju aktivira
    public void SwitchToCameraSequence(List<string> cameraLabels){
        if (activeSequence != null){
            StopCoroutine(activeSequence);
        }
        activeSequence = StartCoroutine(PlayCameraSequence(cameraLabels));
    }
    private IEnumerator PlayCameraSequence(List<string> cameraLabels){
       foreach (string label in cameraLabels){
            SwitchToVirtualCamera(label);
            yield return new WaitForSeconds(0.2f); 
            yield return new WaitWhile(() => cinemachineBrain.IsBlending);
        }
        activeSequence = null;
    }
    void Awake(){
        Instance = this;
    }

    void Start(){
        //mySequence = new List<string> {"Intro1","Intro2","TerrariumMain"};
        SwitchToCameraSequence(introSequnce);
    }

    void Update(){
        if(isShifting == false && activeSequence == null)
        {
            isShifting = true;
            MenuManager.Instance.OpenMenu(MenuType.Title);
        }
    }

    /*public void StartGame()
    {
        SwitchToVirtualCamera("PlayCamera");
    }*/
}
