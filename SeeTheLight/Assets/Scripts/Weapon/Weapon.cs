using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Weapon : MonoBehaviour, Interacts
{
    protected Animator anim;
    protected BoxCollider2D boxColl;
    public int ATK = 4;
    public float pushForce;
    public float cooldown;
    public int weaponLevel;
    public bool canInteracts;
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
    }
    public virtual void Attack()
    {
    }
    public virtual void Tip()
    {
        if (canInteracts)
        {
            mUI.Inst.InteractTip(transform.position, name);
        }
    }
    public virtual void Interact()
    {
        if (canInteracts)
        {
            if (cPlayer.Inst.weapons.Count < cPlayer.Inst.weaponMax)
            {
                gameObject.tag = cPlayer.Inst.tag;
                transform.parent = cPlayer.Inst.weapon.transform.parent;
                transform.localPosition = Vector3.zero;
                canInteracts = false;
                gameObject.SetActive(false);
                cPlayer.Inst.weapons.Add(gameObject);
                boxColl.enabled = false;
            }
            else
            {
                transform.parent = cPlayer.Inst.weapon.transform.parent;
                transform.localPosition = Vector3.zero;

                GameObject weapon = cPlayer.Inst.weapon;
                weapon.tag = tag;
                weapon.transform.parent = null;
                weapon.GetComponent<Weapon>().canInteracts = true;
                weapon.GetComponent<Weapon>().boxColl.enabled = true;
                cPlayer.Inst.weapons.Remove(weapon);

                gameObject.tag = cPlayer.Inst.tag;
                canInteracts = false;
                cPlayer.Inst.weapon = gameObject;
                cPlayer.Inst.weapons.Add(gameObject);
                boxColl.enabled = false;
            }
            mUI.Inst.ColseInteractTip();
        }
    }
    public virtual void ColseTip()
    {
        if (canInteracts)
        {
            mUI.Inst.ColseInteractTip();
        }
    }
}
