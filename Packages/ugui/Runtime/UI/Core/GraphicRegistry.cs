using System.Collections.Generic;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
    /// <summary>
    ///   Registry which maps a Graphic to the canvas it belongs to.
    /// </summary>
    public class GraphicRegistry
    {
        private static GraphicRegistry s_Instance;

        private readonly Dictionary<Canvas, IndexedSet<Graphic>> m_Graphics = new Dictionary<Canvas, IndexedSet<Graphic>>();
        private readonly Dictionary<Canvas, IndexedSet<UICollider>> m_UIColliders = new Dictionary<Canvas, IndexedSet<UICollider>>();

        protected GraphicRegistry()
        {
            // Avoid runtime generation of these types. Some platforms are AOT only and do not support
            // JIT. What's more we actually create a instance of the required types instead of
            // just declaring an unused variable which may be optimized away by some compilers (Mono vs MS).

            // See: 877060

            System.GC.KeepAlive(new Dictionary<Graphic, int>());
            System.GC.KeepAlive(new Dictionary<ICanvasElement, int>());
            System.GC.KeepAlive(new Dictionary<IClipper, int>());
        }

        /// <summary>
        /// The singleton instance of the GraphicRegistry. Creates a new instance if it does not exist.
        /// </summary>
        public static GraphicRegistry instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new GraphicRegistry();
                return s_Instance;
            }
        }

        /// <summary>
        /// Associates a Graphic with a Canvas and stores this association in the registry.
        /// </summary>
        /// <param name="c">The canvas being associated with the Graphic.</param>
        /// <param name="graphic">The Graphic being associated with the Canvas.</param>
        public static void RegisterGraphicForCanvas(Canvas c, Graphic graphic)
        {
            if (c == null || graphic == null)
                return;

            IndexedSet<Graphic> graphics;
            instance.m_Graphics.TryGetValue(c, out graphics);

            if (graphics != null)
            {
                graphics.AddUnique(graphic);

                return;
            }

            // Dont need to AddUnique as we know its the only item in the list
            graphics = new IndexedSet<Graphic>();
            graphics.Add(graphic);
            instance.m_Graphics.Add(c, graphics);
        }

        /// <summary>
        /// Associates a raycastable Graphic with a Canvas and stores this association in the registry.
        /// </summary>
        /// <param name="c">The canvas being associated with the Graphic.</param>
        /// <param name="uiCollider">The Graphic being associated with the Canvas.</param>
        public static void RegisterUIColliderForCanvas(Canvas c, UICollider uiCollider)
        {
            if (c == null || uiCollider == null)
                return;

            IndexedSet<UICollider> uiColliders;
            instance.m_UIColliders.TryGetValue(c, out uiColliders);

            if (uiColliders != null)
            {
                uiColliders.AddUnique(uiCollider);

                return;
            }

            // Dont need to AddUnique as we know its the only item in the list
            uiColliders = new IndexedSet<UICollider>();
            uiColliders.Add(uiCollider);
            instance.m_UIColliders.Add(c, uiColliders);
        }

        /// <summary>
        /// Dissociates a Graphic from a Canvas, removing this association from the registry.
        /// </summary>
        /// <param name="c">The Canvas to dissociate from the Graphic.</param>
        /// <param name="graphic">The Graphic to dissociate from the Canvas.</param>
        public static void UnregisterGraphicForCanvas(Canvas c, Graphic graphic)
        {
            if (c == null || graphic == null)
                return;

            IndexedSet<Graphic> graphics;
            if (instance.m_Graphics.TryGetValue(c, out graphics))
            {
                graphics.Remove(graphic);

                if (graphics.Capacity == 0)
                    instance.m_Graphics.Remove(c);
            }
        }

        /// <summary>
        /// Dissociates a Graphic from a Canvas, removing this association from the registry.
        /// </summary>
        /// <param name="c">The Canvas to dissociate from the Graphic.</param>
        /// <param name="uiCollider">The Graphic to dissociate from the Canvas.</param>
        public static void UnregisterUIColliderForCanvas(Canvas c, UICollider uiCollider)
        {
            if (c == null || uiCollider == null)
                return;

            IndexedSet<UICollider> uiColliders;
            if (instance.m_UIColliders.TryGetValue(c, out uiColliders))
            {
                uiColliders.Remove(uiCollider);

                if (uiColliders.Count == 0)
                    instance.m_UIColliders.Remove(c);
            }
        }

        /// <summary>
        /// Disables a Graphic from a Canvas, disabling this association from the registry.
        /// </summary>
        /// <param name="c">The Canvas to dissociate from the Graphic.</param>
        /// <param name="graphic">The Graphic to dissociate from the Canvas.</param>
        public static void DisableGraphicForCanvas(Canvas c, Graphic graphic)
        {
            if (c == null)
                return;

            IndexedSet<Graphic> graphics;
            if (instance.m_Graphics.TryGetValue(c, out graphics))
            {
                graphics.DisableItem(graphic);

                if (graphics.Capacity == 0)
                    instance.m_Graphics.Remove(c);
            }
        }

        /// <summary>
        /// Disables the raycast for a Graphic from a Canvas, disabling this association from the registry.
        /// </summary>
        /// <param name="c">The Canvas to dissociate from the Graphic.</param>
        /// <param name="uiCollider">The Graphic to dissociate from the Canvas.</param>
        public static void DisableUIColliderForCanvas(Canvas c, UICollider uiCollider)
        {
            if (c == null || !uiCollider)
                return;

            IndexedSet<UICollider> uiColliders;
            if (instance.m_UIColliders.TryGetValue(c, out uiColliders))
            {
                uiColliders.DisableItem(uiCollider);

                if (uiColliders.Capacity == 0)
                    instance.m_UIColliders.Remove(c);
            }
        }

        private static readonly List<UICollider> s_EmptyList = new List<UICollider>();

        /// <summary>
        /// Retrieves the list of Graphics that are raycastable and associated with a Canvas.
        /// </summary>
        /// <param name="canvas">The Canvas to search</param>
        /// <returns>Returns a list of Graphics. Returns an empty list if no Graphics are associated with the specified Canvas.</returns>
        public static IList<UICollider> GetUICollidersForCanvas(Canvas canvas)
        {
            IndexedSet<UICollider> uiColliders;
            if (instance.m_UIColliders.TryGetValue(canvas, out uiColliders))
                return uiColliders;

            return s_EmptyList;
        }
    }
}
