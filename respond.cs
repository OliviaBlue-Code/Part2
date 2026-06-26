using System.Collections;

namespace demo
{
    public class respond
    {
        public respond( ArrayList reply  , ArrayList ignore  )
        {//start of constructor

            answers(reply);
            words(ignore);


        }//end of constructor

        //method to store the list of words
        private void words(ArrayList ignoring)
        {//
         //ignoring questions
            ignoring.Add("a");
            ignoring.Add("about");
            ignoring.Add("above");
            ignoring.Add("across");
            ignoring.Add("after");
            ignoring.Add("afterwards");
            ignoring.Add("again");
            ignoring.Add("against");
            ignoring.Add("all");
            ignoring.Add("almost");
            ignoring.Add("alone");
            ignoring.Add("along");
            ignoring.Add("already");
            ignoring.Add("also");
            ignoring.Add("although");
            ignoring.Add("always");
            ignoring.Add("am");
            ignoring.Add("among");
            ignoring.Add("amongst");
            ignoring.Add("amount");
            ignoring.Add("an");
            ignoring.Add("and");
            ignoring.Add("another");
            ignoring.Add("any");
            ignoring.Add("anyhow");
            ignoring.Add("anyone");
            ignoring.Add("anything");
            ignoring.Add("anyway");
            ignoring.Add("anywhere");
            ignoring.Add("are");
            ignoring.Add("around");
            ignoring.Add("as");
            ignoring.Add("at");
            ignoring.Add("back");
            ignoring.Add("be");
            ignoring.Add("became");
            ignoring.Add("because");
            ignoring.Add("become");
            ignoring.Add("becomes");
            ignoring.Add("becoming");
            ignoring.Add("been");
            ignoring.Add("before");
            ignoring.Add("beforehand");
            ignoring.Add("behind");
            ignoring.Add("being");
            ignoring.Add("below");
            ignoring.Add("beside");
            ignoring.Add("besides");
            ignoring.Add("between");
            ignoring.Add("beyond");
            ignoring.Add("both");
            ignoring.Add("but");
            ignoring.Add("by");
            ignoring.Add("can");
            ignoring.Add("cannot");
            ignoring.Add("could");
            ignoring.Add("did");
            ignoring.Add("do");
            ignoring.Add("does");
            ignoring.Add("doing");
            ignoring.Add("done");
            ignoring.Add("down");
            ignoring.Add("during");
            ignoring.Add("each");
            ignoring.Add("either");
            ignoring.Add("else");
            ignoring.Add("elsewhere");
            ignoring.Add("enough");
            ignoring.Add("etc");
            ignoring.Add("even");
            ignoring.Add("ever");
            ignoring.Add("every");
            ignoring.Add("everyone");
            ignoring.Add("everything");
            ignoring.Add("everywhere");
            ignoring.Add("except");
            ignoring.Add("few");
            ignoring.Add("first");
            ignoring.Add("for");
            ignoring.Add("former");
            ignoring.Add("formerly");
            ignoring.Add("from");
            ignoring.Add("further");
            ignoring.Add("had");
            ignoring.Add("has");
            ignoring.Add("have");
            ignoring.Add("having");
            ignoring.Add("he");
            ignoring.Add("hence");
            ignoring.Add("her");
            ignoring.Add("here");
            ignoring.Add("hereafter");
            ignoring.Add("hereby");
            ignoring.Add("herein");
            ignoring.Add("hereupon");
            ignoring.Add("hers");
            ignoring.Add("herself");
            ignoring.Add("him");
            ignoring.Add("himself");
            ignoring.Add("his");
            ignoring.Add("how");
            ignoring.Add("however");
            ignoring.Add("i");
            ignoring.Add("if");
            ignoring.Add("in");
            ignoring.Add("indeed");
            ignoring.Add("inside");
            ignoring.Add("instead");
            ignoring.Add("into");
            ignoring.Add("is");
            ignoring.Add("it");
            ignoring.Add("its");
            ignoring.Add("itself");
            ignoring.Add("last");
            ignoring.Add("later");
            ignoring.Add("latter");
            ignoring.Add("latterly");
            ignoring.Add("least");
            ignoring.Add("less");
            ignoring.Add("lot");
            ignoring.Add("many");
            ignoring.Add("may");
            ignoring.Add("me");
            ignoring.Add("meanwhile");
            ignoring.Add("might");
            ignoring.Add("more");
            ignoring.Add("moreover");
            ignoring.Add("most");
            ignoring.Add("mostly");
            ignoring.Add("much");
            ignoring.Add("must");
            ignoring.Add("my");
            ignoring.Add("myself");
            ignoring.Add("name");
            ignoring.Add("namely");
            ignoring.Add("neither");
            ignoring.Add("never");
            ignoring.Add("nevertheless");
            ignoring.Add("next");
            ignoring.Add("no");
            ignoring.Add("nobody");
            ignoring.Add("none");
            ignoring.Add("noone");
            ignoring.Add("nor");
            ignoring.Add("not");
            ignoring.Add("nothing");
            ignoring.Add("now");
            ignoring.Add("nowhere");
            ignoring.Add("of");
            ignoring.Add("off");
            ignoring.Add("often");
            ignoring.Add("on");
            ignoring.Add("once");
            ignoring.Add("one");
            ignoring.Add("only");
            ignoring.Add("or");
            ignoring.Add("other");
            ignoring.Add("others");
            ignoring.Add("otherwise");
            ignoring.Add("ought");
            ignoring.Add("our");
            ignoring.Add("ours");
            ignoring.Add("ourselves");
            ignoring.Add("out");
            ignoring.Add("outside");
            ignoring.Add("over");
            ignoring.Add("own");
            ignoring.Add("part");
            ignoring.Add("per");
            ignoring.Add("perhaps");
            ignoring.Add("please");
            ignoring.Add("put");
            ignoring.Add("rather");
            ignoring.Add("re");
            ignoring.Add("same");
            ignoring.Add("see");
            ignoring.Add("seem");
            ignoring.Add("seemed");
            ignoring.Add("seeming");
            ignoring.Add("seems");
            ignoring.Add("several");
            ignoring.Add("she");
            ignoring.Add("should");
            ignoring.Add("show");
            ignoring.Add("side");
            ignoring.Add("since");
            ignoring.Add("so");
            ignoring.Add("some");
            ignoring.Add("somehow");
            ignoring.Add("someone");
            ignoring.Add("something");
            ignoring.Add("sometime");
            ignoring.Add("sometimes");
            ignoring.Add("somewhere");
            ignoring.Add("still");
            ignoring.Add("such");
            ignoring.Add("take");
            ignoring.Add("than");
            ignoring.Add("that");
            ignoring.Add("the");
            ignoring.Add("their");
            ignoring.Add("theirs");
            ignoring.Add("them");
            ignoring.Add("themselves");
            ignoring.Add("then");
            ignoring.Add("thence");
            ignoring.Add("there");
            ignoring.Add("thereafter");
            ignoring.Add("thereby");
            ignoring.Add("therefore");
            ignoring.Add("therein");
            ignoring.Add("thereupon");
            ignoring.Add("these");
            ignoring.Add("they");
            ignoring.Add("this");
            ignoring.Add("those");
            ignoring.Add("though");
            ignoring.Add("through");
            ignoring.Add("throughout");
            ignoring.Add("thru");
            ignoring.Add("thus");
            ignoring.Add("to");
            ignoring.Add("together");
            ignoring.Add("too");
            ignoring.Add("toward");
            ignoring.Add("towards");
            ignoring.Add("under");
            ignoring.Add("unless");
            ignoring.Add("until");
            ignoring.Add("up");
            ignoring.Add("upon");
            ignoring.Add("us");
            ignoring.Add("used");
            ignoring.Add("very");
            ignoring.Add("via");
            ignoring.Add("was");
            ignoring.Add("we");
            ignoring.Add("well");
            ignoring.Add("were");
            ignoring.Add("what");
            ignoring.Add("whatever");
            ignoring.Add("when");
            ignoring.Add("whence");
            ignoring.Add("whenever");
            ignoring.Add("where");
            ignoring.Add("whereafter");
            ignoring.Add("whereas");
            ignoring.Add("whereby");
            ignoring.Add("wherein");
            ignoring.Add("whereupon");
            ignoring.Add("wherever");
            ignoring.Add("whether");
            ignoring.Add("which");
            ignoring.Add("while");
            ignoring.Add("whither");
            ignoring.Add("who");
            ignoring.Add("whoever");
            ignoring.Add("whole");
            ignoring.Add("whom");
            ignoring.Add("whose");
            ignoring.Add("why");
            ignoring.Add("will");
            ignoring.Add("with");
            ignoring.Add("within");
            ignoring.Add("without");
            ignoring.Add("would");
            ignoring.Add("yes");
            ignoring.Add("yet");
            ignoring.Add("hey");
            ignoring.Add("you");
            ignoring.Add("your");
            ignoring.Add("yours");
            ignoring.Add("yourself");
            ignoring.Add("yourselves");

        }//




