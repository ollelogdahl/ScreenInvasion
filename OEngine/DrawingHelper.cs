using System.Drawing;

namespace OEngine {
    // Helper functions for Drawing with Vectors ------------------------------
    static class ODrawing {
        public static void FillCircleFromCenter(Graphics g, Brush b, Vec2 pos,
            int radius) {
            FillElipseFromCenter(g, b, pos, new Vec2(radius, radius));
        }

        public static void FillElipseFromCenter(Graphics g, Brush b, Vec2 pos,
            Vec2 xyRadius) {
            g.FillEllipse(b, pos.x-xyRadius.x, pos.y-xyRadius.y, 
                xyRadius.x*2, xyRadius.y*2);
        }
    }
}