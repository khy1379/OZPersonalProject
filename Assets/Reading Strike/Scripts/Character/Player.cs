using ReadingStrike.Manager;
using UnityEngine;
namespace ReadingStrike.Character
{
    public class Player : Character
    {
        private float y, z;
        private bool isMouseMove = false;
        [SerializeField] Vector3 mouseMovePos = Vector3.zero;
        protected override void StartSetting()
        {

        }
        protected override void UpdateFeat()
        {
            InputKey();
        }

        protected override void FixedUpdateFeat()
        {
            InputKeyMove();
            //InputPointMove();
            SkillUseSearching();
        }
        void SkillUseSearching()
        {
            if (!IsSkillCharged) return;
            if (Physics.Raycast(rb.position, rb.transform.forward, out RaycastHit hit, sc.searchedDistance, targetLm))
            {
                BattleManager.BattleStart(this, hit.collider.GetComponent<Monster>());
            }
        }
        void InputKey()
        {
            y = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetFloat("Speed", 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                anim.SetFloat("Speed", -1);
            }
            else
            {
                anim.SetFloat("Speed", 0);
            }
            if (z > 0 || y > 0) isMouseMove = false;

            if (anim.GetBool("InBattle"))
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    sc.SkillCharging(0);
                    anim.SetTrigger("NormalAtk");
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    sc.SkillCharging(1);
                    anim.SetTrigger("StrongAtk");
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    sc.SkillCharging(2);
                    anim.SetTrigger("Defense");
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    sc.SkillCancel();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    anim.SetBool("InBattle", false);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    GetDamaged(10);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    anim.SetBool("InBattle", true);
                    if (isDeath)
                    {
                        Hp = stat.maxHp;
                    }
                }
            }
        }
        void InputKeyMove()
        {
            Vector3 moveVector = rb.transform.forward * z * Time.deltaTime * stat.moveSpeed;
            rb.MovePosition(rb.transform.position + moveVector);
            //anim.SetBool("IsPlWalk", moveVector != Vector3.zero);
            y = y * Time.deltaTime * stat.moveSpeed * 3;
            rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + Vector3.up * y));
        }
        void InputPoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                mouseMovePos = hit.point;
                mouseMovePos.y = rb.position.y;
                isMouseMove = true;
                //anim.SetBool("IsPlWalk", true);
            }
        }
        void InputPointMove()
        {
            if (!isMouseMove) return;
            Vector3 movePos = Vector3.MoveTowards(rb.position, mouseMovePos, Time.deltaTime * stat.moveSpeed);
            rb.MovePosition(movePos);
            rb.MoveRotation(Quaternion.LookRotation(Vector3.Slerp(mouseMovePos, rb.position, stat.moveSpeed)));
            if (rb.position - mouseMovePos == Vector3.zero)
            {
                isMouseMove = false;
                //anim.SetBool("IsPlWalk", false);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (IsSkillCharged) Gizmos.DrawLine(rb.position, rb.position + rb.transform.forward * sc.searchedDistance);
        }
        bool CheckMovingPossible()
        {
            if (sc.IsStifness) return true;

            return false;
        }
    }
}