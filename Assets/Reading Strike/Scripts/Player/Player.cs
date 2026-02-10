using ReadingStrike.Manager;
using ReadingStrike.Skill;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
namespace ReadingStrike.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private SkillController sc;
        [SerializeField] private Rigidbody rb;
        [SerializeField] LayerMask monLm = 1 << 7;
        public SkillSet ChargedSkill { get { return sc.CurSkill; } }
        public bool IsSkillCharged { get { return sc.IsSkillCharged; } }
        //[SerializeField] private Animator anim;
        private float y, z;
        public float speed = 10;
        [SerializeField] private int maxHp = 100;
        [SerializeField] private int hp = 100;
        bool isDeath;
        CancellationTokenSource cts;
        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp < 0) hp = 0;
                else if (maxHp < hp) hp = maxHp;
                tmp.text = $"{hp}";
            }
        }
        [SerializeField] private int atk = 10;
        public int Atk { get { return atk; } }
        private bool isMouseMove = false;
        [SerializeField] Vector3 mouseMovePos = Vector3.zero;
        public float mouseMoveDis = 0;
        public TextMeshProUGUI tmp;
        [SerializeField] private Animator anim;
        void Update()
        {
            InputKey();
        }
        private void FixedUpdate()
        {
            InputKeyMove();
            //InputPointMove();
            SkillUseSearching();
        }
        void SkillUseSearching()
        {
            if (!IsSkillCharged) return;
            if (Physics.Raycast(rb.position, rb.transform.forward, out RaycastHit hit, sc.searchedDistance, monLm))
            {
                BattleManager.BattleStart(this, hit.collider.GetComponent<Monster.Monster>(), 1);
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
                    PlHit(10);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    anim.SetBool("InBattle", true);
                    if (isDeath)
                    {
                        Hp = maxHp;
                        anim.Play("HumanM@CombatIdle1H01", 1);
                    }
                }
            }
        }
        void InputKeyMove()
        {
            Vector3 moveVector = rb.transform.forward * z * Time.deltaTime * speed;
            rb.MovePosition(rb.transform.position + moveVector);
            //anim.SetBool("IsPlWalk", moveVector != Vector3.zero);
            y = y * Time.deltaTime * speed * 3;
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
            Vector3 movePos = Vector3.MoveTowards(rb.position, mouseMovePos, Time.deltaTime * speed);
            rb.MovePosition(movePos);
            rb.MoveRotation(Quaternion.LookRotation(Vector3.Slerp(mouseMovePos, rb.position, speed)));
            if (rb.position - mouseMovePos == Vector3.zero)
            {
                isMouseMove = false;
                //anim.SetBool("IsPlWalk", false);
            }
        }
        public void PlHit(int damage)
        {
            if (sc.IsStifness || isDeath) return;
            Hp -= damage;
            if (Hp == 0)
            {
                isDeath = true;
                anim.SetTrigger("Death");
                //Destroy(gameObject);
            }
            else if (0 < Hp)
            {
                Stifness();
                anim.SetTrigger("Damaged");
            }
        }
        public void Stifness()
        {
            sc.StartStifnessTask(cts);
        }
        public bool CurSkillUse()
        {
            return sc.SkillUse();
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