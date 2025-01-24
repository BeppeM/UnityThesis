using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class AbstractAvatarWithEyes : AbstractAvatar
{
    protected GameObject avatarBody;
    protected GameObject avatarEyes;
    protected TextMeshProUGUI baloonText;

    protected void initializeAvatarWithEyes()
    {
        // Retrieve avatar parts
        avatarBody = transform.Find("Body").gameObject;
        avatarEyes = transform.Find("anchorVisionCone").gameObject;

        // Find the TextMeshPro component in the children of the avatar
        nameTextMeshPro = transform.Find("avatarName").GetComponent<TextMeshPro>();
        nameTextMeshPro.text = name;

        // Initiating the baloon
        baloonText = gameObject.transform.Find("Canvas/BaloonBg/BaloonTxt").GetComponent<TextMeshProUGUI>();
        baloonText.text = "start";
    }

    public void SetBaloonText(string message)
    {
        baloonText.text = message;
    }

    public void EnableDisableVisionCone(bool isActive)
    {
        avatarEyes.SetActive(isActive);
    }

    public void SendMessageToJaCaMoBrain(string message)
    {
        wsChannel.sendMessage(message);
    }
}
