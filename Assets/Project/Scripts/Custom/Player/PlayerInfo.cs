using Mirror;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerInfo : NetworkBehaviour
{
    [SyncVar] public string playerName = "PLAYER_";
    [SyncVar] public Color playerColor = Color.white;

    private void FixedUpdate()
    {
        var worldPoint = _ball.transform.TransformPoint(Vector3.zero) + (Vector3.up * 1.5f);
        gameObject.transform.position = worldPoint;
        gameObject.transform.rotation = Camera.main.transform.rotation;

        if (isLocalPlayer || _ball.GetComponent<Outline>()) return;
        var outline = _ball.gameObject.AddComponent<Outline>();
        outline.OutlineColor = playerColor;
        outline.OutlineWidth = 4f;
    }

    private Rigidbody _ball;

    private void Start()
    {
        _ball = transform.parent.GetComponentInChildren<Rigidbody>();

        playerName += netId;
        playerColor = Random.ColorHSV(hueMin: 0f, hueMax: 1f,
            saturationMin: 1f, saturationMax: 1f, valueMin: 1f, valueMax: 1f);
        
        var nameText = GetComponent<TextMeshPro>();
        nameText.text = playerName;
        nameText.color = playerColor;
    }
}