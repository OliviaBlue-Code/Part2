using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace demo
{

    public class QuizQ
    {
        public string Question { get; set; }
        public Dictionary<string, string> Options { get; set; }
        public string Correct { get; set; }
        public string Type { get; set; } // "MC" or "TF"
    }
    public class Game
    {

            public List<QuizQ> Questions { get; private set; }
            public int Score { get; private set; }
            public int CurrentIndex { get; private set; }
            public int Total => Questions.Count;

            public Game()
            {
                LoadQuiz();
                Reset();
            }

            private void LoadQuiz()
            {
                Questions = new List<QuizQ>
            {
                new QuizQ{ Question="What should you do if an email asks for your password?", Options=new Dictionary<string,string>{{"A","Reply with password"},{"B","Delete it"},{"C","Report as phishing"},{"D","Ignore it"}}, Correct="C", Type="MC"},
                new QuizQ{ Question="2FA means Two-Factor Authentication.", Options=new Dictionary<string,string>{{"True","True"},{"False","False"}}, Correct="True", Type="TF"},
                new QuizQ{ Question="A Firewall filters network traffic.", Options=new Dictionary<string,string>{{"True","True"},{"False","False"}}, Correct="True", Type="TF"},
                new QuizQ{ Question="Phishing is...", Options=new Dictionary<string,string>{{"A","Fast internet"},{"B","A scam to steal info"},{"C","A game"},{"D","Antivirus"}}, Correct="B", Type="MC"},
                new QuizQ{ Question="Strong passwords should use personal details.", Options=new Dictionary<string,string>{{"True","True"},{"False","False"}}, Correct="False", Type="TF"},
                new QuizQ{ Question="Ransomware does what?", Options=new Dictionary<string,string>{{"A","Speeds up PC"},{"B","Encrypts files for ransom"},{"C","Deletes cookies"},{"D","Updates Windows"}}, Correct="B", Type="MC"},
                new QuizQ{ Question="VPN encrypts your internet traffic.", Options=new Dictionary<string,string>{{"True","True"},{"False","False"}}, Correct="True", Type="TF"},
                new QuizQ{ Question="Malware means...", Options=new Dictionary<string,string>{{"A","Good software"},{"B","Hardware"},{"C","Malicious software"},{"D","Memory"}}, Correct="C", Type="MC"},
                new QuizQ{ Question="You should click all email links.", Options=new Dictionary<string,string>{{"True","True"},{"False","False"}}, Correct="False", Type="TF"},
                new QuizQ{ Question="IDS stands for...", Options=new Dictionary<string,string>{{"A","Internet Data System"},{"B","Intrusion Detection System"},{"C","Internal Disk Storage"},{"D","Instant Download Service"}}, Correct="B", Type="MC"},
                new QuizQ{ Question="Sharing passwords is safe with friends.", Options=new Dictionary<string,string>{{"True","True"},{"False","False"}}, Correct="False", Type="TF"},
                new QuizQ{ Question="Best action if hacked?", Options=new Dictionary<string,string>{{"A","Do nothing"},{"B","Log out all devices + change password"},{"C","Post on social media"},{"D","Ignore"}}, Correct="B", Type="MC"}
            };
            }

            public void Reset()
            {
                Score = 0;
                CurrentIndex = 0;
            }

            public QuizQ GetCurrentQuestion()
            {
                if (CurrentIndex >= Total) return null;
                return Questions[CurrentIndex];
            }

            public bool SubmitAnswer(string answerTag)
            {
                if (CurrentIndex >= Total) return false;
                bool correct = answerTag == Questions[CurrentIndex].Correct;
                if (correct) Score++;
                CurrentIndex++;
                return correct;
            }

            public bool IsFinished() => CurrentIndex >= Total;

            public string GetFeedback()
            {
                return Score >= 9 ? "Great job! You're a cybersecurity pro!" : "Keep learning to stay safe online!";
            }
        }
    }

