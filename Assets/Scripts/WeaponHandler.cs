using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogic;

    // Start is called before the first frame update

    public void enableWeapon()
    {
        weaponLogic.SetActive(true);
    }

    public void disableWeapon()
    {
        weaponLogic.SetActive(false);
    }
}
