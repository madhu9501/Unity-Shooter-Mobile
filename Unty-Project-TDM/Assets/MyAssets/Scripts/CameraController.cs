using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDM
{
    public class CameraController : MonoBehaviour
    {
        [Header("Cameras")]
        public GameObject TPCam;
        public GameObject AimCam;

        [Header("Canvas")]
        public GameObject TPCrossHairCanvas;
        public GameObject AimCrossHairCanvas;

        public void ThirdPersonView(){
            TPCam.SetActive(true);
            TPCrossHairCanvas.SetActive(true);
            AimCam.SetActive(false);
            AimCrossHairCanvas.SetActive(false);
        }

        public void AimView(){
            AimCam.SetActive(true);
            AimCrossHairCanvas.SetActive(true);
            TPCam.SetActive(false);
            TPCrossHairCanvas.SetActive(false);
        }
    }
}

