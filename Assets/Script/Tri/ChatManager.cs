using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System.Collections.Generic;


public class ChatManager : NetworkBehaviour
{
    [Header("UI Components")]
    public GameObject chatPanel;  // Panel chứa UI chat
    public Button chatButton;     // Nút để ẩn/hiện chat panel
    public InputField inputField; // Ô nhập nội dung chat
    public Button sendButton;     // Nút gửi tin nhắn
    public Text chatText;         // Hiển thị nội dung chat


    private List<string> messages = new List<string>(); // Danh sách tin nhắn


    private void Start()
    {
        // Kiểm tra nếu các UI elements bị thiếu
        if (chatPanel == null || chatButton == null || inputField == null || sendButton == null || chatText == null)
        {
            Debug.LogError("ChatManager: Một hoặc nhiều UI Elements chưa được gán trong Inspector!");
            return;
        }


        // Ẩn chat panel khi bắt đầu game
        chatPanel.SetActive(false);


        // Gán sự kiện click cho nút chat
        chatButton.onClick.AddListener(ToggleChatPanel);
        sendButton.onClick.AddListener(SendMessage);
    }


    private void ToggleChatPanel()
    {
        // Ẩn/hiện panel chat khi nhấn vào button
        chatPanel.SetActive(!chatPanel.activeSelf);
    }


    private void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            string message = inputField.text;
            inputField.text = ""; // Xóa nội dung sau khi gửi


            // Gửi tin nhắn đến tất cả người chơi
            SendChatMessageRpc(message);
        }
    }


    // Gửi tin nhắn đồng bộ đến tất cả người chơi
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void SendChatMessageRpc(string message)
    {
        messages.Add(message);
        UpdateChatDisplay();
    }


    private void UpdateChatDisplay()
    {
        chatText.text = ""; // Xóa nội dung cũ


        foreach (var msg in messages)
        {
            chatText.text += msg + "\n"; // Hiển thị tất cả tin nhắn
        }
    }
}
