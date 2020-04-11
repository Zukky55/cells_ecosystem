using System;

namespace CellsEcosystem
{
    public interface IPlayableUIAnimation<TEnum> where TEnum : Enum
    {
        void Play(TEnum tag);
        void SetBool(TEnum tag, bool flag);
        void SetFloat(TEnum tag, float param);
        void SetTrigger(TEnum tag);
    }
}