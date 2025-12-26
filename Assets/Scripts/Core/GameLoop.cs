using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public static GameLoop Instance;

    public const float DeltaTime = 1.0f/120.0f;

    private float _accumulator = 0.0f;

    public delegate void GameAction();
    public event GameAction OnTick;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        _accumulator += Time.deltaTime;

        while (_accumulator >= DeltaTime)
        {
            OnTick?.Invoke();
            _accumulator -= DeltaTime;
        }
    }
}
