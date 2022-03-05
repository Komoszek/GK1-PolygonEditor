using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Autor: Łukasz Komoszyński 305932
namespace GK1_PolygonEditor
{

    public partial class Form1 : Form
    {
        private Scene scene;

        private HoveredObjectType hoveredObjectType;
        private ConstraintType constraintType;
        private EditorState state;

        //Hovered figures and components(polygon's edges and vertices) ids
        private int polygonIdx;
        private int circleIdx;
        private int hoveredComponentId;

        // selected Edge/Circle
        private (int, int) selectedEdge;
        private int selectedCircle;

        //current and last mouse position
        private Point mousePosition;
        private Point lastPosition;

        //temp Figures for drawing
        private Polygon tempPolygon;
        private Circle tempCircle;

        private bool grabbing;
        private bool showCursor;
        private bool drawingCircle;
        private bool figureGrab;

        private float edgeSnap;
        private float R;
        private Font iconFont;
        private Brush iconBrush;
        float tangentEps;


        // variables necessary for 
        private Bitmap sceneBitmap;
        private Graphics bitmapGraphics;

        private bool autoTangent;

        private Pen selectedERPen;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scene = new Scene();

            scene.SetToDefaultScene();

            clearToolData();
            state = EditorState.Select;

            R = 6.0f;
            edgeSnap = 3.0f;
            tangentEps = 2.0f;
            iconFont = new Font(SystemFonts.DefaultFont.FontFamily, (float)(SystemFonts.DefaultFont.SizeInPoints+2.0f));
            iconBrush = Brushes.DarkOrange;
            sceneBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bitmapGraphics = Graphics.FromImage(sceneBitmap);

            selectedERPen = new Pen(Brushes.Green, 3.0f);
            autoTangent = false;
        }

        private bool isHoverableState(EditorState es)
        {
            return state == EditorState.Select || state == EditorState.RemoveVertex || state == EditorState.RemoveConstraint || state == EditorState.AddConstraint;
        }

        private Brush getVertexBrush(int pIdx, int i)
        {
            return isHoverableState(state) &&
                        ((hoveredObjectType == HoveredObjectType.Vertex &&
                        i == hoveredComponentId) || hoveredObjectType == HoveredObjectType.Polygon) &&
                        polygonIdx == pIdx ? Brushes.Red : Brushes.Purple;
        }

        private Brush getCenterBrush(int cIdx)
        {
            if (cIdx == selectedCircle) 
                return Brushes.DarkGreen;

            if (isHoverableState(state) &&
                    (hoveredObjectType == HoveredObjectType.CircleCenter || hoveredObjectType == HoveredObjectType.Circle)
                    && circleIdx == cIdx) return Brushes.Red;

            return Brushes.Purple;
        }

