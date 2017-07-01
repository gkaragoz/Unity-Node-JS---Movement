using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkController : MonoBehaviour {

    public SocketIOComponent socket;
    public GameObject Player;
    public string ID = "";

    public Vector3 Movement = Vector3.zero;

	void Start () {
        socket = GetComponent<SocketIOComponent>();

        socket.On("connect", ConnectSuccess);
        socket.On("state", State);
    }

    void ConnectSuccess(SocketIOEvent e)
    {
        Debug.Log("[CONNECTED] to: " + socket.url);
        socket.Emit("newPlayer");
    }

    void State(SocketIOEvent e)
    {
        Vector2 pos = JSONToVector2(e.data[0]);
        //Debug.Log(pos);
        Player.transform.position = new Vector3(pos.x, pos.y, 0);
    }

    void Move()
    {
        JSONObject data = new JSONObject();
        data.AddField("x", Movement.x);
        data.AddField("y", Movement.y);
        socket.Emit("move", data);
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Movement = Vector3.up;

            Move();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Movement = Vector3.down;

            Move();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Movement = Vector3.right;

            Move();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Movement = Vector3.left;

            Move();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Movement = Vector3.zero;
        }
    }

    Vector2 JSONToVector2(JSONObject data)
    {
        return new Vector2(float.Parse(data.GetField("x").ToString().Replace("\"", "")),
                           float.Parse(data.GetField("y").ToString().Replace("\"", "")));
    }
}
