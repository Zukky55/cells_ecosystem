using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CellsEcosystem
{
    public interface ICell
    {
        void Move(Vector3 dir);
    }
}