        private Pen getRingPen(int cIdx)
        {
            if (selectedCircle == cIdx)
                return selectedERPen;

            if (isHoverableState(state) && (hoveredObjectType == HoveredObjectType.Ring || hoveredObjectType == HoveredObjectType.Circle) && circleIdx == cIdx)
                return Pens.Red;

            return Pens.Black;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Bresenham part draw
            if(sceneBitmap != null)
            {
                bitmapGraphics.Clear(Color.White);

                Rasterizer.drawPolygonsBresenham(sceneBitmap, scene.polygons);

                e.Graphics.DrawImage(sceneBitmap, 0, 0,
                    sceneBitmap.Width, sceneBitmap.Height);
            }

            // draw circles + center
            for (int j = 0; j < scene.circles.Count; j++)
            {
                Circle c = scene.circles[j];
                Pen pen = getRingPen(j);
                e.Graphics.DrawEllipse(pen, c.center.X - c.radius, c.center.Y - c.radius, c.radius * 2, c.radius * 2);

                Brush brush = getCenterBrush(j);

                Rasterizer.drawPoint(e.Graphics, brush, c.center, R);

                if (scene.circleConstraints[j] != null)
                {
                    PointF pf = PointF.Add(c.center, new Size(0, 15));
                    scene.circleConstraints[j].drawIcon(e.Graphics, pf, iconFont, iconBrush, j.ToString());
                }
            }

            // draw hover edge
            if (hoveredObjectType == HoveredObjectType.Edge)
            {
                Polygon p = scene.polygons[polygonIdx];
                e.Graphics.DrawLine(Pens.Red, p.vertices[hoveredComponentId], p.vertices[(hoveredComponentId + 1) % p.vertices.Count]);
            } else if (hoveredObjectType == HoveredObjectType.Polygon) {
                Polygon p = scene.polygons[polygonIdx];

                for (int i = 0; i < p.vertices.Count; i++)
                {
                    e.Graphics.DrawLine(Pens.Red, p.vertices[i], p.vertices[(i + 1) % p.vertices.Count]);
                }
            }

            // draw selected edge
            if(selectedEdge.Item1 != -1)
            {
                Polygon p = scene.polygons[selectedEdge.Item1];

                e.Graphics.DrawLine(selectedERPen, p.vertices[selectedEdge.Item2], p.vertices[(selectedEdge.Item2 + 1) % p.vertices.Count]);
            }

            // draw polygon vertices and constraint icons
            for (int j = 0; j < scene.polygons.Count; j++)
            {
                Polygon polygon = scene.polygons[j];
                for (int i = 0; i < polygon.vertices.Count; i++)
                {

                    Brush brush = getVertexBrush(j, i);

                    Rasterizer.drawPoint(e.Graphics, brush, polygon.vertices[i], R);

                    if(scene.polygonsConstraints[j].edgeConstraints[i] != null)
                    {
                        int i1 = i;
                        int i2 = (i + 1) % polygon.vertices.Count;
                        PointF pf = new PointF((polygon.vertices[i1].X + polygon.vertices[i2].X)/2.0f, (polygon.vertices[i1].Y + polygon.vertices[i2].Y) / 2.0f);
                        scene.polygonsConstraints[j].edgeConstraints[i].drawIcon(e.Graphics, pf, iconFont, iconBrush, i.ToString());
                    }
                }
            }            

            if(state == EditorState.AddPolygon)
            {
                Rasterizer.drawAddPolygon(e.Graphics, tempPolygon, mousePosition, showCursor, R);
            } else if (state == EditorState.AddCircle) {
                // draw temp circle
                if(drawingCircle)
                    Rasterizer.drawAddCircle(e.Graphics, tempCircle, mousePosition, R);

                if (showCursor)
                    Rasterizer.drawPoint(e.Graphics, Brushes.Blue, mousePosition, R);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case EditorState.AddPolygon:
                    tempPolygon.vertices.Add(mousePosition);
                    pictureBox1.Update();
                    break;
                case EditorState.RemoveVertex:
                    switch (hoveredObjectType)
                    {
                        case HoveredObjectType.Vertex:
                            if (polygonIdx != -1 && hoveredComponentId != -1 && scene.polygons[polygonIdx].vertices.Count > 3)
                            {
                                PolygonConstraints pr = scene.polygonsConstraints[polygonIdx];

                                scene.polygons[polygonIdx].vertices.RemoveAt(hoveredComponentId);

                                int i1 = hoveredComponentId;
                                int i2 = (i1 - 1 + scene.polygons[polygonIdx].vertices.Count) % scene.polygons[polygonIdx].vertices.Count;

                                ConstraintHelper.removeEdgeConstraint(pr, i1);
                                ConstraintHelper.removeEdgeConstraint(pr, i2);

                                pr.edgeConstraints.RemoveAt(hoveredComponentId);
                                ConstraintHelper.fixRemoveEdgeConstraints(pr, hoveredComponentId);

                                ConstraintHelper.removeTangentToEdgeConstraints(scene.circleConstraints, polygonIdx, i1);
                                ConstraintHelper.removeTangentToEdgeConstraints(scene.circleConstraints, polygonIdx, i2);

                                hoveredComponentId = -1;
                                polygonIdx = -1;
                            }
                            break;
                        case HoveredObjectType.Polygon:
                            if(polygonIdx != -1)
                            {
                                scene.polygons.RemoveAt(polygonIdx);
                                scene.polygonsConstraints.RemoveAt(polygonIdx);

                                ConstraintHelper.removeTangentToPolygonConstraints(scene.circleConstraints, polygonIdx);
                                polygonIdx = -1;
                            }
                            break;
                        case HoveredObjectType.Circle:
                        case HoveredObjectType.CircleCenter:
                            if(circleIdx != -1)
                            {
                                scene.circles.RemoveAt(circleIdx);
                                scene.circleConstraints.RemoveAt(circleIdx);

                                circleIdx = -1;
                            }
                            break;
                    }
                    
                    break;
                case EditorState.SplitEdge:
                    {
                        if(polygonIdx != -1)
                        {
                            PolygonConstraints pr = scene.polygonsConstraints[polygonIdx];

                            Polygon p = scene.polygons[polygonIdx];
                            int i1 = hoveredComponentId;
                            int i2 = (i1 + 1) % p.vertices.Count;
                            PointF p1 = p.vertices[i1];
                            PointF p2 = p.vertices[i2];
                            p.vertices.Insert(i1 + 1, new PointF((p1.X + p2.X) / 2, ((p1.Y + p2.Y) / 2)));

                            ConstraintHelper.removeEdgeConstraint(pr, i1);
                            pr.edgeConstraints.Insert(i1 + 1, null);
                            ConstraintHelper.fixAddEdgeConstraints(pr, i1 + 1);

                            ConstraintHelper.removeTangentToEdgeConstraints(scene.circleConstraints, polygonIdx, i1);

                            hoveredObjectType = HoveredObjectType.None;
                            hoveredComponentId = -1;
                            polygonIdx = -1;
                        }
                    }
                    
                    break;
                case EditorState.RemoveConstraint:
                    if(hoveredObjectType == HoveredObjectType.Edge)
                    {
                        ConstraintHelper.removeEdgeConstraint(scene.polygonsConstraints[polygonIdx], hoveredComponentId);
                    } 
                    else if(hoveredObjectType == HoveredObjectType.CircleCenter || hoveredObjectType == HoveredObjectType.Ring)
                    {
                        scene.circleConstraints[circleIdx] = null;
                    }
                    break;
                case EditorState.AddConstraint:
                    switch (hoveredObjectType)
                    {
                        case HoveredObjectType.Circle:
                        case HoveredObjectType.CircleCenter:
                        case HoveredObjectType.Ring:
                            if (scene.circleConstraints[circleIdx] == null)
                            {
                                switch (constraintType)
                                {
                                    case ConstraintType.ConstCircleCenter:
                                        scene.circleConstraints[circleIdx] = new ConstCenter(scene.circles[circleIdx].center);
                                        setToSelectTool(false);

                                        break;
                                    case ConstraintType.ConstRadius:
                                        scene.circleConstraints[circleIdx] = new ConstRadius(scene.circles[circleIdx].radius);
                                        setToSelectTool(false);

                                        break;
                                    case ConstraintType.TangentCircle:
                                        if(selectedCircle == -1 && scene.circleConstraints[circleIdx] == null)
                                        {
                                            selectedCircle = circleIdx;
                                        }
                                        
                                        break;
                                }
                            }
                            break;
                        case HoveredObjectType.Edge:
                            if(constraintType == ConstraintType.TangentCircle)
                            {
                                if(selectedCircle != -1)
                                {
                                    //step two
                                    scene.circleConstraints[selectedCircle] = new TangentToEdge(hoveredComponentId, polygonIdx);
                                    updateCircleTangent((TangentToEdge)scene.circleConstraints[selectedCircle], scene.circles[selectedCircle], scene.polygons[polygonIdx]);
                                    setToSelectTool(false);
                                }

                                break;
                            }
                            if (scene.polygonsConstraints[polygonIdx].edgeConstraints[hoveredComponentId] == null)
                            {
                                switch (constraintType)
                                {
                                    case ConstraintType.ConstEdgeLength:
                                        {
                                            int i1 = hoveredComponentId;
                                            int i2 = (hoveredComponentId + 1) % scene.polygons[polygonIdx].vertices.Count;

                                            scene.polygonsConstraints[polygonIdx].edgeConstraints[hoveredComponentId] =
                                                new ConstantEdgeLength((float)DistanceCalculator.GetDistance(scene.polygons[polygonIdx].vertices[i1], scene.polygons[polygonIdx].vertices[i2]));

                                            setToSelectTool(false);
                                        }
                                        break;
                                    case ConstraintType.ParallelEdges:

                                        if (selectedEdge.Item1 == -1)
                                        {
                                            selectedEdge = (polygonIdx, hoveredComponentId);
                                        } 
                                        else
                                        {
                                            if(selectedEdge.Item1 == polygonIdx)
                                            {
                                                int i1 = (hoveredComponentId + 1) % scene.polygons[polygonIdx].vertices.Count;
                                                int i2 = (hoveredComponentId - 1 + scene.polygons[polygonIdx].vertices.Count) % scene.polygons[polygonIdx].vertices.Count;

                                                if (selectedEdge.Item2 == i1 || selectedEdge.Item2 == i2) break;

                                                scene.polygonsConstraints[polygonIdx].edgeConstraints[hoveredComponentId] = new ParallelEdges(selectedEdge.Item2);
                                                scene.polygonsConstraints[polygonIdx].edgeConstraints[selectedEdge.Item2] = new ParallelEdges(hoveredComponentId);
                                                GCS.optimizePolygon(scene.polygons[polygonIdx], scene.polygonsConstraints[polygonIdx], -1);
                                                updateAllCircleTangent(scene.circleConstraints, scene.circles, polygonIdx, scene.polygons[polygonIdx]);

                                                setToSelectTool(false);

                                            }
                                        }
                                        
                                        break;
                                    case ConstraintType.EqualEdges:
                                        if (selectedEdge.Item1 == -1) // step 1
                                        {
                                            selectedEdge = (polygonIdx, hoveredComponentId);
                                        }
                                        else
                                        {
                                            if (selectedEdge.Item1 == polygonIdx) // step 2
                                            {
                                                scene.polygonsConstraints[polygonIdx].edgeConstraints[hoveredComponentId] = new EqualEdgesLengths(selectedEdge.Item2);
                                                scene.polygonsConstraints[polygonIdx].edgeConstraints[selectedEdge.Item2] = new EqualEdgesLengths(hoveredComponentId);
                                                GCS.optimizePolygon(scene.polygons[polygonIdx], scene.polygonsConstraints[polygonIdx], -1);
                                                updateAllCircleTangent(scene.circleConstraints, scene.circles, polygonIdx, scene.polygons[polygonIdx]);

                                                setToSelectTool(false);

                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        

        private double CheckEdgeHover(double minDistance)
        {
            for (int j = 0; j < scene.polygons.Count; j++)
            {
                Polygon p = scene.polygons[j];
                for (int i = 0; i < p.vertices.Count; i++)
                {
                    int i2 = (i + 1) % p.vertices.Count;

                    double lineLength = DistanceCalculator.GetDistance2(p.vertices[i], p.vertices[i2]);
                    double tempDistance = DistanceCalculator.DistanceToEdge2(p.vertices[i], p.vertices[i2], mousePosition);

                    if (tempDistance < R * R && minDistance > tempDistance)
                    {
                        double mx = DistanceCalculator.GetDistance2(p.vertices[i], mousePosition);
                        double m2 = DistanceCalculator.GetDistance2(p.vertices[i2], mousePosition);

                        if (mx < m2)
                            mx = m2;

                        if (lineLength + tempDistance >= mx)
                        {
                            polygonIdx = j;
                            hoveredComponentId = i;
                            hoveredObjectType = HoveredObjectType.Edge;
                        }
                    }

                }
            }

            return minDistance;
        }

        private double CheckVertexHover(double minDistance)
        {
            for (int j = 0; j < scene.polygons.Count; j++)
            {
                Polygon p = scene.polygons[j];
                for (int i = 0; i < p.vertices.Count; i++)
                {
                    double tempDistance = DistanceCalculator.GetDistance2(p.vertices[i], mousePosition);

                    if (tempDistance < R * R && minDistance > tempDistance)
                    {
                        polygonIdx = j;
                        hoveredComponentId = i;
                        hoveredObjectType = HoveredObjectType.Vertex;
                    }

                }
            }

            return minDistance;
        }

        private double CheckCircleCenterHover(double minDistance)
        {
            for(int j = 0; j < scene.circles.Count; j++)
            {
                Circle c = scene.circles[j];
                double tempDistance = DistanceCalculator.GetDistance2(c.center, mousePosition);

                if (tempDistance < R * R && minDistance > tempDistance)
                {
                    //tempCircle = c;
                    circleIdx = j;
                    hoveredObjectType = HoveredObjectType.CircleCenter;
                }
            }

            return minDistance;
        }

        private void updateSceneCollision()
        {
            // polygon vertex
            double minDistance = Double.PositiveInfinity;

            //tempPolygon = null;
            polygonIdx = -1;
            circleIdx = -1;
            hoveredComponentId = -1;
            hoveredObjectType = HoveredObjectType.None;

            minDistance = CheckVertexHover(minDistance);

            // polygon edge
            if (hoveredObjectType == HoveredObjectType.None)
                minDistance = CheckEdgeHover(minDistance);

            // circle Vertex;

            minDistance = CheckCircleCenterHover(minDistance);

            // circle  Ring

            for (int j = 0; j < scene.circles.Count; j++)
            {
                Circle c = scene.circles[j];
                if (c.radius + edgeSnap < R) continue;
                double tempDistance = DistanceCalculator.DistanceToCircle(c, mousePosition);

                if (tempDistance < edgeSnap && minDistance > tempDistance)
                {
                    //tempCircle = c;
                    circleIdx = j;
                    hoveredObjectType = HoveredObjectType.Ring;
                }
            }

            if (figureGrab)
            {
                if (hoveredObjectType == HoveredObjectType.Vertex || hoveredObjectType == HoveredObjectType.Edge)
                {
                    hoveredObjectType = HoveredObjectType.Polygon;
                }
                else if (hoveredObjectType == HoveredObjectType.CircleCenter || hoveredObjectType == HoveredObjectType.Ring)
                {
                    hoveredObjectType = HoveredObjectType.Circle;
                }
            }
        }
        private void updateCircleTangent(TangentToEdge tte, Circle c, Polygon p)
        {
            c.radius = tte.getRadius(c, p);
        }

        private void updateAllCircleTangent(List<CircleConstraint> ccList, List<Circle> circleList, int pIdx, Polygon p)
        {
            for(int i = 0; i < ccList.Count; i++)
            {
                if(ccList[i] is TangentToEdge)
                {
                    TangentToEdge tte = (TangentToEdge)ccList[i];
                    if(tte.polygonIdx == pIdx)
                    {
                        updateCircleTangent(tte, circleList[i], p);
                    }
                }
            }
        }

        private (int, int) getNearestTangentEdge(Circle c, List<Polygon> pl)
        {
            float minDistance = float.PositiveInfinity;
            int pIdx = -1;
            int eIdx = -1;
            for (int i = 0; i < pl.Count; i++)
            {
                for(int j = 0; j < pl[i].vertices.Count; j++)
                {
                    int i1 = j;
                    int i2 = (j + 1) % pl[i].vertices.Count;
                    float tempDistance = (float)Math.Abs(DistanceCalculator.DistanceToEdge(pl[i].vertices[i1], pl[i].vertices[i2], c.center) - c.radius);
                    
                    if(tempDistance < minDistance && tempDistance < tangentEps)
                    {
                        pIdx = i;
                        eIdx = j;
                        minDistance = tempDistance;
                    }
                }
            }

            return (pIdx, eIdx);
        }

        private (int, int) getNearestTangentPolygon(Polygon p, List<Circle> cl, List<CircleConstraint> ccl)
        {
            float minDistance = float.PositiveInfinity;
            int cIdx = -1;
            int eIdx = -1;
            for(int i = 0; i < cl.Count; i++)
            {
                if(ccl[i] == null)
                {
                    for(int j = 0; j < p.vertices.Count; j++)
                    {
                        int i1 = j;
                        int i2 = (j + 1) % p.vertices.Count;
                        float tempDistance = (float)Math.Abs(DistanceCalculator.DistanceToEdge(p.vertices[i1], p.vertices[i2], cl[i].center) - cl[i].radius);

                        if (tempDistance < minDistance && tempDistance < tangentEps)
                        {
                            cIdx = i;
                            eIdx = j;
                            minDistance = tempDistance;
                        }
                    }
                }
            }

            return (cIdx, eIdx);
        }

        private void applyAutoTangentPolygon()
        {
            if (autoTangent)
            {
                (int, int) temp = getNearestTangentPolygon(scene.polygons[polygonIdx], scene.circles, scene.circleConstraints);

                selectedCircle = temp.Item1;
                selectedEdge = selectedCircle == -1 ? (-1, -1) : (polygonIdx, temp.Item2);
            }
        }

        private void applyAutoTangentCircle()
        {
            if (autoTangent && scene.circleConstraints[circleIdx] == null)
            {
                selectedEdge = getNearestTangentEdge(scene.circles[circleIdx], scene.polygons);
                selectedCircle = selectedEdge.Item1 == -1 ? -1 : circleIdx;
            }
        }


        private void selectMoveEntity(Point lastPosition, Point currPosition, HoveredObjectType objType)
        {
            switch (objType)
            {
                case HoveredObjectType.Circle:
                    if (circleIdx != -1)
                    {
                        if (scene.circleConstraints[circleIdx] is ConstCenter) break;

                        float dx = currPosition.X - lastPosition.X;
                        float dy = currPosition.Y - lastPosition.Y;

                        scene.circles[circleIdx].center = new PointF(scene.circles[circleIdx].center.X + dx, scene.circles[circleIdx].center.Y + dy);
                            
                        if( scene.circleConstraints[circleIdx] is TangentToEdge)
                        {
                            TangentToEdge tte = (TangentToEdge)scene.circleConstraints[circleIdx];
                            updateCircleTangent(tte, scene.circles[circleIdx], scene.polygons[tte.polygonIdx]);
                        }

                        applyAutoTangentCircle();
                    }
                    break;
                case HoveredObjectType.CircleCenter:
                    if(circleIdx != -1)
                    {
                        if (scene.circleConstraints[circleIdx] != null && scene.circleConstraints[circleIdx] is ConstCenter) break;

                        scene.circles[circleIdx].center = mousePosition;

                        if (scene.circleConstraints[circleIdx] is TangentToEdge)
                        {
                            TangentToEdge tte = (TangentToEdge)scene.circleConstraints[circleIdx];
                            updateCircleTangent(tte, scene.circles[circleIdx], scene.polygons[tte.polygonIdx]);
                        }


                        applyAutoTangentCircle();
                    }
                        
                    break;
                case HoveredObjectType.Ring:
                    if (circleIdx != -1)
                    {
                        if (scene.circleConstraints[circleIdx] is ConstRadius || scene.circleConstraints[circleIdx] is TangentToEdge) break;

                        scene.circles[circleIdx].radius = (int)DistanceCalculator.GetDistance(scene.circles[circleIdx].center, mousePosition);

                        applyAutoTangentCircle();
                    }
                    break;
                case HoveredObjectType.Vertex:
                    scene.polygons[polygonIdx].vertices[hoveredComponentId] = mousePosition;

                    GCS.optimizePolygon(scene.polygons[polygonIdx], scene.polygonsConstraints[polygonIdx], hoveredComponentId);
                    updateAllCircleTangent(scene.circleConstraints, scene.circles, polygonIdx, scene.polygons[polygonIdx]);

                    applyAutoTangentPolygon();

                    break;
                case HoveredObjectType.Polygon:
                    if (polygonIdx != -1)
                    {
                        float dx = currPosition.X - lastPosition.X;
                        float dy = currPosition.Y - lastPosition.Y;

                        for (int i = 0; i < scene.polygons[polygonIdx].vertices.Count; i++)
                        {
                            scene.polygons[polygonIdx].vertices[i] = new PointF(scene.polygons[polygonIdx].vertices[i].X + dx, scene.polygons[polygonIdx].vertices[i].Y + dy);
                        }

                        updateAllCircleTangent(scene.circleConstraints, scene.circles, polygonIdx, scene.polygons[polygonIdx]);

                        applyAutoTangentPolygon();
                    }
                    break;
                case HoveredObjectType.Edge:
                    if (polygonIdx != -1)
                    {
                        float dx = currPosition.X - lastPosition.X;
                        float dy = currPosition.Y - lastPosition.Y;

                        int i1 = hoveredComponentId;
                        int i2 = (i1 + 1) % scene.polygons[polygonIdx].vertices.Count;
                        scene.polygons[polygonIdx].vertices[i1] = new PointF(scene.polygons[polygonIdx].vertices[i1].X + dx, scene.polygons[polygonIdx].vertices[i1].Y + dy);
                        scene.polygons[polygonIdx].vertices[i2] = new PointF(scene.polygons[polygonIdx].vertices[i2].X + dx, scene.polygons[polygonIdx].vertices[i2].Y + dy);

                        GCS.optimizePolygon(scene.polygons[polygonIdx], scene.polygonsConstraints[polygonIdx], -1);
                        updateAllCircleTangent(scene.circleConstraints, scene.circles, polygonIdx, scene.polygons[polygonIdx]);

                        applyAutoTangentPolygon();

                    }
                    break;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            lastPosition = mousePosition;
            mousePosition = e.Location;
            switch (state)
            {
                case EditorState.SplitEdge:
                    polygonIdx = -1;
                    hoveredComponentId = -1;
                    hoveredObjectType = HoveredObjectType.None;
                    CheckEdgeHover(Double.PositiveInfinity);
                    break;
                case EditorState.Select:
                    if (grabbing)
                    {
                        selectMoveEntity(lastPosition, mousePosition, hoveredObjectType);
                        break;
                    }
                    updateSceneCollision();
                    break;
                case EditorState.RemoveVertex:
                case EditorState.RemoveConstraint:
                case EditorState.AddConstraint:
                    updateSceneCollision();
                    break;
                case EditorState.AddCircle:
                    if (drawingCircle)
                    {
                        tempCircle.radius = (int)Math.Sqrt(DistanceCalculator.GetDistance2(tempCircle.center, mousePosition));
                    }
                    break;
            }

            pictureBox1.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (state == EditorState.AddPolygon && e.KeyCode == Keys.Escape && tempPolygon.vertices.Count > 0)
            {
                if(tempPolygon.vertices.Count > 2)
                {
                    Polygon tp = tempPolygon;
                    scene.polygons.Add(tp);
                    scene.polygonsConstraints.Add(new PolygonConstraints(tempPolygon.vertices.Count));
                }

                setToSelectTool(false);

                return;
            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (state)
            {
                case EditorState.Select:
                        grabbing = hoveredObjectType != HoveredObjectType.None;
                    break;
                case EditorState.AddCircle:
                    if (!drawingCircle)
                    {
                        if (tempCircle == null)
                            tempCircle = new Circle(new Point(e.X, e.Y), 0);

                        drawingCircle = true;
                    }
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            grabbing = false;

            if(state == EditorState.AddCircle && drawingCircle)
            {
                drawingCircle = false;
                scene.circles.Add(tempCircle);
                scene.circleConstraints.Add(null);
                setToSelectTool(false);
                return;
            }

            if(state == EditorState.Select && autoTangent && selectedCircle != -1)
            {
                TangentToEdge tte = new TangentToEdge(selectedEdge.Item2, selectedEdge.Item1);
                scene.circleConstraints[selectedCircle] = tte;
                scene.circles[selectedCircle].radius = tte.getRadius(scene.circles[selectedCircle], scene.polygons[selectedEdge.Item1]);
                selectedEdge = (-1, -1);
                selectedCircle = -1;
                updateSceneCollision();
                pictureBox1.Refresh();
            }
        }

        private void addPolygonButton_Click(object sender, EventArgs e)
        {
            if(state != EditorState.AddPolygon)
            {
                tempPolygon = new Polygon();
                state = EditorState.AddPolygon;
            }

        }

        private void setToSelectTool(bool wholeFigure)
        {
            if (state != EditorState.Select)
            {
                clearToolData();
                state = EditorState.Select;
            }

            if (figureGrab != wholeFigure)
            {
                figureGrab = wholeFigure;
            }

            updateSceneCollision();
            pictureBox1.Refresh();
        }

        private void selectElementButton_Click(object sender, EventArgs e)
        {
            setToSelectTool(false);
        }
        private void setToDeleteTool(bool wholeFigure)
        {
            if (state != EditorState.RemoveVertex)
            {
                clearToolData();
                state = EditorState.RemoveVertex;
            }

            if (figureGrab != wholeFigure)
            {
                figureGrab = wholeFigure;

            }

            updateSceneCollision();
            pictureBox1.Refresh();

        }

        private void clearToolData()
        {
            pictureBox1.Cursor = default;
            tempPolygon = null;
            tempCircle = null;
            circleIdx = -1;
            polygonIdx = -1;
            grabbing = false;
            hoveredComponentId = -1;
            figureGrab = false;
            selectedCircle = -1;
            selectedEdge = (-1, -1);
            hoveredObjectType = HoveredObjectType.None;
        }

        private void setToRemoveConstraintTool()
        {
            if(state != EditorState.RemoveConstraint)
            {
                clearToolData();
                state = EditorState.RemoveConstraint;
            }

            updateSceneCollision();
            pictureBox1.Refresh();
        }

        private void setToAddConstraintTool(ConstraintType cType)
        {
            if(state != EditorState.AddConstraint || constraintType != cType)
            {
                clearToolData();
                state = EditorState.AddConstraint;
                constraintType = cType;
            }

            updateSceneCollision();
            pictureBox1.Refresh();
        }
        private void deleteElementButton_Click(object sender, EventArgs e)
        {
            setToDeleteTool(false);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            showCursor = false;
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {  
            showCursor = state == EditorState.AddCircle || state == EditorState.AddPolygon;
        }

        private void addCircleButton_Click(object sender, EventArgs e)
        {
            if(state != EditorState.AddCircle)
            {
                state = EditorState.AddCircle;
                tempCircle = null;
            }
        }
        private void splitEdgeButton_Click(object sender, EventArgs e)
        {
            if(state != EditorState.SplitEdge)
            {
                tempPolygon = null;
                polygonIdx = -1;
                hoveredObjectType = HoveredObjectType.None;
                hoveredComponentId = -1;
                state = EditorState.SplitEdge;
            }

            updateSceneCollision();
            pictureBox1.Refresh();
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            sceneBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bitmapGraphics = Graphics.FromImage(sceneBitmap);
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (sceneBitmap == null || sceneBitmap.Width != pictureBox1.Width || sceneBitmap.Height != pictureBox1.Height)
            {
                sceneBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                bitmapGraphics = Graphics.FromImage(sceneBitmap);
            }
        }

        private void selectFigureButton_Click(object sender, EventArgs e)
        {
            setToSelectTool(true);
        }

        private void deleteFigureButton_Click(object sender, EventArgs e)
        {
            setToDeleteTool(true);
        }
        private void removeConstraintButton_Click(object sender, EventArgs e)
        {
            setToRemoveConstraintTool();
        }

        private void constantRadiusButton_Click(object sender, EventArgs e)
        {
            setToAddConstraintTool(ConstraintType.ConstRadius);
        }

        private void blockCenterButton_Click(object sender, EventArgs e)
        {
            setToAddConstraintTool(ConstraintType.ConstCircleCenter);
        }

        private void ConstantEdgeButton_Click_1(object sender, EventArgs e)
        {
            setToAddConstraintTool(ConstraintType.ConstEdgeLength);
        }

        private void equalEdgesButton_Click(object sender, EventArgs e)
        {
            setToAddConstraintTool(ConstraintType.EqualEdges);
        }

        private void parallelEdgesButton_Click(object sender, EventArgs e)
        {
            setToAddConstraintTool(ConstraintType.ParallelEdges);
        }

        private void circleTangentButton_Click(object sender, EventArgs e)
        {
            setToAddConstraintTool(ConstraintType.TangentCircle);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if(state == EditorState.Select && !figureGrab)
            {
                switch (hoveredObjectType)
                {
                    case HoveredObjectType.Edge:
                        if(scene.polygonsConstraints[polygonIdx].edgeConstraints[hoveredComponentId] is ConstantEdgeLength)
                        {
                            ConstantEdgeLength cel = (ConstantEdgeLength)scene.polygonsConstraints[polygonIdx].edgeConstraints[hoveredComponentId];

                            Form2 inputBox = new Form2();

                            inputBox.SetLabel("Edege length (in pixels):");
                            inputBox.SetNumericValue(cel.length);

                            if(inputBox.ShowDialog() == DialogResult.OK)
                            {
                                cel.length = (float)inputBox.GetNumericValue();
                                GCS.optimizePolygon(scene.polygons[polygonIdx], scene.polygonsConstraints[polygonIdx], -1);
                                updateAllCircleTangent(scene.circleConstraints, scene.circles, polygonIdx, scene.polygons[polygonIdx]);
                                updateSceneCollision();
                                pictureBox1.Refresh();

                            }

                        }
                        break;
                    case HoveredObjectType.CircleCenter:
                    case HoveredObjectType.Ring:
                        if (scene.circleConstraints[circleIdx] is ConstRadius)
                        {
                            ConstRadius cr = (ConstRadius)scene.circleConstraints[circleIdx];

                            Form2 inputBox = new Form2();

                            inputBox.SetLabel("Circle radius (in pixels):");
                            inputBox.SetNumericValue(cr.radius);

                            if (inputBox.ShowDialog() == DialogResult.OK)
                            {
                                cr.radius = (float)inputBox.GetNumericValue();
                                scene.circles[circleIdx].radius = cr.radius;
                                updateSceneCollision();
                                pictureBox1.Refresh();
                            }
                        }
                        break;
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scene.clearScene();
            clearToolData();
            updateSceneCollision();
            pictureBox1.Refresh();
        }

        private void autoCircleTangentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoTangent = autoCircleTangentToolStripMenuItem.Checked;
        }
    }
}
