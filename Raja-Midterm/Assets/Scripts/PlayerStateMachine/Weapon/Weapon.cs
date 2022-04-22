using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private SO_WeaponData weaponData;
    [SerializeField] private Transform bulletShootPosition;
    public float FireRate { get => weaponData.fireRate; }
    public void ShootBullet() {
        // GameObject bullet = Instantiate((GameObject)Resources.Load("Bullet"), bulletShootPosition.position, Quaternion.identity);

        GameObject shootEffect = ObjectPooler.Instance.SpawnFromPool("shootEffect", bulletShootPosition.position, Quaternion.identity);
        var scale = Vector3.one;
        scale.x *= Player.Instance.FacingDirection == 1 ? 1 : -1;
        shootEffect.transform.localScale = scale;
        shootEffect.GetComponent<ParticleSystem>().emissionRate = 20f;
        shootEffect.GetComponent<ParticleSystem>().Play();


        GameObject bullet = ObjectPooler.Instance.SpawnFromPool("Bullet", bulletShootPosition.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Damage = weaponData.bulletDamage * LocalSave.Instance.saveData.gunLevel;
        bullet.GetComponent<Bullet>().Speed = weaponData.bulletSpeed;
        bullet.GetComponent<Bullet>().Direction = Player.Instance.FacingDirection == 1 ? Vector2.right : Vector2.left;
        bullet.GetComponent<Bullet>().DestroyDelay = weaponData.bulletDestroyDelay;
        bullet.GetComponent<Bullet>().Shoot();

        CinemachineShake.Instance.ShakeCamera(3f, 0.2f);
    }

}
