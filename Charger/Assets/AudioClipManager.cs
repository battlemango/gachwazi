using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;


    public class AudioClipManager : MonoBehaviour
    {
        public static AudioClipManager sinstance = null;        //Allows other scripts to call functions from SoundManager.                

        public AudioClip AC_GEM_GET;
        public AudioClip AC_TIME_OUT;

    void Awake()
        {
            //Check if there is already an instance of SoundManager
            if (sinstance == null)
                //if not, set it to this.
                sinstance = this;
            //If instance already exists:
            else if (sinstance != this)
                //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
                Destroy(gameObject);

            //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad(gameObject);
        }
    }
