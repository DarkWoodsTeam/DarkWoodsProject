using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class DW_GM_Classes : MonoBehaviour
{
    // organisation of the scene panels
    public GameObject preGameEnvironnement;
    public GameObject inGameEnvironnement;

    // set up in game cards
    public GameObject cardClass;
    public Transform gridCards;

    // skill management
    private DW_TeamManager team_manager;
    private DW_Skill skill_to_use;
    public DW_ClassHolderRef initiator_skill;

    private void Start()
    {
        team_manager = GameObject.Find("TeamManager").GetComponent<DW_TeamManager>();
    }

    public void TryStartGame()
    {
        if(team_manager.isTeamFull)
        {
            
            preGameEnvironnement.SetActive(false);
            inGameEnvironnement.SetActive(true);
            foreach(DW_Class c in team_manager.classes_selected)
            {
                var t = Instantiate(cardClass, gridCards);
                t.GetComponent<DW_ClassHolderRef>().classRef = c;
                t.GetComponent<DW_ClassHolderRef>().classSkill = c.classSkill;
                t.GetComponent<DW_ClassHolderRef>().classPassif = c.classPassif;

                t.GetComponent<DW_ClassHolderRef>().InitalizeCard();

                GameObject.FindAnyObjectByType<DW_ClassController>().classes.Add(c);
            }
            GameObject.FindAnyObjectByType<DW_ClassController>().currentClass = GameObject.FindAnyObjectByType<DW_ClassController>().classes[0];

        }
    }

    public void RememberSkill(DW_ClassHolderRef classHolder)
    {
        DW_Skill skill = classHolder.classSkill;
        if(!skill_to_use && skill.skillType == SkillType.Heal)
        {
            initiator_skill = classHolder;
            skill_to_use = skill; 
        }
    }

    public void ApplySkill(DW_ClassHolderRef classHolder)
    {
        if(skill_to_use != null && initiator_skill != null)
        {
            DW_Class foundClass = classHolder.classRef;
            foreach(DW_Class c in GameObject.FindAnyObjectByType<DW_ClassController>().classes)
            {
                if (c == foundClass && initiator_skill.classRef.specialSourceAmount - skill_to_use.percentCost >= 0 && c.currentHealth < c.maxHealth)
                {
                    c.currentHealth += Mathf.RoundToInt( ((skill_to_use.restaurationHealth/100) * c.maxHealth));

                    if (c.currentHealth > c.maxHealth)
                    {
                        c.currentHealth = c.maxHealth;
                    }
                    initiator_skill.classRef.specialSourceAmount -= skill_to_use.percentCost;
                    initiator_skill.UpdateSpecialBar();
                    classHolder.UpdateHealthBar();
                    classHolder.classRef.classSkill.isOnCooldown = true;
                    break;
                }
            }
        }
        
        skill_to_use = null;
        initiator_skill = null;
    }
}