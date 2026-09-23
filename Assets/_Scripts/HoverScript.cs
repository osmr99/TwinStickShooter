using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScript : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AudioClip[] sounds;
    PlayerController playerController;
    int randNum;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        randNum = Random.Range(0, 2);
        SoundManager.Instance.PlaySound3D(sounds[randNum], playerController.transform.position, 0.275f);
    }
}
