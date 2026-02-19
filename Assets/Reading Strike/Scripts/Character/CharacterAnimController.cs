using Cysharp.Threading.Tasks;
using ReadingStrike.Manager;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
namespace ReadingStrike.Animation
{
    public class CharacterAnimController : MonoBehaviour
    {
        public bool isGetDamaged;
        [SerializeField] Animator anim;
        event Action RaiseGetDamaged;
        event Action RaisePlayerDeath;
        CancellationTokenSource cts;
        [SerializeField] GameObject owner;
        private void OnDestroy()
        {
            CTSSetter.CTSCancel(ref cts);
        }
        public void OwnerSet(GameObject owner) { this.owner = owner; }
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
        async UniTaskVoid DeathAnimTask(bool isPlayer)
        {
            try
            {
                CTSSetter.CTSSet(ref cts);
                while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    await UniTask.Yield(cancellationToken: cts.Token);
                }
                await UniTask.Delay(3000, cancellationToken: cts.Token);
                if (!isPlayer)
                {
                    if (owner != null) Destroy(owner);
                }
                else
                {
                    RaisePlayerDeath?.Invoke();
                }
            }
            catch (OperationCanceledException)
            {
                CTSSetter.CTSCancel(ref cts);
            }
        }
        public void StartDeathAnimTask(bool isPlayer = false)
        {
            DeathAnimTask(isPlayer).Forget();
        }
        public void AddEventGetDamaged(Action func) { RaiseGetDamaged += func; }
        public void AddEventPlayerDeath(Action func) { RaisePlayerDeath += func; }
    }
}