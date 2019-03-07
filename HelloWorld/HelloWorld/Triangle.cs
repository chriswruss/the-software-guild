using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld {
    class Triangle {
        public int width;
        public int height;

        public Triangle(int width, int height) {
            this.width = width;
            this.height = height;
        }

        public float Area() {
            return width * height / 2;
        }

        public float Perimeter() {
            return width + height + (float)Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
        }
    }
}
