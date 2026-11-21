using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    
    private PlayableDirector _playableDirector;
    void Awake()
    {
        _playableDirector=GetComponent<PlayableDirector>();
    }
    void OnEnable()
    {
        _playableDirector.Play();
        _playableDirector.stopped+=OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        GameManager.Instance.ChangeGameState(GameState.Play);
    }
}
