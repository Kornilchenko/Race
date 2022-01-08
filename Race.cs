/*Разработать игру "Автомобильные гонки" с использованием делегатов.
1. В игре использовать несколько типов автомобилей: спортивные, легковые, 
грузовые и автобусы.
2. Реализовать игру «Гонки». Принцип игры: Автомобили двигаются от 
старта к финишу со скоростями, которые изменяются в установленных пределах 
случайным образом. Победителем считается автомобиль, пришедший к финишу 
первым
Рекомендации по выполнению работы
1. Разработать абстрактный класс «автомобиль» (класс Car). Собрать в нем 
все общие поля, свойства (например, скорость) методы (например, ехать).
2. Разработать классы автомобилей с конкретной реализацией конструкторов 
и методов, свойств. В классы автомобилей добавить необходимые события 
(например, финиш).
3. Класс игры должен производить запуск соревнований автомобилей, 
выводить сообщения о текущем положении автомобилей, выводить сообщение об 
автомобиле, победившем в гонках. Создать делегаты, обеспечивающие вызов 
методов из классов автомобилей (например, выйти на старт, начать гонку). 
4. Игра заканчивается, когда какой-то из автомобилей проехал определенное 
расстояние (старт в положении 0, финиш в положении 100). Уведомление об
окончании гонки (прибытии какого-либо автомобиля на финиш) реализовать с 
помощью событий.*/
using System;
using System.Collections.Generic;
using static System.Console;
using System.Threading;

namespace Gonki
{
    class Race
    {
        static void Main(string[] args)
        {
            SetWindowSize(120, 35);//размеры консоли
            Track track = new Track(200); //создаем трассу 
            List<RaceParticipant> participants = new List<RaceParticipant> { new RaceParticipant (Avto.TypeAvto.SPORTS, "Alfa Romeo GT" ),
                                                                             new RaceParticipant (Avto.TypeAvto.SPORTS, "Audi RS 6" ),
                                                                             new RaceParticipant (Avto.TypeAvto.CARS, "ZAZ 968" ),
                                                                             new RaceParticipant (Avto.TypeAvto.BUSES, "YUTONG ZK6938HB9" ),
                                                                             new RaceParticipant (Avto.TypeAvto.TRUCKS, "КАМАЗ-43509" )};
            foreach(RaceParticipant item in participants)
            {
                track.start_race += item.statusAvto; //подписка на событие старт
                item.FinishAvto += track.results_table; //подписка на событие финиш
                item.Go(track.lengthTrack, 0); //выводим участников на старт и показываем информацию о них
            }

            track.Start();
            int speed = 0, n= 0;
            while(n!= participants.Count)
            {
                n = 0;//сбрасываем счетчик финишировавших
                Thread.Sleep(1000); //задержка в 1сек
                foreach (RaceParticipant item in participants) //перебираем всех участников
                {
                    speed = track.ChangeSpeed((int)item.my_distance); //определяем скорость на данном участке пути
                    item.Go(track.lengthTrack, speed); //передаем скорость участнику
                    if (item.finish) n++; //проверяем не все ли финишировали
                }
            }
            SetCursorPosition(0, 25);
            WriteLine("Гонка закончилась! Спасибо за внимание!");
        }
    }
}    