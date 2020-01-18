using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CornTheory.Inventory
{
    /// <summary>
    /// Handles changing the UI to reflect search progress.  
    /// 
    /// Search progress bar goes from 0 to 100.  0 means search hasn't started.  100 means it finished successfully
    /// Search skill and time affect speed that it will go from 0 to 100
    /// its the reverse of this: https://www.youtube.com/watch?v=CA2snUe7ARM
    /// </summary>
    public class SearchProgressBar : MonoBehaviour
    {
        [SerializeField] private Image foreGroundImage;
        [SerializeField] private float updateSpeedSeconds = 0.2F;

        private void Awake()
        {
            GetComponentInParent<SearchProgressBarProgress>().OnProgressChanged += OnProgressChanged;
            foreGroundImage.fillAmount = 0;
        }

        private void OnProgressChanged(float percent)
        {
            StartCoroutine(UpdateUI(percent));
        }

        private IEnumerator<object> UpdateUI(float percent)
        {
            float beforeChange = foreGroundImage.fillAmount;
            float elapsed = 0F;

            while (elapsed < updateSpeedSeconds)
            {
                elapsed += Time.deltaTime;
                foreGroundImage.fillAmount = Mathf.Lerp(beforeChange, percent, elapsed / updateSpeedSeconds);
                yield return null;
            }

            foreGroundImage.fillAmount = percent;
        }

        private void LateUpdate()
        {
            transform.LookAt(UnityEngine.Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}
