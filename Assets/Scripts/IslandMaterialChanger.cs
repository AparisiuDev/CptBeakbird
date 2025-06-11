using UnityEngine;
using LevelLocker;

public class IslandMaterialChanger : MonoBehaviour
{
    public Material materialTrue;
    public Material materialFalse;
    private string miTag;
    private Renderer rend;

    void Start()
    {
        miTag = gameObject.tag;
        rend = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        
        switch (miTag)
        {
            case "Level1":
                CambiarMaterial1();
                break;
            case "Level2":
                CambiarMaterial2();
                break;
            case "Tapon":
                CambiarMaterialTapon();
                break;
            default:
                break;
        }
    }

    void CambiarMaterial1()
    {
        if (LevelLocker.VariablesGlobales._lvl1)
        {
            rend.material = materialTrue;
        }
        else
        {
            rend.material = materialFalse;
        }
    }

    void CambiarMaterial2()
    {
        if (LevelLocker.VariablesGlobales._lvl2)
        {
            rend.material = materialTrue;
        }
        else
        {
            rend.material = materialFalse;
        }
    }

    void CambiarMaterialTapon()
    {
        if (LevelLocker.VariablesGlobales._tapon)
        {
            rend.material = materialTrue;
        }
        else
        {
            rend.material = materialFalse;
        }
    }
}
