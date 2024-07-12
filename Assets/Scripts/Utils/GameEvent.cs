using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    public UnityEvent OnBeat;
    public UnityEvent<int> OnBreak;
    public UnityEvent OnSpawn;

    public UnityEvent<AudioClip> PlayClip;
    public UnityEvent<AudioClip> PlayBGMusic;
}
