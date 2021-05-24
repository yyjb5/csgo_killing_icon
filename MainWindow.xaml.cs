using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CSGSI;
using System.Threading;

namespace GSI2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameStateListener gsl;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void whenLoaded(object sender, RoutedEventArgs e)
        {
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            gsl = new GameStateListener("http://127.0.0.1:3000/");
            gsl.NewGameState += new NewGameStateHandler(OnNewGameState);
            gsl.Start();
        }

        private int pre_kill = 0, pre_kill_hs = 0;
        private void OnNewGameState(GameState gs)
        {
            if (gs.Provider.SteamID != gs.Player.SteamID)
                return;
            //Console.WriteLine("prek" + pre_kill + "\t" + "k" + gs.Player.State.RoundKills);
            //Console.WriteLine("prekh" + pre_kill_hs + "\t" + "kh" + gs.Player.State.RoundKillHS);
            if (gs.Player.Activity == CSGSI.Nodes.PlayerActivity.Menu)
            {
                pre_kill = 0; pre_kill_hs = 0;
            }
            if(gs.Player.State.RoundKills <= 0)
            {
                pre_kill = 0; pre_kill_hs = 0;
            }
            if (gs.Player.State.RoundKills > pre_kill)
            {
                switch (gs.Player.State.RoundKills)
                {
                    case 0:
                        break;
                    case -1:
                        break;
                    case 1:
                        Show_Label("1 KILL");
                        break;
                    case 2:
                        Show_Label("2 KILL");
                        break;
                    case 3:
                        Show_Label("3 KILL");
                        break;
                    case 4:
                        Show_Label("4 KILL");
                        break;
                    case 5:
                        Show_Label("5 KILL");
                        break;
                    default:
                        Show_Label("GOD LIKE");
                        break;
                }
                pre_kill = gs.Player.State.RoundKills;
            }
            if (gs.Player.State.RoundKillHS > pre_kill_hs)
            {
                Show_Heads();
                pre_kill_hs = gs.Player.State.RoundKillHS;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            gsl.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Show_Heads();
        }

        private void Show_Label(string show_string)
        {
            labelOUT.Dispatcher.Invoke(new Action(
                delegate
                {
                    labelOUT.Content = show_string;
                    Storyboard st = new Storyboard();
                    DoubleAnimationUsingKeyFrames show_label_animation = new DoubleAnimationUsingKeyFrames();
                    show_label_animation.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, TimeSpan.FromSeconds(0)));
                    show_label_animation.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, TimeSpan.FromSeconds(0.1)));
                    show_label_animation.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, TimeSpan.FromSeconds(2.0)));
                    show_label_animation.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, TimeSpan.FromSeconds(2.1)));
                    Storyboard.SetTarget(show_label_animation, labelOUT);
                    Storyboard.SetTargetProperty(show_label_animation, new PropertyPath("Opacity"));
                    st.Children.Add(show_label_animation);
                    st.Begin();
                    //st.Completed += new EventHandler(Show_Label_Over);
                    //this.labelOUT.Visibility = Visibility.Hidden;
                }

        ));
        }

        private void Show_Heads()
        {
            head_shot.Dispatcher.Invoke(
                new Action(
                delegate
                {
                    Storyboard st1 = new Storyboard();
                    DoubleAnimationUsingKeyFrames head_shot_animation = new DoubleAnimationUsingKeyFrames();
                    head_shot_animation.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, TimeSpan.FromSeconds(0)));
                    head_shot_animation.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, TimeSpan.FromSeconds(0.1)));
                    head_shot_animation.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, TimeSpan.FromSeconds(2.0)));
                    head_shot_animation.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, TimeSpan.FromSeconds(2.1)));
                    Storyboard.SetTarget(head_shot_animation, head_shot);
                    Storyboard.SetTargetProperty(head_shot_animation, new PropertyPath("Opacity"));
                    st1.Children.Add(head_shot_animation);
                    st1.Begin();
                }));
        }


        private void Show_Label_Over(Object o, EventArgs e)
        {
            //labelOUT.Visibility = Visibility.Hidden;
        }
    }
}
