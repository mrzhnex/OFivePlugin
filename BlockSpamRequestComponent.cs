using UnityEngine;

namespace OFivePlugin
{
    class BlockSpamRequestComponent : MonoBehaviour
    {
        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        public float timeProgress = Global.spam_block_time;
        
        public void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                timeProgress -= timeIsUp;
            }
            if (timeProgress <= 0)
            {
                Destroy(gameObject.GetComponent<BlockSpamRequestComponent>());
            }

        }
    }
}