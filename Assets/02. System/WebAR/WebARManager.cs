/*
 * 작성자: Kim Bummoo
 * 작성일: 2024.12.11
 */

using FUTUREVISION.Content;
using System;
using System.Collections;
using UnityEngine;

namespace FUTUREVISION.WebAR
{

    public class WebARManager : BaseManager<WebARManager>
    {
        [Header("WebAR Manager")]
        public ARTrackerModel ARTrackerModel;
        public ARViewModel ARViewModel;
        public ContentViewModel ContentViewModel;

        [Header("WebAR Manager/Setting")]
        [Tooltip("TODO: WebAR의 카메라를 자동으로 배치할지 여부")]
        public bool IsAutomaticPlacement = false;

        public CameraState StartCameraState = CameraState.Back;
        public ARTrackerState StartObjectState = ARTrackerState.ScreenState;

        public override void Initialize()
        {
            base.Initialize();

            ARTrackerModel.SetCameraState(StartCameraState);
            ARTrackerModel.SetARTrackerState(StartObjectState);

            StartCoroutine(RequestCameraPermission(() =>
            {
                ContentViewModel.Initialize();
            }));
        }

        public IEnumerator RequestCameraPermission(Action action)
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);

            // 카메라 권한 요청

            if (Application.isEditor)
            {
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                yield return new WaitUntil(() =>
                {
                    return Application.HasUserAuthorization(UserAuthorization.WebCam);
                });
            }

            yield return new WaitForSeconds(1.5f);

            // 카메라 권한 요청 후 카메라 초기화
            ARTrackerModel.gameObject.SetActive(true);
            action?.Invoke();

            yield return new WaitForSeconds(1.5f);

            if (IsAutomaticPlacement)
            {
                ARTrackerModel.ResetPlacement();
                ARTrackerModel.SetPlacement();
            }
        }
    }
}
