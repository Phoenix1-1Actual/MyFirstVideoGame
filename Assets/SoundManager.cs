using System.Collections;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource shootingSoundM1911;
    public AudioSource reloadingSoundM1911;
    public AudioSource emptyMagazineSoundM1911;
    public AudioSource shootingSoundM4A1;
    public AudioSource reloadingSoundM4A1;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                shootingSoundM1911.Play();
                break;
            case WeaponModel.M4_8:
                break;
        }
    }
    
    public void PlayReloadSound(WeaponModel weapon)
    {
    switch (weapon)
    {
            case WeaponModel.M1911:
                reloadingSoundM1911.Play();
            break;
        case WeaponModel.M4_8:
                reloadingSoundM4A1.Play();
            break;
    }
}

}

