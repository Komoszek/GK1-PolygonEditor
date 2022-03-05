using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace GK1_PolygonEditor
{

    public static class GCS
    {
        const float MaxGrad = 4;
        const int MaxIters = 1500;
        const float h = 0.001f;
        const float eps = 1e-7F;

        private static PointF calculatePoint(Polygon poly, PolygonConstraints pc, int pIdx)
        {
            float initialError = 0;
            float xChangeError = 0;
            float yChangeError = 0;


            for (int i = 0; i < pc.edgeConstraints.Count; i++)
            {
                if(pc.edgeConstraints[i] != null)
                {
                    float err = pc.edgeConstraints[i].calculateError(poly, i);
                    initialError += err;
                }
            }

            PointF pLast = poly.vertices[pIdx];
            poly.vertices[pIdx] = new PointF(pLast.X + h, pLast.Y);

            for (int i = 0; i < pc.edgeConstraints.Count; i++)
            {
                if (pc.edgeConstraints[i] != null)
                {
                    float err = pc.edgeConstraints[i].calculateError(poly, i);

                    xChangeError += err;
                }
            }

            poly.vertices[pIdx] = new PointF(pLast.X, pLast.Y + h);

            for (int i = 0; i < pc.edgeConstraints.Count; i++)
            {
                if (pc.edgeConstraints[i] != null)
                {
                    float err = pc.edgeConstraints[i].calculateError(poly, i);

                    yChangeError += err;
                }
            }

            float gradientX = (xChangeError - initialError) / h;
            float gradientY = (yChangeError - initialError) / h;

            float dx = 0 == gradientX ? 0 : (initialError / gradientX);
            float dy = 0 == gradientY ? 0 : (initialError / gradientY);

            dx = Math.Max(Math.Min(dx, MaxGrad), -MaxGrad)/100;
            dy = Math.Max(Math.Min(dy, MaxGrad), -MaxGrad)/100;

            return PointF.Add(pLast, new SizeF(-dx, -dy));
        }

        public static bool optimizePolygon(Polygon p, PolygonConstraints pc, int disIdx)
        {
            float initialError = 0;
            float newError = 0;
            for(int i = 0; i < pc.edgeConstraints.Count; i++)
            {
                if (pc.edgeConstraints[i] != null)
                {
                    float err = pc.edgeConstraints[i].calculateError(p, i);
                    initialError += err;

                }
            }

            int count = 0;

            float lastError = initialError;


            if (initialError < eps) return true;


            Polygon tempP = new Polygon();
            tempP.vertices = new List<PointF>(p.vertices);


            for (int j = 0; j < MaxIters; j++)
            {

                for (int i = 0; i < p.vertices.Count; i++)
                {
                    if(disIdx == -1 || i != disIdx)
                    {
                        p.vertices[i] = calculatePoint(p, pc, i);
                    }
                }

                newError = 0;
                for (int i = 0; i < pc.edgeConstraints.Count; i++)
                {
                    if (pc.edgeConstraints[i] != null)
                    {
                        float err = pc.edgeConstraints[i].calculateError(p, i);

                        newError += err;

                    }
                }


                if (Math.Abs(newError - lastError) < eps) count++;

                if (count == 10) break;
            }

            return count > 0;
        }
    }

    public static class ConstraintHelper {
        public static void fixRemoveEdgeConstraints(PolygonConstraints pr, int eIdx)
        {
            for (int i = 0; i < pr.edgeConstraints.Count; i++)
            {
                if (pr.edgeConstraints[i] != null)
                    pr.edgeConstraints[i].edgeRemovedFix(eIdx);
            }
        }

        public static void fixAddEdgeConstraints(PolygonConstraints pr, int eIdx)
        {
            for (int i = 0; i < pr.edgeConstraints.Count; i++)
            {
                if (pr.edgeConstraints[i] != null)
                    pr.edgeConstraints[i].edgeAddedFix(eIdx);
            }
        }

        public static void removeEdgeConstraint(PolygonConstraints pr, int eIdx)
        {
            if (pr.edgeConstraints[eIdx] != null)
            {
                int sister1 = pr.edgeConstraints[eIdx].getSisterConstraint();
                if (sister1 != -1)
                    pr.edgeConstraints[sister1] = null;
            }

            // maybe fix other part
            pr.edgeConstraints[eIdx] = null;
        }


        public static void removeTangentToPolygonConstraints(List<CircleConstraint> cc, int pIdx)
        {
            for (int i = 0; i < cc.Count; i++)
            {
                if (cc[i] is TangentToEdge)
                {
                    TangentToEdge tte = (TangentToEdge)cc[i];
                    if (tte.polygonIdx == pIdx)
                    {
                        cc[i] = null;
                    }
                }
            }

        }

        public static void removeTangentToEdgeConstraints(List<CircleConstraint> cc, int pIdx, int eIdx)
        {
            for (int i = 0; i < cc.Count; i++)
            {
                if (cc[i] is TangentToEdge)
                {
                    TangentToEdge tte = (TangentToEdge)cc[i];
                    if (tte.polygonIdx == pIdx && tte.edgeIdx == eIdx)
                    {
                        cc[i] = null;
                    }
                }
            }
        }
    }

    public class PolygonConstraints
    {
        public List<EdgeConstraint> edgeConstraints;

        public PolygonConstraints(int n)
        {
            edgeConstraints = new List<EdgeConstraint>();
            for(int i = 0; i < n; i++)
            {
                edgeConstraints.Add(null);
            }
        }
    }

    public static class DistanceCalculator
    {
        public static double GetDistance2(PointF p1, PointF p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;

            return dx * dx + dy * dy;
        }

        public static double GetDistance(PointF p1, PointF p2)
        {
            return Math.Sqrt(GetDistance2(p1, p2));
        }

        public static double DistanceToCircle(Circle c, PointF p)
        {
            return Math.Abs(Math.Sqrt(GetDistance2(c.center, p)) - c.radius);
        }

        public static double DistanceToEdge2(PointF p1, PointF p2, PointF p)
        {
            double lineLength = DistanceCalculator.GetDistance2(p1, p2);

            double denom = (p2.X - p1.X) * ((p1.Y - p.Y)) - (p1.X - p.X) * ((p2.Y - p1.Y));
            double tempDistance = denom * denom / lineLength;

            return tempDistance;
        }

        public static double DistanceToEdge(PointF p1, PointF p2, PointF p)
        {
            double lineLength = DistanceCalculator.GetDistance2(p1, p2);

            double denom = (p2.X - p1.X) * ((p1.Y - p.Y)) - (p1.X - p.X) * ((p2.Y - p1.Y));
            double tempDistance = denom * denom / lineLength;

            return Math.Sqrt(DistanceToEdge2(p1, p2, p));
        }

    }
    public abstract class EdgeConstraint
    {
        public abstract void drawIcon(Graphics g, PointF pf, Font font, Brush brush, string text);

        public abstract int getSisterConstraint();

        public abstract void edgeRemovedFix(int eIdx);
        public abstract void edgeAddedFix(int eIdx);

        public abstract float calculateError(Polygon p, int edgeIdx);
    }

    public class ConstantEdgeLength : EdgeConstraint
    {
        public float length;
        public ConstantEdgeLength(float l)
        {
            length = l;
        }

        public override void drawIcon(Graphics g, PointF pf, Font font, Brush brush, string text)
        {
            string s = $"Len: {length.ToString("0.00")}";
            SizeF szf = g.MeasureString(s, font);
            pf = new PointF(pf.X - szf.Width / 2, pf.Y - szf.Height/2);
            g.DrawString(s, font, brush, pf);
        }

        public override int getSisterConstraint()
        {
            return -1;
        }

        public override void edgeRemovedFix(int eIdx)
        {
        }
        public override void edgeAddedFix(int eIdx)
        {
        }

        public override float calculateError(Polygon p, int edgeIdx)
        {
            int N = p.vertices.Count;
            int i1 = edgeIdx;
            int i2 = (i1 + 1) % N;
            float dist = (float)DistanceCalculator.GetDistance(p.vertices[i1], p.vertices[i2]);
            float temp = Math.Abs(dist - length);
            return temp;
        }
    }

    public class EqualEdgesLengths : EdgeConstraint
    {
        public int edgeIdx;

        public EqualEdgesLengths(int eIdx)
        {
            edgeIdx = eIdx;
        }

        public override void drawIcon(Graphics g, PointF pf, Font font, Brush brush, string text)
        {
            int k = int.Parse(text);

            string s = edgeIdx > k ? $"{k} = {edgeIdx}" : $"{edgeIdx} = {k}";
            SizeF szf = g.MeasureString(s, font);
            pf = new PointF(pf.X - szf.Width / 2, pf.Y - szf.Height / 2);
            g.DrawString(s, font, brush, pf);
        }

        public override int getSisterConstraint()
        {
            return edgeIdx;
        }

        public override void edgeRemovedFix(int eIdx)
        {
            if (edgeIdx > eIdx) edgeIdx--;
        }
        public override void edgeAddedFix(int eIdx)
        {
            if (edgeIdx > eIdx) edgeIdx++;
        }

        public override float calculateError(Polygon p, int eIdx)
        {
            int i1 = eIdx;
            int i2 = (i1 + 1) % p.vertices.Count;

            int i3 = edgeIdx;
            int i4 = (i3 + 1) % p.vertices.Count;

            float distance1 = (float)DistanceCalculator.GetDistance(p.vertices[i1], p.vertices[i2]);
            float distance2 = (float)DistanceCalculator.GetDistance(p.vertices[i3], p.vertices[i4]);

            float desiredDistance = (distance1 + distance2) / 2;

            float temp = Math.Abs((distance1 - desiredDistance)/(desiredDistance));

            return temp;
        }
    }

    public class ParallelEdges : EdgeConstraint
    {
        public int edgeIdx;
        public ParallelEdges(int eIdx)
        {
            edgeIdx = eIdx;
        }
        public override void drawIcon(Graphics g, PointF pf, Font font, Brush brush, string text)
        {
            int k = int.Parse(text);

            string s = edgeIdx > k ? $"{k} || {edgeIdx}" : $"{edgeIdx} || {k}";
            SizeF szf = g.MeasureString(s, font);
            pf = new PointF(pf.X - szf.Width / 2, pf.Y - szf.Height / 2);
            g.DrawString(s, font, brush, pf);
        }

        public override int getSisterConstraint()
        {
            return edgeIdx;
        }

        public override void edgeRemovedFix(int eIdx)
        {
            if (edgeIdx > eIdx) edgeIdx--;
        }
        public override void edgeAddedFix(int eIdx)
        {
            if (edgeIdx > eIdx) edgeIdx++;
        }

        public override float calculateError(Polygon p, int eIdx)
        {
            //edge with constraint
            PointF p1 = p.vertices[eIdx];
            PointF p2 = p.vertices[(eIdx + 1) % p.vertices.Count];

            // edge to constraint on
            PointF p3 = p.vertices[edgeIdx];
            PointF p4 = p.vertices[(edgeIdx + 1) % p.vertices.Count];

            float dx1 = p1.X - p2.X;
            float dx2 = p3.X - p4.X;

            float dy1 = p1.Y - p2.Y;
            float dy2 = p3.Y - p4.Y;

            float h1 = (float)DistanceCalculator.GetDistance(p1, p2);
            float h2 = (float)DistanceCalculator.GetDistance(p3, p4);

            dx1 /= h1;
            dy1 /= h1;

            dx2 /= h2;
            dy2 /= h2;

            float err = (float)Math.Abs(dy1 * dx2 - dy2 * dx1);
            return err*err;
        }
    }

    public abstract class CircleConstraint
    {
        public abstract void drawIcon(Graphics g, PointF pf, Font font, Brush brush, string text);
    }
    public class ConstRadius : CircleConstraint
    {
        public float radius;
        public ConstRadius(float r)
        {
            radius = r;
        }
        public override void drawIcon(Graphics g, PointF pf, Font font, Brush brush, string text)
        {
            string s = $"R = {radius}";
            SizeF szf = g.MeasureString(s, font);
            pf = new PointF(pf.X - szf.Width / 2, pf.Y - szf.Height / 2);
            g.DrawString(s, font, brush, pf);
        }
    }

    public class ConstCenter : CircleConstraint
    {
        private PointF center;
        public ConstCenter(PointF c)
        {
            center = c;
        }

        public bool assertFor(Circle c)
        {
            return c.center == center;
        }
        public override void drawIcon(Graphics g, PointF pf, Font font, Brush brush, string text)
        {
            string s = $"C = ({center.X}, {center.Y})";
            SizeF szf = g.MeasureString(s, font);
            pf = new PointF(pf.X - szf.Width / 2, pf.Y - szf.Height / 2);
            g.DrawString(s, font, brush, pf);
        }
    }

    public class TangentToEdge : CircleConstraint
    {
        public int edgeIdx;
        public int polygonIdx;

        public TangentToEdge(int eIdx, int pIdx)
        {
            edgeIdx = eIdx;
            polygonIdx = pIdx;
        }

        public override void drawIcon(Graphics g, PointF pf, Font font, Brush brush, string text)
        {
            string s = $"T: ({polygonIdx},{edgeIdx})";
            SizeF szf = g.MeasureString(s, font);
            pf = new PointF(pf.X - szf.Width / 2, pf.Y - szf.Height / 2);
            g.DrawString(s, font, brush, pf);
        }

        public int getRadius(Circle c, Polygon p)
        {
            int i1 = edgeIdx;
            int i2 = (edgeIdx + 1) % p.vertices.Count;
            return (int)DistanceCalculator.DistanceToEdge(p.vertices[i1], p.vertices[i2], c.center);
        }

        public bool assertFor(Circle c, Scene s)
        {
            Polygon p = s.polygons[polygonIdx];

            int N = p.vertices.Count;
            int i1 = edgeIdx;
            int i2 = (edgeIdx + 1) % N;

            return Math.Abs(DistanceCalculator.DistanceToEdge(p.vertices[i1], p.vertices[i2], c.center) - c.radius) < 0.001;
        }
    }

}
