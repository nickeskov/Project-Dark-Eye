using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

 class ClipplingEyes : MonoBehaviour
 {
     private VignetteAndChromaticAberration _vignetteScript;
     private float _clipplingTime = 0f;

     private bool _isClosed;

     // Use this for initialization
     void Start()
     {
         _vignetteScript = GetComponent<VignetteAndChromaticAberration>();
         _clipplingTime += Time.deltaTime;
         _isClosed = false;
     }

     // Update is called once per frame
     void Update()
     {
         if (_vignetteScript == null) return;

         if (_clipplingTime >= 4f && !_isClosed)
         {
             _vignetteScript.intensity = Mathf.Lerp(_vignetteScript.intensity, 1f, 1f);
             _clipplingTime = 0f;
             _isClosed = true;
             return;
         }

         if (_isClosed && _clipplingTime >= .05f)
         {
             _vignetteScript.intensity = Mathf.Lerp(_vignetteScript.intensity, 0.34f, 1f);
             _isClosed = false;
             return;
         }

         _clipplingTime += Time.deltaTime;
     }
}
