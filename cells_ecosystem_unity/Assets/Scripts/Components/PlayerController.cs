using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellsEcosystem
{
    /// <summary>
    /// Player controller
    /// </summary>
    public class PlayerController : SingletonMonoBehaviour<PlayerController>
    {
        #region Properties
        public virtual Vector3 Direction
        {
            get
            {
                switch (cameraManager.Mode)
                {
                    case CameraManager.CameraMode.YAxis:
                        return new Vector3(Horizontal, Vertical, 0f);
                    case CameraManager.CameraMode.TPS:
                        return new Vector3(Horizontal, 0f, Vertical);
                    default:
                        return Vector3.zero;
                }
            }
        }
        /// <summary>
        /// 入力が真後ろを向いていないかどうか
        /// </summary>
        public bool IsFacingBack { get => Direction.x == 0 && Direction.y == 0 && Direction.z < 0; }
        public Vector3 AdjustDirection
        {
            get
            {
                var dir = directionalStandard.TransformDirection(Direction);
                return new Vector3(dir.x, 0, dir.z);
            }
        }
        public float Horizontal { get => Input.GetAxis("Horizontal"); }
        public float Vertical { get => Input.GetAxis("Vertical"); }
        #endregion
        #region Variables
        [SerializeField] Cell targetCell;
        /// <summary>DirectionをこのTransformの向きを基準にする(should use transform component in camera.)</summary>
        [SerializeField] Transform directionalStandard;

        CameraManager cameraManager;

        #endregion
        #region Methods
        /// <summary>
        /// セルを動かす
        /// </summary>
        public void Move()
        {
            var adjustDir = directionalStandard.TransformDirection(Direction);
            if (cameraManager.Mode == CameraManager.CameraMode.TPS)
            {
                adjustDir.y = 0;
            }
            if (Direction != Vector3.zero) // Isn't input direction Vector3.back and something input direction key.
            {
                Ecosystem.PlayerCell.Move(adjustDir,IsFacingBack);
            }
        }
        #endregion
        #region Callbacks
        private void Awake()
        {
            cameraManager = FindObjectOfType<CameraManager>();
        }
        private void FixedUpdate()
        {
            Move();
        }
        #endregion
    }
}
