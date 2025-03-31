using UnityEngine;

namespace CrazyPawns.Scripts.Base
{
    public static class EventsProvider
    {
        #region Mouse Events
        public class MouseDownEvent { }
        public class MouseUpEvent { }
        public class MouseHoldEvent { }
        public class MouseDownWithRayEvent
        {
            public readonly Ray Ray;

            public MouseDownWithRayEvent(Ray ray)
            {
                Ray = ray;
            }
        }
        public class MouseHoldWithRayEvent
        {
            public readonly Ray Ray;

            public MouseHoldWithRayEvent(Ray ray)
            {
                Ray = ray;
            }
        }
        #endregion

        #region Sphere Events
        public class ClickSphereEvent
        {
            public SpherePawn ClickedSphere { get; }

            public ClickSphereEvent(SpherePawn sphere)
            {
                ClickedSphere = sphere;
            }
        }
        
        public class RegisterSphereEvent
        {
            public SpherePawn RegisterSphere { get; }

            public RegisterSphereEvent(SpherePawn sphere)
            {
                RegisterSphere = sphere;
            }
        }
        #endregion
    }
}