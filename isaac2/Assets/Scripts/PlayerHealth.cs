using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    /// <summary>
    ///     Player implentation of Health abstract class, 
    ///     which handles Entity max & cur health, damage,
    ///     healing, death, etc.
    ///     Implements Death() method, which handles Player death.
    /// </summary>
    public override void Death()
    {
        // Player dies
    }
}
