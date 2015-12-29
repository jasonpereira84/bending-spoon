using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections;

namespace JFPGeneric
{
    public class PlayingCardHash
    {
        public enum Suits { Joker = -1, Spades = 0, Clubs = 1, Hearts = 2, Diamonds = 3 };
        public enum FaceValues { Unknown = -1, Ace = 0, Two = 1, Three = 2, Four = 3, Five = 4, Six = 5, Seven = 6, Eight = 7, Nine = 8, Ten = 9, Jack = 10, Queen = 11, King = 12 };

        private Suits _suit;
        private FaceValues _faceVal;
        private UInt16 _deckNum;

        public PlayingCardHash(UInt32 theNumericalValue)
        {
            uint modVal = theNumericalValue;
            while (modVal > 52000) { modVal = modVal % 52000; }

            ushort deckNum = 0;
            ushort suitVal = 0;
            ushort faceVal = 0;
            if (modVal != 52000)
            {
                ushort cardNum = Convert.ToUInt16(modVal % 52);

                deckNum = Convert.ToUInt16(modVal / 52);
                suitVal = Convert.ToUInt16(cardNum / 13);
                faceVal = Convert.ToUInt16(cardNum % 13);
            }

            _deckNum = deckNum;

            switch (suitVal)
            {
                case 0:
                    _suit = Suits.Spades;
                    break;
                case 1:
                    _suit = Suits.Clubs;
                    break;
                case 2:
                    _suit = Suits.Hearts;
                    break;
                case 3:
                    _suit = Suits.Diamonds;
                    break;
                default:
                    _suit = Suits.Joker;
                    break;
            }

            switch (faceVal)
            {
                case 0:
                    _faceVal = FaceValues.Ace;
                    break;
                case 1:
                    _faceVal = FaceValues.Two;
                    break;
                case 2:
                    _faceVal = FaceValues.Three;
                    break;
                case 3:
                    _faceVal = FaceValues.Four;
                    break;
                case 4:
                    _faceVal = FaceValues.Five;
                    break;
                case 5:
                    _faceVal = FaceValues.Six;
                    break;
                case 6:
                    _faceVal = FaceValues.Seven;
                    break;
                case 7:
                    _faceVal = FaceValues.Eight;
                    break;
                case 8:
                    _faceVal = FaceValues.Nine;
                    break;
                case 9:
                    _faceVal = FaceValues.Ten;
                    break;
                case 10:
                    _faceVal = FaceValues.Jack;
                    break;
                case 11:
                    _faceVal = FaceValues.Queen;
                    break;
                case 12:
                    _faceVal = FaceValues.King;
                    break;
                default:
                    _faceVal = FaceValues.Unknown;
                    break;
            }
        }

        public UInt16 DeckNumber
        {
            get { return _deckNum; }
        }

        public Suits Suit
        {
            get { return _suit; }
        }

        public FaceValues FaceValue
        {
            get { return _faceVal; }
        }

        public String DeckNumber_4Display
        {
            get { return _deckNum.ToString(); }
        }

        public String Suit_4Display
        {
            get
            {
                string retVal = "\u2655";//♕
                switch (_suit)
                {
                    case PlayingCardHash.Suits.Spades:
                        retVal = "\u2660";//♠
                        break;
                    case PlayingCardHash.Suits.Clubs:
                        retVal = "\u2663";//♣
                        break;
                    case PlayingCardHash.Suits.Hearts:
                        retVal = "\u2665";//♥
                        break;
                    case PlayingCardHash.Suits.Diamonds:
                        retVal = "\u2666";//♦
                        break;
                    default:
                        break;
                }
                return retVal;
            }
        }

        public String FaceValue_4Display
        {
            get
            {
                string retVal = "";
                switch (_faceVal)
                {
                    case PlayingCardHash.FaceValues.Ace:
                        retVal = "A";
                        break;
                    case PlayingCardHash.FaceValues.Two:
                        retVal = "2";
                        break;
                    case PlayingCardHash.FaceValues.Three:
                        retVal = "3";
                        break;
                    case PlayingCardHash.FaceValues.Four:
                        retVal = "4";
                        break;
                    case PlayingCardHash.FaceValues.Five:
                        retVal = "5";
                        break;
                    case PlayingCardHash.FaceValues.Six:
                        retVal = "6";
                        break;
                    case PlayingCardHash.FaceValues.Seven:
                        retVal = "7";
                        break;
                    case PlayingCardHash.FaceValues.Eight:
                        retVal = "8";
                        break;
                    case PlayingCardHash.FaceValues.Nine:
                        retVal = "9";
                        break;
                    case PlayingCardHash.FaceValues.Ten:
                        retVal = "10";
                        break;
                    case PlayingCardHash.FaceValues.Jack:
                        retVal = "J";
                        break;
                    case PlayingCardHash.FaceValues.Queen:
                        retVal = "Q";
                        break;
                    case PlayingCardHash.FaceValues.King:
                        retVal = "K";
                        break;
                    default:
                        break;
                }
                return retVal;
            }
        }

        public override string ToString()
        {
            return DeckNumber_4Display + "-" + FaceValue_4Display + Suit_4Display;
        }

        public static String Hash(UInt32 theNumericalValue)
        {
            return new PlayingCardHash(theNumericalValue).ToString();
        }

        public static String Hash(Int32 theNumericalValue)
        {
            return Hash(Convert.ToUInt32(theNumericalValue));
        }
    }
}
