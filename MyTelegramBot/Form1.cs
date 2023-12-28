
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Data.SQLite;
using Telegram.Bot.Types.ReplyMarkups;


namespace MyTelegramBot
{
    public partial class Form1 : Form
    {
        public static string Token = "";
        public Thread botThread;
        public string message;
        public Telegram.Bot.TelegramBotClient bot;
        public ReplyKeyboardMarkup mainKebordMarkup;
        public InlineKeyboardMarkup pollKeybord;
        public SQLiteConnection connection = new SQLiteConnection("URI=file:./telBot.db");

        public Form1()
        {
            InitializeComponent();
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
        private void btnStart_Click_1(object sender, EventArgs e)
        {
            Token = txtToken.Text;
            botThread = new Thread(runBot);
            botThread.Start();
        }
        void runBot()
        {

            int offset = 0;
            bool sendError = false;
            bool setDataGride = false;
            while (true)
            {
                try
                {
                    bot = new Telegram.Bot.TelegramBotClient(Token);
                    Telegram.Bot.Types.Update[] update = bot.GetUpdatesAsync(offset).Result;
                    this.Invoke(new Action(() =>
                    {
                        lblStatus.Text = "آنلاین";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }));                    
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM Reports WHERE Token = '{Token}'";
                    SQLiteDataReader reader = command.ExecuteReader();
                    if (!setDataGride)
                    {
                        dgReport.Rows.Clear();
                        this.Invoke(new Action(() =>
                        {
                            while (reader.Read())
                            {

                                dgReport.Rows.Add(reader.GetInt64(1), reader.GetString(3), reader.GetString(4), reader.GetInt64(2), reader.GetString(5));

                            }
                           
                        }));
                        setDataGride = true;
                    }
                    reader.Close();
                    connection.Close();
                    for (int i = 0; i < update.Length; i++)
                    {


                        offset = update[i].Id + 1;
                        if (update[i].Message.Text == null)
                        {
                            continue;
                        }

                        var text = update[i].Message.Text.ToLower();
                        var from = update[i].Message.From;
                        var chatId = update[i].Message.Chat.Id;
                        if (text.Contains("/start"))
                        {
                            message = $"<b>{from.Username}</b> welcome to my bot\nسلام <b>{from.Username}</b> به بات من خوش آمدید.";
                            bot.SendTextMessageAsync(chatId, message, 0, Telegram.Bot.Types.Enums.ParseMode.Html);
                            message = "Please specify bot language to continue\nلطفا برای ادامه زبان بات را مشخص کنید";
                            bot.SendTextMessageAsync(chatId, message, 0, default, null, false, false, null, 0, null, selectLanguage());
                        }
                        else if (text.Contains("فارسی"))
                        {
                            message = "لطفا یک گزینه را انتخاب کنید";
                            bot.SendTextMessageAsync(chatId, message, 0, default, null, false, false, null, 0, null, persianMainMenu());
                        }
                        else if (text.Contains("english"))
                        {
                            message = "Please select an option";
                            bot.SendTextMessageAsync(chatId, message, 0, default, null, false, false, null, 0, null, englishMainMenu());
                        }
                        else if (text.Contains("برگشت") || text == "back 🔙")
                        {
                            message = "Please specify bot language to continue\nلطفا برای ادامه زبان بات را مشخص کنید";
                            bot.SendTextMessageAsync(chatId, message, 0, default, null, false, false, null, 0, null, selectLanguage());
                        }
                        else if (text.Contains("درباره من"))
                        {
                            message = "سلام من امیرعلی افشارپور یک برنامه نویس در سطح جونیور هستم که حدودا یک سال است که علاقه ی خودم رو  پیدا کردم و وارد حوزه برنامه نویسی شدم.\n" +
                                " در طی این یک سال سعی کردم توانایی های خودم رو در حوزه بک اند وبسایت افزایش دهم و با زبان سی شارپ , فریم ورک asp.net core و پایگاه داده sql server کار کردم.\n" +
                                " از آنجایی که من عاشق یادگیری چیز های جدید هستم ، در حال حاضر درحال گسترش توانایی های خودم در حوزه  فرانت اند هستم تا بتوانم به عنوان یک برنامه نویس فول استک به جامعه خدمت کنم.\n"
                                + "راستی من در حوزه طراحی لوگو هم فعالیت دارم اگر نیاز به لوگوی حرفه ای داشتی میتونی به من بسپاریش 😉";
                            bot.SendTextMessageAsync(chatId, message);

                        }
                        else if (text.Contains("about me"))
                        {
                            message = "Hi, I’m Amirali Afsharpour, a junior-level programmer who has been interested in programming for about a year and has entered the field.\n" +
                                " During this year, I tried to increase my abilities in the field of website backend and worked with C# language, asp.net core framework, and SQL Server database.\n" +
                                " Since I love learning new things, I am currently expanding my skills in the frontend area so that I can serve the community as a full-stack developer.\n" +
                                " By the way, I also work in the field of logo design. If you need a professional logo, you can entrust it to me 😉.";
                            bot.SendTextMessageAsync(chatId, message);

                        }
                        else if (text.Contains("گیت هاب من"))
                        {
                            message = "این گیت هاب منه که داخلش پروژه هامو میزارم از جمله همین ربات.\n" +
                                "فالو و استار یادت نره 😉" +
                                "\nhttps://github.com/AmirAfsharpour";
                            bot.SendTextMessageAsync(chatId, message);
                        }
                        else if (text.Contains("my github"))
                        {
                            message = "This is my GitHub where I put my projects.\n" +
                                " Don't forget to follow and star 😉." +
                                "\nhttps://github.com/AmirAfsharpour";
                            bot.SendTextMessageAsync(chatId, message);

                        }
                        else if (text.Contains("تماس با من"))
                        {
                            message = "لطفا یک گزینه را انتخاب کنید";
                            bot.SendTextMessageAsync(chatId, message, 0, default, null, false, false, null, 0, null, persianContactMe());


                        }
                        else if (text.Contains("contact me"))
                        {
                            message = "Please select an option";
                            bot.SendTextMessageAsync(chatId, message, 0, default, null, false, false, null, 0, null, englishContactMe());
                        }
                        else if (text.Contains("بازگشت به منوی اصلی"))
                        {
                            message = "لطفا یک گزینه را انتخاب کنید";
                            bot.SendTextMessageAsync(chatId, message, 0, default, null, false, false, null, 0, null, persianMainMenu());
                        }
                        else if (text.Contains("back to main menu"))
                        {
                            message = "Please select an option";
                            bot.SendTextMessageAsync(chatId, message, 0, default, null, false, false, null, 0, null, englishMainMenu());

                        }
                        else if (text.Contains("تلگرام"))
                        {
                            message = "از طریق این اکانت تلگرام میتونی با من در ارتباط باشی" +
                                "\nhttps://t.me/AMIR_AFSHARPOOR";
                            bot.SendTextMessageAsync(chatId, message);

                        }
                        else if (text.Contains("telegram"))
                        {
                            message = "You can contact me through this Telegram account" +
                                "\nhttps://t.me/AMIR_AFSHARPOOR";
                            bot.SendTextMessageAsync(chatId, message);

                        }
                        else if (text.Contains("شماره تماس"))
                        {
                            message = "از طریق این شماره تماس میتونی با من در ارتباط باشی" +
                                "\n+989388200115";
                            bot.SendTextMessageAsync(chatId, message);
                        }
                        else if (text.Contains("phone number"))
                        {
                            message = "You can contact me through this phone number" +
                                "\n+989388200115";
                            bot.SendTextMessageAsync(chatId, message);
                        }
                        else if (text.Contains("ایمیل"))
                        {
                            message = "از طریق این آدرس ایمیل میتونی با من در ارتباط باشی" +
                                "\nafsharamir4555@gmail.com";
                            bot.SendTextMessageAsync(chatId, message);
                        }
                        else if (text.Contains("email"))
                        {
                            message = "You can contact me through this Email address" +
                                "\nafsharamir4555@gmail.com";
                            bot.SendTextMessageAsync(chatId, message);
                        }

                        this.Invoke(new Action(() =>
                        {
                            dgReport.Rows.Add(chatId, from.Username, text, update[i].Message.MessageId, DateTime.Now.ToString("yyyy/MM/dd - hh:mm"));
                            connection.Open();
                            command.CommandText = $"INSERT INTO Reports(ChatId, MessageId, Username, Command, DateTime, Token) VALUES ({chatId}, {update[i].Message.MessageId}, '{from.Username}', '{text}', '{DateTime.Now.ToString("yyyy/MM/dd - hh:mm")}', '{Token}')";
                            command.ExecuteNonQuery();
                            connection.Close();
                        }));

                    }

                }
                catch
                {

                    MessageBoxButtons button = MessageBoxButtons.OK;
                    if (!sendError && txtToken.Text == "" && lblStatus.Text != "آنلاین")
                    {
                        MessageBoxIcon icon = MessageBoxIcon.Warning;
                        MessageBox.Show("توکن وارد نشده است ", "هشدار", button, icon);
                    }
                    else if (!sendError && lblStatus.Text != "آنلاین")
                    {
                        MessageBoxIcon icon = MessageBoxIcon.Error;
                        MessageBox.Show("خطایی رخداده است\nلطفا توکن یا اینترنت خود را بررسی کنید", "اخطار", button, icon);
                    }


                    sendError = true;
                }
        }
        }
        public ReplyKeyboardMarkup selectLanguage()
        {
            KeyboardButton[] row =
            {
                new KeyboardButton("English"),
                new KeyboardButton("فارسی")
            };
            ReplyKeyboardMarkup selectLanguage = new ReplyKeyboardMarkup(row);
            selectLanguage.ResizeKeyboard = true;
            selectLanguage.Keyboard = new KeyboardButton[][]
            {
                row
            };
            return selectLanguage;
        }
        public ReplyKeyboardMarkup persianMainMenu()
        {
            KeyboardButton[] row1 =
            {
                new KeyboardButton("درباره من 📝"),
                new KeyboardButton("تماس با من 🤙"),
                new KeyboardButton("گیت هاب من 🔗")
            };
            KeyboardButton[] row2 =
            {
                new KeyboardButton("برگشت 🔙")
            };
            ReplyKeyboardMarkup selectLanguage = new ReplyKeyboardMarkup(row1);
            selectLanguage.ResizeKeyboard = true;
            selectLanguage.Keyboard = new KeyboardButton[][]
            {
                row1,row2
            };
            return selectLanguage;
        }
        public ReplyKeyboardMarkup englishMainMenu()
        {
            KeyboardButton[] row1 =
            {
                new KeyboardButton("About me 📝"),
                new KeyboardButton("Contact me 🤙"),
                new KeyboardButton("My GitHub 🔗")
            };
            KeyboardButton[] row2 =
            {
                new KeyboardButton("Back 🔙")
            };
            ReplyKeyboardMarkup selectLanguage = new ReplyKeyboardMarkup(row1);
            selectLanguage.ResizeKeyboard = true;
            selectLanguage.Keyboard = new KeyboardButton[][]
            {
                row1,row2
            };
            return selectLanguage;
        }
        public ReplyKeyboardMarkup persianContactMe()
        {
            KeyboardButton[] row1 =
            {
                new KeyboardButton("تلگرام 🚀"),
                new KeyboardButton("شماره تماس 📞"),
                new KeyboardButton("ایمیل 📧")
            };
            KeyboardButton[] row2 =
            {
                new KeyboardButton("بازگشت به منوی اصلی 🔙")
            };
            ReplyKeyboardMarkup selectLanguage = new ReplyKeyboardMarkup(row1);
            selectLanguage.ResizeKeyboard = true;
            selectLanguage.Keyboard = new KeyboardButton[][]
            {
                row1,row2
            };
            return selectLanguage;
        }
        public ReplyKeyboardMarkup englishContactMe()
        {
            KeyboardButton[] row1 =
            {
                new KeyboardButton("Telegram 🚀"),
                new KeyboardButton("Phone number 📞"),
                new KeyboardButton("Email 📧")
            };
            KeyboardButton[] row2 =
            {
                new KeyboardButton("Back to main menu 🔙")
            };
            ReplyKeyboardMarkup selectLanguage = new ReplyKeyboardMarkup(row1);
            selectLanguage.ResizeKeyboard = true;
            selectLanguage.Keyboard = new KeyboardButton[][]
            {
                row1,row2
            };
            return selectLanguage;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            botThread.Abort();               
            
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = openFile.FileName;
            }
        }
        
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("آیا از پاک کردن همه ی اطلاعات اطمینان دارید؟", "هشدار", button, icon);
            if (result == DialogResult.Yes)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM Reports WHERE Token = '{Token}'";
                command.ExecuteNonQuery();
                connection.Close();
                dgReport.Rows.Clear();
            }
        }

        private void btnSend_Click_1(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                long chatId = long.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                if (txtFile.Text != "")
                {

                    FileStream stream = System.IO.File.Open(txtFile.Text, FileMode.Open);
                    InputFileStream file = InputFile.FromStream(stream, stream.Name);
                    bot.SendDocumentAsync(chatId, file, default, default, default, default, default);
                    txtFile.Text = "";
                    txtmessage.Text = "";

                }
                else
                {
                    bot.SendTextMessageAsync(chatId, txtmessage.Text, 0, Telegram.Bot.Types.Enums.ParseMode.Html);
                    txtmessage.Text = "";
                }

            }
            else
            {
                MessageBox.Show("لطفا یک کاربر را مشخص کنید");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("آیا از پاک کردن این اطلاعات اطمینان دارید؟", "هشدار", button, icon);
            if (result == DialogResult.Yes)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                try
                {
                    command.CommandText = $"DELETE FROM Reports WHERE MessageId = '{dgReport.CurrentRow.Cells[3].Value.ToString()}'";

                }
                catch
                {
                    icon = MessageBoxIcon.Error;
                    button = MessageBoxButtons.OK;
                    MessageBox.Show("رکوردی برای حذف کردن اطلاعات انتخاب نشده است", "خطا", button, icon);
                }
                command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();
                connection.Close();
                dgReport.Rows.Remove(dgReport.CurrentRow);
            }

        }
    }
}
