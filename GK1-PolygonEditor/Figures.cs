using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GK1_PolygonEditor
{
    public static class Rasterizer
    {
        static public void drawPoint(Graphics g, Brush brush, PointF point, float R)
        {
            g.FillEllipse(brush, point.X - R, point.Y - R, R * 2, R * 2);
        }
        static public void drawAddCircle(Graphics g, Circle circle, PointF mousePosition, float R)
        {
            g.FillEllipse(Brushes.Pink, circle.center.X - R, circle.center.Y - R, R * 2, R * 2);
            g.DrawLine(Pens.Crimson, circle.center, mousePosition);

            g.DrawEllipse(Pens.Black, circle.center.X - circle.radius, circle.center.Y - circle.radius, circle.radius * 2, circle.radius * 2);   
        }
        static public void drawAddPolygon(Graphics g, Polygon poly, PointF mousePosition, bool cursorVertex, float R)
        {
            int vCount = poly.vertices.Count;

            if (vCount > 0)
            {
                PointF startingPoint = poly.vertices[0];
                PointF lastPoint = startingPoint;

                for (int i = 1; i < vCount; i++)
                {
                    PointF currentPoint = poly.vertices[i];
                    g.DrawLine(Pens.Black, currentPoint, lastPoint);
                    lastPoint = currentPoint;
                }


                if (cursorVertex)
                {
                    g.DrawLine(Pens.Black, lastPoint, mousePosition);

                    lastPoint = mousePosition;


                    vCount++;
                }

                if (vCount > 2)
                {
                    g.DrawLine(Pens.Black, lastPoint, startingPoint);
                }

            }

            foreach (PointF p in poly.vertices)
                drawPoint(g, Brushes.Blue, p, R);

            if (cursorVertex)
                drawPoint(g, Brushes.Blue, mousePosition, R);
        }

        static public void drawLineBresenham(Bitmap image, Point p1, Point p2)
        {
            Point ps, pf;
            bool reverse = false;
            if (Math.Abs(p1.X - p2.X) < Math.Abs(p1.Y - p2.Y))
            {
                ps = new Point(p1.Y, p1.X);
                pf = new Point(p2.Y, p2.X);
                reverse = true;
            }
            else
            {
                ps = p1;
                pf = p2;
            }

            if (ps.X >= pf.X)
            {
                Point pt = ps;
                ps = pf;
                pf = pt;
            }


            //normalnie
            if (pf.Y >= ps.Y)
            {
                _drawLinev1(image, ps.X, ps.Y, pf.X, pf.Y, reverse);
            }
            else
            {
                _drawLinev2(image, ps.X, ps.Y, pf.X, pf.Y, reverse);
            }

        }

        static private void _drawLinev1(Bitmap image, int x1, int y1, int x2, int y2, bool reverse)
        {
            // Line going right and down
            int dx = x2 - x1;
            int dy = y2 - y1;
            int d = 2 * dy - dx; //initial value of d
            int incrE = 2 * dy; //increment used for move to E
            int incrNE = 2 * (dy - dx); //increment used for move to NE
            int x = x1;
            int y = y1;

            if (reverse)
            {
                SetPixelWithCheck(image, y, x, Color.Black);
            }
            else
            {
                SetPixelWithCheck(image, x, y, Color.Black);
            }

            while (x < x2)
            {
                if (d < 0) //choose E
                {
                    d += incrE;
                    x++;
                }
                else //choose NE
                {
                    d += incrNE;
                    x++;
                    y++;
                }

                if (x < 0 || y < 0) continue;

                if (reverse)
                {
                    SetPixelWithCheck(image, y, x, Color.Black);
                }
                else
                {
                    SetPixelWithCheck(image, x, y, Color.Black);
                }
            }
        }

        static private void SetPixelWithCheck(Bitmap image, int x, int y, Color color)
        {
            if(x >= 0  && x < image.Width && y >= 0 && y < image.Height)
                image.SetPixel(x, y, color);

        }

        static private void _drawLinev2(Bitmap image, int x1, int y1, int x2, int y2, bool reverse)
        {
            // Line going right and up
            int dx = x2 - x1;
            int dy = y2 - y1;
            int d = 2 * dy + dx; //initial value of d
            int incrE = 2 * dy; //increment used for move to E
            int incrNE = 2 * (dy + dx); // 
            int x = x1;
            int y = y1;

            if (reverse) {
                SetPixelWithCheck(image, y, x, Color.Black);
            } else {
                SetPixelWithCheck(image, x, y, Color.Black);
            }

            while (x < x2)
            {
                if (d > 0) //choose E
                {
                    d += incrE;
                    x++;
                }
                else //choose NE
                {
                    d += incrNE;
                    x++;
                    y--;
                }

                if (reverse)
                {
                    SetPixelWithCheck(image, y, x, Color.Black);
                }
                else
                {
                    SetPixelWithCheck(image, x, y, Color.Black);
                }
            }
        }

        static public void drawPolygonBresenham(Bitmap image, Polygon p)
        {
            if (p.vertices.Count > 1)
            {
                PointF startingPoint = p.vertices[0];
                PointF lastPoint = startingPoint;
                PointF currentPoint;

                for (int i = 1; i < p.vertices.Count; i++)
                {
                    currentPoint = p.vertices[i];

                    drawLineBresenham(image, Point.Round(lastPoint), Point.Round(currentPoint));

                    lastPoint = currentPoint;
                }

                drawLineBresenham(image, Point.Round(lastPoint), Point.Round(startingPoint));
            }
        }

        static public void drawPolygonsBresenham(Bitmap image, IEnumerable<Polygon> polygons)
        {
            foreach (Polygon p in polygons)
                drawPolygonBresenham(image, p);
        }

    }

    public class Polygon
    {
        public List<PointF> vertices;

        public Polygon()
        {
            vertices = new List<PointF>();
        }
    }

    public class Circle
    {
        public PointF center;
        public float radius;

        public Circle(PointF c, int r)
        {
            center = c;
            radius = r;
        }
    }

    public class Scene
    {
        public List<Polygon> polygons;
        public List<Circle> circles;

        public List<PolygonConstraints> polygonsConstraints;
        public List<CircleConstraint> circleConstraints;

        public Scene()
        {
            polygons = new List<Polygon>();
            circles = new List<Circle>();

            polygonsConstraints = new List<PolygonConstraints>();
            circleConstraints = new List<CircleConstraint>();
        }

        public void SetToDefaultScene()
        {
            clearScene();

            Polygon poly = new Polygon();

            poly.vertices.Add(new PointF(50, 50));
            poly.vertices.Add(new PointF(100, 50));
            poly.vertices.Add(new PointF(50, 100));
            poly.vertices.Add(new PointF(25, 75));

            polygons.Add(poly);

            poly = new Polygon();
            poly.vertices.Add(new PointF(300, 200));
            poly.vertices.Add(new PointF(310, 240));
            poly.vertices.Add(new PointF(350, 230));
            poly.vertices.Add(new PointF(400, 320));
            poly.vertices.Add(new PointF(360, 120));

            polygons.Add(poly);

            circles.Add(new Circle(new PointF(200, 200), 90));
            circles.Add(new Circle(new PointF(270, 50), 30));


            polygonsConstraints = new List<PolygonConstraints>();
            circleConstraints = new List<CircleConstraint>();

            for (int i = 0; i < polygons.Count; i++)
            {
                polygonsConstraints.Add(new PolygonConstraints(polygons[i].vertices.Count));
            }

            polygonsConstraints[0].edgeConstraints[1] = new ParallelEdges(3);
            polygonsConstraints[0].edgeConstraints[3] = new ParallelEdges(1);

            polygonsConstraints[1].edgeConstraints[1] = new ConstantEdgeLength((float)DistanceCalculator.GetDistance(polygons[1].vertices[1], polygons[1].vertices[2]));


            for (int i = 0; i < circles.Count; i++)
            {
                circleConstraints.Add(null);
            }

            circleConstraints[0] = new ConstRadius(90);
            circleConstraints[1] = new ConstCenter(circles[1].center);
        }

        public void clearScene()
        {
            polygons.Clear();
            circles.Clear();

            polygonsConstraints.Clear();
            circleConstraints.Clear();
        }
    }

}
