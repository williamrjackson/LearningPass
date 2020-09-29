using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 3f)]
    private float crossfadeDuration = 1f;

    private AudioSource audioSrc1;
    private AudioSource audioSrc2;

    void Start()
    {
        audioSrc1 = new GameObject().AddComponent<AudioSource>();
        audioSrc1.name = "SrcA";
        audioSrc1.transform.parent = transform;
        audioSrc1.loop = true;
        audioSrc2 = new GameObject().AddComponent<AudioSource>();
        audioSrc2.name = "SrcB";
        audioSrc2.transform.parent = transform;
        audioSrc2.loop = true;
    }

    public void Play(AudioClip audioClip)
    {
        audioSrc1.clip = audioClip;
        audioSrc1.Play();
        Wrj.Utils.Switcheroo(ref audioSrc1, ref audioSrc2);
        Wrj.Utils.MapToCurve.Linear.CrossFadeAudio(audioSrc1, audioSrc2, 1f, crossfadeDuration);
    }
}
