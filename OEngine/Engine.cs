using System;
using System.Drawing;

namespace OEngine {

    // Base class for Math ----------------------------------------------------
    static class OMath {

        // Constants ----------------------------------------------------------
        public const float HalfPI   =  1.57079632679f;
        public const float DegToRad =  0.01745329252f;
        public const float RadToDeg = 57.29577951308f;

        // Linear interpolation -----------------------------------------------
        // Basic types
        public static int Lerp(int a, int b, float t) {
            return (int)(a * (1-t) + b * t);
        }
        public static float Lerp(float a, float b, float t) {
            return a * (1-t) + b * t;
        }

        // Interpolation for 2D vectors/points
        public static Vec2 Lerp(Vec2 a, Vec2 b, float t) {
            return new Vec2(Lerp(a.x, b.x, t), Lerp(a.y, b.y, t));
        }
        public static Point Lerp(Point a, Point b, float t) {
            return new Point(Lerp(a.X, b.X, t), Lerp(a.Y, b.Y, t));
        }

        // Clampings ----------------------------------------------------------
        public static float Clamp(float a, float min, float max) {
            return Math.Min(Math.Max(a, min), max);
        }
        public static int Clamp(int a, int min, int max) {
            return (int)Math.Min(Math.Max(a, min), max);
        }
    }

    // Class for implementing Lehmer (Park-Miller) ----------------------------
    //    https://en.wikipedia.org/wiki/Lehmer_random_number_generator
    //    State following formula:   x_k+1 = a * x_k mod m
    public class OLehmer {
        // MLCG Constants
        private const uint a = 48271;
        private const uint m = 2147483647;
        private const uint q = 44488;        // m / a
        private const uint r = 3399;         // m % a
        private uint seed;

        // Initializers -------------------------------------------------------
        // When initializing the engine, seed it with tickCount.
        //    Working similar to System.Random. Decent for non-cryptology uses.
        public OLehmer() : this(Environment.TickCount) {}
        public OLehmer(int seed) {
            Seed(seed);
        }

        public void Seed(int s) {
            if (s <= 0 || s == int.MaxValue)
                throw new ArgumentOutOfRangeException("Bad seed.");
            seed = (uint)s;
        }


        // Implementation using Schrage's method ------------------------------
        public float Next() {
            uint div = seed / q; // max: m / q = a
            uint rem = seed % q; // max: q - 1

            seed = (a * rem) - (r * div);

            if (seed <= 0) seed += m;

            // returns a double between 0 and 1 (excluding 1)
            return (float)seed / m;
        }

        // Helper methods for Next() ------------------------------------------
        public int RandomRange(int min, int max) {
            float x = Next();
            int rand = (int)((max - min) * x + min);
            return rand;
        }
        public float RandomRange(float min, float max) {
            float x = Next();
            float rand = ((max - min) * x + min);
            return rand;
        }
    }

    // Structure for representing 2D Vectors ----------------------------------
    struct Vec2 {
        public float x;
        public float y;

        public Vec2(float _x, float _y) {
            x = _x; y = _y;
        }
        public Vec2(Point p) : this(p.X, p.Y) {}

        // Operator overloads -------------------------------------------------
        public static Vec2 operator +(Vec2 a, Vec2 b) {
            return new Vec2(a.x + b.x, a.y + b.y);
        }
        public static Vec2 operator -(Vec2 a, Vec2 b) {
            return new Vec2(a.x - b.x, a.y - b.y);
        }
        public static Vec2 operator *(Vec2 a, float b) {
            return new Vec2(a.x * b, a.y * b);
        }
        public static Vec2 operator *(Vec2 a, Vec2 b) {
            return new Vec2(a.x * b.x, a.y * b.y);
        }
        public static Vec2 operator /(Vec2 a, float b) {
            return new Vec2(a.x / b, a.y / b);
        }
        public static Vec2 operator /(Vec2 a, Vec2 b) {
            return new Vec2(a.x / b.x, a.y / b.y);
        }
        public static bool operator ==(Vec2 a, Vec2 b) {
            return (a.x == b.x && a.y == b.y) ? true : false;
        }
        public static bool operator !=(Vec2 a, Vec2 b) {
            return (a.x != b.x || a.y != b.y) ? true : false;
        }

        // Dynamic type method overrides --------------------------------------
        public override bool Equals(object obj) {
            return (obj is Vec2) && this == (Vec2)obj;
        }
        public override int GetHashCode() => HashCode.Combine(x, y);

        // Vector operators ---------------------------------------------------
        public Vec2 Normalized() {
            float mag = Magnitude();
            return new Vec2(x/mag, y/mag);
        }

        public float Magnitude() {
            return (float)Math.Sqrt(x*x + y*y);
        }

        public float Distance(Vec2 v) {
            return (this - v).Magnitude();
        }

        // Trigonometric functions --------------------------------------------
        public static Vec2 GetFromAngleDegrees(float angle) {
            float radians = angle * OMath.DegToRad;
            return new Vec2(
                (float)Math.Cos(radians), (float)Math.Sin(radians)
            );
        }
        public float GetAngleDegrees() {
            return (float)Math.Atan2(y, x) * OMath.RadToDeg;
        }

        public Vec2 RotateTowards(float angle) {
            // Applies Matrix rotation
            float r = angle * OMath.DegToRad;
            float newX = ((float)Math.Cos(r) * x) - ((float)Math.Sin(r) * y);
            float newY = ((float)Math.Sin(r) * x) + ((float)Math.Cos(r) * y);
            
            return new Vec2(newX, newY);
        }

        // Converting to System.Drawing point.
        public Point ToPoint() {
            return new Point((int)x, (int)y);
        }
    }
}