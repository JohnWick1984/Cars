using System;
using System.Threading;

// Абстрактный класс автомобиля
public abstract class Car
{
    public string Name { get; set; }
    public int Position { get; set; } = 0;

    // Делегат для события "Финиш"
    public delegate void FinishHandler(object sender, EventArgs e);
    public event FinishHandler Finish;

    // Метод для передвижения автомобиля
    public virtual void Move()
    {
        // Логика движения автомобиля
        Position += new Random().Next(1, 10); // Перемещение на случайное расстояние

        Console.WriteLine($"{Name} находится на позиции {Position}.");

        // Проверка на достижение финиша
        if (Position >= 100)
        {
            OnFinish();
        }

        Thread.Sleep(80); // Задержка для наглядности
    }

    // Метод, который вызывается при достижении финиша
    protected virtual void OnFinish()
    {
        if (Finish != null)
            Finish(this, EventArgs.Empty);
    }
}

// Класс для спортивного автомобиля
public class SportsCar : Car
{
    public SportsCar(string name)
    {
        Name = name;
    }
}

// Класс для легкового автомобиля
public class SedanCar : Car
{
    public SedanCar(string name)
    {
        Name = name;
    }
}

// Класс для грузовика
public class Truck : Car
{
    public Truck(string name)
    {
        Name = name;
    }
}

// Класс для автобуса
public class Bus : Car
{
    public Bus(string name)
    {
        Name = name;
    }
}

// Класс игры
public class RaceGame
{
    public event Car.FinishHandler RaceFinished;
    private bool raceFinished = false;

    // Метод для запуска гонок
    public void StartRace(Car[] cars)
    {
        Console.WriteLine("Гонка началась!");

        while (!raceFinished)
        {
            foreach (Car car in cars)
            {
                car.Move();

                // Проверяем, достигла ли какая-либо машина позиции 100
                if (car.Position >= 100)
                {
                    raceFinished = true;
                    OnRaceFinished(car);
                    break; // Прерываем цикл, так как гонка завершена
                }
            }
        }
    }

    // Метод для обработки события завершения гонки
    protected virtual void OnRaceFinished(Car winner)
    {
        if (RaceFinished != null)
            RaceFinished(winner, EventArgs.Empty);

        Console.WriteLine($"Гонку выиграл {winner.Name}!");
    }
}

class Program
{
    static void Main()
    {
        // Создаем автомобили разных типов
        Car sportsCar = new SportsCar("Спортивный автомобиль");
        Car sedanCar = new SedanCar("Легковой автомобиль");
        Car truck = new Truck("Грузовик");
        Car bus = new Bus("Автобус");

        // Создаем массив автомобилей
        Car[] cars = { sportsCar, sedanCar, truck, bus };

        // Создаем объект игры
        RaceGame raceGame = new RaceGame();

        // Подписываемся на событие завершения гонки
        raceGame.RaceFinished += (sender, e) =>
        {
            Console.WriteLine("Гонка завершена!");
        };

        // Запускаем гонку
        raceGame.StartRace(cars);

        Console.ReadLine();
    }
}
