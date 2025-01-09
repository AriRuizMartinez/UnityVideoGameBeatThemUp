using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void Restart()
    {
        GameManager.ChangeScene(GameManager.GameScenes.SampleScene);
    }
}
