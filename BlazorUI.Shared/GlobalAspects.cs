using BlazorUI.Shared;
using PostSharp.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: LogWhen(AttributeTargetTypes="BlazorUI.Shared.Topics.*",
    AttributeTargetElements = MulticastTargets.Method,
    AttributeTargetMemberAttributes = MulticastAttributes.Private)]
[assembly: LogWhen(AttributeTargetTypes = "BlazorUI.Shared.Queries.*",
    AttributeTargetElements = MulticastTargets.Method,
    AttributeTargetMemberAttributes = MulticastAttributes.Private)]
[assembly: LogWhen(AttributeTargetTypes = "BlazorUI.Shared.Events.*",
    AttributeTargetElements = MulticastTargets.Method,
    AttributeTargetMemberAttributes = MulticastAttributes.Private, AttributePriority = 1)]
[assembly: LogWhen(AttributeTargetTypes = "BlazorUI.Shared.Events.Commands*", AttributeExclude = true, AttributePriority = 2)]
[assembly: LogWhen(AttributeTargetTypes = "BlazorUI.Shared.Events.Database.QueryEvents*", AttributeExclude = true, AttributePriority = 2)]

