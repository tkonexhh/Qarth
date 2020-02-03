using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qarth
{
    [RequireComponent(typeof(Button))]
    public class ButtonSoundCompo : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private string soundName;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(PlaySound);
        }

        private void OnDisable()
        {
            gameObject.GetComponent<Button>().onClick.RemoveListener(PlaySound);
        }

        #endregion

        #region Private Methods

        private void PlaySound()
        {
            if (!string.IsNullOrEmpty(soundName))
            {
                AudioMgr.S.PlaySound(soundName);
            }
            else if (!string.IsNullOrEmpty(SoundButton.defaultClickSound))
            {
                AudioMgr.S.PlaySound(SoundButton.defaultClickSound);
            }
        }

        #endregion
    }
}
