using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    GameObject manager;
    Conect4Manager managerScript;

    private float _fallTime = 0.01f;
    private float _fallTimer = 0.01f;
    private bool isMove = true;
    [SerializeField] MeshRenderer _stone = null;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("StageManager");
        managerScript = manager.GetComponent<Conect4Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            _fallTimer -= Time.deltaTime;
            if (_fallTimer <= 0.0f)
            {
                float moveZ = managerScript._moveFinishZ;
                float z = this.transform.position.z;
                if (z <= moveZ)
                {
                    this.transform.position = new Vector3(this.transform.position.x, 0, moveZ);
                    isMove = false;
                    managerScript.isAct = true;
                    return;
                }
                else
                {
                    z -= 0.1f;
                    this.transform.position = new Vector3(this.transform.position.x, 0, z);
                    _fallTimer = _fallTime;
                }
            }
        }

    }



    
}
