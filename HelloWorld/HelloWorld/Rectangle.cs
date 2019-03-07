using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld {
    class Rectangle {
        public int height;
        public int width;

        public Rectangle(int height, int width) {
            this.height = height;
            this.width = width;
        }
        
        public float Area() {
            return height * width;
        }

        public float Perimeter() {
            return 2 * (height + width);
        }
    }
}
