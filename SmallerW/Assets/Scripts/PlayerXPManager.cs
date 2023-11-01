using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXPManager : MonoBehaviour
{
    public int xp = 0; // Current Xp
    public int level = 1; // Curent level

    public void AddXP(int amount)
    {
        xp += amount;
        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        if (xp >= 100)
        {
            xp -= 100;
            level++;
            // Level up rewards
        }
    }
}
