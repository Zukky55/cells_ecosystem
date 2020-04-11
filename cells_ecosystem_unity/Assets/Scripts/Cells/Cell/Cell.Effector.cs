using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace CellsEcosystem
{
    public partial class Cell
    {
        protected virtual void OnCollisionEnter(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Ground":
                    IsGrounded = true;
                    this.IsGrounded = false;
                    break;
                case "Cell":
                    OnCollideCell(collision.gameObject.GetComponent<Cell>());
                    break;
                default:
                    break;
            }
        }
        protected virtual void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                IsGrounded = true;
            }
        }
        protected virtual void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                IsGrounded = false;
            }
        }
    }

}