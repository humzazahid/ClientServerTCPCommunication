using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientServerTCPCommunication
{
    class Employee
    {
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public string EmpDesignation { get; set; }
    }

    class Program
    {
        static Employee[] employees;
        static TcpListener listener;

        static void FillEmployeesData()
        {
            employees = new Employee[5];

            Employee emp1 = new Employee();
            emp1.EmpID = 1;
            emp1.EmpName = "Emp1";
            emp1.EmpDesignation = "Desig1";
            employees[0] = emp1;

            Employee emp2 = new Employee();
            emp2.EmpID = 2;
            emp2.EmpName = "Emp2";
            emp2.EmpDesignation = "Desig2";
            employees[1] = emp2;

            Employee emp3 = new Employee();
            emp3.EmpID = 3;
            emp3.EmpName = "Emp3";
            emp3.EmpDesignation = "Desig3";
            employees[2] = emp3;

            Employee emp4 = new Employee();
            emp4.EmpID = 4;
            emp4.EmpName = "Emp4";
            emp4.EmpDesignation = "Desig4";
            employees[3] = emp4;

            Employee emp5 = new Employee();
            emp5.EmpID = 5;
            emp5.EmpName = "Emp5";
            emp5.EmpDesignation = "Desig5";
            employees[4] = emp5;
        }

        static void Main(string[] args)
        {
            //-------------------------------------------------------------
            string name = Dns.GetHostName();
            try
            {
                IPAddress[] addrs = Dns.GetHostEntry(name).AddressList;
                foreach (IPAddress addr in addrs)
                    Console.WriteLine("{0}/{1}", name, addr);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //-------------------------------------------------------------


            FillEmployeesData();

            listener = new TcpListener(2055);
            listener.Start();
            Console.WriteLine("Server mounted, listening to port 2055");
            Thread t = new Thread(new ThreadStart(Service));
            t.Start();
        }

        static void Service()
        {
            while (true)
            {
                Socket soc = listener.AcceptSocket();
                try
                {
                    Stream s = new NetworkStream(soc);
                    StreamReader sr = new StreamReader(s);
                    StreamWriter sw = new StreamWriter(s);
                    sw.AutoFlush = true;
                    sw.WriteLine("{0} Employees available", employees.Length);
                    while (true)
                    {
                        string name = sr.ReadLine();
                        if (name == "" || name == null) break;
                        foreach (Employee emp in employees)
                        {
                            if (emp.EmpName == name)
                            {
                                sw.WriteLine(emp.EmpDesignation);
                                Console.WriteLine(emp.EmpDesignation);
                            }

                        }
                    }
                    s.Close();
                }
                catch (Exception e)
                {
                }

                soc.Close();
            }
        }
    }
}
