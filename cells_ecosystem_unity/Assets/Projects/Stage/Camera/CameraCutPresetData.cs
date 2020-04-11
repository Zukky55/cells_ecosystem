using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace CellsEcosystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "CameraPreset", menuName = "ScriptableObject/CameraCut")]
    public class CameraCutPresetData : ScriptableObject
    {
     public List<CameraCutPreset> cameraCutPresets;


        [System.Serializable]
        public class CameraCutPreset
        {
            public CameraManager.CameraMode Mode => mode;
            public float Distance => distance;
            public float PositionInterpolate => positionInterpolate;
            public float TurnInterpolate => turnInterpolate;
            public Vector3 OffsetPosition => offsetPosition;
            public Vector3 OffsetEulerAngle => offsetEulerAngle;

            [SerializeField] CameraManager.CameraMode mode;
            /// <summary>Distance from target</summary>
            [SerializeField] float distance;
            /// <summary>interpolate value of position</summary>
            [SerializeField] float positionInterpolate = .1f;
            /// <summary>interpolate value of rotation.</summary>
            [SerializeField] float turnInterpolate = .1f;
            /// <summary>Distance to shift from pivot.</summary>
            [SerializeField] Vector3 offsetPosition;
            /// <summary>Angle to shift from CameraParent</summary>
            [SerializeField] Vector3 offsetEulerAngle;
        }
    }
}