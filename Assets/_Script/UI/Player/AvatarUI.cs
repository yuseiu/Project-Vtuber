using UnityEngine;
using UnityEngine.UI;

public class AvatarUI : MonoBehaviour
{
    public Image avatar;

    void Start()
    {
        avatar.sprite = PlayerStat.Instance.characterInformation.Avatar;
    }
}
