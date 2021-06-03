using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using OEngine;

namespace AntInvasion {
    class Ant {

        public Vec2 Position {get; set;}
        public Vec2 Velocity {get; set;}
        public float Rotation {get; set;}
        public Task State {get; private set;} = Task.Idle;

        public Vec2 Target {get; private set;}
        public Vec2 TargetRotation {get; set;}

        public float RotationRandomness {get; set;} = 180f;
        
        public float MoveSpeed {get; private set;} = 0.7f;

        public TextureBrush AntBrush {get;}
        public TextureBrush ShadowBrush {get;}
        public Vec2 ShadowSize {get; set;} = new Vec2(5, 2.5f);
        public int ShadowHeight {get; set;} = 2;

        public Rig Rig {get; set;}

        private OLehmer lehmer;

        public Ant(Point p, OLehmer r) {
            Position = new Vec2(p.X, p.Y);
            lehmer = r;

            // Creates Drawing Tools ------------------------------------------
            // Creates bitmap for ShadowBrush texture
            Bitmap shadowBitmap = new Bitmap(2, 2);
            shadowBitmap.SetPixel(0, 0, Color.Transparent);
            shadowBitmap.SetPixel(1, 1, Color.Transparent);
            shadowBitmap.SetPixel(1, 0, Color.Black);
            shadowBitmap.SetPixel(0, 1, Color.Black);

            // Creates bitmap for antbrush
            Bitmap antBitmap = new Bitmap(1, 1);
            Random random = new Random();
            int rnd = random.Next(0, 4);
            switch (rnd) {
                case 0:
                    antBitmap.SetPixel(0, 0, Color.Brown);
                    break;
                case 1:
                    antBitmap.SetPixel(0, 0, Color.RosyBrown);
                    break;
                case 2:
                    antBitmap.SetPixel(0, 0, Color.SaddleBrown);
                    break;
                case 3:
                    antBitmap.SetPixel(0, 0, Color.SandyBrown);
                    break;
            }
            
            // Create texturebrushes
            ShadowBrush = new TextureBrush(shadowBitmap);
            AntBrush = new TextureBrush(antBitmap);

            // Creates Rig ----------------------------------------------------
            Rig = new Rig();
        }

        public void Tick() {
            if(State == Task.Idle) {
                // Select new wander location ---------------------------------
                Target = new Vec2(lehmer.RandomRange(0, 1920),
                    lehmer.RandomRange(0, 1080));

                State = Task.Wander;
            }

            if(State == Task.Wander) {
                Vec2 diff = Target - Position;

                Vec2 movementDirection = OMath.Lerp(Vec2.GetFromAngleDegrees(
                    Rotation), diff, 0.001f);
                Rotation = movementDirection.GetAngleDegrees();

                Velocity = movementDirection.Normalized() * MoveSpeed;
                Position += Velocity;

                if(Position.Distance(Target) <= 10f) {
                    State = Task.Idle;
                }
            }
        }

        public void Render(Graphics g) {
            // Draws Shadow ---------------------------------------------------
            Vec2 sPos = Position - new Vec2(0, -ShadowHeight);
            ODrawing.FillElipseFromCenter(g, ShadowBrush, sPos, ShadowSize);
            
            // Draws Ant ------------------------------------------------------
            Rig.CalculateRig(Position, Rotation);

            ODrawing.FillCircleFromCenter(g, AntBrush, Rig.BodyA, 
                                          (int)Rig.BodyASize);
            ODrawing.FillCircleFromCenter(g, AntBrush, Rig.BodyB, 
                                          (int)Rig.BodyBSize);
            ODrawing.FillCircleFromCenter(g, AntBrush, Rig.BodyC, 
                                          (int)Rig.BodyCSize);
        }
    }

    enum Task {
        Idle,
        Wander,
        Select
    }
}