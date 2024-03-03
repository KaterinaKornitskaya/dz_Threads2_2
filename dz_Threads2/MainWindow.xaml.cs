using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dz_Threads2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SynchronizationContext uiContext;
        Thread th1;
        Thread th2;
        public MainWindow()
        {
            InitializeComponent();
            // контекст синхронизации необходим, когда мы в новом потоке
            // th1 попытаемся обращаться к жлементам формы
            uiContext = SynchronizationContext.Current;
        }

        private void btnTask1_Click(object sender, RoutedEventArgs e)
        {
            int start = 0;
            if (txtBoxTask1_1.Text == string.Empty)
                start = 0;
            else
                start = int.Parse(txtBoxTask1_1.Text);

            int end = 0;
            if (txtBoxTask1_2.Text == string.Empty)
            {
                end = 0;
            }
            else
                end = int.Parse(txtBoxTask1_2.Text);

            Point p = new Point(start, end);

            th1 = new Thread(new ParameterizedThreadStart(GenerateNum));
            //th1.IsBackground = true;
            th1.Start(p);
            //th.Join();
        }

        // структура для передачи параметров в метод GenerateNum
        struct Point
        {
            public int x;
            public int y;

            public Point(int X, int Y)
            {
                x = X;
                y = Y;
            }
        }
        
        // метод Генерации простых чисел
        public void GenerateNum(object obj)
        {
            Point p = (Point)obj;
            int start = p.x;
            int end = p.y;
            uiContext.Send(e => listBoxTask1.Items.Clear(), null);

            if(start == 0)
            {
                for (int i = 2; i <= end; i++)
                {
                    if (IsPrime(i)==true)
                    {
                        uiContext.Send(e => listBoxTask1.Items.Add(i), null);
                    }
                }
            }
            else if(end == 0)
            {
                int i = 2;
                while(true)
                {
                    if (IsPrime(i)==true)
                    {
                        uiContext.Send(e => listBoxTask1.Items.Add(i), null);
                    }
                    i++;
                }
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    if (IsPrime(i)==true)
                    {
                        uiContext.Send(e => listBoxTask1.Items.Add(i), null);
                    }
                }
            }
        }

        // метод для определения, простое ли число
        // если число простое - вернет true
        public bool IsPrime(int n)
        {
            for(int i = 2; i < n;  i++)
            {
                if (n%i == 0)
                    return false;
            }
            return true;
        }

        private void btnTask2_Click(object sender, RoutedEventArgs e)
        {
            th2 = new Thread(() =>
            {
                long num0 = 0;
                long num1 = 1;
                long num2;
                uiContext.Send(e => listBoxTask2.Items.Add(num0), null);
                uiContext.Send(e => listBoxTask2.Items.Add(num1), null);

                // Генерация чисел Фибоначчи
                int i = 3;
                while (true)
                {
                    num2 = num0 + num1;

                    //Каждый следующий элемент выводим в цикле:
                    uiContext.Send(e => listBoxTask2.Items.Add(num2), null);
                    Thread.Sleep(200);

                    //Предыдущим двум переменным присваиваем новые значения:
                    num0 = num1;
                    num1 = num2;

                    i++;
                    // выход из цикла при генерации 94е чисел Фибоначчи,
                    // так как 95ое число не помещается в тип long
                    if (i==94)
                        break;
                }
            });
            th2.Start();
        }

        private void btnAbortTh1_Click(object sender, RoutedEventArgs e)
        {
            // метод Abort() не работает для WPF, нужен  CancellationToken,
            // но я с ним пока не разобралась, поэтому здесь будет ошибка
            //if (th1 != null)
            //{
            //    th1.Abort();
            //}
        }

        private void btnAbortTh2_Click(object sender, RoutedEventArgs e)
        {
            // метод Abort() не работает для WPF, нужен  CancellationToken,
            // но я с ним пока не разобралась, поэтому здесь будет ошибка
            //if (th2 != null)
            //{
            //    th2.Abort();
            //}
        }
    }

    // Задание 1:
    // Создайте оконное приложение, генерирующее набор простых чисел
    // в диапазоне, указанном пользователем.
    // Если не указана нижняя граница, поток с стартует с 2. 
    // Если не указана верхняя граница, генерирование происходит до
    // завершения приложения.
    // Используйте механизм потоков.
    // Числа должны отображаться в оконном интерфейсе.

    // Задание 2
    // Добавьте к первому заданию поток, генерирующий
    // набор чисел Фибоначчи.Числа должны отображаться
    // в оконном интерфейсе.

    // Задание 3
    // Добавьте ко второму заданию кнопки для полной остановки каждого
    // из потоков. Одна кнопка на один поток.
    // Если пользователь нажал на кнопку остановки,
    // поток полностью прекращает свою работу.
}