using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http.Headers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpCardGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static string[] suits = {"clubs", "diamonds", "hearts", "spades"};
        private List<String> Deck = new List<string>();
        private int numberOfRounds = 5, roundsPlayed = 0;
        private string hCard, pCard;
        private int pScore, hScore;
        private static string[] numbers = {"ace", "two", "three", "four", "five", "six",
            "seven", "eight", "nine", "ten", "jack", "queen", "king"};

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            MakeADeck();
        }

        private void SwitchOnClick(object sender, RoutedEventArgs e)
        {
            string temp = pCard;
            pCard = hCard;
            hCard = temp;
            BSwitchCards.IsEnabled = false;
        }

        private void PlayOnClick(object sender, RoutedEventArgs e)
        {

            roundsPlayed++;
            ImgPlayerCard.Source = new BitmapImage(new Uri(createPath(pCard)));
            ImgHouseCard.Source = new BitmapImage(new Uri(createPath(hCard)));
            int hValue = int.Parse(hCard.Substring(1, 2));
            char hSuit = hCard[0];
            int pValue = int.Parse(pCard.Substring(1, 2));
            char pSuit = pCard[0];
            if (pValue > hValue)
            {
                pScore++;
                TxtScoreBoard.Text = String.Format("Player won with {0} of {1} against {2} of {3}.",
                    numbers[pValue-1], FindHouseFromChar(pSuit), numbers[hValue-1], FindHouseFromChar(hSuit));
                TxtPlayerScore.Text = pScore.ToString();
            } else if (pValue < hValue)
            {
                hScore++;
                TxtScoreBoard.Text = String.Format("House won with {0} of {1} against {2} of {3}.",
                    numbers[hValue-1], FindHouseFromChar(hSuit), numbers[pValue-1], FindHouseFromChar(pSuit));
                TxtHouseScore.Text = hScore.ToString();
            } else
            {
                TxtScoreBoard.Text = "House Tied With Player.";
            }
            BSwitchCards.IsEnabled = false;
            BPlayCards.IsEnabled = false;
            BDealCards.IsEnabled = true;
            if (roundsPlayed == numberOfRounds)
            {
                BDealCards.IsEnabled = false;
                Restart.Visibility = Windows.UI.Xaml.Visibility.Visible;
                int result = hScore.CompareTo(pScore);
                switch (result)
                {
                    case 1:
                        TxtScoreBoard.Text += "\r\nHouse won against player.";
                        break;
                    case -1:
                        TxtScoreBoard.Text += "\r\nPlayer won against house.";
                        break;
                    case 0:
                        TxtScoreBoard.Text += "\r\nPlayer and house tied.";
                        break;
                }
            }

        }

        private void DealOnClick(object sender, RoutedEventArgs e)
        {
            ImgHouseCard.Source = ImgPlayerCard.Source = new BitmapImage(new Uri(createPath("CardBack")));
            TxtScoreBoard.Text = "";
            hCard = PickACard();
            pCard = PickACard();
            BSwitchCards.IsEnabled = true;
            BPlayCards.IsEnabled = true;
            BDealCards.IsEnabled = false;
        }

        private void MakeADeck()
        {
            Deck.Clear();
            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j< 13; j++)
                {
                    Deck.Add(String.Format("{0}{1}", suits[i].Substring(0, 1), (j + 1).ToString().PadLeft(2, '0')));
                }
            }
        }

        private void RestartOnClick(object sender, RoutedEventArgs e)
        {
            Restart.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            roundsPlayed = 0;
            pScore = 0;
            hScore = 0;
            TxtScoreBoard.Text = "Welcome to the card game!";
            TxtPlayerScore.Text = TxtHouseScore.Text = "0";
            MakeADeck();
            BDealCards.IsEnabled = true;

        }

        private string PickACard()
        {
            Random rand = new Random();
            int indexPicked = rand.Next(0, Deck.Count);
            string cardPicked = Deck[indexPicked].ToString();
            Deck.RemoveAt(indexPicked);
            return cardPicked;
        }

        private static string createPath(string card)
        {
            return String.Format("ms-appx:///Assets/CardImages/{0}.png", card);
        }

        private string FindHouseFromChar(char house)
        {
            switch (house)
            {
                case 'c':
                    return suits[0];
                case 'd':
                    return suits[1];
                case 'h':
                    return suits[2];
                default:
                    return suits[3];
            }
        }
    }
}
