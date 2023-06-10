using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    [SerializeField] public int ammoSize;
    [SerializeField] public int magazineSize;
    [SerializeField] public int damage;
    [SerializeField] public float timeBetweenShots = 0.2f;
}
