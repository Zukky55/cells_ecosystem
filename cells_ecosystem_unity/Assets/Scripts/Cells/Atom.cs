using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellsEcosystem
{
    public class Atom : MonoBehaviour, IResource
    {
        #region Properties
        #endregion
        #region Variables
        [SerializeField] float force;

        AudioSource audioSource;
        Rigidbody rb;

        bool isUsing;
        bool isInitialized;
        public bool IsUsing => isUsing;
        public bool IsInitialized => isInitialized;

        #endregion
        #region Methods
        public void DestoryAtom()
        {
            audioSource.Play();
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            Ecosystem.AtomsOnStage.Remove(this);
            Destroy(gameObject, audioSource.clip.length);
        }
        #endregion
        #region Callbacks
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Cell")
            {
                var cell = collision.gameObject.GetComponent<Cell>();
                if ("Boss" != cell.Tribe.Name)
                {
                    Destroy(gameObject.GetComponent<Collider>());
                    if (cell)
                    {
                        Ecosystem.GenerateTribe(cell);
                    }
                    DestoryAtom();
                }
            }
        }
        private void FixedUpdate()
        {
            if (rb.velocity == Vector3.zero)
            {
                rb.AddForce(Vector3.up, ForceMode.Impulse);
            }
        }

        public void Activate()
        {
            // TODO: 初期化処理。
            isInitialized = true;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            isInitialized = false;
            isUsing = false;
        }


        #endregion
        #region Enums
        #endregion

    }
}
