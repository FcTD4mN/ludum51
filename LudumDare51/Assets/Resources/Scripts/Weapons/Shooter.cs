using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Weapon mWeapon;

    public float mMultiplierDamage = 1f;
    public float mMultiplierArea = 1f;
    public float mMultiplierReloadTime = 1f;
    public float mMultiplierFireRate = 1f;
    public float mMultiplierProjectileSpeed = 1f;
    public int mMultiplierProjectileCount = 1;


    public void SetWeapon(Weapon weapon)
    {
        mWeapon = weapon;
        mWeapon.mShooter = this;
    }

    public Weapon GetWeapon()
    {
        return mWeapon;
    }

    public void TryShoot(Vector2 from, Vector2 to)
    {
        if (mWeapon.mIsShootAvailable)
        {
            Shoot(from, to);
            mWeapon.mIsShootAvailable = false;

            float timeToWait = 1 / (mWeapon.mBaseFireRatePerSec * mMultiplierFireRate);

            mWeapon.mParent.StartCoroutine(Utilities.ExecuteAfter(timeToWait, () =>
            {
                mWeapon.mIsShootAvailable = true;
            }));
        }

    }


    private void Shoot(Vector2 from, Vector2 to)
    {
        if (mWeapon.mBullets > 0)
        {
            Debug.DrawLine(from, to, Color.green, 10);

            if (mMultiplierProjectileCount == 1)
            {
                mWeapon.SpawnProjectile(from, to);
            }
            else
            {
                bool evenCount = (mMultiplierProjectileCount % 2 == 0);

                float distanceFromPlayer = (to - from).magnitude;
                float minDistance = 1f;
                float maxDistance = 12f;
                float angleMax = 20f;
                float angleMin = 2f;
                float distRatio = Mathf.Min((distanceFromPlayer - minDistance) / (maxDistance - minDistance), 1);

                // Reversed as we want to be bigger when closer, so small values 0.01 = closer to max
                float angle = angleMin * distRatio + angleMax * (1 - distRatio);

                int preCount = (int)(mMultiplierProjectileCount / 2) - 1;
                float firstAngle = evenCount ? angle / 2f : angle;
                firstAngle += angle * preCount;

                Vector2 pointTo = Utilities.RotatePointAroundPivot(to, from, firstAngle);
                mWeapon.SpawnProjectile(from, pointTo);
                for (int i = 1; i < mMultiplierProjectileCount; i++)
                {
                    pointTo = Utilities.RotatePointAroundPivot(pointTo, from, -angle);
                    mWeapon.SpawnProjectile(from, pointTo);
                }
            }

            mWeapon.mBullets -= 1;
        }
        else
        {
            mWeapon.Reload();
        }
    }


    public void Reload()
    {
        mWeapon.Reload();
    }


    public void ResetShooter()
    {
        mWeapon.Reset();
    }
}