        public void answers(ArrayList add_answers  )
        {//start of method

           


            add_answers.Add("How-are-you Hi'm doing well, thanks for asking! how are you doing today ?");
            add_answers.Add("How-are-you i'm great today, thanks for asking! how can i help you today ?");
            add_answers.Add("How-are-you I'm doing good! hope you are also doing well today ?");

            add_answers.Add("Hello Hi, how are you doing today ?😊");
            add_answers.Add("Hello Hi, how can i help you today ? 😊");
            add_answers.Add("Hello Hi hope you are doing well today 😊");


            add_answers.Add("purpose my purpose is to educate you on how to stay safe online and guide your cybersecurity questions.");
            add_answers.Add("purpose i help users understand online safety and digital protection.");
            add_answers.Add("purpose i assist with cybersecurity awareness and safety guidance.");

            //cyber security topics

            add_answers.Add("Cybersecurity cybersecurity is about protecting systems and networks from digital threats.");
            add_answers.Add("Cybersecurity it involves protecting devices and online accounts from attacks.");
            add_answers.Add("Cybersecurity it focuses on securing digital information and systems.");


            add_answers.Add("Phishing phishing is a scam where attackers pretend to be trusted sources to steal information.");
            add_answers.Add("Phishing it uses fake messages or websites to trick users into revealing sensitive data.");
            add_answers.Add("Phishing phishing is a cyber attack that uses disguised emails to trick victims into revealing sensitive information.");
           

            add_answers.Add("Firewall A Firewall is a network security device that monitors and filters incoming and outgoing network traffic based on an organization's previously established security policies.");
            add_answers.Add("Firewall it helps block unwanted access to your device or network.");
            add_answers.Add("Firewall A firewall acts as a protective barrier between trusted and untrusted networks.");


            add_answers.Add("Password a password is used to secure access to your accounts or devices.");
            add_answers.Add("Password it should be strong, long and not easy to guess.");
            add_answers.Add("Password avoid using personal details when creating one.");


            add_answers.Add("hacked account immediately secure your account and log out of all devices.");
            add_answers.Add("hacked account contact support if your account has been compromised.");
            add_answers.Add("hacked account enable extra security like two-factor authentication.");

            add_answers.Add("2FA A 2FA or a Two-Factor Authentication is a security method requiring two forms of verification (password + code).");
            add_answers.Add("2FA Logging into Gmail with a password and SMS code is an example of 2FA");
            add_answers.Add("2FA Enable 2FA on all accounts to reduce risk of unauthorized access.It enables extra security.");

            add_answers.Add("IDS An IDS or Intrusion Detection System is a cybersecurity tool that continuously monitors network or system activity to detect suspicious behavior, policy violations, or potential intrusions.");
            add_answers.Add("IDS  IDS is essentially a digital alarm system for networks. For example, An IDS alerting administrators when multiple failed login attempts occur on a server. ");
            add_answers.Add("IDS Regularly update detection rules and signatures to catch new threats.Train staff to respond quickly to IDS alerts to prevent escalation into full-blown incidents.");


            add_answers.Add("Ransomware Ransomware is a type of malware that encrypts a victim's files. The attacker then demands a payment (ransom) to restore access.");
            add_answers.Add("Ransomware  AN example is a WannaCry attack that encrypted hospital data — ransomware is digital extortion.");
            add_answers.Add("Ransomware To avoid this back up data regularly, don’t pay attackers, and patch vulnerabilities.");


            add_answers.Add("Malware  malware is malicious software designed to damage, disrupt, or steal data from computers.");
            add_answers.Add("Malware A virus that deletes files is an example of malware. It is harmful software.");
            add_answers.Add("Malware To avoid it use antivirus software, keep systems updated, and avoid downloading from untrusted sites.");


            add_answers.Add("VPN (Virtual Private Network) or a vpn extends a private network across a public network, allowing users to send and receive data across shared or public networks as if their computing devices were directly connected to the private network.");
            add_answers.Add("VPN you can use a (Virtual Private Network) to safely browse on public Wi-Fi because it is a privacy shield for internet use.it encrypts your internet traffic for safety.");
            add_answers.Add("VPN (Virtual Private Network) it improves security when using public networks.");

            // Thanks & Bye
            add_answers.Add("thanks You're welcome! Glad I could help.");
            add_answers.Add("thanks No problem at all. Ask me anytime.");
            add_answers.Add("thanks Happy to help. Anything else on your mind?");

            add_answers.Add("bye Take care! Come back if you have more questions.");
            add_answers.Add("bye Catch you later. Stay safe online.");
            add_answers.Add("bye Goodbye! Hope I helped clear things up.");


            //sentiment detection

            // Frustrated
            add_answers.Add("frustrated Take a breath. We’ll go slow and figure it out together.");
            add_answers.Add("frustrated I know this feels like a headache. Tell me what part’s bugging you most.");
            add_answers.Add("frustrated I get that it's frustrating. Tech stuff can be annoying. Let's break it down step by step.");

            // Confused
            add_answers.Add("confused No wrong answer here. Just tell me where it’s messy in your head and we’ll untangle it together, one piece at a time.I’m not going anywhere 💙.");
            add_answers.Add("confused Happens to everyone. Point me to the part that doesn’t make sense.");
            add_answers.Add("confused No worries, confusion is normal with this stuff. I'll explain it without the jargon.");

            // Worried
            add_answers.Add("worried Ncooh come here 🤗 I got you Its okay to be worried.Most risks drop a lot with basic habits.");
            add_answers.Add("worried Ncooh do'nt worry. It’s smart to be cautious online. Can you tell me what’s worrying you? Is it stress? Or marks? Or family? Or something else entirely?");
            add_answers.Add("worried It's okay to worry — cyber stuff can feel scary. Most issues are fixable though. What's going on?");

            // Happy
            add_answers.Add("happy Love to hear it! Good mood makes learning easier.");
            add_answers.Add("happy I’m so glad you’re happy now. You deserve that feeling. Pass some of that energy over here.");
            add_answers.Add("happy That's awesome to hear! Glad you're doing well.");

            // Sad
            add_answers.Add("sad I’m here if you want to vent or just chat about something lighter.");
            add_answers.Add("sad Sorry you’re feeling that way. Want help with something to take your mind off it?");
            add_answers.Add("sad I'm sorry you're feeling down. I'm here if you want to talk or need a distraction.");

            // Angry
            add_answers.Add("angry Ncooh 🤗 I got you. Scams and hacks are infuriating. Let’s see what we can do.");
            add_answers.Add("angry Ncooh thats okay. Wanna vent it out? Let’s turn that anger into making your setup safer.");
            add_answers.Add("angry What’s making you angry right now? I’m listening. No judgment here. Scream into the chat if you need to. I can take it 😤");





        }//end of method








    }
}