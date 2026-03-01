using UnityEngine;

namespace FUTUREVISION.Content
{
    /// <summary>
    /// OpenStreetMap 기반 도로 로더 스텁.
    /// 실제 구현 시 OSM API 연동 필요.
    /// </summary>
    public class OSMRoadLoader : MonoBehaviour
    {
        public double centerLat;
        public double centerLon;

        public void UpdateRoad()
        {
            Debug.LogWarning("[OSMRoadLoader] UpdateRoad() 미구현 상태입니다.");
        }

        public Vector3 GeoToUnity(double latitude, double longitude)
        {
            Debug.LogWarning("[OSMRoadLoader] GeoToUnity() 미구현 상태입니다.");
            return Vector3.zero;
        }
    }
}
