using NUnit.Framework;
using UnityEngine.TestTools;
using TheArchitect.Core;

namespace TheArchitect.Tests
{
    public class StringTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void StringTests1()
        {
            ResourceString.Parse("... my legs... what did he say about my legs...");
        }

        
    }
}
