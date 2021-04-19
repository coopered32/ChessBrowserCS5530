using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ChessTools
{
    public class PGNReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"> Location of PGN file </param>
        /// <returns> List of ChessGame's obtained from PGN file </returns>
        public static List<ChessGame> FileReader(string filepath)
        {
            // Convert file into array of lines
            string[] readText = File.ReadAllLines(filepath);

            return GameBuilder(readText);
        }

        /// <summary>
        /// Helper method that takes an array of game data and converts it to ChessGame objects
        /// </summary>
        /// <returns> List of ChessGame's obtained from PGN file </returns>
        private static List<ChessGame> GameBuilder(string[] readText)
        {
            List<ChessGame> rtnGames = new List<ChessGame>();

            ChessGame tmpGame = new ChessGame();
            Player tmpWhitePlayer = new Player();
            Player tmpBlackPlayer = new Player();

            int count = 0;

            bool firstSpace = false;

            foreach (string s in readText)
            {
                // Event
                if (Regex.Match(s, @"\bEvent\b", RegexOptions.IgnoreCase).Success)
                {
                    // Used to indicate a new game - insert old game
                    
                    tmpGame.Event = Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", "");
                }

                // Site
                else if (s.Contains("Site"))
                {
                    tmpGame.Site = Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", "");
                }

                // Round
                else if (s.Contains("Round"))
                {
                    tmpGame.Round = Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", "");
                }

                // ECO
                else if (s.Contains("ECO"))
                {
                    tmpGame.ECO = Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", "");
                }

                // WhitePlayer
                else if (Regex.Match(s, @"\bWhite\b", RegexOptions.IgnoreCase).Success)
                {
                    tmpWhitePlayer.Name = Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", "");
                }

                // BlackPlayer
                else if (Regex.Match(s, @"\bBlack\b", RegexOptions.IgnoreCase).Success)
                {
                    tmpBlackPlayer.Name = Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", "");
                }

                // White Elo
                else if (s.Contains("WhiteElo"))
                {
                    tmpWhitePlayer.Elo = Int32.Parse(Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", ""));
                }

                // Black Elo
                else if (s.Contains("BlackElo"))
                {
                    tmpBlackPlayer.Elo = Int32.Parse(Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", ""));
                }

                // Result
                else if (s.Contains("Result"))
                {
                    string gameResult = Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", "");
                    if (gameResult == "1/2-1/2")
                    {
                        tmpGame.Result = "D";
                    }
                    else if (gameResult == "0-1")
                    {
                        tmpGame.Result = "B";
                    }
                    else if (gameResult == "1-0")
                    {
                        tmpGame.Result = "W";
                    }

                }

                // EventDate
                else if (s.Contains("EventDate"))
                {
                    string tmp = Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", "");
                    if (tmp.Contains("?"))
                    {
                        tmpGame.EventDate = "0000-00-00";
                    }
                    else
                    {
                        tmp = tmp.Replace('.', '-');
                        tmpGame.EventDate = tmp;
                    }
                }

                // EventDate
                else if (s.Contains("Date"))
                {
                    string tmp = Regex.Match(s, "\"(.*?)]").ToString().Replace("\"", "").Replace("]", "");
                    if (tmp.Contains("?"))
                    {
                        tmpGame.Date = "0000-00-00";
                    }
                    else
                    {
                        tmp = tmp.Replace('.', '-');
                        tmpGame.Date = tmp;
                    }
                }

                // Moves
                else
                {
                    if (s != "")
                    {
                        tmpGame.Moves += s;
                    }                    
                    else
                    {
                        if (!firstSpace)
                        {
                            firstSpace = true;
                        }
                        else
                        {
                            tmpGame.WhitePlayer = tmpWhitePlayer;
                            tmpGame.BlackPlayer = tmpBlackPlayer;
                            rtnGames.Add(tmpGame);

                            tmpGame = new ChessGame();
                            tmpWhitePlayer = new Player();
                            tmpBlackPlayer = new Player();

                            firstSpace = false;
                        }
                    }
                }
                count++;
            }

            return rtnGames;
        }
    }
}
