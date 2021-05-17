using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher Instance;
    RoomManager RoomManager;

    [SerializeField] TMP_InputField roomnameinput;
    [SerializeField] TMP_Text errortext;
    [SerializeField] TMP_Text roomnametext;
    [SerializeField] Transform roomlistcontent;
    [SerializeField] Transform playerlistcontent;
    [SerializeField] GameObject roomlistItePrefab;
    [SerializeField] GameObject PlayerlistItemPrefab;
    [SerializeField] GameObject startGameButton;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Menu_Manager.Instance.OpenMenu("Title");
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomnameinput.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomnameinput.text);
        Menu_Manager.Instance.OpenMenu("Loading");
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        PhotonNetwork.Disconnect();
    }

    public override void OnJoinedRoom()
    {
        Menu_Manager.Instance.OpenMenu("RoomMenu");
        roomnametext.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerlistcontent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerlistItemPrefab, playerlistcontent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }



    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errortext.text = "Room creation failed" + message;
        Menu_Manager.Instance.OpenMenu("ErrorMenu");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(3);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Menu_Manager.Instance.OpenMenu("Loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        Menu_Manager.Instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom()
    {
        Menu_Manager.Instance.OpenMenu("Title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomlistcontent)
        {
            Destroy(trans.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomlistItePrefab, roomlistcontent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerlistItemPrefab, playerlistcontent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

}
