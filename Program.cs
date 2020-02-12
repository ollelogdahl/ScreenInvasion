using System;
using System.Threading;
using System.Windows.Forms;

namespace AntInvasion {
    class Program {

        public Game Game {get;}

        public MainForm MainForm {get;}
        public BufferedPanel BufferedPanel {get;}

        public Program() {
            // Initializes MainForm -------------------------------------------
            MainForm = new MainForm();
            BufferedPanel = new BufferedPanel();

            // Adds the buffered panel as a component to MainForm
            MainForm.Controls.Add(BufferedPanel);

            // Initializes game, loading resources
            Game = new Game();
            Game.Init();

            // Add delegates/actions for the MainForm update and quit.
            BufferedPanel.Paint += Game.Update;
            Game.QuitAlphaOne += HandleQuit;

            // Subscribe to the application idle event, re-awakening the window
            Application.Idle += HandleApplicationIdle;
            // Start running the form
            Application.EnableVisualStyles();
            Application.Run(MainForm);
        }

        public void HandleQuit() {
            Thread.Sleep(200);
            Application.Exit();
        }

        public void HandleApplicationIdle(Object sender, EventArgs e) {
            // DEBUG -
            //     MessageBox.Show($"You are in the Application.Idle event.\n 
            //                    {NativeMethods.IsApplicationIdle()}");
            BufferedPanel.Invalidate();
            Thread.Sleep(8);
        }
    }
}