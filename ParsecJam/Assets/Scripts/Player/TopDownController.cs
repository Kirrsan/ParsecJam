using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TopDownController : MonoBehaviour
{
    public TopDownEntity[] players;
    private Player[] _rewiredPlayer;

    private Vector2[] _aimMoveDir;
    [SerializeField] private float _deadZone;

    private void Awake()
    {
        _rewiredPlayer = new Player[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetIndex(i);
            _rewiredPlayer[i] = ReInput.players.GetPlayer("Player" + (i + 1).ToString());
        }
        _aimMoveDir = new Vector2[players.Length];

        ScoreManager.instance.numberOfPlayers = players.Length;

        if(LevelManager.instance != null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                LevelManager.instance.players[i] = players[i];
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.onInstanceCreated += () =>
        {
            for (int i = 0; i < players.Length; i++)
            {
                LevelManager.instance.players[i] = players[i];
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            players[0]._anim.SetTrigger("Dance");            
        }
        
        if (Input.GetKeyDown(KeyCode.F9))
        {
            players[1]._anim.SetTrigger("Dance");            
        }
        
        if (GameManager.instance.isPlaying)
        {
            #region Player1
            float dirX = _rewiredPlayer[0].GetAxis("MoveX");
            float dirY = _rewiredPlayer[0].GetAxis("MoveY");

            float aimDirX = _rewiredPlayer[0].GetAxis("AimX");
            float aimDirY = _rewiredPlayer[0].GetAxis("AimY");

            Vector2 moveDir = new Vector2(dirX, dirY);
            moveDir.Normalize();
            
            if ((aimDirX > _deadZone || aimDirX < -_deadZone) || (aimDirY > _deadZone || aimDirY < -_deadZone))
            {
                _aimMoveDir[0] = new Vector2(aimDirX, aimDirY);
                _aimMoveDir[0].Normalize();
            }
            players[0].Move(moveDir, _aimMoveDir[0]);

            if (_rewiredPlayer[0].GetButtonDown("Dash"))
            {
                players[0].Dash();
            }
            if (_rewiredPlayer[0].GetButton("Shoot"))
            {
                players[0].shootFunc.SetIsShooting(true);
                players[0].shootFunc.ShootBullet();
            }
            if (_rewiredPlayer[0].GetButtonUp("Shoot"))
            {
                players[0].shootFunc.SetIsShooting(false);
            }
            if (_rewiredPlayer[0].GetButton("PickUp"))
            {
                players[0].Pickup();
            }
            if (_rewiredPlayer[0].GetButtonDown("Power"))
            {
                players[0].powerBehaviour.UsePower();
            }
            #endregion

            #region Player2
            dirX = _rewiredPlayer[1].GetAxis("MoveX");
            dirY = _rewiredPlayer[1].GetAxis("MoveY");

            aimDirX = _rewiredPlayer[1].GetAxis("AimX");
            aimDirY = _rewiredPlayer[1].GetAxis("AimY");

            moveDir = new Vector2(dirX, dirY);
            moveDir.Normalize();


            if ((aimDirX > _deadZone || aimDirX < -_deadZone) || (aimDirY > _deadZone || aimDirY < -_deadZone))
            {
                _aimMoveDir[1] = new Vector2(aimDirX, aimDirY);
                _aimMoveDir[1].Normalize();
            }

            players[1].Move(moveDir, _aimMoveDir[1]);

            if (_rewiredPlayer[1].GetButtonDown("Dash"))
            {
                players[1].Dash();
            }
            if (_rewiredPlayer[1].GetButton("Shoot"))
            {
                players[1].shootFunc.SetIsShooting(true);
                players[1].shootFunc.ShootBullet();
            }
            if (_rewiredPlayer[1].GetButtonUp("Shoot"))
            {
                players[1].shootFunc.SetIsShooting(false);
            }
            if (_rewiredPlayer[1].GetButton("PickUp"))
            {
                players[1].Pickup();
            }
            if (_rewiredPlayer[1].GetButtonDown("Power"))
            {
                players[1].powerBehaviour.UsePower();
            }
            #endregion
        }

        if (_rewiredPlayer[0].GetButtonDown("Pause"))
        {
            GameManager.instance.Pause();
        }
        if (_rewiredPlayer[1].GetButtonDown("Pause"))
        {
            GameManager.instance.Pause();
        }

    }
}
