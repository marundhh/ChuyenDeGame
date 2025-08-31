using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCSystem : MonoBehaviour
{
    bool player_detection = false;
    public NPCConversation con;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_detection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player_detection = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player_detection && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Vao vung noi chuyen");
            ConversationManager.Instance.StartConversation(con);
        }
    }
}
