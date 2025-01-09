using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUIRound;
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUIHealth;
    [SerializeField] private RoundController m_RoundController;

    public void UpdateHealth(int health) 
    {
        m_TextMeshProUGUIHealth.text = "Health: " + health;
    }
    public void UpdateRound()
    {
        m_TextMeshProUGUIRound.text = "Round: " + m_RoundController.Round;
    }
}
