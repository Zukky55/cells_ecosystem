using UnityEngine;
using System.Collections;

namespace CellsEcosystem
{
    public class GizmoObject : MonoBehaviour
    {
        /// <summary></summary>
        public float WidthAngle => widthAngle;
        /// <summary></summary>
        public float HeightAngle => heightAngle;
        /// <summary></summary>
        public float Length => length;
        /// <summary></summary>
        public Color Color => color;

        [SerializeField]
        [Range(0f, 360f)]
        float widthAngle = 0f;
        [SerializeField]
        [Range(0f, 360f)]
        float heightAngle = 0f;
        [SerializeField]
        [Range(0f, 360f)]
        float length = 0f;
        [SerializeField]
        Color color;

        public void SetGizmo(float width, float height, float length)
        {
            SetGizmo(width, height, length, color);
        }
        public void SetGizmo(Color color)
        {
            SetGizmo(widthAngle, heightAngle, length, color);
        }
        public void SetGizmo(float length)
        {
            SetGizmo(widthAngle, heightAngle, length, color);
        }
        public void SetGizmo(float width, float height, float length, Color color)
        {
            widthAngle = width;
            heightAngle = height;
            this.length = length;
            this.color = color;
        }

        private void Update()
        {
            transform.up = Vector3.up;
        }
    }
}