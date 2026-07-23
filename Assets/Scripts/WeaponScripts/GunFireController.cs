using UnityEngine;

public class GunFireController : MonoBehaviour
{
    public PlayerController playerController;

    public void Shooting()
    {
        playerController.Shooting();
    }

    public void EndFiring()
    {
        playerController.EndFiring();
    }
}