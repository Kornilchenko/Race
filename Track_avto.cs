using System;
using static System.Console;
using System.Threading;

namespace Gonki
{
    public class Track //трасса
    {
        public delegate void start_of_the_race(bool start= false); //делeгат (старт гонок)
        public event start_of_the_race start_race; //определение события старт гонок
        static Random rand = new Random();
        public int lengthTrack { get; private set; } //протяженность трассы
        int winner = 1; //порядок финишированных
        public Track(int length) { lengthTrack = length; } //конструктор
        public Track() : this(100) { } //конструктор по умолчанию
        public void Start () //старт гонок
        {
            int n = 5; //отчет времени до старта
            SetCursorPosition(1, 1);
            Write("Старт через:");
            while(n>=0)
            {
                if (n == 0)
                {
                    SetCursorPosition(1, 1);
                    Write("                  "); //затираем старую стоку
                    start_race?.Invoke(true); //создаем событие старт
                }
                else
                {
                    SetCursorPosition(14, 1);
                    Write(n);
                    Thread.Sleep(1000); //задержка в 1сек
                }
                n--;
            }
        }
        public int ChangeSpeed(int distance) //скорость для данного участка дороги
        {
            int temp = (int)(distance * 1.0/ lengthTrack * 100); //процентное соотношение пройденого пути
            if (temp < 25)
                return rand.Next(80, 120);
            else if (temp < 50)
                return rand.Next(130, 220);
            else if (temp < 75)
                return rand.Next(180, 255);
            else if (temp < 100)
                return rand.Next(90, 150);
            else //если достиг финиша
                return 0;
        }
        public void results_table(string brand, int num)
        {
            SetCursorPosition(1, winner );
            Write(winner + " место занял автомобиль " + brand + " под номером " + num);
            winner++;
        }
    }
    public abstract class Avto
    {
        public enum TypeAvto
        {
            SPORTS, //спортивные
            CARS, //легковые
            TRUCKS, //грузовики
            BUSES //автобусы
        };
        public TypeAvto type_avto { get; set; } //тип авто
        public string car_brand { get; set; } //марка авто
        public float speed { get; set; } //скорость авто
        public float my_distance { get; set; } = 0;
        public abstract void Go(int distance , int speed); //абстрактный метод ехать
        protected Avto(TypeAvto type, string brand)
        { 
            type_avto = type;
            car_brand = brand;
        }
    }
    public class RaceParticipant : Avto //участник гонки
    {
        public delegate void finish_avto(string str, int num); //делегат (финиш для объекта) 
        public event finish_avto FinishAvto;// событие на финиш
        public int num { get; set; } = 0; //номер участника гонки
        static int s_num = 1; //переменная для присвоения номера участника
        public bool finish = false; //финишировал ли
       
        public RaceParticipant(TypeAvto type, string brand):base(type, brand) //конструктор
        {
            num = s_num++;
        }
        public override void Go(int distance, int speed)
        {
            if (finish == true)//если финишировал выходим
                return;
            if (my_distance >= distance)
            {
                finish = true; //не даст еще раз создать событие 
                FinishAvto?.Invoke(car_brand, num); //событие 
                SetCursorPosition(9, 3 * num + 6);
                Write("Участник гонок под номером " + num + " на автомобиле " + car_brand + ". Скорость " + speed + "км/ч. Пройденный путь " + distance + "км.    ");
                statusAvto();
                show_the_way(100);
            }
            else
            {
                my_distance += (float)(speed * 0.05);
                SetCursorPosition(9, 3 * num + 6);
                Write("Участник гонок под номером " + num + " на автомобиле " + car_brand + ". Скорость " + speed + "км/ч. Пройденный путь " + (int)(my_distance) + "км.");
                show_the_way((int)(my_distance / distance * 100));
            } 
        }
        private void show_the_way(int distance)
        {
            int i = 0;
            SetCursorPosition(0, 3 * num + 7);
            while (i++<= distance+6) //затираем прошлую позицию машины
            { Write(" "); }
            SetCursorPosition(distance, 3 * num + 7);// новая позиция авто
            BackgroundColor = ConsoleColor.Red;
            Write("  ");
            ResetColor();
        }
        public void statusAvto(bool start = false)
        {
            if (start)
            {
                SetCursorPosition(1, 3 * num + 6);
                Write("START ");
            }
            else
            {
                SetCursorPosition(1, 3 * num + 6);
                Write("FINISH");
            }
        }
    } 
}
