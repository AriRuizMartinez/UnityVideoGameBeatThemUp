using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] private RoundController m_RoundController;
    private void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = "You have achieved round " + m_RoundController.Round;
    }
}
