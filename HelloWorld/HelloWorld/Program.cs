using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld {
    class Program {
        static void Main(string[] args) {
            //string s1;
            //string s2;
            //string s3;
            //string s4;
            //string s5;

            Rectangle rectangle = new Rectangle(3, 4);
            Console.WriteLine("The area of the rectangle is " + rectangle.Area());

            Overload overload = new Overload();
            overload.Foo((Circle)null);
            overload.Foo(rectangle);
            Console.WriteLine(overload.Area());

            Console.ReadLine();
        }
    }
}
