using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    private List<Transform> nodes = new List<Transform>();

    public Transform target;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        Transform[] pathTransforms = GetComponentsInChildren<Transform>();
        //Debug.Log("pathTransforms.Length ex: 12 actual: " + pathTransforms.Length);

        //nodes = new List<Transform>();

        //Debug.Log("nodes: " + nodes);

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

        //Debug.Log("node count: " + nodes.Count);

        for (int i = 0; i < nodes.Count; i++) {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;

            if (i > 0) {
                previousNode = nodes[i - 1].position;
            } else if (i == 0 && nodes.Count > 1) {
                previousNode = nodes[nodes.Count - 1].position;
            }

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 1);
        }
    }
}
