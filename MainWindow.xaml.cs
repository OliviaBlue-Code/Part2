using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static Microsoft.Data.SqlClient.Internal.SqlClientEventSource;

namespace demo
{
    public partial class MainWindow : Window
    {
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();
        TasksRepo repo = new TasksRepo();
        NLPProcessor nlp = new NLPProcessor();

        string username = string.Empty;
        string currentUser = string.Empty;
        int counting = 0;
        Game quizGame;
        GameVoice voice = new GameVoice();

        public MainWindow()
        {
            InitializeComponent();
            new respond(reply, ignore) { };
            voice_greeting greet = new voice_greeting();
            greet.greet();
            quizGame = new Game();
        }

        private void proceed(object sender, RoutedEventArgs e)
        {
            home_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        // === NAVIGATION ===
        private void StartChat_Click(object sender, RoutedEventArgs e)
        {
            menu_grid.Visibility = Visibility.Hidden;
            chat_grid.Visibility = Visibility.Visible;
        }
        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            menu_grid.Visibility = Visibility.Hidden;
            quiz_grid.Visibility = Visibility.Visible;
            quizGame.Reset();
            ShowQuestion();
        }
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            chat_grid.Visibility = Visibility.Hidden;
            quiz_grid.Visibility = Visibility.Hidden;
            menu_grid.Visibility = Visibility.Visible;
        }

        // === QUIZ UI LOGIC ===
        private void ShowQuestion()
        {
            var q = quizGame.GetCurrentQuestion();
            if (q == null) { EndQuiz(); return; }

            QuestionText.Text = $"Q{quizGame.CurrentIndex + 1}/12: {q.Question}";
            QuizScoreText.Text = $"Score: {quizGame.Score}/12";
            ResetButtonColors();
            SetButtonsEnabled(true);

            BtnA.Visibility = BtnB.Visibility = BtnC.Visibility = BtnD.Visibility = Visibility.Collapsed;
            BtnTrue.Visibility = BtnFalse.Visibility = Visibility.Collapsed;

            if (q.Type == "MC")
            {
                BtnA.Content = $"A) {q.Options["A"]}"; BtnA.Visibility = Visibility.Visible;
                BtnB.Content = $"B) {q.Options["B"]}"; BtnB.Visibility = Visibility.Visible;
                BtnC.Content = $"C) {q.Options["C"]}"; BtnC.Visibility = Visibility.Visible;
                BtnD.Content = $"D) {q.Options["D"]}"; BtnD.Visibility = Visibility.Visible;
            }
            else { BtnTrue.Content = "True"; BtnTrue.Visibility = Visibility.Visible; BtnFalse.Content = "False"; BtnFalse.Visibility = Visibility.Visible; }
        }

