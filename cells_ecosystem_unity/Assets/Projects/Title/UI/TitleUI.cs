using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace CellsEcosystem
{
    public class TitleUI : EventUI<TitleUI.Tag>
    {
        public enum Tag
        {
            Init,
        }

        public void Init() => Play(Tag.Init.ToString());
    }
}