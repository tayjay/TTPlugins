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
        
        public static IEnumerator<float> Yield()
        {
            yield return 0;
        }
        
        private static IEnumerator<float> Coroutine(DynValue coroutine)
        {
            DynValue result = null;
            yield return Timing.WaitForSeconds(2f);
            
            
            /*
            if(args == null || args.Length==0)
                result = coroutine.Coroutine.Resume();
            else
                result = coroutine.Coroutine.Resume(args);
            */
            
            /*for(result = coroutine.Coroutine.Resume(); result.Type==DataType.YieldRequest; result = coroutine.Coroutine.Resume())
            {
                // Do something with the result.
                Log.Debug("Coroutine yielded.");
                yield return Timing.WaitForSeconds(1f);
            }*/
            
            while (true)
            {
                DynValue x = coroutine.Coroutine.Resume();
                Log.Debug(x.ToString());
                if(x.IsNil() || x.Number == 0)
                    break;
                yield return Timing.WaitForSeconds((float)x.Number);
            }
            Log.Debug("Coroutine finished.");
            yield return Timing.WaitForOneFrame;
        }

        public static CoroutineHandle CallCoroutine(Closure closure)
        {
            if (closure == null)
                throw new NullReferenceException("Closure cannot be null.");
            DynValue coroutine = closure.OwnerScript.CreateCoroutine(closure);
            coroutine.Coroutine.AutoYieldCounter = 0;
            return Timing.RunCoroutine(Coroutine(coroutine));
        }
        
    }
}