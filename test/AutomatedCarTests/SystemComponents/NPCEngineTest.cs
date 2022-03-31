namespace Tests.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    public class NPCEngineTest
    {
        [Fact]
        public void Constructor_ReceiveNullArgument_ShouldThrowArgumentNullException()
        {
            Action nullConstructor= () => new NPCEngine(null);

            Assert.Throws<ArgumentNullException>(nullConstructor);
        }

        [Fact]
        public void Constructor_ReceiveEmptyList_ShouldThrowInvalidOperationException()
        {

            List<NPC> emptyList = new List<NPC>();

            Action nullConstructor= () => new NPCEngine(emptyList);
            
            Assert.Throws<InvalidOperationException>(nullConstructor);
            
        }
        
        [Fact]
        public void Constructor_ReceiveValidArgumentWithNullElement_ShouldThrowNullReferenceException()
        {
            List<NPC> oneElementList = new List<NPC>();
            oneElementList.Add(null);

            Action nullListElement = () => new NPCEngine(oneElementList);
            
            Assert.Throws<NullReferenceException>(nullListElement);
        }
        
        
        [Theory]
        [InlineData(0,0,"car_1_blue.png")]
        [InlineData(10,10,"man.png")]
        [InlineData(101,101,"car_2_red.png")]
        [InlineData(1000,1000,"car_3_black.png")]
        [InlineData(-10,-10,"woman.png")]
        [InlineData(-101,-101,"car_2_blue.png")]
        [InlineData(-1000,-1000,"car_1_white.png")]
        [InlineData(10,-10,"bicycle.png.png")]
        [InlineData(101,-101,"car_2_red.png")]
        [InlineData(1000,-1000,"car_3_black.png")]
        [InlineData(-10,10,"car_1_red.png")]
        [InlineData(-101,101,"car_2_blue.png")]
        [InlineData(-1000,1000,"tree.png")]
        public void Constructor_ReceiveValidArgument_NPCsShouldNotBeEmpty(int x, int y, string filename)
        {
            List<NPC> oneElementList = new List<NPC>();
            oneElementList.Add(new NPC(x,y,filename));

            NPCEngine npcEngine = new NPCEngine(oneElementList);
            
            Assert.True(npcEngine.NPCs.Any());
        }
        
        

        [Theory]
        [InlineData(0,0,"car_1_blue.png")]
        [InlineData(10,10,"man.png")]
        [InlineData(101,101,"car_2_red.png")]
        [InlineData(1000,1000,"car_3_black.png")]
        [InlineData(-10,-10,"woman.png")]
        [InlineData(-101,-101,"car_2_blue.png")]
        [InlineData(-1000,-1000,"car_1_white.png")]
        [InlineData(10,-10,"bicycle.png.png")]
        [InlineData(101,-101,"car_2_red.png")]
        [InlineData(1000,-1000,"car_3_black.png")]
        [InlineData(-10,10,"car_1_red.png")]
        [InlineData(-101,101,"car_2_blue.png")]
        [InlineData(-1000,1000,"tree.png")]
        public void Constructor_ReceiveValidArgument_NPCsShouldContainsElementData(int x, int y, string filename)
        {
            List<NPC> oneElementList = new List<NPC>();
            oneElementList.Add(new NPC(x,y,filename));

            NPCEngine npcEngine = new NPCEngine(oneElementList);
            
            Assert.Contains(npcEngine.NPCs, npc => npc.X == x && npc.Y == y && npc.Filename == filename);
        }
    }
}