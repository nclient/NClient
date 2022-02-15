using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;

namespace NClient.Providers.Transport.SystemNetHttp.Helpers
{
    /// <summary>
    /// An adapter of <see cref="StreamContent"/> that won't automatically dispose its underlying stream.
    /// </summary>
    // ReSharper disable once UnusedType.Global
    internal sealed class LeavingOpenStreamContent : StreamContent
    {
        public LeavingOpenStreamContent(Stream stream) : base(stream)
        {
        }

        public LeavingOpenStreamContent(Stream stream, int bufferSize) : base(stream, bufferSize)
        {
        }

        private static readonly Action<LeavingOpenStreamContent> HttpContentDispose;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                HttpContentDispose(this);
            else
                base.Dispose(false);
        }

        static LeavingOpenStreamContent()
        {
            var method = new DynamicMethod(
                name: $"${nameof(LeavingOpenStreamContent)}.{nameof(Dispose)}()", 
                returnType: null,
                parameterTypes: new[] { typeof(LeavingOpenStreamContent) }, 
                owner: typeof(LeavingOpenStreamContent));
            
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);  // this
            il.Emit(OpCodes.Ldc_I4_1); // disposing
            il.EmitCall(
                opcode: OpCodes.Call,
                methodInfo: typeof(HttpContent).GetTypeInfo().DeclaredMethods
                    .First(m => m.Name == nameof(Dispose) && m.GetParameters().FirstOrDefault()?.ParameterType == typeof(bool)),
                optionalParameterTypes: null);
            il.Emit(OpCodes.Ret);
            
            HttpContentDispose = (Action<LeavingOpenStreamContent>) method.CreateDelegate(typeof(Action<LeavingOpenStreamContent>));
        }
    }
}
