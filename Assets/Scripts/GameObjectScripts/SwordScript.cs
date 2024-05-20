using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{

    [SerializeField]
    private GameObject swordTip, swordBase, trailMesh;

    [SerializeField]
    private int trailFrameLength;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private int frameCount;
    private Vector3 previousTipPosition, previousBasePosition;

    private const int NUM_VERTICES = 12;



    void Start()
    {
        mesh = new Mesh();
        trailMesh.GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[trailFrameLength * NUM_VERTICES];
        triangles = new int[vertices.Length];

        previousTipPosition = swordTip.transform.position;
        previousBasePosition = swordBase.transform.position;

    }

    private void LateUpdate()
    {
        if (frameCount == (trailFrameLength * NUM_VERTICES))
        {
            frameCount = 0;
        }
        //Draw first triangle vertices for back and front
        vertices[frameCount] = swordBase.transform.position;
        vertices[frameCount + 1] = swordTip.transform.position;
        vertices[frameCount + 2] = previousTipPosition;
        vertices[frameCount + 3] = swordBase.transform.position;
        vertices[frameCount + 4] = previousTipPosition;
        vertices[frameCount + 5] = swordTip.transform.position;

        //Draw fill in triangle vertices
        vertices[frameCount + 6] = previousTipPosition;
        vertices[frameCount + 7] = swordBase.transform.position;
        vertices[frameCount + 8] = previousBasePosition;
        vertices[frameCount + 9] = previousTipPosition;
        vertices[frameCount + 10] = previousBasePosition;
        vertices[frameCount + 11] = swordBase.transform.position;

        //Set triangles
        triangles[frameCount] = frameCount;
        triangles[frameCount + 1] = frameCount + 1;
        triangles[frameCount + 2] = frameCount + 2;
        triangles[frameCount + 3] = frameCount + 3;
        triangles[frameCount + 4] = frameCount + 4;
        triangles[frameCount + 5] = frameCount + 5;
        triangles[frameCount + 6] = frameCount + 6;
        triangles[frameCount + 7] = frameCount + 7;
        triangles[frameCount + 8] = frameCount + 8;
        triangles[frameCount + 9] = frameCount + 9;
        triangles[frameCount + 10] = frameCount + 10;
        triangles[frameCount + 11] = frameCount + 11;

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        previousTipPosition = swordTip.transform.position;
        previousBasePosition = swordBase.transform.position;
        frameCount += NUM_VERTICES;
    }
}
