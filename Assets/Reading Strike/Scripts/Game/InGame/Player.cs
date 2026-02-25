using UnityEngine;
namespace ReadingStrike.Game.InGame
{
    public class Player : Character
    {
        protected override bool CheckPlayer { get { return true; } }
        private float hor, ver;
        private bool isMouseMove = false;
        [SerializeField] Vector3 mouseMovePos = Vector3.zero;
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
                //InputPointMove();
                SkillUseSearching();
            }
        }
        void InputKey()
        {
            hor = Input.GetAxisRaw("Horizontal");
            ver = Input.GetAxisRaw("Vertical");
            if (hor != 0 || ver != 0)
            {
                cAnim.SetAnimFloat("Speed", 1);
            }
            else
            {
                cAnim.SetAnimFloat("Speed", 0);
            }
            if (ver > 0 || hor > 0) isMouseMove = false;
            ccm.CCMove(hor, ver);

            if (cAnim.GetAnimBool("InBattle"))
            {
                if (Input.GetKeyDown(KeyCode.Z))
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
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    cAnim.SetAnimBool("InBattle", false);
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
                    cAnim.SetAnimBool("InBattle", true);
                    if (IsDeath)
                    {
                        Hp = stat.maxHp;
                    }
                }
            }
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
            if (IsSkillCharged) Gizmos.DrawLine(transform.position, transform.position + transform.transform.forward * sc.searchedDistance);
        }
        bool CheckMovingPossible()
        {
            if (sc.IsStifness) return true;

            return false;
        }
    }
}