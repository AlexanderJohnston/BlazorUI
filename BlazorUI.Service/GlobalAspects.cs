using BlazorUI.Shared;
using PostSharp.Extensibility;
using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: Log(AttributePriority = 1,
    AttributeTargetElements = MulticastTargets.Method,
    AttributeTargetMemberAttributes = MulticastAttributes.Private | MulticastAttributes.Public | MulticastAttributes.Internal )]
[assembly: Log(AttributePriority = 2, AttributeExclude = true, AttributeTargetMembers = "get_*")]

//[assembly: LogWhen(AttributeTargetTypes="BlazorUI.Shared.Topics.*",
//    AttributeTargetElements = MulticastTargets.Method,
//    AttributeTargetMemberAttributes = MulticastAttributes.Private)]
//[assembly: LogWhen(AttributeTargetTypes = "BlazorUI.Shared.Queries.*",
//    AttributeTargetElements = MulticastTargets.Method,
//    AttributeTargetMemberAttributes = MulticastAttributes.Private)]
//[assembly: LogWhen(AttributeTargetTypes = "BlazorUI.Shared.Events.*",
//    AttributeTargetElements = MulticastTargets.Method,
//    AttributeTargetMemberAttributes = MulticastAttributes.Private, AttributePriority = 1)]
//[assembly: LogWhen(AttributeTargetTypes = "BlazorUI.Shared.Events.Commands*", AttributeExclude = true, AttributePriority = 2)]
//[assembly: LogWhen(AttributeTargetTypes = "BlazorUI.Shared.Events.Database.QueryEvents*", AttributeExclude = true, AttributePriority = 2)]

