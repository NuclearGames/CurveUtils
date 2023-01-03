using NUnit.Framework;
using System.Numerics;
using TransformStructures;

namespace TransformStructures_UnitTests {
    internal class Matrix3x3UnitTests {
        [Test]
        public void MultiplyMatrixTest() {
            var t = Matrix3x3.Translation(new Vector2(5, 4));
            var s = Matrix3x3.Scale(new Vector2(3, 2));
            var ts = s * t;

            var expected = new Matrix3x3(
                3, 0, 5,
                0, 2, 4,
                0, 0, 1);

            Assert.That(ts, Is.EqualTo(expected));
        }

        [Test]
        public void MultiplyVectorTest() {
            var t = Matrix3x3.TranslationScale(new Vector2(5, 4), new Vector2(3, 2));
            var v1 = new Vector3(1, 2, 1);
            var v2 = new Vector3(1, 2, 0);
            var vt1 = t * v1;
            var vt2 = t * v2;

            var expectedV1 = new Vector3(8, 8, 1);
            var expectedV2 = new Vector3(3, 4, 0);

            Assert.That(vt1, Is.EqualTo(expectedV1));
            Assert.That(vt2, Is.EqualTo(expectedV2));
        }

        [Test]
        public void InverseTest() {
            var t = Matrix3x3.TranslationScale(new Vector2(6, 4), new Vector2(4, 2));
            var inv = t.Inverse();

            var expected = new Matrix3x3(
                0.25f, 0, -1.5f,
                0, 0.5f, -2f,
                0, 0, 1);

            Assert.That(inv, Is.EqualTo(expected));
        }

        [Test]
        public void RotationTest() {
            var t = Matrix3x3.Rotation(90);
            var v = new Vector3(4, 2, 1);

            var vt = t * v;

            var expected = new Vector3(-2, 4, 1);

            Assert.AreEqual(expected.X, vt.X, 0.001f);
            Assert.AreEqual(expected.Y, vt.Y, 0.001f);
        }
    }
}
