using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Vector2 _boxSize = new Vector2(0.8f, 0.2f); // 박스 크기 (가로, 세로)
    [SerializeField] private float _offset = -0.1f; 
    [SerializeField] private bool _drawGizmo = true;

    public bool toggleChecker = true;

    private void FixedUpdate()
    {
        bool isGrounded = IsGrounded();
        Player.Instance.OnGround = isGrounded;

        // 경사면 법선 벡터 계산
        if (isGrounded)
        {
            Vector2 rayOrigin = (Vector2)transform.position + Vector2.up * _offset;
            // 박스 크기의 절반보다 조금 더 길게 쏴서 바닥을 확실히 체크
            float rayDistance = _boxSize.y + 0.5f;
            
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, _groundLayer);
            
            if (hit.collider != null)
            {
                Player.Instance.GroundNormal = hit.normal;
                // 디버그용: 법선 그리기
                Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            }
            else
            {
                Player.Instance.GroundNormal = Vector2.up;
            }
            // 노말 y 값이 1보다 작으면 슬로프위에 있는것
            if (Player.Instance.GroundNormal.y < 1f) Player.Instance.OnSlope = true;
            else Player.Instance.OnSlope = false;
        }
        else
        {
            Player.Instance.GroundNormal = Vector2.up;
        }
    }
    private void OnDrawGizmos()
    {
        if (!_drawGizmo)
            return;

        Gizmos.color = Color.cyan;
        // 박스 중심 위치 계산
        Vector3 center = transform.position + Vector3.up * _offset;
        // 박스 그리기
        Gizmos.DrawWireCube(center, _boxSize);
    }

    public bool IsGrounded()
    {
        if (!toggleChecker) return false;

        // 박스 중심 위치
        Vector2 center = (Vector2)transform.position + Vector2.up * _offset;

        // Physics2D.OverlapBox(중심, 크기, 회전각, 레이어마스크)
        // 충돌체가 하나라도 있으면 true를 반환 (Collider2D 리턴되므로 null 체크)
        Collider2D hit = Physics2D.OverlapBox(center, _boxSize, 0f, _groundLayer);

        return hit != null;
    }
}