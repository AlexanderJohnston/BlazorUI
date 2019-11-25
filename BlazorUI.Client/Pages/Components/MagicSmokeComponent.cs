using BlazorUI.Shared.Queries;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorUI.Client.Pages.Components
{
	public class MagicSmokeComponent: ComponentBase
	{
		private readonly Random _rng = new Random();

		private readonly string[] _positive = new[]
		{
			"Send it!",
			"I feel it.",
			"There is no other option.",
		};

		private readonly string[] _ambivalent = new[]
		{
			"How the fuck should I know?",
			@"¯\_(ツ)_/¯",
			"Have you tried turning it off and on again?",
			"Ask Nick.",
			"I'll tell you when you're older."
		};

		private readonly string[] _negative = new[]
		{
			"You better don't",
			"Bruh",
			"ಠ_ಠ",
			"Not with that attitude.",
			@"Mr. Madison, what you've just asked is one of the most insanely idiotic questions I have ever heard.

At no point in your random, incoherent question were you even close to anything that could be considered a rational thought.

Everyone in this room is now dumber for having listened to it. I award you no points, and may God have mercy on your soul."
		};

		private string[] Options => _positive.Concat(_ambivalent).Concat(_negative).ToArray();

		public string AskMagicSmoke()
		{
			return Options[_rng.Next(0, Options.Length)];
		}
	}
}