using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld {
    class Overload {
        private Rectangle _rectangle;
        private Triangle _triangle;
        private Circle _circle;

        private void Reset() {
            _rectangle = null;
            _triangle = null;
            _circle = null;
        }

        public void Foo(Rectangle rectangle) {
            Reset();
            _rectangle = rectangle;
        }

        public void Foo(Triangle triangle) {
            Reset();
            _triangle = triangle;
        }

        public void Foo(Circle circle) {
            Reset();
            _circle = circle;
        }

        public float Area() {
            if (_circle != null) {
                return _circle.Area();
            }
            else if (_rectangle != null) {
                return _rectangle.Area();
            }
            else if (_triangle != null) {
                return _triangle.Area();
            }
            return 0;
        }

        public float Perimeter() {
            if (_circle != null) {
                return _circle.Perimeter();
            }
            else if (_rectangle != null) {
                return _rectangle.Perimeter();
            }
            else if (_triangle != null) {
                return _triangle.Perimeter();
            }
            return 0;
        }
    }
}
