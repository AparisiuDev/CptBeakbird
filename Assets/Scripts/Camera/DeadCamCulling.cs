using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DeadCamCulling : MonoBehaviour
{
    public Camera main;
    public Camera cull;
    public GameObject rawImageUI;
    public PlayerHealth playerHealth;
    private VideoPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.loopPointReached += OnVideoEnd;
        player.Prepare();
    }

    // Update is called once per frame
    void Update()
    {
       if(playerHealth.hasFinishedDiedAnim)
        {
            PlayCircleIn();
        }
    }

    // Culling para maybe a futuro?

    public void AddCulling()
    {
        int playerLayerMask = 1 << 11;
        cull.cullingMask |= playerLayerMask;
        main.cullingMask &= ~playerLayerMask;
    }

    public void OnVideoEnd(VideoPlayer player)
    {
        playerHealth.hasFinishedCircle = true;
    }

    public void PlayCircleIn()
    {
        rawImageUI.SetActive(true);
        //AddCulling();
        player.Play();
    }
}
