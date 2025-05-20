using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Animations
{
    public class AnimatorManager: MonoBehaviour
    {
        public static Animator myAnimator;
        // Start is called before the first frame update
        void Start()
        {
            myAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
