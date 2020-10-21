using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PremierLeagueTABLE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public EplTeams myTeams;
        public MainWindow()
        {
            //string path = @"en.1.clubs.json";
            //String myStream = File.ReadAllText(path);
            InitializeComponent();

            try
            {
                string path = @"https://raw.githubusercontent.com/openfootball/football.json/master/2020-21/en.1.clubs.json";
                var textFromFile = (new WebClient()).DownloadString(path);
                //String myStream = ReadAllText(textFromFile);
                myTeams = JsonConvert.DeserializeObject<EplTeams>(textFromFile);
                nameListBox.ItemsSource = from team in myTeams.clubs
                                          select team;
                //itemsControlBox.ItemsSource = from team in myTeams.clubs
                //                  select team;
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
            }
        }

        private void viewTeamBtn_Click(object sender, RoutedEventArgs e)
        {
            string path = @"https://raw.githubusercontent.com/openfootball/football.json/master/2020-21/en.1.json";
            var textFromGit = (new WebClient()).DownloadString(path);
            EplMatches myMatches = JsonConvert.DeserializeObject<EplMatches>(textFromGit);
            var selected = (Club)nameListBox.SelectedItem;
            string team;
            try
            {
                team = (string)selected.name;
            }
            catch (NullReferenceException)
            {
                team = "None";
            }
            //selectedBox.Text = "OK all cool!";
            var match = from games in myMatches.matches where (games.team1 == team || games.team2 == team) && games.score != null
                        select new ScoreBoard
                        {
                            round1 = games.round,
                            date1 = games.date,
                            team1 = (from t in myTeams.clubs where t.name == games.team1 select t.code).ToArray()[0]==null?
                            "LEE": (from t in myTeams.clubs where t.name == games.team1 select t.code).ToArray()[0],
                            team2 = (from t in myTeams.clubs where t.name == games.team2 select t.code).ToArray()[0] == null ?
                            "LEE" : (from t in myTeams.clubs where t.name == games.team2 select t.code).ToArray()[0],// == null ? "LUFC" : t.code,
                            score1 = games.score.ft[0],
                            score2 = games.score.ft[1]
                        };
            matchListBox.ItemsSource = match;
            
            //((Club)nameListBox.SelectedItem).code+" is a club in the premier league of football in Englad.";
            
            //var finalScores = from game in match
            //                  where game.score != null 
            //                  select new { score1 = game.score.ft[0], score2 = game.score.ft[1] };
            //games.score==null ? new { score1 = 00, score2 = 00} : new { score1 = games.score.ft[0], score2 = games.score.ft[1] };

            //itemsControlBox.ItemsSource = finalScores;

            //foreach(var game in match)
            //{
            //    var k = new scores();
            //    k.score1 = game.ft.ft[0];
            //    k.score2 = game.ft.ft[1];
            //}

        }

        private void viewMatchBtn_Click(object sender, RoutedEventArgs e)
        {
            var scbo = (ScoreBoard)(matchListBox.SelectedItem);
            //var scbo1 = from games in myMatches.matches
            //            where (games.team1 == (string)scbo.team1 && games.team2 == (string)scbo.team2 && games.score != null)
            //            select new ScoreBoard
            //            {
            //                round1 = games.round,
            //                date1 = games.date,
            //                team1 = (from t in myTeams.clubs where t.name == games.team1 select t.code).ToArray()[0] == null ?
            //                "LEE" : (from t in myTeams.clubs where t.name == games.team1 select t.code).ToArray()[0],
            //                team2 = (from t in myTeams.clubs where t.name == games.team2 select t.code).ToArray()[0] == null ?
            //                "LEE" : (from t in myTeams.clubs where t.name == games.team2 select t.code).ToArray()[0],// == null ? "LUFC" : t.code,
            //                score1 = games.score.ft[0],
            //                score2 = games.score.ft[1]
            //            };
            ScoreBoardGrid.DataContext = scbo;//new ScoreBoard() { round1 = "First Round", date1 = "09-09-2020", team1 = "Liverpool", team2 = "Leeds", score1 = 4, score2 = 3 };//scbo;
        }
    }
    //public string ToCode(string team)
    //{
    //    return "NotImplemented";
    //}
    public class ScoreBoard
    {
        public string round1 { get; set; }
        public string date1 { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public int score1 { get; set; }
        public int score2 { get; set; }
    }

    public class EplMatches
    {
        public string name { get; set; }
        public IList<Match> matches { get; set; }
    }
    public class Match
    {
        public string round { get; set; }
        public string date { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public score score { get; set; }
    }
    public class score
    {
        public int[] ft { get; set; }
    }
    public class EplTeams
    {
        public string name { get; set; }
        public IList<Club> clubs { get; set; }
    }
    public class Club
    {
        public string name { get; set; }
        public string code { get; set; }
        public string country { get; set; }
    }
    
}
