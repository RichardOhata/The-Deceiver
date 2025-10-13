using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CaptchaGenerator", menuName = "Scriptable Objects/CaptchaGenerator")]
public class CaptchaGenerator : ScriptableObject
{
    public Captcha[] captchas;

    public static int index = 0;
    public Captcha Generate()
    {
        return captchas[(index++ % captchas.Length)];
    }

    public bool IsCodeValid(string input, Captcha c)
    {
        return (input == c.value);
    }
}
