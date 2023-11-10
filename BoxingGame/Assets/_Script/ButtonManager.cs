using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    Button attackA;
    Button attackS;
    Button skill;

    MultiPlayer multiPlayerscrpit;

    // Start is called before the first frame update
    void Start()
    {
        attackA = transform.GetChild(0).GetComponent<Button>();
        attackA.onClick.AddListener(multiPlayerscrpit.OnClickAttackA);
        attackA.onClick.RemoveListener(multiPlayerscrpit.OnClickAttackA);

        attackS = transform.GetChild(1).GetComponent<Button>();
        attackA.onClick.AddListener(multiPlayerscrpit.OnClickAttackS);
        attackA.onClick.RemoveListener(multiPlayerscrpit.OnClickAttackS);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
