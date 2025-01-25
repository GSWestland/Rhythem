using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Linq;
using System.Runtime.CompilerServices;
using Rhythem.Player;
using static UnityEngine.ParticleSystem;
using UnityEngine.TextCore.Text;

namespace EM
{
    public interface ICharacter
    {
        Character character { get; set; }
        CollisionModule CollisionModule { get; set; }
    }

    public static class Utility
    {
        public static Vector3 GetClosestPointOnNavMesh(Vector3 target, NavMeshAgent agent)
        {
            // This gives us a sample radius for the NavMesh check relative to our NavMesh agent size, so given either scenerio where we are passed a floor point or the character's position, we should be able to find a point on the NavMesh
            var sampleRadius = agent.height + agent.baseOffset;

            target = NavMesh.SamplePosition(target, out var hit, sampleRadius, NavMesh.AllAreas) ? hit.position : agent.transform.position;

            return target;
        }

        public static int GetNavMeshPointArea(Vector3 target)
        {
            if (NavMesh.SamplePosition(target, out var hit, 0.33f, NavMesh.AllAreas))
            {
                for (var i = 0; i < 32; ++i)
                {
                    if ((1 << i) == hit.mask)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var comp = gameObject.GetComponent<T>();
            return comp != null ? comp : gameObject.AddComponent<T>();
        }

        public static T GetOrAddComponentInChildren<T>(this GameObject gameObject) where T : Component
        {
            var comp = gameObject.GetComponentInChildren<T>();
            return comp != null ? comp : gameObject.AddComponent<T>();
        }

        public static T GetOrAddComponentInParent<T>(this GameObject gameObject) where T : Component
        {
            var comp = gameObject.GetComponentInParent<T>();
            return comp != null ? comp : gameObject.AddComponent<T>();
        }

        public static bool ContainsAny(this string source, params string[] values)
        {
            return values.Any(source.Contains);
        }

        public static bool ContainsAll(this string source, params string[] values)
        {
            return values.All(source.Contains);
        }

        public static float ParseFloat(string desiredText, float defaultValue)
        {
            if (!float.TryParse(desiredText, out var value)) value = defaultValue;
            return value;
        }

        public static int ParseInt(string desiredText, int defaultValue)
        {
            if (!int.TryParse(desiredText, out var value)) value = defaultValue;
            return value;
        }

        public static Vector3 GetMouseWorldPosition()
        {
            var vector = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vector.z = 0f;
            return vector;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            var worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        /// <summary>
        /// Returns 00-FF, value 0->255
        /// </summary>
        public static string DecimalToHex(int value)
        {
            return value.ToString("X2");
        }

        /// <summary>
        /// Returns 0-255
        /// </summary>
        public static int HexToDecimal(string hex)
        {
            return Convert.ToInt32(hex, 16);
        }

        /// <summary>
        /// Returns a hex string based on a number between 0->1
        /// </summary>
        public static string Dec01ToHex(float value)
        {
            return DecimalToHex((int)Mathf.Round(value * 255f));
        }

        /// <summary>
        /// Returns a float between 0->1
        /// </summary>
        public static float HexToDec01(string hex)
        {
            return HexToDecimal(hex) / 255f;
        }

        /// <summary>
        /// Converts a Color into a Color32.
        /// </summary>
        public static Color ConvertColor32ToColor(Color32 givenColor)
        {
            var color = new Color((givenColor.r / 255f), (givenColor.g / 255f), (givenColor.b / 255f), (givenColor.a / 255f));
            return color;
        }

        /// <summary>
        /// Converts a Color32 into a Color.
        /// </summary>
        public static Color32 ConvertColorToColor32(Color givenColor)
        {
            var color = new Color32((byte)(givenColor.r * 255f), (byte)(givenColor.g * 255f), (byte)(givenColor.b * 255f), (byte)(givenColor.a * 255f));
            return color;
        }

        /// <summary>
        /// Get Hex Color FF00FF
        /// </summary>
        public static string GetStringFromColor(Color color)
        {
            var red = Dec01ToHex(color.r);
            var green = Dec01ToHex(color.g);
            var blue = Dec01ToHex(color.b);
            return red + green + blue;
        }

        /// <summary>
        /// Get Hex Color FF00FFAA
        /// </summary>
        public static string GetStringFromColorWithAlpha(Color color)
        {
            var alpha = Dec01ToHex(color.a);
            return GetStringFromColor(color) + alpha;
        }

        /// <summary>
        /// Sets out values to Hex String 'FF'
        /// </summary>
        public static void GetStringFromColor(Color color, out string red, out string green, out string blue, out string alpha)
        {
            red = Dec01ToHex(color.r);
            green = Dec01ToHex(color.g);
            blue = Dec01ToHex(color.b);
            alpha = Dec01ToHex(color.a);
        }

        /// <summary>
        /// Get Hex Color FF00FF
        /// </summary>
        public static string GetStringFromColor(float r, float g, float b)
        {
            var red = Dec01ToHex(r);
            var green = Dec01ToHex(g);
            var blue = Dec01ToHex(b);
            return red + green + blue;
        }

        /// <summary>
        /// Get Hex Color FF00FFAA
        /// </summary>
        public static string GetStringFromColor(float r, float g, float b, float a)
        {
            var alpha = Dec01ToHex(a);
            return GetStringFromColor(r, g, b) + alpha;
        }

        /// <summary>
        /// Get Color from Hex string FF00FFAA
        /// </summary>
        public static Color GetColorFromString(string color)
        {
            var red = HexToDec01(color.Substring(0, 2));
            var green = HexToDec01(color.Substring(2, 2));
            var blue = HexToDec01(color.Substring(4, 2));
            var alpha = 1f;

            if (color.Length >= 8)
            {
                // Color string contains alpha
                alpha = HexToDec01(color.Substring(6, 2));
            }

            return new Color(red, green, blue, alpha);
        }

        public static Vector3 GetRandomDirection()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }

        public static Vector3 GetVectorFromAngle(float desiredAngle)
        {
            var angleRad = desiredAngle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float GetFloatAngleFromVector(Vector3 desiredDirection)
        {
            desiredDirection = desiredDirection.normalized;
            var n = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            return n;
        }

        public static int GetIntAngleFromVector(Vector3 desiredDirection)
        {
            desiredDirection = desiredDirection.normalized;
            var n = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            var angle = Mathf.RoundToInt(n);
            return angle;
        }

        public static int GetIntAngleFromVector180(Vector3 desiredDirection)
        {
            desiredDirection = desiredDirection.normalized;
            var n = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg;
            var angle = Mathf.RoundToInt(n);
            return angle;
        }

        public static Vector3 ApplyRotationToVector(Vector3 desiredVector, Vector3 vectorRotation)
        {
            return ApplyRotationToVector(desiredVector, GetFloatAngleFromVector(vectorRotation));
        }

        public static Vector3 ApplyRotationToVector(Vector3 desiredVector, float desiredAngle)
        {
            return Quaternion.Euler(0, 0, desiredAngle) * desiredVector;
        }

        public static Vector2 GetWorldUIPosition(Vector3 worldPosition, Transform parent, Camera uiCamera, Camera worldCamera)
        {
            var screenPosition = worldCamera.WorldToScreenPoint(worldPosition);
            var uiCameraWorldPosition = uiCamera.ScreenToWorldPoint(screenPosition);
            var localPos = parent.InverseTransformPoint(uiCameraWorldPosition);
            return new Vector2(localPos.x, localPos.y);
        }

        public static Vector3 GetWorldPositionFromUIZeroZ()
        {
            var vector = GetWorldPositionFromUI(Input.mousePosition, Camera.main);
            vector.z = 0f;
            return vector;
        }

        public static Vector3 GetWorldPositionFromUI()
        {
            return GetWorldPositionFromUI(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetWorldPositionFromUI(Camera worldCamera)
        {
            return GetWorldPositionFromUI(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetWorldPositionFromUI(Vector3 screenPosition, Camera worldCamera)
        {
            var worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static Vector3 GetWorldPositionFromUI_Perspective()
        {
            return GetWorldPositionFromUI_Perspective(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetWorldPositionFromUI_Perspective(Camera worldCamera)
        {
            return GetWorldPositionFromUI_Perspective(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetWorldPositionFromUI_Perspective(Vector3 screenPosition, Camera worldCamera)
        {
            var ray = worldCamera.ScreenPointToRay(screenPosition);
            var xy = new Plane(Vector3.forward, new Vector3(0, 0, 0f));
            xy.Raycast(ray, out var distance);
            return ray.GetPoint(distance);
        }

        public static bool IntIsOutsideOf(int value, int minValue, int maxValue, bool includeMinValue = true, bool includeMaxValue = true)
        {
            return !MinValueCheck(value, minValue, includeMinValue) || !MaxValueCheck(value, maxValue, includeMaxValue);
        }

        public static bool IntIsOutsideOf(int value, int range, bool includeRange = true)
        {
            return !MinValueCheck(value, -range, includeRange) || !MaxValueCheck(value, range, includeRange);
        }

        public static bool IntIsBetween(int value, int minValue, int maxValue, bool includeMinValue = true, bool includeMaxValue = true)
        {
            return MinValueCheck(value, minValue, includeMinValue) && MaxValueCheck(value, maxValue, includeMaxValue);
        }

        public static bool IntIsBetween(int value, int range, bool includeRange = true)
        {
            return MinValueCheck(value, -range, includeRange) && MaxValueCheck(value, range, includeRange);
        }

        public static bool FloatIsOutsideOf(float value, float minValue, float maxValue, bool includeMinValue = true, bool includeMaxValue = true)
        {
            return !MinValueCheck(value, minValue, includeMinValue) || !MaxValueCheck(value, maxValue, includeMaxValue);
        }

        public static bool FloatIsOutsideOf(float value, float range, bool includeRange = true)
        {
            return !MinValueCheck(value, -range, includeRange) || !MaxValueCheck(value, range, includeRange);
        }

        public static bool FloatIsBetween(float value, float minValue, float maxValue, bool includeMinValue = true, bool includeMaxValue = true)
        {
            return MinValueCheck(value, minValue, includeMinValue) && MaxValueCheck(value, maxValue, includeMaxValue);
        }

        public static bool FloatIsBetween(float value, float range, bool includeRange = true)
        {
            return MinValueCheck(value, -range, includeRange) && MaxValueCheck(value, range, includeRange);
        }

        private static bool MinValueCheck(float givenValue, float desiredValue, bool includeMinValue = true)
        {
            if (includeMinValue)
            {
                return givenValue >= desiredValue;
            }
            else
            {
                return givenValue > desiredValue;
            }
        }

        private static bool MaxValueCheck(float givenValue, float desiredValue, bool includeMaxValue = true)
        {
            if (includeMaxValue)
            {
                return givenValue <= desiredValue;
            }
            else
            {
                return givenValue < desiredValue;
            }
        }

        private static bool MinValueCheck(int givenValue, int desiredValue, bool includeMinValue = true)
        {
            if (includeMinValue)
            {
                return givenValue >= desiredValue;
            }
            else
            {
                return givenValue > desiredValue;
            }
        }

        private static bool MaxValueCheck(int givenValue, int desiredValue, bool includeMaxValue = true)
        {
            if (includeMaxValue)
            {
                return givenValue <= desiredValue;
            }
            else
            {
                return givenValue < desiredValue;
            }
        }

        public static void Log(string desiredMessage, Type type = null)
        {
            Debug.Log($"[LOG] {(type != null ? $"[{type.Name}]" : "")} [{DateTime.Now}]: {desiredMessage}");
        }

        public static void LogWarning(string desiredMessage, Type type = null, [CallerLineNumber] int lineNumber = 0)
        {
            Debug.LogWarning($"[WARNING] {(type != null ? $"[{type.Name}.{lineNumber}]" : "")} [{DateTime.Now}]: {desiredMessage}");
        }

        public static void LogError(string desiredMessage, Type type = null, [CallerLineNumber] int lineNumber = 0)
        {
            Debug.LogError($"[ERROR] {(type != null ? $"[{type.Name}.{lineNumber}]" : "")} [{DateTime.Now}]: {desiredMessage}");
        }

        public static void LogError(Exception givenException, Type type = null, [CallerLineNumber] int lineNumber = 0)
        {
            Debug.LogError($"[ERROR] {(type != null ? $"[{type.Name}.{lineNumber}]" : "")} [{DateTime.Now}]: {givenException.Message} \n {givenException.StackTrace}");
        }
    }

    public static class ListExtension
    {
        public static void MoveIndex<T>(this List<T> desiredList, T item, int newIndex)
        {
            try
            {
                var currentIndex = desiredList.IndexOf(item);
                var itemCopy = desiredList[currentIndex];
                desiredList.RemoveAt(currentIndex);
                desiredList.Insert(newIndex, itemCopy);
            }
            catch (Exception e)
            {
                Utility.LogError(e);
            }
        }
    }

    public static class RectTransformExtension
    {
        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible in screen space.
        /// </summary>
        private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera = null)
        {
            var screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
            var objectCorners = new Vector3[4];
            var visibleCorners = 0;

            rectTransform.GetWorldCorners(objectCorners);

            for (var i = 0; i < objectCorners.Length; i++)
            {
                var tempScreenSpaceCorner = camera != null ? camera.WorldToScreenPoint(objectCorners[i]) : objectCorners[i];

                if (screenBounds.Contains(tempScreenSpaceCorner))
                {
                    visibleCorners++;
                }
            }

            return visibleCorners;
        }

        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible in screen space.
        /// </summary>
        private static int CountCornersVisibleFrom(this RectTransform rectTransform, Canvas canvas)
        {
            if (canvas == null)
            {
                Utility.LogWarning("The given canvas is null, cannot check to see if rect transform is in bounds.");
                return 0;
            }

            var pixelRect = canvas.pixelRect;
            var canvasBounds = new Rect(0f, 0f, pixelRect.width, pixelRect.height);
            var objectCorners = new Vector3[4];
            var visibleCorners = 0;

            rectTransform.GetWorldCorners(objectCorners);

            for (var i = 0; i < objectCorners.Length; i++)
            {
                if (canvasBounds.Contains(objectCorners[i]))
                {
                    visibleCorners++;
                }
            }

            return visibleCorners;
        }

        /// <summary>
        /// Determines if this RectTransform is fully visible.
        /// Works by checking if each bounding box corner of this RectTransform is inside the desired canvas.
        /// </summary>
        public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Canvas canvas)
        {
            return CountCornersVisibleFrom(rectTransform, canvas) == 4;
        }

        /// <summary>
        /// Determines if this RectTransform is at least partially visible.
        /// Works by checking if each bounding box corner of this RectTransform is inside the desired canvas
        /// </summary>
        public static bool IsVisibleFrom(this RectTransform rectTransform, Canvas canvas)
        {
            return CountCornersVisibleFrom(rectTransform, canvas) > 0;
        }

        /// <summary>
        /// Determines if this RectTransform is fully visible.
        /// Works by checking if each bounding box corner of this RectTransform is inside the screen space view frustrum.
        /// </summary>
        public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera = null)
        {
            return CountCornersVisibleFrom(rectTransform, camera) == 4;
        }

        /// <summary>
        /// Determines if this RectTransform is at least partially visible.
        /// Works by checking if any bounding box corner of this RectTransform is inside the screen space view frustrum.
        /// </summary>
        public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera = null)
        {
            return CountCornersVisibleFrom(rectTransform, camera) > 0;
        }
    }
}