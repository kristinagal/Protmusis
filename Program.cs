namespace Protmusis
{
    public class Program
    {
        // klausimu ir atsakymu zodynai, viename 1-asis atsakymas visada teisingas, antrame random klausimu ir atsakymu tvarka
        public static Dictionary<string, List<string>> questionsWithAnswers1stCorrect = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> questionsWithAnswersShuffled = new Dictionary<string, List<string>>();

        ///pagrindinis zaideju ir ju tasku zodynas
        public static Dictionary<string, int> playersDictionary = new Dictionary<string, int>();

        //zaidejas ir tiek kelintu klausimu sustojo atsakinet, naudinga, jei vidury zaidimo atsijungia ir vel prisijungia
        public static Dictionary<string, int> playersCurrentQuestion = new Dictionary<string, int>();

        //kiekvieno zaidejo klausimu tvarkos listas, jei atsijungia ir vel prisijungia
        public static Dictionary<string, List<string>> playersCurrentQuestionOrder = new Dictionary<string, List<string>>();

        //abu naudojami pateikti teisingai/neteisingai atsakytu klausimu sarasus pabaigus zaidima
        public static Dictionary<string, List<int>> playersCorrectAnsweredQuestions = new Dictionary<string, List<int>>(); 
        public static Dictionary<string, List<int>> playersIncorrectAnsweredQuestions = new Dictionary<string, List<int>>();

        //cia saugosim dabartini zaideja
        public static string currentPlayer = "";

        //cia saugosim, i kuri klausima duotuoju metu turetu atsakineti zaidejas (atsijungimo ir pakartotinio prisijungimo atveju)
        public static int currentQuestion = 1;

        //kiek klausimu uzduodama kiekvienam zaidejui, ne daugiau nei yra pradiniame klausimu atsakymu zodyne questionsWithAnswers1stCorrect
        public static int questionCount = 5;

        public static Random random = new Random(); 

        static void Main(string[] args)
        {
            Program.playersDictionary.Add("Player1", 0);
            Program.playersDictionary.Add("Player2", 5);
            Program.playersDictionary.Add("Player3", 1);
            Program.playersDictionary.Add("Player4", 4);
            Program.playersDictionary.Add("Player5", 2);
            Program.playersDictionary.Add("Player6", 3);
            Program.playersDictionary.Add("Player7", 0);
            Program.playersDictionary.Add("Player8", 5);
            Program.playersDictionary.Add("Player9", 2);
            Program.playersDictionary.Add("Player10", 5);
            Program.playersDictionary.Add("Player11", 3);

            Protmusis();


            ////GetValidPlayersAnswer testavimas (del console.readline nepavyko padaryti unit testu)
            
            //int lower = 1;
            //int upper = 10;
            //bool qCheck = false;
            //Console.WriteLine("input 5");
            //Console.WriteLine($"test successful? {Program.GetValidPlayersAnswer(lower, upper, qCheck) == 5}"); 

            //lower = 1;
            //upper = 10;
            //qCheck = true;
            //Console.WriteLine("input 5");
            //Console.WriteLine($"test successful? {Program.GetValidPlayersAnswer(lower, upper, qCheck) == 5}");

            //lower = 1;
            //upper = 10;
            //qCheck = true;
            //Console.WriteLine("input q");
            //Console.WriteLine($"test successful? {Program.GetValidPlayersAnswer(lower, upper, qCheck) == 0}");

            //lower = 1; //invalid range - cia mums tinka tik jei iveda q
            //upper = 0;
            //qCheck = true;
            //Console.WriteLine("input q");
            //Console.WriteLine($"test successful? {Program.GetValidPlayersAnswer(lower, upper, qCheck) == 0}");
        }

        public static void Protmusis()
        {
            Console.WriteLine("Welcome to PROTMUSIS!");
            Console.WriteLine();
            CreateQuestions(); // sitas vykdomas tik viena karta

            LogIn(); //pradedame nuo login lango

            PrintMenu(); //sekmingai prisijungus atspausdinamas meniu
        }


        //*******************Going through menu*******************
        public static void PrintMenu()
        {
            int option = -1;

            ShowCurrentPlayer(); //visuomet virsuje rodomas siuo metu prisijunges zaidejas

            do
            {
                Console.Clear();
                Console.WriteLine("Menu:");
                Console.WriteLine();
                Console.WriteLine("1. Log In");
                Console.WriteLine("2. Rules of the game");
                Console.WriteLine("3. Results and Players");
                Console.WriteLine("4. Start Game");
                Console.WriteLine("5. Log Out");
                Console.WriteLine("6. Exit");
                Console.WriteLine();


                if (currentPlayer == "") //jei niekas neprisijunges, visada pirmiausia reikes prisijungti
                {
                    option = 1;
                }
                else
                {
                    option = ReadPlayersMenuDecision(); //option priskiriama zaidejo ivestis
                }

                Console.Clear();
                GoToPlayersMenuDecision(option); //einama i pasirinkta meniu punkta

                if (option != 6)
                {
                    ShowCurrentPlayer();
                }             

            }
            while (option != 6);

            Console.Clear();

        }

        public static void PrintResultsMenu()
        {
            int option = -1;

            ShowCurrentPlayer();

            do
            {
                Console.WriteLine("Results menu:");
                Console.WriteLine();
                Console.WriteLine("1. Players list");
                Console.WriteLine("2. Rankings");
                Console.WriteLine();

                option = ReadPlayersResultsDecision(); //option priskiriama zaidejo ivestis
                GoToPlayersResultsDecision(option);  //einama i pasirinkta rezultatu meniu punkta

                Console.Clear();
                ShowCurrentPlayer();
            }
            while (option != 0); // cia nulis atitinka ivesta q
            Console.Clear();

        }

        public static void GoToPlayersMenuDecision(int option)
        {
            switch (option) //judejimui po meniu punktus
            {
                case 1:
                    LogIn();
                    break;
                case 2:
                    PrintRules();
                    break;
                case 3:
                    PrintResultsMenu();
                    break;
                case 4:
                    StartGame();
                    break;
                case 5:
                    LogOut();
                    break;
                case 6: //pasirinko exit, griztam i meniu ir jame iseinama is ciklo
                    break;
                case 0: //paspaude q
                    break;
                default:
                    break;
            }
        }

        //*******************Player input handling*******************
        public static void GoToPlayersResultsDecision(int option)
        {
            switch (option)
            {
                case 1:
                    PrintPlayers();
                    break;
                case 2:
                    PrintRankings(AddStarsToTopPlayers());
                    break;
                case 0: //paspaude q
                    break;
                default:
                    break;
            }
        }

        public static int GetValidPlayersAnswer(int lower, int upper, bool qCheck)
        //grazina tik validu atsakyma = tarp lower ir upper, papildomai gali tikrint ir q
        {
            int answer = -1;
            bool valid = false;
            string text = "";

            while (true)
            {
                int currentLineCursor = Console.CursorTop;

                text = Console.ReadLine()?.Trim().ToUpper();

                if (text == "Q" && qCheck) // paspaude q
                {
                    return 0; //jei metodui padaveme qCheck = true ir buvo ivesta q, grazinamas 0
                }

                valid = int.TryParse(text, out answer);

                if (!valid || answer < lower || answer > upper)
                {
                    ClearInvalidInputFromConsole(); //istrinama is konsoles mums netinkanti ivestis
                }
                else
                {
                    break; // ivede validu atsakyma
                }
            }

            return answer;
        }

        public static void ClearInvalidInputFromConsole()
        //naudojama kituose metoduose istrinti is konsoles ivesta mums netinkancia ivesti
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }
        public static bool PlayerPressedQtoExit()
        {
            //cia tikimasi, kad zaidejas ives butent q, nes paduodam neimanomas ispildyti lower ir upper vertes
            int option = GetValidPlayersAnswer(1, 0, true);

            if (option == 0) //buvo ivesta q
            {
                return true;
            }

            return false;
        }

        public static bool PlayerPressedAnyKeyOrQ() //tinka bet koks paspaustas key, jei paspausta q - true
        {
            string option = Console.ReadLine()?.Trim().ToUpper();
            return option.Equals("q", StringComparison.OrdinalIgnoreCase);
        }

        public static int ReadPlayersResultsDecision()
        {
            Console.WriteLine("Choose what you like to see (1-2) or press 'q' to go to menu");

            return GetValidPlayersAnswer(1, 2, true); //cia mums tinka, jei zaidejas ives 1, 2 arba q
        }

        public static int ReadPlayersMenuDecision()
        {
            Console.WriteLine("What would you like to do (1-6)?");

            return GetValidPlayersAnswer(1, 6, true); //cia mums tinka, jei zaidejas ives 1-6 arba q
        }

        public static void ReadPlayersQ()
        {
            //sitas metodas pridedamas visur, kur norime isvesti i konsole galimybe paspausti q grizimui i meniu
            Console.WriteLine("Press 'q' to go to menu.");
            bool exit;
            do
            {
                exit = PlayerPressedQtoExit();
            }
            while (!exit);
            Console.Clear();
        }

        public static int ReadPlayersAnswer()
        {
            Console.WriteLine("Your answer is (1-4): ");

            return GetValidPlayersAnswer(1, 4, false); //naudojamas zaidejui ivesti atsakyma i klausima, tinka tik 1-4 ivestys

        }

        //*******************Game*******************
        public static void StartGame()
        {
            if (string.IsNullOrEmpty(currentPlayer)) //tikrina, ar yra prisijunges zaidejas
            {
                Console.Clear();
                ShowCurrentPlayer();
                Console.WriteLine("You are not logged in and cannot play.");
                Console.WriteLine();
                return;
            }

            if (playersCurrentQuestion.TryGetValue(currentPlayer, out int savedQuestion)) 
            //tikrina, ar zaidejas jau buvo prisijunges seniau ir jau atsake i dali klausimu
            {
                currentQuestion = Math.Max(currentQuestion, savedQuestion);
            }

            while (currentQuestion <= questionCount) //questioncount nustatomas globaliuose kintamuosiuose
            {
                Console.Clear();
                ShowCurrentPlayer();
                Console.WriteLine($"Question {currentQuestion}/{questionCount}."); //kelintas klausimas is visu klausimu
                PrintOneQuestionWithAnswersFromShuffled(currentQuestion - 1); //atspausdina klausima is shuffled zodyno, paduodame norimo klausimo indeksa

                int answer = ReadPlayersAnswer(); //nuskaitomas zaidejo atsakymas

                bool isCorrect = CheckIfAnswerIsCorrect(currentQuestion - 1, answer); //patikrinama, ar atsake teisingai

                if (isCorrect) //priklausomai nuo to, ar atsake teisingai, ar ne, isvedama informacija zaidejui i konsole
                {
                    UpdatePointsForCurrentPlayer(1);
                    Console.WriteLine($"You are correct! Total points: {playersDictionary[currentPlayer]}.");
                    AddToListQuestionWithAnswer(playersCorrectAnsweredQuestions);
                }
                else
                {
                    Console.WriteLine($"You are wrong... Total points: {playersDictionary[currentPlayer]}.");
                    AddToListQuestionWithAnswer(playersIncorrectAnsweredQuestions);
                }
                Console.WriteLine();

                currentQuestion++; //pereinama prie kito klausimo

                if (currentQuestion <= questionCount) //zaidziama, kol neatsakoma i nurodyta klausimu kieki
                {
                    Console.WriteLine("Press any key to continue or press 'q' to go to menu.");
                    if (PlayerPressedAnyKeyOrQ())
                    {
                        return;
                    }
                }
            }

            Console.Clear();
            //isvedami galutiniai dabartinio zaidejo rezultatai
            Console.WriteLine($"{currentPlayer}, you have successfully finished your game.");
            Console.WriteLine($"Your ranking: {GetPlayerRanking(currentPlayer)}");
            Console.WriteLine($"Your correctly answered questions: {GetAnsweredQuestionsListed(playersCorrectAnsweredQuestions)}");
            Console.WriteLine($"Your incorrectly answered questions: {GetAnsweredQuestionsListed(playersIncorrectAnsweredQuestions)}");
            Console.WriteLine();

            ReadPlayersQ();
        }

        public static string GetPlayerRanking(string playerName) //is visu zaideju turnyrines lenteles paimamas konkretus zaidejas ir jo pozicija
        {
            var playersListWithStars = AddStarsToTopPlayers(); //perskaiciuojama turnyrine lentele


            if (playersListWithStars.ContainsKey(playerName) && playersDictionary.ContainsKey(playerName))
            {
                return $"{playersListWithStars[playerName]} {playersDictionary[playerName]}";
            }
            else
            {
                throw new KeyNotFoundException($"Player '{playerName}' not found in dictionary.");
            }
        }

        public static void AddToListQuestionWithAnswer(Dictionary<string, List<int>> dictionary)
            // registruojame zaidejo atsakytus ir neatsakytus klausimus zaidimo metu 
        {
            if (!dictionary.ContainsKey(currentPlayer))
            {
                dictionary[currentPlayer] = new List<int>();
            }
            dictionary[currentPlayer].Add(currentQuestion);
        }

        public static string GetAnsweredQuestionsListed(Dictionary<string, List<int>> dictionary)
            //grazina string tipo kintamaji, jame isvardinti per kableli atsakyti/neatsakyti klausimai
        {
            if (dictionary.TryGetValue(currentPlayer, out List<int> indexes) && indexes.Count > 0)
            {
                return string.Join(", ", indexes);
            }
            else
            {
                return "None"; // jei visus atsake teisingai arba neteisingai
            }
        }

        public static void UpdatePointsForCurrentPlayer(int points)
        //zaidejui pridedami taskai
        {
            playersDictionary[currentPlayer] += points;
        }

        public static void PrintOneQuestionWithAnswersFromShuffled(int questionIndex)
        //atspausdinamas paduoto indekso klausimas, o cikle atspausdinami atsakymu variantai
        {
            var question = GetQuestionOutOfShuffled(questionIndex); //paimame is zodyno mums reikalinga key value pora

            Console.WriteLine(question.Key);
            for (int i = 0; i < question.Value.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {question.Value[i]}");
            }
        }

        public static KeyValuePair<string, List<string>> GetQuestionOutOfShuffled(int questionIndex)
        {
            if (questionIndex < 0 || questionIndex >= questionsWithAnswersShuffled.Count)
            {
                Console.WriteLine("Invalid question index.");
                return default;
            }

            return questionsWithAnswersShuffled.ElementAt(questionIndex);
        }

        public static bool CheckIfAnswerIsCorrect(int questionIndex, int playersAnswer)
        //tikrinama, ar zaidejo pasirinktas atsakymas sutampa su pirmuoju atsakymu zodyne questionsWithAnswers1stCorrect
        {
            var question = questionsWithAnswersShuffled.ElementAt(questionIndex);

            var correctAnswer = questionsWithAnswers1stCorrect[question.Key][0]; //teisingas atsakymas string tipo

            int correctAnswerIndex = question.Value.IndexOf(correctAnswer);

            return correctAnswerIndex == playersAnswer - 1;
        }

        //*******************Log in and Log out*******************
        public static void LogOut()
        //atsijungimo metu pakeiciami tam tikri kintamieji - currentPlayer, currentQuestion
        {
            if (currentPlayer != "")
                playersCurrentQuestion[currentPlayer] = currentQuestion;

            currentPlayer = "";
            currentQuestion = 1;

            Console.WriteLine("You are now logged out.");
            Console.WriteLine();

            ReadPlayersQ();
        }
        public static void LogIn()
        {
            if (currentPlayer == "") //leidziame login tik kai niekas neprisijunges
            {
                Console.WriteLine("Please enter your name and surname to log in");
                Console.WriteLine();

                Console.WriteLine("Name:");
                var text1 = CapitalizeFirstLetter(GetValidPlayerName()); //formatavimas ir ivesties nuskaitymas
                Console.WriteLine("Surname:");
                var text2 = CapitalizeFirstLetter(GetValidPlayerName());
                Console.WriteLine();

                string playerLoggingIn = $"{text1} {text2}";
                bool isNewPlayer = playersDictionary.TryAdd(playerLoggingIn, 0);
                //jei buvo ivestas naujas zaidejas, pridedamas i zaideju sarasa, jei jau esamas, tai nedaroma nieko

                currentPlayer = playerLoggingIn; //nustatome currentPlayer
                RandomizeQuestionsOrder(); //kiekvienam zaidejui nustatoma random klausimu ir atsakymu tvarka

                Console.Clear();
                ShowCurrentPlayer();
                Console.WriteLine();
                Console.WriteLine($"Welcome to the game, {currentPlayer}.");
                if (!isNewPlayer) //ismetamas pranesimas, jei prisijungiama su tuo paciu useriu nebe pirma karta
                {
                    Console.WriteLine("This is not your first log in. You will continue your previous game.");
                }
                Console.WriteLine();
            }
            else //bandoma prisijungti, kai jau yra prisijungta
            {
                Console.WriteLine("You are already logged in.");
                Console.WriteLine();
            }
            
            ReadPlayersQ();

        }
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        public static string GetValidPlayerName()
        {
            string formattedName = "";
            bool isValid = false;

            while (!isValid) //cia nustatoma, kad mums tinka bet kokia ne tuscia ivestis
            {
                string input = Console.ReadLine()?.Trim(); // pridetas ? neleis errorintis trim'ui, jei ivesta empty value

                if (!string.IsNullOrEmpty(input))
                {
                    formattedName = CapitalizeFirstLetter(input);
                    isValid = true;
                }
                else
                {
                    ClearInvalidInputFromConsole();
                }
            }
            return formattedName;
        }

        //*******************Rankings and listing players*******************
        public static void PrintPlayers()
        {
            Console.Clear();
            ShowCurrentPlayer();

            if (!playersDictionary.Any())
            {
                Console.WriteLine("The players list is empty.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Players list");
                Console.WriteLine();

                foreach (var player in playersDictionary)
                {
                    Console.WriteLine(player.Key);
                }

                Console.WriteLine();
            }

            ReadPlayersQ();

        }

        public static void PrintRankings(Dictionary<string, string> playersListWithStars)
        {
            Console.Clear();
            ShowCurrentPlayer();

            if (!playersDictionary.Any())
            {
                Console.WriteLine("The players list is empty.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Rankings");
                Console.WriteLine();

                foreach (var player in playersListWithStars)
                {
                    Console.WriteLine(GetPlayerRanking(player.Key));
                }

                Console.WriteLine();
            }

            ReadPlayersQ();
        }

        public static Dictionary<string, string> AddStarsToTopPlayers()
        {
            var sortedPlayers = playersDictionary.OrderByDescending(x => x.Value).ToList();

            var playersListWithStars = new Dictionary<string, string>();

            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                var player = sortedPlayers[i];
                string playerNameWithStars = player.Key;

                switch (i) //pridedamos * ir pozicijos pirmiems trims zaidejams
                {
                    case 0:
                        playerNameWithStars = $"1. {player.Key}*";
                        break;
                    case 1:
                        playerNameWithStars = $"2. {player.Key}**";
                        break;
                    case 2:
                        playerNameWithStars = $"3. {player.Key}***";
                        break;
                    default:
                        if (i >= 3 && i < 10) //pridedamos tik pozicijos iki 10 vietos imtinai
                        {
                            playerNameWithStars = $"{i + 1}. {player.Key}";
                        }
                        break;
                }

                playersListWithStars.Add(player.Key, playerNameWithStars); //visi like zaidejai lieka nepasikeite
            }

            return playersListWithStars;
        }
        //*******************Rules*******************
        public static void PrintRules()
        {
            ShowCurrentPlayer();

            Console.WriteLine("Welcome to Protmusis!");
            Console.WriteLine();
            Console.WriteLine($"You will get {questionCount} random questions.");
            Console.WriteLine("Each will have 4 answer options. One of them is correct.");
            Console.WriteLine("Please answer each question by typing number of the answer (1-4).");
            Console.WriteLine();
            Console.WriteLine("You can leave game in the middle of questions and come back later.");
            Console.WriteLine("When you log out, other players will be able to join the game.");
            Console.WriteLine();
            Console.WriteLine("Good luck!");
            Console.WriteLine();
            ReadPlayersQ();
        }
        //*******************Show current player*******************
        public static void ShowCurrentPlayer()
        {
            if (string.IsNullOrEmpty(currentPlayer))
            {
                Console.WriteLine("Currently no one is logged in.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Currently logged in: {currentPlayer}.");
                Console.WriteLine();
            }                    
        }

        //*******************Questions and shuffling*******************
        public static void RandomizeQuestionsOrder() //zodyne questionsWithAnswersShuffled sukeiciami klausimai vietoms
        {
            questionsWithAnswersShuffled.Clear();

            CopyOriginalToShuffled();

            ShuffleAnswers();

            var shuffledQuestions = GetShuffledQuestionOrder();
            ReconstructShuffledQuestions(shuffledQuestions);
        }

        private static void CopyOriginalToShuffled()
            //is zodyno questionsWithAnswers1stCorrect nukopijuojami klausimai/atsakymai i questionsWithAnswersShuffled zodyna
        {
            foreach (var question in questionsWithAnswers1stCorrect)
            {
                questionsWithAnswersShuffled.Add(question.Key, new List<string>(question.Value));
            }
        }

        private static void ShuffleAnswers() //sukeiciami vietomis visu klausimu atsakymai
        { 
            foreach (var question in questionsWithAnswersShuffled)
            {
                ShuffleAnswersList(question.Value);
            }
        }

        public static List<string> GetShuffledQuestionOrder()
        //gauname klausimu eiles tvarka arba nauja, arba jau zaidusio zaidejo is playersCurrentQuestionOrder
        {
            if (!string.IsNullOrEmpty(currentPlayer) && playersCurrentQuestionOrder.ContainsKey(currentPlayer))
            //jei zaidejas buvo atsijunges ir prisijunge vel, gauna ta pacia klausimu eiles tvarka
            {
                return playersCurrentQuestionOrder[currentPlayer];
            }
            else
            {
                var shuffledQuestions = questionsWithAnswersShuffled.Keys.ToList().OrderBy(x => random.Next()).ToList();

                if (!string.IsNullOrEmpty(currentPlayer))
                {
                    playersCurrentQuestionOrder[currentPlayer] = shuffledQuestions;
                }

                return shuffledQuestions;
            }
        }

        private static void ReconstructShuffledQuestions(List<string> shuffledQuestions)
        //pagal gauta tvarka rekonstruojamas zodynas questionsWithAnswersShuffled
        {
            var shuffledQuestionsWithAnswers = new Dictionary<string, List<string>>();

            foreach (var questionKey in shuffledQuestions)
            {
                shuffledQuestionsWithAnswers.Add(questionKey, questionsWithAnswersShuffled[questionKey]);
            }

            questionsWithAnswersShuffled = shuffledQuestionsWithAnswers;
        }

        public static void ShuffleAnswersList(List<string> list)
            //paduodamas list'as - kazkurio klausimo visi galimi atsakymai
            //jie sukeiciami vietomis
        {
            int n = list.Count; //musu atveju n = 4
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1); //gaunamas random skaicius nuo 1 iki n imtinai
                string value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void CreateQuestions()
        //uzpildomas zodynas questionsWithAnswers1stCorrect 1 karta paleidus programa
        {

            if (questionsWithAnswers1stCorrect.Count == 0)
            {
                questionsWithAnswers1stCorrect.Add("What is the capital of France?", new List<string> { "Paris", "London", "Berlin", "Vilnius" });
                questionsWithAnswers1stCorrect.Add("What is the largest planet in our solar system?", new List<string> { "Jupiter", "Mars", "Venus", "Earth" });
                questionsWithAnswers1stCorrect.Add("Who painted the Mona Lisa?", new List<string> { "Leonardo da Vinci", "Michelangelo", "Vincent van Gogh", "Pablo Picasso" });
                questionsWithAnswers1stCorrect.Add("What is the chemical symbol for water?", new List<string> { "H2O", "CO2", "NaCl", "O2" });
                questionsWithAnswers1stCorrect.Add("What is the currency of Japan?", new List<string> { "Yen", "Euro", "Dollar", "Pound" });
                questionsWithAnswers1stCorrect.Add("Who wrote 'Romeo and Juliet'?", new List<string> { "William Shakespeare", "Charles Dickens", "Jane Austen", "F. Scott Fitzgerald" });
                questionsWithAnswers1stCorrect.Add("What is the tallest mountain in the world?", new List<string> { "Mount Everest", "Kilimanjaro", "K2", "Denali" });
                questionsWithAnswers1stCorrect.Add("What is the capital of Australia?", new List<string> { "Canberra", "Sydney", "Melbourne", "Perth" });
                questionsWithAnswers1stCorrect.Add("Who discovered penicillin?", new List<string> { "Alexander Fleming", "Louis Pasteur", "Marie Curie", "Albert Einstein" });
                questionsWithAnswers1stCorrect.Add("What is the chemical symbol for gold?", new List<string> { "Au", "Ag", "Fe", "Pb" });
                questionsWithAnswers1stCorrect.Add("What is the largest ocean on Earth?", new List<string> { "Pacific Ocean", "Atlantic Ocean", "Indian Ocean", "Arctic Ocean" });
                questionsWithAnswers1stCorrect.Add("Who painted the ceiling of the Sistine Chapel?", new List<string> { "Michelangelo", "Raphael", "Leonardo da Vinci", "Donatello" });
                questionsWithAnswers1stCorrect.Add("What is the speed of light in a vacuum?", new List<string> { "299,792,458 meters per second", "30 kilometers per hour", "3,000,000 meters per second", "3,000 kilometers per second" });
                questionsWithAnswers1stCorrect.Add("Who is known as the 'Father of Modern Physics'?", new List<string> { "Albert Einstein", "Isaac Newton", "Galileo Galilei", "Nikola Tesla" });
                questionsWithAnswers1stCorrect.Add("What is the chemical symbol for oxygen?", new List<string> { "O2", "H2O", "CO2", "N2" });
                questionsWithAnswers1stCorrect.Add("What is the longest river in the world?", new List<string> { "Nile River", "Amazon River", "Mississippi River", "Yangtze River" });
                questionsWithAnswers1stCorrect.Add("Who wrote 'To Kill a Mockingbird'?", new List<string> { "Harper Lee", "Mark Twain", "Ernest Hemingway", "J.D. Salinger" });
                questionsWithAnswers1stCorrect.Add("What is the largest desert in the world?", new List<string> { "Sahara Desert", "Arabian Desert", "Gobi Desert", "Kalahari Desert" });
                questionsWithAnswers1stCorrect.Add("Who was the first person to step on the moon?", new List<string> { "Neil Armstrong", "Buzz Aldrin", "Michael Collins", "Yuri Gagarin" });
                questionsWithAnswers1stCorrect.Add("What is the chemical symbol for carbon?", new List<string> { "C", "Ca", "Co", "Cr" });

            }

        }
    }
}
