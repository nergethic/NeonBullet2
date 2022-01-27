using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructablePortalWall : DestructableWall
{
    [SerializeField] Portal portal;
    private void OnDestroy()
    {
        var spawnedPortal = Instantiate(portal);
        spawnedPortal.transform.position = gameObject.transform.position;
    }
}
