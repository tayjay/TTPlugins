using System;
using System.Collections.Generic;
using MEC;
using MoonSharp.Interpreter;
using PluginAPI.Core;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaCoroutines
    {
        /* This needs to be called in Lua to generate a C# coroutine
         * Calling this would look like this:
         * local co = Coroutines.create(function()
         *      while true do
         *          print("Hello, world!")
         *          Coroutines.yield(1)
         *      end
         * end)
         */

        public static void Create()
        {
            //MEC.Timing.RunCoroutine()
        }

        public static void CallDelayed(float delay, Closure closure)
        {
            Timing.CallDelayed(delay, () => closure.Call());
        }
        
        public static void CallDelayed(float delay, Closure closure, object[] args)
        {
            Timing.CallDelayed(delay, () => closure.Call(args));
        }
        
        private static IEnumerator<float> Coroutine(DynValue coroutine)
        {
            DynValue result = null;
            while (true)
            {
                DynValue x = coroutine.Coroutine.Resume();
                //Log.Debug(x.ToString());
                if(x.IsNil() || x.Number == 0)
                    break;
                yield return Timing.WaitForSeconds((float)x.Number);
            }
            Log.Debug("Coroutine finished.");
            yield return Timing.WaitForOneFrame;
        }

        private static IEnumerator<float> Coroutine(DynValue coroutine, object[] args)
        {
            DynValue result = null;
            while (true)
            {
                DynValue x;
                if (coroutine.Coroutine.State == CoroutineState.NotStarted)
                {
                    x = coroutine.Coroutine.Resume(args);
                }
                else
                {
                    x = coroutine.Coroutine.Resume();
                }
                if(x.IsNil() /*|| x.Number == 0*/) //todo: Confirm if 0 returned is necessary to break
                    break;
                yield return Timing.WaitForSeconds((float)x.Number);
            }
        }

        public static CoroutineHandle CallCoroutine(Closure closure)
        {
            if (closure == null)
                throw new NullReferenceException("Closure cannot be null.");
            DynValue coroutine = closure.OwnerScript.CreateCoroutine(closure);
            coroutine.Coroutine.AutoYieldCounter = 0;
            return Timing.RunCoroutine(Coroutine(coroutine));
        }
        
        public static CoroutineHandle CallCoroutine(Closure closure, object[] args)
        {
            if (closure == null)
                throw new NullReferenceException("Closure cannot be null.");
            //Exiled.API.Features.Log.Debug("Calling coroutine with args. "+args.Length+" args.");
            if (args == null || args.Length == 0)
            {
                return CallCoroutine(closure);
            }
            DynValue coroutine = closure.OwnerScript.CreateCoroutine(closure);
            coroutine.Coroutine.AutoYieldCounter = 0;
            return Timing.RunCoroutine(Coroutine(coroutine, args));
        }
        
        public static CoroutineHandle CallContinuously(float timeframe, Closure closure)
        {
            if (closure == null)
                throw new NullReferenceException("Closure cannot be null.");
            return Timing.CallContinuously(timeframe, () => closure.Call());
        }
        
        public static CoroutineHandle CallContinuously(float timeframe, Closure closure, object[] args)
        {
            if (closure == null)
                throw new NullReferenceException("Closure cannot be null.");
            return Timing.CallContinuously(timeframe, () => closure.Call(args));
        }
        
        public static CoroutineHandle CallPeriodically(float timeframe, float period, Closure closure)
        {
            if (closure == null)
                throw new NullReferenceException("Closure cannot be null.");
            return Timing.CallPeriodically(timeframe, period, () => closure.Call());
        }
        
        public static CoroutineHandle CallPeriodically(float timeframe, float period, Closure closure, object[] args)
        {
            if (closure == null)
                throw new NullReferenceException("Closure cannot be null.");
            return Timing.CallPeriodically(timeframe, period, () => closure.Call(args));
        }
        
        public static void Kill(CoroutineHandle handle)
        {
            KillCoroutine(handle);
        }
        
        public static void KillCoroutine(CoroutineHandle handle)
        {
            Timing.KillCoroutines(handle);
        }
        
    }
}