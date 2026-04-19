using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace ReadingStrike.Game.InGame
{

    public class Player : Character
    {
        protected override bool CheckPlayer { get { return true; } }
        public SkillController Sc { get { return sc; } }
        //public static Player instanse;
        [SerializeField] Vector3 mouseMovePos = Vector3.zero;
        [SerializeField] SpriteRenderer moveTartgetPos;
        private void Awake()
        {
            GameManager.instance.PlayerSetting(this);
        }
        protected override void StartSetting()
        {

        }
        protected override void UpdateFeat()
        {
            if (!IsDeath)
            {
                InputKey();
            }
        }

        protected override void FixedUpdateFeat()
        {
            if (!IsDeath)
            {
                InputPointMove();
                SkillUseSearching();
            }
        }
        protected override void OnDestroySetting()
        {
            base.OnDestroySetting(); 
            sc.RemoveEventSkillCharging(SkillRangeShow);
        }
        void InputKey()
        {
            if (Input.GetMouseButtonDown(0) && !sc.IsStifness && !EventSystem.current.IsPointerOverGameObject(-1))
            {
                InputPoint();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                sc.SkillCharging(0);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                sc.SkillCharging(1);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                sc.SkillCharging(2);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                sc.SkillCancel();
            }
        }
        void InputPoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.layer == targetLm)
                {
                    mouseMovePos = hit.transform.position;
                }
                else
                {
                    mouseMovePos = hit.point;
                }
                mouseMovePos.y = rb.position.y;
                cAnim.SetAnimFloat("Speed", 1);
                IsMove = true;
                moveTartgetPos.gameObject.SetActive(true);
                moveTartgetPos.transform.position = mouseMovePos;
                //anim.SetBool("IsPlWalk", true);
            }
        }
        void InputPointMove()
        {
            if (!IsMove) return;
            if (!ccm.ClickMove(rb.position, mouseMovePos))
            {
                IsMove = false;
                moveTartgetPos.gameObject.SetActive(false);
                cAnim.SetAnimFloat("Speed", 0);
            }
        }
        public override void MoveStop()
        {
            base.MoveStop();
            moveTartgetPos.gameObject.SetActive(false);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Vector3 startPos = transform.position + new Vector3(0, 0.5f, 0);
            //if (IsSkillCharged) Gizmos.DrawLine(transform.position, transform.position + transform.transform.forward * sc.CurSkill.Data.skillRange);
            if (IsSkillCharged) Gizmos.DrawLine(startPos, startPos + transform.transform.forward * sc.CurSkill.Data.skillRange);
        }
        bool CheckMovingPossible()
        {
            if (sc.IsStifness) return true;

            return false;
        }
        public void AddEventSkillUseImpossible(Action<int> func) => sc.AddEventSkillUseImpossible(func);
        public void AddEventSkillUsePossible(Action<int> func) => sc.AddEventSkillUsePossible(func);
        public void RemoveEventSkillUseImpossible(Action<int> func) => sc.RemoveEventSkillUseImpossible(func);
        public void RemoveEventSkillUsePossible(Action<int> func) => sc.RemoveEventSkillUsePossible(func);
    }
}