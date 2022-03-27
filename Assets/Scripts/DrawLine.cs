using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject linePref;
    public GameObject currentLine;
    public GameObject drawObject;
    public GameObject draw;
    public GameObject foot1, foot2;
    public GameObject Player;
    public LineRenderer lineRenderer;
    public List<Vector2> fingerPos;
    public List<Vector2> newPos;
    Camera cam;
    [SerializeField]Animator playerAnim;
    PlayerMovement playerMovement;

    public Vector3 coords;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // setting camera position to player position
        cam.transform.position = new Vector3(Player.transform.position.x,cam.transform.position.y, cam.transform.position.z);
        // if mouse is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // putting the ray into screen
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Ray, out hit))
            {
                // stop player
                playerMovement.isGameStop = true;
                Player.transform.position += Player.transform.forward * 0;
                //create line
                playerAnim.SetInteger("walk", 0);
                CreateLine();
            }
            else
            {
                Destroy(currentLine);
                return;
            }
            
        }
        if (Input.GetMouseButton(0))
        {
            // updating line if mouse is pressed
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(tempFingerPos, fingerPos[fingerPos.Count - 1]) > .1f&& lineRenderer !=null)
            {
                UpddateLine(tempFingerPos);
            }
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            //drawObject = currentLine;
            // when mouse is unpressed
            newPos = UpdateList(fingerPos);
            
             var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             RaycastHit hit;
             if (Physics.Raycast(Ray, out hit) && lineRenderer != null)
             {
                
                CreateMesh(foot1);
                CreateMesh(foot2);
            }
            Destroy(currentLine);
            playerAnim.SetInteger("walk", 1);
            playerMovement.isGameStop = false;

        }
    }

    void CreateLine()
    {

        currentLine = Instantiate(linePref, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        fingerPos.Clear();
        fingerPos.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition)); 
        fingerPos.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer.SetPosition(0, fingerPos[0]);
        lineRenderer.SetPosition(1, fingerPos[1]);

    }
    void UpddateLine(Vector2 newFingerPos)
    {
        fingerPos.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
    }

    void CreateMesh(GameObject meshObject)
    {
        Vector3 cubePos;
        for (int i = 0; i < fingerPos.Count - 1; i++)
        {
            cubePos.x =  fingerPos[i].x;
            cubePos.z = meshObject.transform.position.z -10;
            cubePos.y = fingerPos[i].y;
            draw = Instantiate(drawObject, cubePos, Quaternion.identity);
            draw.transform.parent = meshObject.transform;

        }
     
         Combine(meshObject);
    }
    void Combine(GameObject meshObject)
    {

        Destroy(meshObject.gameObject.GetComponent<BoxCollider>());
        MeshFilter[] meshFilters =  meshObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Destroy(meshObject.gameObject.GetComponent<MeshCollider>());

        int i = 1;
        
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix; 
            meshFilters[i].gameObject.SetActive(false);
            Destroy(meshFilters[i].gameObject);
            i++;
            
        }
      
        meshObject.gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
        meshObject.gameObject.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true);
        meshObject.gameObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        meshObject.gameObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        meshObject.gameObject.GetComponent<MeshFilter>().mesh.Optimize();
        meshObject.AddComponent<BoxCollider>();

        if (meshObject.transform.position.y < 4)  Player.transform.position = new Vector3(Player.transform.position.x, 5, 10); 
        meshObject.gameObject.SetActive(true);
        meshObject.transform.localScale = new Vector3(0.3f,0.3f,0.3f);

        
    }


    List<Vector2> UpdateList(List<Vector2> positions)
    {
        float maxY = 0f, maxX=0f;
        
        for (int i = 0; i < positions.Count; i++)
        {
            if (positions[i].y > maxY) { maxY = positions[i].y; maxX = positions[i].x; }
        }
        for (int i = 0; i < positions.Count; i++)
        {
            positions[i] = new Vector2(positions[i].x - maxX, positions[i].y - maxY);
        }
        return positions;
    }
}




