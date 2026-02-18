using ReadingStrike.Manager;
using System;
using UnityEngine;
namespace ReadingStrike.Animation
{
    public class CharacterAnimController : MonoBehaviour
    {
        public bool isGetDamaged;
        [SerializeField] Animator anim;
        event Action RaiseGetDamaged;
        public void SetAnimTrigger(string animName) { anim.SetTrigger(animName); }
        public void SetAnimFloat(string animName, float x) { anim.SetFloat(animName, x); }
        public bool GetAnimBool(string animName) { return anim.GetBool(animName); }
        public void SetAnimBool(string animName, bool isValue) { anim.SetBool(animName, isValue); }
        public void GetDamageAnim()
        {
            if (!isGetDamaged) return;
            RaiseGetDamaged?.Invoke();
            isGetDamaged = false;
        }
        public void AddEventGetDamaged(Action func) { RaiseGetDamaged += func; }
    }
}