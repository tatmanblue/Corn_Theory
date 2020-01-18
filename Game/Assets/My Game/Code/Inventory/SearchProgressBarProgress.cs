using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CornTheory.Inventory
{
    /// <summary>
    /// this represents the progress of a search and should be used to show progress on screen.  
    /// 0 means nothing done or achieved.  
    /// 100 means complete 
    /// and everything else means the search is ongoing
    /// </summary>
    public class SearchProgressBarProgress : MonoBehaviour
    {
        [SerializeField] private int maxProgress = 100;
        private int currentProgress = 0;

        public event Action<float> OnProgressChanged = delegate { };

        public void Reset()
        {
            currentProgress = 0;
        }

        public void ModifyProgress(int amount)
        {
            currentProgress += amount;

            float progressPercent = (float)currentProgress / maxProgress;
            OnProgressChanged(progressPercent);
        }
    }
}
