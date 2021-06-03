using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using OEngine;

namespace AntInvasion {
    class AntManager {

        public List<Ant> Ants {get;} = new List<Ant>();

        public int AntCount {get;} = 25000;
        public int AntSpawnRate {get; set;} = 40;

        private OLehmer lehmer = new OLehmer();
        private int spawnCounter = -100;

        public AntManager() {

            // create timer for slowly spawning ants
            //for(int i = 0; i < AntCount; i++) {
            //    AddAnt();
            //}
        }

        private void AddAnt() {
            Point position = new Point(0, 0);

            

            // Ant spawn location ---------------------------------------------
            // Selects wall to spawn at
            int wall = lehmer.RandomRange(0, 4);
            // Selects a position for the ant to spawn at along wall
            // ┌─0─┐
            // 3   1
            // └─2─┘
            switch(wall) {
                case 0:
                    position = new Point(lehmer.RandomRange(0, 1920), 0);
                    break;
                case 1:
                    position = new Point(1920, lehmer.RandomRange(0, 1080));
                    break;
                case 2:
                    position = new Point(lehmer.RandomRange(0, 1920), 1080);
                    break;
                case 3:
                    position = new Point(0, lehmer.RandomRange(0, 1080));
                    break;
            }

            Ants.Add(new Ant(position, lehmer));
        }

        public void TickAll() {
            foreach(Ant ant in Ants) {
                ant.Tick();
            }

            spawnCounter += 1;

            if(Ants.Count < AntCount && spawnCounter >= AntSpawnRate) {
                spawnCounter = 0;
                AddAnt();
            }
        }
        public void RenderAll(Graphics g) {
            foreach(Ant ant in Ants) {
                ant.Render(g);
            }
        }
    }
}