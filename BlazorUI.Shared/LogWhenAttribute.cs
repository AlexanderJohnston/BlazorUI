//using Microsoft.Extensions.Logging;
//using PostSharp.Aspects;
//using PostSharp.Serialization;
//using System;
//using System.Reflection;
//using System.Text;
//using Totem.Runtime;

//namespace BlazorUI.Shared
//{
//    [PSerializable]
//    public class LogWhenAttribute : OnMethodBoundaryAspect
//    {
//        public override void OnEntry(MethodExecutionArgs args)
//        {
//            var topic = (Notion)args.Instance;

//            var stringBuilder = new StringBuilder();
//            stringBuilder.Append("Start: ");
//            stringBuilder.Append(' ');
//            AppendCallInformation(args, stringBuilder);
//            topic.Log.LogInformation(stringBuilder.ToString());
//            base.OnEntry(args);
//        }

//        public override void OnSuccess(MethodExecutionArgs args)
//        {
//            var topic = (Notion)args.Instance;
//            var stringBuilder = new StringBuilder();

//            stringBuilder.Append("Success: ");
//            stringBuilder.Append(' ');
//            AppendCallInformation(args, stringBuilder);

//            if (!args.Method.IsConstructor && ((MethodInfo)args.Method).ReturnType != typeof(void))
//            {
//                stringBuilder.Append(" with exception ");
//                stringBuilder.Append(args.Exception.GetType().Name);
//            }
//            topic.Log.LogInformation(stringBuilder.ToString());
//        }

//        public override void OnException(MethodExecutionArgs args)
//        {
//            var topic = (Notion)args.Instance;

//            var stringBuilder = new StringBuilder();

//            stringBuilder.Append("Failure: ");
//            stringBuilder.Append(' ');
//            AppendCallInformation(args, stringBuilder);

//            if (!args.Method.IsConstructor && ((MethodInfo)args.Method).ReturnType != typeof(void))
//            {
//                stringBuilder.Append(" with exception ");
//                stringBuilder.Append(args.Exception.GetType().Name);
//            }

//            topic.Log.LogInformation(stringBuilder.ToString());
//        }

//        private static void AppendCallInformation(MethodExecutionArgs args, StringBuilder stringBuilder)
//        {
//            var declaringType = args.Method.DeclaringType;
//            AppendTypeName(stringBuilder, declaringType);
//            stringBuilder.Append('.');
//            stringBuilder.Append(args.Method.Name);

//            if (args.Method.IsGenericMethod)
//            {
//                var genericArguments = args.Method.GetGenericArguments();
//                AppendGenericArguments(stringBuilder, genericArguments);
//            }

//            var arguments = args.Arguments;

//            AppendArguments(stringBuilder, arguments);
//        }

//        public static void AppendTypeName(StringBuilder stringBuilder, Type declaringType)
//        {
//            stringBuilder.Append(declaringType.Name);
//            if (declaringType.IsGenericType)
//            {
//                var genericArguments = declaringType.GetGenericArguments();
//                AppendGenericArguments(stringBuilder, genericArguments);
//            }
//        }

//        public static void AppendGenericArguments(StringBuilder stringBuilder, Type[] genericArguments)
//        {
//            stringBuilder.Append('<');
//            for (var i = 0; i < genericArguments.Length; i++)
//            {
//                if (i > 0)
//                {
//                    stringBuilder.Append(", ");
//                }

//                stringBuilder.Append(genericArguments[i].Name);
//            }
//            stringBuilder.Append('>');
//        }

//        public static void AppendArguments(StringBuilder stringBuilder, Arguments arguments)
//        {
//            stringBuilder.Append('(');
//            for (var i = 0; i < arguments.Count; i++)
//            {
//                if (i > 0)
//                {
//                    stringBuilder.Append(", ");
//                }

//                stringBuilder.Append(arguments[i]);
//            }
//            stringBuilder.Append(')');
//        }
//    }
//}
