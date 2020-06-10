using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpCardGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string[] suits = {"clubs", "diamonds", "hearts", "spades"};
        private List<String> Deck = new List<string>();
        private int numberOfRounds = 5, roundsPlayed = 0;
        private string hCard, pCard;
        private int pScore, hScore;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void SwitchOnClick(object sender, RoutedEventArgs e)
        {
            string temp = uCard;
            uCard = hCard;
            hCard = temp;
        }

        private void PlayOnClick(object sender, RoutedEventArgs e)
        {
            roundsPlayed++;
            ImgPlayerCard.Source = new BitmapImage(new Uri(createPath(pCard)));
            ImgHouseCard.Source = new BitmapImage(new Uri(createPath(hCard)));
        }

        private void DealOnClick(object sender, RoutedEventArgs e)
        {
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
                    Deck.Add(String.Format("{0}{1}", suits[i].Substring(0, 1), j+1));
                }
            }
        }

        private string PickACard()
        {
            Random rand = new Random();
            int indexPicked = rand.Next(0, Deck.Count);
            string cardPicked = Deck[indexPicked].ToString();
            Deck.RemoveAt(indexPicked);
            return cardPicked;
        }

        private string createPath(string card)
        {
            return String.Format("/Assets/CardImages/{0}.png", card);
        }
    }
}
