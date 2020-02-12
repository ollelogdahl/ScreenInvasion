using OEngine;

namespace AntInvasion {
    class Rig {
        // Ant Body -----------------------------------------------------------
        public Vec2 BodyA {get; private set;}
        public Vec2 BodyB {get; private set;}
        public Vec2 BodyC {get; private set;}

        public float BodyASize {get; private set;} = 2;
        public float BodyBSize {get; private set;} = 2;
        public float BodyCSize {get; private set;} = 2;

        // All Legs -----------------------------------------------------------
        public Vec2 LegFL {get; private set;}
        public Vec2 LegFR {get; private set;}
        public Vec2 LegBL {get; private set;}
        public Vec2 LegBR {get; private set;}
        public Vec2 LegFLE {get; private set;}
        public Vec2 LegFRE {get; private set;}
        public Vec2 LegBLE {get; private set;}
        public Vec2 LegBRE {get; private set;}

        public float LegFLRotation {get; private set;} = -10f;
        public float LegFRRotation {get; private set;} =  10f;
        public float LegBLRotation {get; private set;}
        public float LegBRRotation {get; private set;}

        public float LegLength {get; private set;} = 3;

        public void CalculateRig(Vec2 position, float rotation) {
            // Calculate relative positions -----------------------------------
            BodyA = new Vec2(0, 2);
            BodyB = new Vec2(0, 0);
            BodyC = new Vec2(0, -2);

            LegFL = new Vec2(-0.5f, 1);
            LegFR = new Vec2( 0.5f, 1);

            // Calculate leg ends
            LegFLE = LegFL + new Vec2(-LegLength, 0);
            LegFRE = LegFR + new Vec2( LegLength, 0);

            // Rotate joints --------------------------------------------------
            // Calculate local rotation, 1/2pi
            rotation -= 90;
            BodyA = BodyA.RotateTowards(rotation);
            BodyC = BodyC.RotateTowards(rotation);

            // Convert to absolute/world --------------------------------------
            BodyA += position;
            BodyB += position;
            BodyC += position;

            LegFL += position;
            LegFLE += position;
        }
    }
}