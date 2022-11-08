﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BezierCurveLib.Utils {
    public static class StorageSystem {

        public static Vector2 ConvertToStorage(Vector2 point, float width, float height) {
            return new Vector2(point.X / width, point.Y / height);
        }
    }
}
