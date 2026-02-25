using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ReadingStrike.Game.InGame
{
    public class CharacterControllerMove : MonoBehaviour
    {
        [SerializeField] CharacterController cc;
        [SerializeField] float speed;
        public void InitSetting(float speed)
        {
            this.speed = speed;
        }
        public void CCMove(float h, float v)
        {
            Vector3 dir = new Vector3(h, 0, v) * speed;
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
            }
            cc.Move(dir * Time.deltaTime);
        }
    }
}