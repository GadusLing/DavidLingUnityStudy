using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_RolePropertyType
{
    ATK,
    DEF,
    MaxHP,
    HP
}

public class RolePropertiesReward : MonoBehaviour
{
    public E_RolePropertyType type = E_RolePropertyType.ATK;

    public int changeValue = 2;

    public GameObject getEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerObj player = other.GetComponent<PlayerObj>();
            switch (type)
            {
                case E_RolePropertyType.ATK:
                    player.atk += changeValue;
                    break;
                case E_RolePropertyType.DEF:
                    player.def += changeValue;
                    break;
                case E_RolePropertyType.MaxHP:
                    player.maxHP += changeValue;
                    GamePanel.Instance.UpdateHP(player.maxHP, player.HP);
                    break;
                case E_RolePropertyType.HP:
                    player.HP += changeValue;
                    if (player.HP > player.maxHP)
                        player.HP = player.maxHP;
                    GamePanel.Instance.UpdateHP(player.maxHP, player.HP);
                    break;
            }
            GameObject eff = Instantiate(getEffect, transform.position, Quaternion.identity);
            AudioSource audioS = eff.AddComponent<AudioSource>();
            audioS.volume = GameDataManager.Instance.musicData.soundVolume;
            audioS.mute = !GameDataManager.Instance.musicData.isMusicOn;
            Destroy(gameObject);
        }
    }
}
