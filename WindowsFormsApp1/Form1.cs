using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    struct Note
    {
        public string name;
        public string surname;
        public long number;
        public int[] date;
        //Конструктор
        public Note(string name, string surname, long number, int[] date)
        {
            this.name = name;
            this.surname = surname;
            this.number = number;
            this.date = new int[3];
            this.date[0] = date[0];
            this.date[1] = date[1];
            this.date[2] = date[2];
        }
        //Задаем определенные значения нашей структуре
        //Выводим инфу о нашем челике
        public string Info()
        {
            return $"{this.name} {this.surname} {this.number} {this.date[0]}.{this.date[1]}.{this.date[2]}";
        }
    }

    struct Date
    {
        private int day;
        private int month;
        private int year;

        //Конструктор класса
        public Date(int day, int month, int year)
        {
            this.day = day;
            this.month = month;
            this.year = year;
        }
        //Получить дату, которая была n дней назад
        public string BeforeDate(int n)
        {
            return DateOfNumber(CurrentNumber() - n);
        }

        //Получить дату в строке по дню от рождества христова
        private string DateOfNumber(int current_number)
        {
            return $"{GetDay(current_number)}.{GetMonth(current_number)}.{GetYear(current_number)}";
        }

        //Получить текущий день от рождества христова
        private int CurrentNumber()
        {
            //Функция по дате возвращает колличество дней, например ввели 12.12.2022 - функция вернула все дни с 1.1.0 по 12.12.2022
            return YearsInDays(this.year - 1) + MonthInDays(this.month - 1, this.year) + this.day;
        }

        //(год,день,месяц) по дню от рождества христова
        private int GetDay(int current_number)
        {
            //Нам известно колличество дней в прошлых годах  (Years_In_Days(Get_year(current_number) - 1)
            //Известно колличество дней в этом году, не считая текущего месяца   Month_In_Days(Get_month(current_number) - 1, Get_year(current_number)))
            //Тут минус 1 везде потому что нам не надо считать количество дней в текущем месяце и текущем году
            //Ну и из нашего большого числа вычитаем все это и получаем номер дня, в нашей дате
            return current_number - (YearsInDays(GetYear(current_number) - 1) + MonthInDays(GetMonth(current_number) - 1, GetYear(current_number)));
        }
        private int GetMonth(int current_number)
        {
            //Ну, тут находим номер месяца в нашем году, ищем также по дням
            //Из общего колличество дней вычитаем Все дни, которые находиться в предыдущих годах
            //Например, у нас дата 31.5.2023, в функции Current_Number() мы преобразовали дату в дни
            //В Get_year Мы по этим дням определили текущий год

            //Здесь мы находим колличество дней в нашем году
            //из current_number(это все дни) Вычитаем колличестве дней за прошлые года, важно, не вычитать с этим годом, т.е если 2023 вычитаем все до 2022, т.к функция Year_In_Days() Считает все дни в годах включая год
            //В итоге получаем колличество дней в шаем году
            //По нему находим наш месяц
            int days_in_year = current_number - YearsInDays(GetYear(current_number) - 1);
            int current_month = 0;
            //Запускаем цикл, пока дней больше нуля, работа, по определенным условия будем вычитать из него дни
            //В current_month Будем записывать номер месяца, на каждом шагу цикла он будет увеличиваться
            while (days_in_year > 0)
            {
                //Прибавляем наш месяц
                current_month++;
                //Тут будут идти два главных условия if, проверка на то, четный у нас месяц или нечетных
                if (current_month % 2 != 0)
                {
                    //В первых семи нечетных месяцах года 31 день, поэтому вычитает 31
                    if (current_month <= 7)
                        days_in_year -= 31;
                    //Иначе,если у нас нечетный месяц, но во второй половине года, вычитаем 30
                    else
                        days_in_year -= 30;
                }
                //Тут ситуация аналогичная, но все наоборот, рассматриваем четные месяцы
                else if (current_month % 2 == 0)
                {
                    if (current_month <= 7)
                        days_in_year -= 30;
                    else
                        days_in_year -= 31;
                    //Вот здесь единственное отличие,если наш месяц февраль(в нем 28 или 29 дней) 
                    if (current_month == 2)
                    {
                        //Прибавляем 2 дня, так как мы вычили за него 30, (считаем, что в нем 28)
                        days_in_year += 2;
                        //А тут условие на високосный год, если выполняется, то возвращаем 1 день, считаем, что в феврале 29
                        if (GetYear(current_number) % 4 == 0)
                            days_in_year -= 1;
                    }
                }
            }

            return current_month;
        }
        //Тут мы получаем год по нашему числу, указывающему сколько дней прошло от 1.1.0
        private int GetYear(int current_number)
        {
            //1461=(365+365+365+366),как известно, каждый 4-ый год високосный, и в нем 366 дней, так что это надо учитывать
            //(current_number / 1461) * 4 - тут будет выводиться ближайший к нашем году високосный , например изначальная дата 2023, этот оператор выведет 2020
            //Для получения точного года прибавляем остаток: ((current_number % 1461) / 365)
            int current_year = (current_number / 1461) * 4 + ((current_number % 1461) / 365);
            //МЫ получили год, допустим 2022 и осталось еще что-то в остатке, например 3, то тогда, логично, что наш год 2023, январь, 3 число, если же у нас 0, то сейчас 31.12.2022
            //Это условие именно это проверяет
            if ((current_year / 4 * 366) + (current_year - (current_year / 3)) * 365 > current_year)
                current_year++;
            return current_year;
        }

        //Колличество дней в этом и предыдущих годах(передаем год)
        private int YearsInDays(int year)
        {
            //year/4-колличество високосных годов до текущего,       (year - (year / 4))-колличество невисокосных         Умножаем их на колличество дней 
            //Функцияя будет передавать колличество дней, например, ввели 2023, функция вывела все дни считая с 1.1.0 по 31.12.2023
            return ((year / 4 * 366) + (year - (year / 4)) * 365);
        }
        //Колличество дней в этом и предыдущих месяцах в указанном году
        //Сюда передаем месяц и год, т.к года бывают високосными
        private int MonthInDays(int month, int year)
        {
            int month_in_days = 0;
            //Узнаем какой по счету идет день в этом году, считаем, что если месяц нечетный, то в нем 31, если четный, то в нем 30 дней, дальше корректируем эти условия
            month_in_days += ((month / 2) * 30) + ((month - month / 2) * 31);
            //Если больше 1 месяца, То вычитаем 2 дня, так как в феврале 28 или 29
            if (month >= 2)
                month_in_days -= 2;
            //если месяц 10-ый или 8-ой то прибавляем 1 день, это делает потому, что в 7 и 8 (июле и августе) по 31-му дню подряд
            if (month == 10 || month == 8)
                month_in_days++;
            //Ну и если год високосный и уже февраль прошел, то прибавляем еще 1 день (29 учитываем)
            if (year % 4 == 0 && month >= 2)
                month_in_days++;
            //Передаем колличество дней в году по месяцу, например, если ввели апрель, 2023 (то функция находит все дни в 2023 году с 1 января по 30 апреля)
            return month_in_days;
        }

    }

    public partial class Form1 : Form
    {

        Note[] notes;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] input = textBox1.Text.Split('.');
            Date date = new Date(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));
           
            int m = Int32.Parse(textBox2.Text);

            textBox3.Text = date.BeforeDate(m);
        }


        Note[] GetNotesFromLines(string inp, int n)
        {
            Note[] notes = new Note[n];

            string[] lines = inp.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < n; i++)
            {
                string[] tmp = lines[i].Split(' ');
                string[] date = tmp[3].Split('.');
                int[] birthDate = {
                    Int32.Parse(date[0]),
                    Int32.Parse(date[1]),
                    Int32.Parse(date[2])
                };
                notes[i] = new Note(tmp[0], tmp[1], Int64.Parse(tmp[2]), birthDate);
            }
            return notes;
        }

        //Сортируем наши данные по двум первым цифрам телефона
        static Note[] SortStruct(Note[] notes)
        {
            Note note = new Note();
            //Тут у нас сортировка пузырьком
            for (int i = 0; i < notes.Length - 1; i++)
            {
                for (int j = 0; j < notes.Length - i - 1; j++)
                {
                    //Вот тут мы берем первые две цифры нашего номера телефона
                    //Такой способ:Преобразуем номер телефона в строка, методом substring(0,2) извлекаем из этой страке два первых символа
                    //А потом с помощью Convert.toint32 возвращаем обратно к числу
                    if (Convert.ToInt32(notes[j].number.ToString().Substring(0, 2)) > Convert.ToInt32(notes[j + 1].number.ToString().Substring(0, 2)))
                    {
                        note = notes[j];
                        notes[j] = notes[j + 1];
                        notes[j + 1] = note;
                    }
                }
            }
            return notes;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string input = richTextBox1.Text;
            string surname = textBox4.Text;
            string output = ""; // результирующая строка
            notes = GetNotesFromLines(input, 8); // получить массив Note[] из входной строки
            foreach (var note in notes) // перебор каждой записи
            {
                if (note.surname == surname) // если совпадает фамилия с нужной
                {
                    output += note.Info() + "\n"; // добавить запись в результат
                }
            }

            richTextBox1.Text = output; // вывести результат

        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = File.ReadAllText("notes.txt");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string input = richTextBox1.Text;
            File.WriteAllText("notes.txt", ""); // очистить файл
            notes = GetNotesFromLines(input, 8); // получить массив Note[] из входных строк
            notes = SortStruct(notes); // отсортировать массив структур

            foreach (var note in notes)
            {
                File.AppendAllText("notes.txt", note.Info() + "\n");
            }
        }
    }
}
