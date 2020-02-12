using System;
using System.Drawing;
using System.Windows.Forms;

using OEngine;

namespace AntInvasion {
    class Game {
        public Action QuitAlphaOne;
        public AntManager AntManager {get; set;}

        private Font font = new Font("Consolas", 11f, FontStyle.Regular);
        private string quitString = "Hold ESC to quit...";
        float currentQuitAlpha = 0.0f;

        public void Init() {
            AntManager = new AntManager();

            string pathToResources = NativeMethods.GetPathToFileInAssembly("Assets/");
        }

        public void Update(Object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;

            // Tick all ants
            AntManager.TickAll();

            // Render all ants
            AntManager.RenderAll(g);

            // Updates the quit screen
            UpdateExitScreen(g);
        }

        public void UpdateExitScreen(Graphics g) {
            // increments/decrements exit counter on ESC key.
            if(NativeMethods.GetAsyncKeyState((int)Keys.Escape) != 0) {
                currentQuitAlpha += 0.015f;
            } else if(currentQuitAlpha > 0) {
                currentQuitAlpha -= 0.06f;
            }

            if(currentQuitAlpha >= 1f) {
                QuitAlphaOne.Invoke();
            }

            if(currentQuitAlpha >= 0.1f) {
                // Calculates a float value, that is slightly higher than 
                // currentQuitAlpha. Clamped to 1.
                float progress = OMath.Clamp(currentQuitAlpha * 1.2f, 0f, 1f);
                SizeF fontSize = g.MeasureString(quitString, font, int.MaxValue);
                Rectangle fontBox = new Rectangle(5, 5, (int)fontSize.Width + 10,
                    (int)fontSize.Height + 10);
                Rectangle progressBox = new Rectangle(10, 10, OMath.Lerp(0, 
                    (int)fontSize.Width, progress), (int)fontSize.Height);

                g.FillRectangle(Brushes.Black, fontBox);
                g.FillRectangle(Brushes.PaleVioletRed, progressBox);
                g.DrawString(quitString, font, Brushes.White, 10, 10);
            }
        }
    }
}