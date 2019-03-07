using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld {
    class Circle {
        public int radius;

        public Circle(int radius) {
            this.radius = radius;
        }

        public float Area() {
            return (float)(Math.PI * (Math.Pow(radius, 2)));
        }

        public float Perimeter() {
            return 2 * (float)Math.PI * radius;
        }
    }
}
