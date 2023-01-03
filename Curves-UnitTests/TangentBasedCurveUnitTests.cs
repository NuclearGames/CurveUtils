using Curves;
using NUnit.Framework;
using System.Numerics;

namespace Curves_UnitTests {
    internal class TangentBasedCurveUnitTests {
        [TestCaseSource(nameof(SectionCases))]
        public void TestTangent(Vector2 left, Vector2 right, float leftTangent, float rightTangent, float a, float b, float c, float d) {
            var section = TangentBasedCurve.Segment.Fit(left, right, leftTangent, rightTangent);

            Assert.That(section.A, Is.EqualTo(a));
            Assert.That(section.B, Is.EqualTo(b));
            Assert.That(section.C, Is.EqualTo(c));
            Assert.That(section.D, Is.EqualTo(d));
        }

        private static object[] SectionCases =
        {
            new object[] { new Vector2(0.5f, 0.5f), new Vector2(1f, 1f), 4f, 1f, 
                1f, 1f, 6f, 12f },

            new object[] { new Vector2(0f, 0f), new Vector2(0.5f, 0.5f), 2f, 4f, 
                0.5f, 4f, 14f, 16f },

            new object[] { new Vector2(0f, 0f), new Vector2(1f, 1f), -1f, 1f,
                1f, 1f, -2f, -2f },

            new object[] { new Vector2(0f, 0f), new Vector2(1f, 1f), 0f, 0f,
                1f, 0f, -3f, -2f }
        };
    }
}
