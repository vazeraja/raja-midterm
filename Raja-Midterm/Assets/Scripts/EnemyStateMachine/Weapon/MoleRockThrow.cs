using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleRockThrow : MonoBehaviour {

    [SerializeField] private SO_WeaponData weaponData;
    [SerializeField] private Transform rockThrowPosition;
    public float FireRate { get => weaponData.rockFireRate; }
    public void ShootBullet() {
        // GameObject bullet = Instantiate((GameObject)Resources.Load("Bullet"), bulletShootPosition.position, Quaternion.identity);
        GameObject rock = ObjectPooler.Instance.SpawnFromPool("Rock", rockThrowPosition.position, Quaternion.identity);
        rock.GetComponent<Rock>().Damage = weaponData.rockDamage * 5f;
        rock.GetComponent<Rock>().Speed = weaponData.rockSpeed;
        Entity entity = GetComponentInParent<Entity>();
        rock.GetComponent<Rock>().Direction = entity.FacingDirection == 1 ? Vector2.right : Vector2.left;
        rock.GetComponent<Rock>().DestroyDelay = weaponData.rockDestroyDelay;
        rock.GetComponent<Rock>().Shoot();
    }

}
