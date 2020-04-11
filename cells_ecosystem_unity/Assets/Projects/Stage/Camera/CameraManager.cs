using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace CellsEcosystem
{
    public class CameraManager : MonoBehaviour
    {

        /// <summary>Camera mode</summary>
        public CameraMode Mode
        {
            get => CurrentPreset.Mode;
            private set => CurrentPreset = cameraCutPresetData.cameraCutPresets.Find(preset => preset.Mode == value);
        }
        Transform FocusTarget
        {
            get
            {
                if (!focusTarget)
                {
                    focusTarget = Ecosystem.PlayerCell?.transform;
                    ChangeMode(CameraMode.Animation);
                }
                return focusTarget;
            }
        }
        public CameraCutPresetData.CameraCutPreset CurrentPreset { get => currentPreset; set => currentPreset = value; }
        public float MouseWheel { get => Input.GetAxis("Mouse ScrollWheel"); }

        [SerializeField] CameraCutPresetData cameraCutPresetData;
        [SerializeField] CameraCutPresetData.CameraCutPreset currentPreset;
        [SerializeField] CameraMode firstMode; // check
        /// <summary>mouse wheel roll per count.</summary>
        [SerializeField] float mouseWheelRPC = 1.2f;

        /// <summary></summary>
        Transform cameraParent;
        /// <summary></summary>
        Transform cameraChild;
        /// <summary></summary>
        Transform cameraTransfom;
        PlayerController playerCtrl;
        Transform focusTarget;
        float additionalYAxisOffsetAmount;
        float additionalDistanceOffsetAmount;
        Quaternion rot;
        CameraMode previousMode;

        #region Methods
        /// <summary>
        /// Switch camera mode .
        /// </summary>
        private void ChangeMode()
        {
            if (currentPreset.Mode != CameraMode.Animation)
            {
                previousMode = currentPreset.Mode;
            }
            var modes = Enum.GetNames(typeof(CameraMode)).ToList();
            var index = modes.IndexOf(CurrentPreset.Mode.ToString());
            //var mode = (CameraMode)Enum.Parse(typeof(CameraMode), modes[(index + 1) % modes.Count]);
            var mode = currentPreset.Mode == CameraMode.TPS ? CameraMode.YAxis : CameraMode.TPS;
            Mode = mode;
        }
        /// <summary>
        /// Switch camera mode to specified mode.
        /// </summary>
        /// <param name="mode">to mode.</param>
        private void ChangeMode(CameraMode mode)
        {
            if (currentPreset.Mode != CameraMode.Animation)
            {
                previousMode = currentPreset.Mode;
            }
            Mode = mode;
        }
        /// <summary>
        /// Camera behavior on TPS mode.
        /// </summary>
        void TPSModeBehavior()
        {
            if (FocusTarget == null) return;
            if (playerCtrl.Direction != Vector3.zero)
            {
                rot = Quaternion.LookRotation(playerCtrl.AdjustDirection, Vector3.up);
            }
            cameraParent.position = Vector3.Slerp(cameraParent.position, FocusTarget.position, CurrentPreset.PositionInterpolate);
            cameraChild.localPosition = new Vector3(0, 0, -CurrentPreset.Distance + additionalDistanceOffsetAmount);
            cameraTransfom.localRotation = Quaternion.Euler(CurrentPreset.OffsetEulerAngle);
            cameraTransfom.localPosition = CurrentPreset.OffsetPosition;
            // TPSモードの時だけカメラをキャラクターの後ろに追従するようにする
            if (!playerCtrl.IsFacingBack)
            {
                cameraParent.rotation = Quaternion.Slerp(cameraParent.rotation, rot, CurrentPreset.TurnInterpolate);
            }
        }
        /// <summary>
        /// Camera behavior on YAxis mode.
        /// </summary>
        void YAxisModeBehavior()
        {
            if (FocusTarget == null) return;
            if (playerCtrl.Direction != Vector3.zero)
            {
                rot = Quaternion.LookRotation(playerCtrl.AdjustDirection, Vector3.up);
            }
            cameraParent.position = Vector3.Slerp(cameraParent.position, FocusTarget.position, CurrentPreset.PositionInterpolate);
            cameraChild.localPosition = new Vector3(0, 0, -CurrentPreset.Distance);
            cameraTransfom.localRotation = Quaternion.Euler(CurrentPreset.OffsetEulerAngle);
            cameraTransfom.localPosition = new Vector3(CurrentPreset.OffsetPosition.x, CurrentPreset.OffsetPosition.y + additionalYAxisOffsetAmount, CurrentPreset.OffsetPosition.z);
        }
        void CameraMoveAnimation()
        {
            if (FocusTarget == null) return;
            // フォーカス対象とカメラの位置がVector3.oneの距離
            if ((Ecosystem.PlayerCell.transform.position - cameraParent.transform.position).sqrMagnitude < Vector3.one.sqrMagnitude)
            {
                switch (previousMode)
                {
                    case CameraMode.YAxis:
                        ChangeMode(CameraMode.YAxis);
                        break;
                    case CameraMode.TPS:
                        ChangeMode(CameraMode.TPS);
                        break;
                    case CameraMode.Animation:
                        break;
                    default:
                        break;
                }
            }
            cameraParent.position = Vector3.Slerp(cameraParent.position, FocusTarget.position, CurrentPreset.PositionInterpolate);
        }
        #endregion
        #region Callbacks
        private void Awake()
        {
            playerCtrl = PlayerController.Instance;
            CurrentPreset = cameraCutPresetData.cameraCutPresets.Find(preset => preset.Mode == CameraMode.YAxis);
            cameraParent = transform;
            cameraChild = cameraParent.GetChild(0);
            cameraTransfom = cameraChild.GetChild(0);
            Mode = firstMode; // check
        }
        private void Start()
        {
            rot = FocusTarget.rotation;
        }
        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeMode();
                return;
            }
            switch (Mode)
            {
                case CameraMode.YAxis:
                    YAxisModeBehavior();
                    break;
                case CameraMode.TPS:
                    TPSModeBehavior();
                    break;
                case CameraMode.Animation:
                    CameraMoveAnimation();
                    break;
                default:
                    break;
            }
        }
        private void Update()
        {


            if (MouseWheel != 0)
            {
                switch (Mode)
                {
                    case CameraMode.YAxis:
                        additionalYAxisOffsetAmount -= MouseWheel * mouseWheelRPC;
                        break;
                    case CameraMode.TPS:
                        additionalDistanceOffsetAmount += MouseWheel * mouseWheelRPC;
                        break;
                    case CameraMode.Animation:
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
        #region Enumerables
        public enum CameraMode
        {
            /// <summary>上から見下ろす視点</summary>
            YAxis = 1,
            /// <summary>TPS視点</summary>
            TPS,
            /// <summary>Animation</summary>
            Animation,
        }
        #endregion
    }
}
