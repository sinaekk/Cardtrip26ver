/*
 * 작성자: Kim Bummoo
 * 작성일: 2025.03.03
 *
 */

using Imagine.WebAR;
using UnityEngine;

namespace FUTUREVISION.WebAR
{
    public class ARViewModel : BaseViewModel
    {
        [Space(10)]
        public ARObjectView ARObjectView;
        public ARUIView ARUIView;

        public override void Initialize()
        {
            ARObjectView.Initialize();
            ARUIView.Initialize();

            ARUIView.SetActivePlacedButton(false);

            WebARManager.Instance.ARTrackerModel.OnPlacementVisibilityChanged.AddListener((isVisible) =>
            {
                ARUIView.SetPlaceButtonInteractable(isVisible);
            });

            // Event Bindings
            ARUIView.SwitchCameraButton.Button.onClick.AddListener(() =>
            {
                // 전면 카메라, 후면 카메라 전환
                ToggleCameraState();
            });
            ARUIView.PlaceButton.Button.onClick.AddListener(() =>
            {
                // AR 오브젝트를 배치합니다.
                WebARManager.Instance.ARTrackerModel.SetPlacement();
                ARUIView.SetActivePlacedButton(false);
            });
            ARUIView.TakeScreenshotButton.Button.onClick.AddListener(() =>
            {
                // 스크린샷을 찍습니다.
                WebARManager.Instance.ARTrackerModel.TakeScreenShot();
            });
            ARUIView.SwitchARButton.Button.onClick.AddListener(() =>
            {
                // AR 모드를 전환합니다.
                SwitchSetCurrentObjectState();
            });

            // 이전, 다음 버튼 클릭 시 오브젝트 변경
            ARUIView.BeforeButton.Button.onClick.AddListener(() =>
            {
                ARObjectView.SetCurrentObject(ARObjectView.GetCurrentObjectIndex() - 1);
            });
            ARUIView.NextButton.Button.onClick.AddListener(() =>
            {
                ARObjectView.SetCurrentObject(ARObjectView.GetCurrentObjectIndex() + 1);
            });
        }

        /// <summary>
        /// 전면 카메라, 후면 카메라 전환
        /// </summary>
        public void ToggleCameraState()
        {
            CameraState state = WebARManager.Instance.ARTrackerModel.GetCameraState();
            CameraState newState = CameraState.None;
            switch (state)
            {
                case CameraState.Front:
                    newState = CameraState.Back;

                    WebARManager.Instance.ARTrackerModel.SetARTrackerState(WebARManager.Instance.ARTrackerModel.GetARTrackerState());
                    ARUIView.SetActivePlacedButton(false);
                    ARUIView.SetActiveSwitchARButton(true);

                    break;
                case CameraState.Back:
                    newState = CameraState.Front;

                    // 전면 카메라는 스크린 상태로만 이용 (AR X)
                    WebARManager.Instance.ARTrackerModel.SetARTrackerState(ARTrackerState.ScreenState);
                    ARUIView.SetActivePlacedButton(false);
                    ARUIView.SetActiveSwitchARButton(false);

                    break;
                case CameraState.None:
                    Debug.LogWarning("정의되지 않음");
                    break;
            }
            WebARManager.Instance.ARTrackerModel.SetCameraState(newState);
        }

        /// <summary>
        /// AR 모드를 전환합니다.
        /// </summary>
        public void SwitchSetCurrentObjectState()
        {
            var currentObjectState = WebARManager.Instance.ARTrackerModel.GetARTrackerState();
            switch (currentObjectState)
            {
                case ARTrackerState.ScreenState:
                    {
                        WebARManager.Instance.ARTrackerModel.SetARTrackerState(ARTrackerState.WorldState);
                        ARUIView.SetActivePlacedButton(true);
                    }
                    break;
                case ARTrackerState.WorldState:
                    {
                        WebARManager.Instance.ARTrackerModel.SetARTrackerState(ARTrackerState.ScreenState);
                        ARUIView.SetActivePlacedButton(false);
                    }
                    break;
            }
        }

        public void SetActiveObjectView(bool newState)
        {
            ARObjectView.gameObject.SetActive(newState);
        }
        
        public void SetActiveUIView(bool newState)
        {
            ARUIView.gameObject.SetActive(newState);
        }
    }
}
