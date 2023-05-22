using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public enum GameState
{
    idle, ready, roulette, play, finished
}
public class GameManager : MonoBehaviourPun
{
    public Transform iceGround;
    public Transform giantPos;
    public Transform miniPos;
    public Material deadMat;
    Transform deadZone;
    GameState gameState;

    bool playingSettings = false;
    int round = 0;
    int deadZoneCount = 1;
    


    void Start()
    {
        gameState = GameState.idle;
        Transform[] ices = iceGround.GetComponentsInChildren<Transform>();
        foreach (Transform t in ices)
        {
            t.transform.AddComponent<IceAction_KSW>();
        }
    }


    void Update()
    {
        GameFlow();
        GameRule();
    }

    void GameFlow()
    {
        switch(gameState)
        {
            case GameState.idle:
                // idle 상태에서 플레이어 접속을 체크

                gameState = GameState.ready;
                break;
            case GameState.ready:
                // Game을 시작하기 위한 준비 단계
                gameState = GameState.roulette;
                break;
            case GameState.roulette:
                // 우선 빙판을 깰 수 없도록
                if (playingSettings)
                {
                    DisableBreaking();
                    playingSettings = false;
                    round++;
                }
                // 돌림판이 돌아가는 중의 상태

                // 돌림판이 끝나면, 데드존 세팅 등등..
                if (deadZone == null)
                    SetDeadZone();
                Invoke("ChangeToPlay", 3f);
                break;
            case GameState.play:
                // 특수효과 등이 적용된 후 실제로 빙판을 깨는 단계
                // 빙판을 깰 수 있도록 세팅, 미니펭귄 이동 가능, 
                if (!playingSettings)
                {
                    SettingsToPlay();
                    playingSettings = true;
                    print("Round " + round);
                }
                break;
        }
    }

    void GameRule()
    {
        switch(gameState)
        {
            case GameState.play:
                if (deadZone.GetComponent<IceAction_KSW>().GetLife() == 0)
                {
                    DeadZoneKill();
                }
                break;
        }
    }

    void ChangeToPlay()
    {
        gameState = GameState.play;
    }

    void SettingsToPlay()
    {
        Transform[] ices = iceGround.GetComponentsInChildren<Transform>();
        foreach (Transform t in ices)
        {
            t.transform.GetComponent<IceAction_KSW>().CanBreak();
        }
    }
    void DisableBreaking()
    {
        Transform[] ices = iceGround.GetComponentsInChildren<Transform>();
        foreach (Transform t in ices)
        {
            t.transform.GetComponent<IceAction_KSW>().NotBreak();
        }
    }
    
    void SetDeadZone()
    {
        Transform[] ices = iceGround.GetComponentsInChildren<Transform>();
        List<Transform> temp = new List<Transform>();
        foreach (Transform t in ices)
        {
            if (t.GetComponent<IceAction_KSW>().GetLife() > 0)
                temp.Add(t);
        }
        deadZone = temp[Random.Range(0, temp.Count)];
        deadZone.GetComponent<MeshRenderer>().material = deadMat;
    }

    void DeadZoneKill()
    {
        Transform[] ices = iceGround.GetComponentsInChildren<Transform>();
        foreach (Transform t in ices)
        {
            t.transform.GetComponent<IceAction_KSW>().LifeZero();
        }
    }
}
