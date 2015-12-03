using System;
using System.Reflection;

namespace SomeService.Web.Support
{
    public static class Exceptions
    {
        /// <summary>
        /// When <paramref name="ex"/> is a <see cref="TargetInvocationException"/>, unwraps its <see cref="Exception.InnerException"/> when its not <c>null.</c>
        /// </summary>
        public static Exception UnwrapIfTargetInvocationException(Exception ex)
        {
            var tie = ex as TargetInvocationException;
            if (tie == null)
                return ex;
            return tie.InnerException ?? tie;
        }
    }
}
