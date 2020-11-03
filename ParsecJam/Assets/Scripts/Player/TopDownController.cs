using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TopDownController : MonoBehaviour
{
    public TopDownEntity entity;
    private Player _rewiredPlayer;

    private Vector2 aimMoveDir;

    // Start is called before the first frame update
    void Start()
    {
        _rewiredPlayer = ReInput.players.GetPlayer("Player1");
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = _rewiredPlayer.GetAxis("MoveX");
        float dirY = _rewiredPlayer.GetAxis("MoveY");

        float aimDirX = _rewiredPlayer.GetAxis("AimX");
        float aimDirY = _rewiredPlayer.GetAxis("AimY");

        Vector2 moveDir = new Vector2(dirX, dirY);
        moveDir.Normalize();


        if (aimDirX != 0 && aimDirY != 0)
        {
            aimMoveDir = new Vector2(aimDirX, aimDirY);
            aimMoveDir.Normalize();
        }


        entity.Move(moveDir, aimMoveDir);

        if (_rewiredPlayer.GetButtonDown("Dash"))
        {
            entity.Dash();
        }
        if (_rewiredPlayer.GetButtonDown("Shoot"))
        {
            entity.shootFunc.ShootBullet();
        }


        if (_rewiredPlayer.GetButtonDown("Pause"))
        {
            GameManager.instance.Pause();
        }

    }
}
