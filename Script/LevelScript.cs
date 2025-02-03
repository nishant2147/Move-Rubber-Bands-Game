using UnityEngine;

public class LevelScript : MonoBehaviour
{

    public static LevelScript instance;
    public bool red;
    public bool green;


    private void Awake()
    {
        instance = this;
    }
    void Update()
    {

    }
}
