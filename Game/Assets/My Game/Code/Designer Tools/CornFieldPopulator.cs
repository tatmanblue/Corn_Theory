using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CornTheory.DesignerTools
{
    [ExecuteInEditMode]
    public class CornFieldPopulator : MonoBehaviour
    {
        [SerializeField] double WidthDensity = 3.0;
        [SerializeField] double LengthDensity = 3.0;
        [SerializeField] double FieldWidth = 0.0;
        [SerializeField] double FieldLength = 0.0;
        

        // Start is called before the first frame update
        void Start()
        {
        }

        void DisplayLocalCorners()
        {
            RectTransform rt;
            rt = GetComponent<RectTransform>();

            Vector3[] v = new Vector3[4];

            rt.rotation = Quaternion.AngleAxis(45, Vector3.forward);
            rt.GetLocalCorners(v);

            Debug.Log("Local Corners");
            for (var i = 0; i < 4; i++)
            {
                Debug.Log("Local Corner " + i + " : " + v[i]);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // scale.z length
            // scale.x width
            FieldLength = gameObject.transform.localScale.z;
            FieldWidth = gameObject.transform.localScale.x;

             // rt = GetComponent<RectTransform>();
            // DisplayLocalCorners();
        }
    }
}
