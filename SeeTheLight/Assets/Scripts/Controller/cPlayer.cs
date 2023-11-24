using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class cPlayer : MonoBehaviour
{
    static cPlayer inst;
    public static cPlayer Inst => inst;

    public int maxHp;
    int nowHp;
    public int maxDef;
    int nowDef;
    
    public int torches = 1;
    public float torchTime = 5f;

    public int weaponMax = 2;
    public GameObject weapon;
    public List<GameObject> weapons = new List<GameObject>();
    int weaponIndex = 0;
    bool isGun;

    float xInput;
    float yInput;
    float scrollAmount;

    float weaponChangeTime = 0;

    public float cooldown;
    public float cooldownMultiple = 1;
    public Vector2 direction;
    GameObject interactObject;
    UnityAction hurtAction;
    float invincibleTime = 1;
    float nowInvincibleTime = 0;

    private void Awake()
    {
        inst = this;
        isGun = weapon.GetComponent<Gun>() != null;
        weapons.Add(weapon);
        hurtAction = () =>
        {
            nowInvincibleTime = invincibleTime;
        };
        nowHp = maxHp;
        nowDef = maxDef;
        StartCoroutine("TorchCycle");
    }

    IEnumerator TorchCycle()
    {
        while (true)
        {
            if (torches == 0)
            {
                nowHp = 0;
                mUI.Inst.PlayerUpdateHp(maxHp, nowHp);
                Dead();
                yield return null;
            }

            yield return new WaitForSeconds(torchTime);
            torches--;
        }
    }

    void Start()
    {
        mUI.Inst.PlayerUpdate(maxHp, nowHp, maxDef, nowDef);
    }
    void Update()
    {
        cooldown -= Time.deltaTime * cooldownMultiple;
        nowInvincibleTime -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        direction = (cCursor.Inst.transform.position - weapon.transform.position).normalized;
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        WeaponForword();
        SwitchWeapon();
        Interactive();
        if (Input.GetMouseButton(0))
        {
            weapon.GetComponent<Weapon>().Attack();
        }
    }
    private void FixedUpdate()
    {
        if (xInput != 0 || yInput != 0)
        {
            transform.position += new Vector3(xInput, yInput, 0).normalized * (Time.deltaTime * 4);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interact"))
        {
            interactObject = collision.gameObject;
            interactObject.GetComponent<Interacts>().Tip();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactObject == collision.gameObject)
        {
            interactObject.GetComponent<Interacts>().ColseTip();
            interactObject = null;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine("collision_damage", collision);  
;       }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            StopCoroutine("collision_damage");
        }
    }

    IEnumerator collision_damage(Collision2D collision)
    {
        while (true)
        {
            Hurt(collision.gameObject.GetComponent<Enemy>().attackDamage);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void SwitchWeapon()
    {
        weaponChangeTime -= Time.deltaTime;
        if (scrollAmount != 0)
        {
            if (weaponChangeTime < 0)
            {
                weaponChangeTime = 0.5f;
                weapons[weaponIndex].gameObject.SetActive(false);
                if (scrollAmount > 0)
                {
                    if (weaponIndex == 0)
                        weaponIndex = weapons.Count - 1;
                    else
                        weaponIndex -= 1;
                }
                else if (scrollAmount < 0)
                {
                    if (weaponIndex == weapons.Count - 1)
                        weaponIndex = 0;
                    else
                        weaponIndex += 1;
                }
                weapons[weaponIndex].gameObject.SetActive(true);
                weapon = weapons[weaponIndex].gameObject;
                if (!weapon.CompareTag(tag))
                    weapon.tag = tag;
                isGun = weapon.GetComponent<Gun>() != null;
            }
        }
    }
    void WeaponForword()
    {
        if (isGun)
        {
            if (direction.x > 0)
            {
                weapon.transform.localScale = Vector3.one;

            }
            else if (direction.x < 0)
            {
                weapon.transform.localScale = new Vector3(-1, -1, 1);
            }
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            weapon.transform.localScale = Vector3.one;
        }
    }
    void Interactive()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactObject != null)
        {
            interactObject.GetComponent<Interacts>().Interact();
            interactObject = null;
        }
    }
    public void Hurt(int atk)
    {
        if (nowInvincibleTime > 0)
        {

        }
        else
        {
            if (nowDef > atk)
            {
                nowDef -= atk;
                mUI.Inst.PlayerUpdateDef(maxDef, nowDef);
            }
            else
            {
                nowHp -= (atk - nowDef);
                nowDef = 0;
                mUI.Inst.PlayerUpdateDef(maxDef, nowDef);
                mUI.Inst.PlayerUpdateHp(maxHp, nowHp);
            }

            hurtAction();
            if (nowHp < 0)
                Dead();
        }
    }
    void Dead()
    {
        SceneManager.LoadScene(2);
    }
}
