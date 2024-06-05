using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProduttoreConsumatore
{
    internal class Program
    {
        public static int NUM_MAX_PRODUTTORI { get; private set; } = 5;
        public static int NUM_MAX_CONSUMATORI { get; private set; } = 10;
        public static int MAX_VALUE = 30;

        public static int value = 0;

        public static void ProduceItems(Queue<int> queue)
        {
            while (true)
            {
                for (int i = 0; i < 10; i++)
                {
                    lock (queue)
                    {
                        Console.WriteLine($"Producing item {i}");
                        queue.Enqueue(i);
                        Thread.Sleep(100); // simulate some work
                    }
                }
            }
        }

        public static void ConsumeItems(Queue<int> queue)
        {
            while (true)
            {
                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        var item = queue.Dequeue();
                        Console.WriteLine($"Consuming item {item}");
                        Thread.Sleep(50); // simulate some work
                    }
                    else
                    {
                        Console.WriteLine("Queue is empty, waiting...");
                        Thread.Sleep(100); // wait for more items to be produced
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            // Create a queue to hold the produced items
            var queue = new Queue<int>();

            // Create a list of producer threads
            var producers = new List<Thread>();
            for (int i = 0; i < NUM_MAX_PRODUTTORI; i++)
            {
                var producer = new Thread(() => ProduceItems(queue));
                producers.Add(producer);
                producer.Start();
            }

            // Create a list of consumer threads
            var consumers = new List<Thread>();
            for (int i = 0; i < NUM_MAX_CONSUMATORI; i++)
            {
                var consumer = new Thread(() => ConsumeItems(queue));
                consumers.Add(consumer);
                consumer.Start();
            }

            // Wait for all threads to complete
            foreach (var producer in producers)
            {
                producer.Join();
            }
            foreach (var consumer in consumers)
            {
                consumer.Join();
            }
        }
    }
}
