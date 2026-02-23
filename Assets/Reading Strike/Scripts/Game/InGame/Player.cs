using UnityEngine;
namespace ReadingStrike.Game.InGame
{
    public class Player : Character
    {
        protected override bool CheckPlayer { get { return true; } }
        private float y, z;
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
                InputKeyMove();
                //InputPointMove();
                SkillUseSearching();
            }
        }
        void SkillUseSearching()
        {
            if (!IsSkillCharged) return;
            if (Physics.Raycast(rb.position, rb.transform.forward, out RaycastHit hit, sc.searchedDistance, targetLm))
            {
                if (hit.rigidbody.GetComponent<IBattleable>() is IBattleable temp)
                {
                    BattleManager.instance.BattleStart(this, temp);
                }
                else
                {
                    Debug.Log("IBattleable 없음");
                }
            }
        }
        void InputKey()
        {
            y = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
            if (Input.GetKey(KeyCode.W))
            {
                cAnim.SetAnimFloat("Speed", 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                cAnim.SetAnimFloat("Speed", -1);
            }
            else
            {
                cAnim.SetAnimFloat("Speed", 0);
            }
            if (z > 0 || y > 0) isMouseMove = false;

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
        void InputKeyMove()
        {
            Vector3 moveVector = rb.transform.forward * z * Time.deltaTime * stat.moveSpeed;
            rb.MovePosition(rb.transform.position + moveVector);
            //Vector3 moveVelocity = rb.transform.forward * z * stat.moveSpeed;
            //moveVelocity.y = rb.velocity.y;
            //rb.velocity = moveVelocity;
            y = y * Time.deltaTime * stat.moveSpeed * 5;
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