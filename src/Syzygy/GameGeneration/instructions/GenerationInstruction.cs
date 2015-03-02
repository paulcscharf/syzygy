using Bearded.Utilities;
using Lidgren.Network;
using Syzygy.Game;

namespace Syzygy.GameGeneration
{
    static class GenerationInstruction
    {
        public static IGenerationInstruction FromBuffer(GenerationMessageType type, NetBuffer buffer)
        {
            switch (type)
            {
                case GenerationMessageType.NewFixedBody:
                    return new NewFixedBodyInstruction(buffer);
                case GenerationMessageType.NewOrbitingBody:
                    return new NewOrbitingBodyInstruction(buffer);
                case GenerationMessageType.AssignPlayerToBody:
                    return new AssignPlayerToBodyInstruction(buffer);
                default:
                {
                    Log.Warning("Unknown generation message type: " + type);
                    return null;
                }
            }
        }
    }

    abstract class GenerationInstruction<TParameters> : IGenerationInstruction
        where TParameters : struct
    {
        private readonly GenerationMessageType type;
        protected readonly TParameters parameters;

        protected GenerationInstruction(GenerationMessageType type, NetBuffer buffer)
        {
            this.type = type;
            buffer.Read(out this.parameters);
        }

        protected GenerationInstruction(GenerationMessageType type, TParameters parameters)
        {
            this.type = type;
            this.parameters = parameters;
        }

        public abstract void Execute(GameState game);

        public void WriteMessage(NetBuffer buffer)
        {
            buffer.Write((byte)this.type);
            buffer.Write(this.parameters);
        }
    }
}