        public void Answer_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button clickedBtn)) return;
            string tag = clickedBtn.Tag?.ToString();
            if (string.IsNullOrEmpty(tag)) return;

            SetButtonsEnabled(false);
            string correctAnswer = quizGame.Questions[quizGame.CurrentIndex].Correct;
            bool correct = quizGame.SubmitAnswer(tag);
            HighlightAnswer(clickedBtn, correctAnswer);

            if (correct)
            {
                voice.Speak("Correct! Well done");
                error_method("ChatBot", "Correct! Well done 👏");
            }
            else
            {
                voice.Speak($"Wrong. The correct answer is {correctAnswer}");
                error_method("ChatBot", $"Wrong ❌ Correct answer: {correctAnswer}");
            }

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += (s, args) => { timer.Stop(); ShowQuestion(); };
            timer.Start();
        }

        private void HighlightAnswer(Button clickedBtn, string correctAnswer)
        {
            foreach (var btn in GetAllAnswerButtons()) { btn.Background = new SolidColorBrush(Color.FromRgb(220, 220, 220)); btn.Foreground = Brushes.Black; }
            Button correctBtn = GetButtonByTag(correctAnswer);
            if (correctBtn != null) { correctBtn.Background = new SolidColorBrush(Color.FromRgb(144, 238, 144)); correctBtn.Foreground = Brushes.DarkGreen; }
            if (clickedBtn.Tag.ToString() != correctAnswer) { clickedBtn.Background = new SolidColorBrush(Color.FromRgb(255, 99, 71)); clickedBtn.Foreground = Brushes.White; }
        }

        private void ResetButtonColors() { foreach (var btn in GetAllAnswerButtons()) { btn.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)); btn.Foreground = Brushes.Black; } }
        private void SetButtonsEnabled(bool enabled) { foreach (var btn in GetAllAnswerButtons()) btn.IsEnabled = enabled; }
        private List<Button> GetAllAnswerButtons() { return new List<Button> { BtnA, BtnB, BtnC, BtnD, BtnTrue, BtnFalse }; }
        private Button GetButtonByTag(string tag) { return GetAllAnswerButtons().FirstOrDefault(b => b.Tag?.ToString() == tag); }

        private void EndQuiz()
        {
            voice.Speak($"Quiz over. Final Score: {quizGame.Score} out of 12. {quizGame.GetFeedback()}");
            string feedback = quizGame.GetFeedback();
            error_method("ChatBot", $"Quiz over! Final Score: {quizGame.Score}/12. {feedback}");
            MessageBox.Show($"Quiz over!\nFinal Score: {quizGame.Score}/12\n{feedback}");
            BackToMenu_Click(null, null);
        }

        private void submit_name(object sender, RoutedEventArgs e)
        {
            string input = usernames_input.Text.Trim();

            if (string.IsNullOrWhiteSpace(input)) { MessageBox.Show("Name cannot be empty."); usernames_input.Clear(); usernames_input.Focus(); return; }
            if (input.Length < 3) { MessageBox.Show("Name must be at least 3 characters long."); usernames_input.Clear(); usernames_input.Focus(); return; }
            if (!HasNoNumbers(input)) { MessageBox.Show("Numbers are not allowed in names."); usernames_input.Clear(); usernames_input.Focus(); return; }

            bool isReturning = CheckNameExists(input);
            if (!isReturning) SaveName(input);
            username = input;
            currentUser = input;

            username_grid.Visibility = Visibility.Hidden;
            menu_grid.Visibility = Visibility.Visible;
            MenuWelcome.Text = $"Welcome, {username} 👋";

            string timeGreeting = GetTimeGreeting();
            if (isReturning) error_method("ChatBot", $"{timeGreeting} {username} 👋 Welcome back!");
            else error_method("ChatBot", $"{timeGreeting} {username}! Welcome to AEGISNODE");
        }

        private void usernames_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                submit_name(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }
        private string GetTimeGreeting() { int hour = DateTime.Now.Hour; if (hour < 12) return "Good morning"; else if (hour < 17) return "Good afternoon"; else return "Good evening"; }

        private void send(object sender, RoutedEventArgs e)
        {
            string Question = question.Text.ToString().Trim();
            if (string.IsNullOrWhiteSpace(Question)) { error_method("ChatBot", "Please enter a question."); return; }
            string questions = RemoveSpecialCharacters(Question);
            error_method(username, Question);
            question.Clear();
            string lower = Question.ToLower();

            if (lower.StartsWith("remind me to "))
            {
                string taskText = Question.Substring("remind me to ".Length).Trim();
                DateTime? date = nlp.ExtractDate(taskText);
                string title = nlp.ExtractTitle(taskText);
                repo.AddTask(currentUser, title, date);
                error_method("ChatBot", date.HasValue ? $"Got it ✅ I'll remind you to '{title}' on {date.Value:dd MMM yyyy HH:mm}" : $"Got it ✅ I'll remind you to '{title}'");
                return;
            }
            else if (lower == "view tasks")
            {
                var tasks = repo.GetTasks(currentUser);
                if (tasks.Count == 0) error_method("ChatBot", "You have no active tasks 💤");
                else { error_method("ChatBot", "Here are your tasks:"); foreach (var t in tasks) { string date = t.ReminderDate.HasValue ? t.ReminderDate.Value.ToString("dd MMM yyyy HH:mm") : "No date"; error_method("ChatBot", $"[ID:{t.TaskId}] {t.Title} | Due: {date} | Status: {(t.IsCompleted ? "DONE" : "PENDING")}"); } }
                return;
            }
            else if (lower.StartsWith("delete task "))
            {
                string idPart = lower.Replace("delete task ", "").Trim();
                if (int.TryParse(idPart, out int taskId)) { repo.DeleteTask(taskId, currentUser); error_method("ChatBot", $"Task {taskId} deleted 🗑️"); }
                else error_method("ChatBot", "Use: delete task 2");
                return;
            }

            auto_show_interest();
            ai_check(questions);
        }

        public bool HasNoNumbers(string input) => Regex.IsMatch(input, @"^\D+$");
        private bool CheckNameExists(string name) { string filename = "user_names.txt"; if (!File.Exists(filename)) { File.WriteAllText(filename, ""); return false; } string[] names = File.ReadAllLines(filename); foreach (string name_found in names) if (name_found.Trim().Equals(name, StringComparison.OrdinalIgnoreCase)) return true; return false; }
        private void SaveName(string name) { string filename = "user_names.txt"; File.AppendAllText(filename, name + Environment.NewLine); }

        private void SaveInterest(string word)
        {
            if (string.IsNullOrWhiteSpace(currentUser)) return;
            string filename = "interested_topic.txt";
            string newLine = currentUser + ": interested in: " + word;
            if (!File.Exists(filename)) { File.WriteAllText(filename, newLine + Environment.NewLine); return; }
            var lines = File.ReadAllLines(filename).ToList();
            int index = lines.FindIndex(l => l.StartsWith(currentUser + ":"));
            if (index >= 0) lines[index] = newLine; else lines.Add(newLine);
            File.WriteAllLines(filename, lines);
        }

        private void ai_check(string questions)
        {
            if (string.IsNullOrWhiteSpace(questions)) { error_method("ChatBot", "Please enter a valid question."); question.Clear(); return; }

            questions = questions.ToLower();
            string[] words = questions.Split(new char[] { ' '}, StringSplitOptions.RemoveEmptyEntries);
            string emotionReply = null;
            HashSet<string> topicReplies = new HashSet<string>(); //  No dupes

            string[] emotions = { "frustrated", "angry", "confused", "sad", "happy", "excited", "worried" };
            string[] greetings = { "hello", "hi", "hey", "good morning", "good evening", "good day" };

            // 1. Emotion check
            foreach (string word in words)
            {
                if (emotions.Contains(word))
                {
                    foreach (string answerStr in reply)
                    {
                        if (answerStr.StartsWith(word + " ")) { emotionReply = answerStr.Substring(word.Length + 1); break; }
                    }
                    if (emotionReply != null) break;
                }
            }

            // 2. Greeting check
            bool onlyGreeting = words.All(w => greetings.Contains(w) || ignore.Contains(w));
            if (onlyGreeting) topicReplies.Add("🤗Hey! Ask me about passwords, phishing, malware, encryption, 2FA, or ransomware.");

            // 3. Topic check - runs once
            foreach (string word in words)
            {
                foreach (string answerStr in reply)
                {
                    string[] parts = answerStr.Split(new char[] { ' '}, 2);
                    string keyword = parts[0].ToLower();
                    if (word == keyword && !emotions.Contains(word) && !greetings.Contains(word))
                    {
                        string responseText = parts.Length > 1 ? parts[1] : "";
                        topicReplies.Add(responseText); // HashSet kills duplicates
                        if (questions.Contains("interested in " + keyword) || questions.Contains("i like " + keyword)) { SaveInterest(keyword); }
                    }
                }
            }

            // 4. Send only 1 response - OUTSIDE ALL LOOPS
            Random random = new Random();
            List<string> responsesToSend = new List<string>();
            if (emotionReply != null) responsesToSend.Add(emotionReply);
            if (topicReplies.Count > 0) responsesToSend.Add(topicReplies.ElementAt(random.Next(0, topicReplies.Count)));

            if (responsesToSend.Count > 0) error_method("ChatBot", string.Join("\n\n", responsesToSend));
            else { string[] fallbackMessages = { "Hmm, I'm not sure I got that. Try asking me about phishing, passwords, or malware.", "I didn't catch that. What part of cybersecurity do you want to talk about?", "Mind rephrasing that? I'm still learning." }; error_method("ChatBot", fallbackMessages[random.Next(fallbackMessages.Length)]); }

            question.Clear();
        }

        private string RemoveSpecialCharacters(string input) { if (string.IsNullOrWhiteSpace(input)) return string.Empty; StringBuilder sanitized = new StringBuilder(); int i = 0; while (i < input.Length) { string ch = input.Substring(i, 1); if (Regex.IsMatch(ch, "[a-zA-Z0-9\\s'\\-]")) sanitized.Append(ch); else sanitized.Append(" "); i = i + 1; } string result = sanitized.ToString(); result = Regex.Replace(result, "\\s+", " ").Trim(); return result; }

        private void auto_show_interest()
        {
            counting += 1; //  Count every message
            if (counting >= 3) //  Show every 3 messages
            {
                string filename = "interested_topic.txt";
                if (File.Exists(filename))
                {
                    string[] lines = File.ReadAllLines(filename);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith(currentUser))
                        {
                            int colonIndex = line.IndexOf("interested in:");
                            if (colonIndex >= 0) { string interests = line.Substring(colonIndex + 14).Trim(); error_method("ChatBot", "I remember you're interested in " + interests + " 😊"); break; }
                        }
                    }
                }
                counting = 0; // Reset after showing
            }
        }

        private void error_method(string name, string message)
        {
            Border messageBorder = new Border { Margin = new Thickness(0, 2, 0, 2), Padding = new Thickness(5, 3, 5, 3), CornerRadius = new CornerRadius(5) };
            if (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat")) { messageBorder.Background = new SolidColorBrush(Color.FromRgb(240, 248, 255)); messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(173, 216, 230)); }
            else { messageBorder.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)); messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(211, 211, 211)); }
            messageBorder.BorderThickness = new Thickness(1);
            TextBlock messageText = new TextBlock { TextWrapping = TextWrapping.Wrap, Margin = new Thickness(2) };
            Brush nameColor = (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat")) ? Brushes.DarkBlue : Brushes.DarkGreen;
            Brush messageColor = Brushes.Black;
            messageText.Inlines.Add(new Run { Text = name + ": ", Foreground = nameColor, FontWeight = FontWeights.Bold });
            messageText.Inlines.Add(new Run { Text = message, Foreground = messageColor });
            messageBorder.Child = messageText;
            chats.Items.Add(messageBorder);
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
        }
    }
}
