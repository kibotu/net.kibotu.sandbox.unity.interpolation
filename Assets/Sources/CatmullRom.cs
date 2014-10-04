/*******************************************************************************
* Copyright 2011 See AUTHORS file.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
******************************************************************************/
/** @author Xoppa */
/** @converted to unity by https://github.com/kibotu */
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Sources
{
    public class CatmullRomSpline
    {

        /** Calculates the catmullrom value for the given position (t).
        * @param out The Vector to set to the result.
        * @param t The position (0<=t<=1) on the spline
        * @param points The control points
        * @param continuous If true the b-spline restarts at 0 when reaching 1
        * @param tmp A temporary vector used for the calculation
        * @return The value of out */
        public bool continuous;
        public Vector3[] controlPoints;
        public int spanCount;
        private Vector3 tmp;
        private Vector3 tmp2;
        private Vector3 tmp3;

        public CatmullRomSpline()
        {
        }

        public CatmullRomSpline(Vector3[] controlPoints, bool continuous)
        {
            set(controlPoints, continuous);
        }

        public static Vector3 calculate(Vector3 _out, float t, Vector3[] points, bool continuous, Vector3 tmp)
        {
            int n = continuous ? points.Count() : points.Count() - 3;
            float u = t*n;
            int i = (t >= 1f) ? (n - 1) : (int) u;
            u -= i;
            return calculate(_out, i, u, points, continuous, tmp);
        }

        /** Calculates the catmullrom value for the given span (i) at the given position (u).
        * @param out The Vector to set to the result.
        * @param i The span (0<=i<spanCount) spanCount = continuous ? points.length : points.length - degree
        * @param u The position (0<=u<=1) on the span
        * @param points The control points
        * @param continuous If true the b-spline restarts at 0 when reaching 1
        * @param tmp A temporary vector used for the calculation
        * @return The value of out */
        public static Vector3 calculate(Vector3 _out, int i, float u, Vector3[] points,
            bool continuous, Vector3 tmp)
        {
            int n = points.Count();
            float u2 = u*u;
            float u3 = u2*u;
            _out.Set(points[i]).Scl(1.5f*u3 - 2.5f*u2 + 1.0f);
            if (continuous || i > 0) _out.Add(tmp.Set(points[(n + i - 1)%n]).Scl(-0.5f*u3 + u2 - 0.5f*u));
            if (continuous || i < (n - 1)) _out.Add(tmp.Set(points[(i + 1)%n]).Scl(-1.5f*u3 + 2f*u2 + 0.5f*u));
            if (continuous || i < (n - 2)) _out.Add(tmp.Set(points[(i + 2)%n]).Scl(0.5f*u3 - 0.5f*u2));
            return _out;
        }

        /** Calculates the derivative of the catmullrom spline for the given position (t).
        * @param out The Vector to set to the result.
        * @param t The position (0<=t<=1) on the spline
        * @param points The control points
        * @param continuous If true the b-spline restarts at 0 when reaching 1
        * @param tmp A temporary vector used for the calculation
        * @return The value of out */
        public static Vector3 derivative(Vector3 _out, float t, Vector3[] points, bool continuous, Vector3 tmp)
        {
            int n = continuous ? points.Count() : points.Count() - 3;
            float u = t*n;
            int i = (t >= 1f) ? (n - 1) : (int) u;
            u -= i;
            return derivative(_out, i, u, points, continuous, tmp);
        }

        /** Calculates the derivative of the catmullrom spline for the given span (i) at the given position (u).
        * @param out The Vector to set to the result.
        * @param i The span (0<=i<spanCount) spanCount = continuous ? points.length : points.length - degree
        * @param u The position (0<=u<=1) on the span
        * @param points The control points
        * @param continuous If true the b-spline restarts at 0 when reaching 1
        * @param tmp A temporary vector used for the calculation
        * @return The value of out */
        public static Vector3 derivative(Vector3 _out, int i, float u, Vector3[] points,bool continuous, Vector3 tmp)
        {
            /*
            * catmull'(u) = 0.5 *((-p0 + p2) + 2 * (2*p0 - 5*p1 + 4*p2 - p3) * u + 3 * (-p0 + 3*p1 - 3*p2 + p3) * u * u)
            */
            int n = points.Count();
            float u2 = u*u;
            //  float u3 = u2 * u;
            _out.Set(points[i]).Scl(-u*5 + u2*4.5f);
            if (continuous || i > 0) _out.Add(tmp.Set(points[(n + i - 1)%n]).Scl(-0.5f + u*2 - u2*1.5f));
            if (continuous || i < (n - 1)) _out.Add(tmp.Set(points[(i + 1)%n]).Scl(0.5f + u*4 - u2*4.5f));
            if (continuous || i < (n - 2)) _out.Add(tmp.Set(points[(i + 2)%n]).Scl(-u + u2*1.5f));
            return _out;
        }

        public CatmullRomSpline set(Vector3[] controlPoints, bool continuous)
        {
            if (tmp == null) tmp = controlPoints[0].Cpy();
            if (tmp2 == null) tmp2 = controlPoints[0].Cpy();
            if (tmp3 == null) tmp3 = controlPoints[0].Cpy();
            this.controlPoints = controlPoints;
            this.continuous = continuous;
            spanCount = continuous ? controlPoints.Count() : controlPoints.Count() - 3;
            return this;
        }

        public Vector3 valueAt(Vector3 _out, float t)
        {
            int n = spanCount;
            float u = t*n;
            int i = (t >= 1f) ? (n - 1) : (int) u;
            u -= i;
            return valueAt(_out, i, u);
        }

        /** @return The value of the spline at position u of the specified span */
        public Vector3 valueAt(Vector3 _out, int span, float u)
        {
            return calculate(_out, continuous ? span : (span + 1), u, controlPoints, continuous, tmp);
        }

        public Vector3 derivativeAt(Vector3 _out, float t)
        {
            int n = spanCount;
            float u = t*n;
            int i = (t >= 1f) ? (n - 1) : (int) u;
            u -= i;
            return derivativeAt(_out, i, u);
        }

        /** @return The derivative of the spline at position u of the specified span */
        public Vector3 derivativeAt(Vector3 _out, int span, float u)
        {
            return derivative(_out, continuous ? span : (span + 1), u, controlPoints, continuous, tmp);
        }

        /** @return The span closest to the specified value */
        public int nearest(Vector3 _in)
        {
            return nearest(_in, 0, spanCount);
        }

        /** @return The span closest to the specified value, restricting to the specified spans. */
        public int nearest(Vector3 _in, int start, int count)
        {
            while (start < 0)
                start += spanCount;
            int result = start%spanCount;
            float dst = _in.Dst2(controlPoints[result]);
            for (int i = 1; i < count; i++)
            {
                int idx = (start + i)%spanCount;
                float d = _in.Dst2(controlPoints[idx]);
                if (d < dst)
                {
                    dst = d;
                    result = idx;
                }
            }
            return result;
        }

        public float approximate(Vector3 v)
        {
            return approximate(v, nearest(v));
        }

        public float approximate(Vector3 _in, int start, int count)
        {
            return approximate(_in, nearest(_in, start, count));
        }

        public float approximate(Vector3 _in, int near)
        {
            int n = near;
            Vector3 nearest = controlPoints[n];
            Vector3 previous = controlPoints[n > 0 ? n - 1 : spanCount - 1];
            Vector3 next = controlPoints[(n + 1)%spanCount];
            float dstPrev2 = _in.Dst2(previous);
            float dstNext2 = _in.Dst2(next);
            Vector3 P1, P2, P3;
            if (dstNext2 < dstPrev2)
            {
                P1 = nearest;
                P2 = next;
                P3 = _in;
            }
            else
            {
                P1 = previous;
                P2 = nearest;
                P3 = _in;
                n = n > 0 ? n - 1 : spanCount - 1;
            }
            float L1Sqr = P1.Dst2(P2);
            float L2Sqr = P3.Dst2(P2);
            float L3Sqr = P3.Dst2(P1);
            var L1 = (float) Math.Sqrt(L1Sqr);
            float s = (L2Sqr + L1Sqr - L3Sqr)/(2f*L1);
            float u = Mathf.Clamp((L1 - s)/L1, 0f, 1f);
            return (n + u)/spanCount;
        }

        public float locate(Vector3 v)
        {
            return approximate(v);
        }

        public float approxLength(int samples)
        {
            float tempLength = 0;
            for (int i = 0; i < samples; ++i)
            {
                tmp2.Set(tmp3);
                valueAt(tmp3, (i)/((float) samples - 1));
                if (i > 0) tempLength += tmp2.Dst(tmp3);
            }
            return tempLength;
        }
    }

    internal static class Vector3Extensions
    {
        public static Vector3 Set(this Vector3 v, Vector3 other)
        {
            v.Set(other.x, other.y, other.z);
            return v;
        }

        public static Vector3 Scl(this Vector3 v, float scalar)
        {
            v.Set(v.x * scalar, v.y * scalar, v.z * scalar);
            return v;
        }

        public static Vector3 Add(this Vector3 v, Vector3 other)
        {
            v += other;
            return v;
        }

        public static Vector3 Cpy(this Vector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static float Dst(this Vector3 p, Vector3 q)
        {
            float a = q.x - p.x;
            float b = q.y - p.y;
            float c = q.z - p.z;
            return Mathf.Sqrt(a * a + b * b + c * c);
        }

        public static float Dst2(this Vector3 p, Vector3 q)
        {
            float a = q.x - p.x;
            float b = q.y - p.y;
            float c = q.z - p.z;
            return a * a + b * b + c * c;
        }
    }
}