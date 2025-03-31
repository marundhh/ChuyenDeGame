using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class PlayerCanvasFusion : NetworkBehaviour
{
    public GameObject playerCanvas; // Gán Canvas của nhân vật vào đây
    

    public override void Spawned()
    {
        // Kiểm tra nếu đây là Player tôi điều khiển
        if (HasInputAuthority)
        {
            // Canvas này chỉ có mình tôi thấy
            playerCanvas.SetActive(true);
        }
        else
        {
            // Ẩn Canvas của Player khác
            playerCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Cập nhật tên của Player
        
    }
}
