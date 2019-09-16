using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using System.Collections.Generic;

public class CircularImage : Image
{
    /// <summary>
    /// 圆形由多少个三角块拼成
    /// </summary> 
    [SerializeField]
    private int segements = 100;

    /// <summary>
    /// 显示出来的百分比
    /// </summary>
    [SerializeField]
    private float percent = 1;

    private List<Vector2> _vertexList = new List<Vector2>();

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        toFill.Clear();
        AddVertex(toFill);

        for(int i = 1; i <= segements; i++)
        {
            toFill.AddTriangle(i, 0, i + 1);
        }
    }

    private void AddVertex(VertexHelper toFill)
    {
        //获得Rect长宽和需要显示为亮的三角片的数量
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        int whiteSegement = (int)(segements * percent);

        //计算原点的uv和uv与长宽的换算关系
        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
        float uvWidth = uv.z - uv.x;
        float uvHeight = uv.w - uv.y;
        Vector2 uvCenter = new Vector2(uvWidth / 2, uvHeight / 2);
        Vector2 converRatio = new Vector2(uvWidth / width, uvHeight / height);

        //求出每个三角块的弧度和圆形的半径
        float radian = (2 * Mathf.PI) / segements;
        float radius = Mathf.Min(height, width) / 2;

        //计算原点的顶点数据
        UIVertex orgin = new UIVertex();
        byte tempColor = (byte)(percent * 255);
        orgin.color = new Color32(tempColor, tempColor, tempColor, 255);
        //当pivot不是(0.5, 0.5)的时候，依然要保证图片显示在原来位置
        Vector2 orginPos = new Vector2((0.5f - rectTransform.pivot.x) * width, (0.5f - rectTransform.pivot.y) * height);
        orgin.position = orginPos;
        orgin.uv0 = new Vector2(uvCenter.x, uvCenter.y);
        toFill.AddVert(orgin);

        int vertexCount = segements + 1;
        float curRadian = 0f;
        //对于圆周上每一个顶点的计算
        for (int i = 0; i < vertexCount; i++)
        {
            //计算圆周上顶点与圆心之间x，y值差值
            float x = Mathf.Cos(curRadian) * radius;
            float y = Mathf.Sin(curRadian) * radius;
            curRadian += radian;

            UIVertex tempVertex = new UIVertex();
            //三角片的颜色区分
            if (i <= whiteSegement)
                tempVertex.color = color;
            else
                tempVertex.color = new Color32(60, 60, 60, 255);
            //计算每一个顶点的位置，使用圆心位置加上距离圆心的差值
            Vector2 tempPos = new Vector2(x, y) + orginPos;
            tempVertex.position = tempPos;
            //计算每个顶点对应的uv值，x * converRatio.x是相对于圆心的uv，所以再加上圆心的uv得到顶点的uv
            tempVertex.uv0 = new Vector2(x * converRatio.x + uvCenter.x, y * converRatio.y + uvCenter.y);
            toFill.AddVert(tempVertex);
            _vertexList.Add(tempPos);
        }
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out Vector2 localPoint);
        return IsVaild(localPoint);
    }

    private bool IsVaild(Vector2 localPoint)
    {
        return GetCrossPointNum(localPoint, _vertexList) % 2 == 1;
    }

    private int GetCrossPointNum(Vector2 localPoint, List<Vector2> vertexList)
    {
        Vector2 vert1 = Vector2.zero;
        Vector2 vert2 = Vector2.zero;
        int count = vertexList.Count;
        int crossPointCount = 0;

        for(int i = 0; i < count; i++)
        {
            vert1 = vertexList[i];
            vert2 = vertexList[(i + 1)% count];

            if (IsYInRange(localPoint, vert1, vert2) && localPoint.x < GetXInLine(vert1, vert2, localPoint.y))
            {
                crossPointCount++;
            }
        }
        return crossPointCount;
    }

    private bool IsYInRange(Vector2 localPoint, Vector2 vert1, Vector2 vert2)
    {
        return localPoint.y > Mathf.Min(vert1.y, vert2.y) &&
            localPoint.y < Mathf.Max(vert1.y, vert2.y);
    }

    private float GetXInLine(Vector2 vert1, Vector2 vert2, float y)
    {
        float k = (vert1.y - vert2.y) / (vert1.x - vert2.x);
        return (y - vert2.y) / k + vert2.x;
    }
}
