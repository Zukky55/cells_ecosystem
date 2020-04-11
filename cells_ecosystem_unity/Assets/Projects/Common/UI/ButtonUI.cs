using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CellsEcosystem
{
    public class ButtonUI : EventUI<ButtonUI.Tag>
    {
        public Button button => GetComponent<Button>();

        public void Push() => Play(Tag.Push);
        public void Stay() => Play(Tag.Stay);


        public enum Tag
        {
            Stay,
            Push,
            Init,
        }
    }
}
