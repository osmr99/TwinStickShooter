using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance;

    [SerializeField] private Gamepad _gamePad;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void RumblePulse(float lowFreq, float highFreq, float duration)
    {
        _gamePad = Gamepad.current;
        if(_gamePad != null)
        {
            _gamePad.SetMotorSpeeds(lowFreq, highFreq);
            StartCoroutine(StopRumble(duration, _gamePad));
        }
    }

    IEnumerator StopRumble(float duration, Gamepad _gamepad)
    {
        //float elapsedTime = 0;
        /*
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        */
        yield return new WaitForSeconds(duration);
        _gamepad.SetMotorSpeeds(0, 0);
    }
}
