using System;
using System.Threading.Tasks;
using Orleans;
using SampleOrleans.GrainInterfaces;

namespace SampleOrleans.GrainCollection
{
    /// <summary>
    /// Grain implementation class Grain1.
    /// </summary>
    public class DefaultRandomizerGrain : Grain, IRandomizerGrain
    {
        private static readonly string[] WELCOME_MESSAGES = new string[]
        {
            "Hi {NAME}! Hello from grain...",
            "I'm afraid I can't let you do that, {NAME}...",
            "Nice to see you again {NAME}!",
            "Hello {NAME}",
            "Wilkommen {NAME}",
            "Hiya {NAME}!",
            "Howdy {NAME}!?",
            "How you doing {NAME}?"
        };

        private string _format;

        public override Task OnActivateAsync()
        {
            var random = new Random().Next(0, WELCOME_MESSAGES.Length);
            _format = WELCOME_MESSAGES[random];
            return base.OnActivateAsync();
        }

        public Task<string> SayRandomWelcome(string name)
        {
            var result = _format.Replace("{NAME}", name);
            return Task.FromResult(result);
        }
    }
}
