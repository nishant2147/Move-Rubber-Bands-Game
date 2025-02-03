using UnityEngine;

public class LineRenderGenerate : MonoBehaviour
{
    
    public Transform dot1;
    public Transform dot2;
    public LineRenderer lineRenderer;
    private Camera mainCamera;

    public bool isDraggingline1 = false;
    public bool isDraggingline2 = false;

    public LayerMask target;

    private Vector3 lastline1Position;
    private Vector3 lastline2Position;
    
    public float maxDragDistance;

    public Vector3 originalline1Position;
    public Vector3 originalline2Position;

    //public Transform[] targetPositions;
    //public float winTolerance = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;

        mainCamera = Camera.main;

        /* lastDot1Position = dot1.position;
         lastDot2Position = dot2.position;*/

        lineRenderer.SetPosition(0, dot1.position);
        lineRenderer.SetPosition(1, dot2.position);

        originalline1Position = dot1.position;
        originalline2Position = dot2.position;
    }

    // Update is called once per frame         
    void Update()
    {
        /*lineRenderer.SetPosition(0, isDraggingDot1 ? GetPosition(GetMouseWorldPosition()) : lastDot1Position);            
        lineRenderer.SetPosition(1, isDraggingDot2 ? GetPosition(GetMouseWorldPosition()) : lastDot2Position);*/

        HandleMouseInput();

        /*if (CheckWinCondition())        
        {
            Debug.Log("Level Completed!");      
        }*/
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = GetMouseWorldPosition();
            float distanceToDot1 = Vector2.Distance(mousePos, lineRenderer.GetPosition(0));
            float distanceToDot2 = Vector2.Distance(mousePos, lineRenderer.GetPosition(1));

            if (distanceToDot1 < 0.5f)
            {
                isDraggingline1 = true;
            }
            else if (distanceToDot2 < 0.5f)
            {
                isDraggingline2 = true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = GetMouseWorldPosition();
            Vector3 pos = ClampPosition(mousePos);
            Vector3 clampedPos = GetPositions(pos);

            if (isDraggingline1)
            {
                lineRenderer.SetPosition(0, clampedPos);
            }
            else if (isDraggingline2)
            {
                lineRenderer.SetPosition(1, clampedPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            /*if (isDraggingDot1)
                lastDot1Position = GetPosition(GetMouseWorldPosition());         
            if (isDraggingDot2)
                lastDot2Position = GetPosition(GetMouseWorldPosition());*/

            isDraggingline1 = false;
            isDraggingline2 = false;

            lineRenderer.SetPosition(0, originalline1Position);
            lineRenderer.SetPosition(1, originalline2Position);
        }
    }
    private Vector3 ClampPosition(Vector3 position)
    {
        if (isDraggingline1)
        {
            Vector3 direction = position - originalline1Position;
            float distance = direction.magnitude;

            if (distance > maxDragDistance)      
            {
                direction = direction.normalized * maxDragDistance;                 
            }

            return originalline1Position + direction;
        }
        else if (isDraggingline2)
        {
            Vector3 direction = position - originalline2Position;
            float distance = direction.magnitude;

            if (distance > maxDragDistance)
            {
                direction = direction.normalized * maxDragDistance;
            }

            return originalline2Position + direction;
        }

        return position;
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;       
        mouseScreenPos.z = 10f;       
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }
    private Vector3 GetPositions(Vector3 mouseWorldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, target);

        if (hit.collider != null)
        {
            if (isDraggingline1)
                originalline1Position = hit.collider.transform.position;
            else if (isDraggingline2)
                originalline2Position = hit.collider.transform.position;

            if (hit.collider.CompareTag("red") && gameObject.CompareTag("line1"))
            {
                if (lineRenderer != null && lineRenderer.GetPosition(lineRenderer.positionCount - 1) == hit.collider.transform.position)
                {
                    Debug.Log("Level Completed red!");
                    //isRedInPosition = true;
                    LevelScript.instance.red = true;
                }
            }
            else if (hit.collider.CompareTag("green") && gameObject.CompareTag("line2"))
            {
                if (lineRenderer != null && lineRenderer.GetPosition(lineRenderer.positionCount - 1) == hit.collider.transform.position)
                {
                    Debug.Log("Level Completed green!");
                    // isGreenInPosition = true;
                    LevelScript.instance.green = true;
                }
            }

            if (LevelScript.instance.red && LevelScript.instance.green)
            {
                Debug.Log("Level Completed!");
            }

            ///Debug.Log("===>" + hit.collider.name);              
            return hit.collider.transform.position;
        }

        return mouseWorldPos;
    }
    /*private Vector3 GetlineRendererPullRange(Vector3 initialPos, Vector3 targetPos)
    {
        Vector3 direction = targetPos - initialPos;

        float distance = direction.magnitude;
        if (distance > maxDragDistance)
        {
            direction = direction.normalized * maxDragDistance;
            //Debug.Log("<<<<" + direction);           
        }

        return initialPos + direction;
    }*/
    private Vector3 GetlineRendererPosition(Vector3 currentPosition)
    {
        Collider2D closestPoint = Physics2D.OverlapCircle(currentPosition, 0.2f, target);

        if (closestPoint != null)
        {
            Debug.Log("-----> " + closestPoint);
            return closestPoint.transform.position;
        }

        //Debug.Log("....." + currentPosition);           
        return currentPosition;
    }
    /*private bool CheckWinCondition()      
    {
        if (targetPositions.Length < 2) return false;      

        bool dot1Matched = Vector2.Distance(lineRenderer.GetPosition(0), targetPositions[0].position) <= winTolerance;               

        bool dot2Matched = Vector2.Distance(lineRenderer.GetPosition(1), targetPositions[1].position) <= winTolerance;                   

        return dot1Matched && dot2Matched;                     
    }*/
}
