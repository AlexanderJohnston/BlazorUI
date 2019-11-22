using BlazorUI.Shared.Services.Metrics;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using System.Reflection;

namespace BlazorUI.Shared.Services.Aspect
{
    [PSerializable]
    public sealed class ProfileAttribute : MethodLevelAspect
    {
        CapturedMetadata _metadata;

        public override void RuntimeInitialize(MethodBase method)
        {
            this._metadata = ReferenceFrame.StaticCollector.RegisterMethod(method);
        }

        [OnMethodEntryAdvice, SelfPointcut]
        public void OnEntry([State(StateScope.MethodInvocation)] out MethodCallData callData)
        {
            callData = default;

            if (ReferenceFrame.CheckEnabled?.Invoke() ?? false) // Look into proper C# 8 nullability constraints. -a
            {
                callData.Start(this._metadata);
            }
        }

        [OnMethodSuccessAdvice(Master = nameof(OnEntry))]
        public static void OnSuccess([State(StateScope.MethodInvocation)] ref MethodCallData callData)
        {
            if (!callData.IsNull)
            {
                callData.Stop();
                Publish(callData);
            }
        }


        [OnMethodExceptionAdvice(Master = nameof(OnEntry))]
        public static void OnException([State(StateScope.MethodInvocation)] ref MethodCallData callData)
        {
            if (!callData.IsNull)
            {
                callData.AddException();
                callData.Stop();
                Publish(callData);
            }
        }


        [OnMethodYieldAdvice(Master = nameof(OnEntry))]
        public static void OnYield([State(StateScope.MethodInvocation)] ref MethodCallData callData)
        {
            if (!callData.IsNull)
            {
                callData.Pause();
            }
        }

        [OnMethodResumeAdvice(Master = nameof(OnEntry))]
        public static void OnResume([State(StateScope.MethodInvocation)] ref MethodCallData callData)
        {
            if (!callData.IsNull)
            {
                callData.Resume();
            }
        }

        private static void Publish(in MethodCallData data)
        {
            ReferenceFrame.StaticCollector.GetCollector().AddSample(data.Metadata, data.MetricData);
        }



    }
}
