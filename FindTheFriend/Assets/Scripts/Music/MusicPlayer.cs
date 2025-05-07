using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] musicTracks;
    [Range(0f, 1f)] public float volume = 0.1f;

    private AudioSource _audioSource;
    private int _currentTrackIndex = -1;
    private static MusicPlayer _instance;
    private float _trackPlaybackPosition;
    private bool _shouldRestorePlayback;

    void Awake()
    {
        // ���������� ���������
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            _audioSource.playOnAwake = false;
            _audioSource.volume = volume;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ������������� ������ ���� ��� ������ ������
        if (_currentTrackIndex == -1 && musicTracks.Length > 0)
        {
            PlayRandomTrack();
        }

        // ������������ ���������� ��������� ��������
#if UNITY_WEBGL && !UNITY_EDITOR
        SetupVisibilityListener();
#endif
    }

    void Update()
    {
        // ��������� ��������� ����� ������ ���� �� ����� ��������������� ���������������
        if (!_shouldRestorePlayback && !_audioSource.isPlaying && _audioSource.clip != null)
        {
            PlayRandomTrack();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_shouldRestorePlayback)
        {
            _audioSource.time = _trackPlaybackPosition;
            _audioSource.Play();
            _shouldRestorePlayback = false;
        }
    }

    void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void PlayRandomTrack()
    {
        if (musicTracks == null || musicTracks.Length == 0) return;

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, musicTracks.Length);
        }
        while (randomIndex == _currentTrackIndex && musicTracks.Length > 1);

        _currentTrackIndex = randomIndex;
        _audioSource.clip = musicTracks[_currentTrackIndex];
        _audioSource.Play();
    }

    void OnEnable()
    {
        // ��������� ��������� ����� ��������� �����
        if (_audioSource != null)
        {
            _trackPlaybackPosition = _audioSource.time;
            _shouldRestorePlayback = _audioSource.isPlaying;
        }
    }



    // ���������� �� JavaScript, ����� �������� ������
    public void OnPageHidden()
    {
        AudioListener.pause = true;  // ������������� ���� ����
        AudioListener.volume = 0;   // ����� ����� �������� ���������
    }

    // ���������� �� JavaScript, ����� �������� ����� �������
    public void OnPageVisible()
    {
        AudioListener.pause = false;
        AudioListener.volume = 1;   // ������������ ���������
    }

    // ������ ������� �� JavaScript
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void SetupVisibilityListener();
}